using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Gsafety.PTMS.Media.Metadata;
using Gsafety.PTMS.Media.TransportStream.TsParser.Descriptor;
using Gsafety.PTMS.Media.Utility;
using Gsafety.PTMS.Media.Common.Loggers;

namespace Gsafety.PTMS.Media.TransportStream.TsParser
{
    public class TsProgramMapTable : TsProgramSpecificInformation
    {
        const int MinimumProgramMapSize = 9;
        readonly ITsDecoder _decoder;
        readonly ITsDescriptorFactory _descriptorFactory;
        readonly Dictionary<uint, ProgramMap> _newProgramStreams = new Dictionary<uint, ProgramMap>();
        readonly uint _pid;
        readonly int _programNumber;
        readonly Dictionary<uint, ProgramMap> _programStreamMap = new Dictionary<uint, ProgramMap>();
        readonly Dictionary<Tuple<uint, TsStreamType>, ProgramMap> _retiredProgramStreams = new Dictionary<Tuple<uint, TsStreamType>, ProgramMap>();
        readonly Action<IProgramStreams> _streamFilter;
        readonly List<ProgramMap> _streamList = new List<ProgramMap>();
        bool _foundPcrPid;
        TsDescriptor[] _newProgramDescriptors;
        ulong? _pcr;
        int? _pcrIndex;
        uint _pcrPid;

        public TsProgramMapTable(ITsDecoder decoder, ITsDescriptorFactory descriptorFactory, int programNumber, uint pid, Action<IProgramStreams> streamFilter)
            : base(TsTableId.TS_program_map_section)
        {
            if (null == decoder)
                throw new ArgumentNullException("decoder");
            if (null == descriptorFactory)
                throw new ArgumentNullException("descriptorFactory");

            _decoder = decoder;
            _descriptorFactory = descriptorFactory;
            _programNumber = programNumber;
            _pid = pid;
            _streamFilter = streamFilter;
        }

        protected override void ParseSection(TsPacket packet, int offset, int length)
        {
            if (length < MinimumProgramMapSize)
                return;

            var i = offset;
            var buffer = packet.Buffer;

            var program_number = (buffer[i] << 8) | buffer[i + 1];
            i += 2;

            var version_number = buffer[i++];

            var current_next_indicator = 0 != (version_number & 1);

            version_number >>= 1;
            version_number &= 0x1f;

            var section_number = buffer[i++];
            var last_section_number = buffer[i++];

            if (last_section_number < section_number)
                return;

            var PCR_PID = ((uint)buffer[i] << 8) | buffer[i + 1];
            i += 2;
            PCR_PID &= 0x1fff;

            _pcrPid = PCR_PID;

            var program_info_length = (buffer[i] << 8) | buffer[i + 1];
            i += 2;
            program_info_length &= 0x0fff;

            if (i - offset + program_info_length >= length)
                return;

            TsDescriptor[] programDescriptors = null;

            if (program_info_length > 0)
            {
                //LoggerInstance.Debug("TsProgramMapTable.Add() program descriptor for program " + program_number);
                //TsDescriptors.DebugWrite(buffer, i, program_info_length);

                programDescriptors = _descriptorFactory.Parse(buffer, i, program_info_length).ToArray();
            }

            _newProgramDescriptors = programDescriptors;

            i += program_info_length;

            var mappingEnd = offset + length;

            while (i + 5 <= mappingEnd)
            {
                var stream_type = buffer[i++];

                var elementary_PID = ((uint)buffer[i] << 8) | buffer[i + 1];
                i += 2;

                elementary_PID &= 0x1fff;

                var ES_info_length = (buffer[i] << 8) | buffer[i + 1];
                i += 2;

                ES_info_length &= 0x0fff;

                if (i + ES_info_length > mappingEnd)
                    return;

                TsDescriptor[] streamDescriptors = null;

                if (ES_info_length > 0)
                {
                    //LoggerInstance.Debug("TsProgramMapTable.Add() ES descriptor for PID " + elementary_PID);
                    //TsDescriptors.DebugWrite(buffer, i, ES_info_length);

                    streamDescriptors = _descriptorFactory.Parse(buffer, i, ES_info_length).ToArray();
                }

                i += ES_info_length;

                var streamType = TsStreamType.FindStreamType(stream_type);

                var programMap = new ProgramMap
                {
                    Pid = elementary_PID,
                    StreamType = streamType,
                    StreamDescriptors = streamDescriptors
                };

                _newProgramStreams[elementary_PID] = programMap;
            }

            if (section_number == last_section_number)
                MapProgramStreams();
        }

        void AddPcr(TsPacket packet)
        {
            if (null == packet || !packet.Pcr.HasValue)
                return;

            _pcrIndex = packet.TsIndex;
            _pcr = packet.Pcr;
        }

        void ClearProgram(ProgramMap program)
        {
            _decoder.UnregisterHandler(program.Pid);

            var pes = program.Stream;

            if (null != pes)
                pes.Clear();

            var remove = _programStreamMap.Remove(program.Pid);

            Debug.Assert(remove);
        }

        public void Clear()
        {
            foreach (var program in _programStreamMap.Values.ToArray())
                ClearProgram(program);

            Debug.Assert(0 == _programStreamMap.Count);

            _newProgramStreams.Clear();

            _newProgramDescriptors = null;

            _retiredProgramStreams.Clear();
        }

        public void FlushBuffers()
        {
            foreach (var program in _programStreamMap.Values)
                program.Stream.FlushBuffers();

            _newProgramStreams.Clear();
        }

        void MapProgramStreams()
        {
            _streamList.Clear();

            foreach (var program in _programStreamMap.Values)
            {
                ProgramMap newProgramMap;
                if (_newProgramStreams.TryGetValue(program.Pid, out newProgramMap))
                {
                    if (!Equals(newProgramMap.StreamType, program.StreamType))
                        _streamList.Add(program);
                }
                else
                    _streamList.Add(program);
            }

            if (_streamList.Count > 0)
            {
                foreach (var program in _streamList)
                {
                    LoggerInstance.Debug("*** TsProgramMapTable.MapProgramStreams(): retiring " + program);

                    ClearProgram(program);

                    _retiredProgramStreams[Tuple.Create(program.Pid, program.StreamType)] = program;
                }

                _streamList.Clear();
            }

            var descriptors = _newProgramDescriptors;

            var programLanguage = Iso639_2Normalization.Normalize(descriptors.GetDefaultLanguage());

            var programStreams = new ProgramStreams
            {
                ProgramNumber = _programNumber,
                Language = programLanguage,
                Streams = _newProgramStreams.Values
                    .Select(s => new ProgramStreams.ProgramStream
                    {
                        Pid = s.Pid,
                        StreamType = s.StreamType,
                        Language =  Iso639_2Normalization.Normalize(s.StreamDescriptors.GetDefaultLanguage()) ?? programLanguage
                    })
                    .ToArray()
            };

            if (null != _streamFilter)
                _streamFilter(programStreams);

            foreach (var programStream in from ps in programStreams.Streams
                                          join pm in _newProgramStreams.Values on ps.Pid equals pm.Pid
                                          select new
                                          {
                                              ps.BlockStream,
                                              ProgramStream = pm,
                                              ps.Language
                                          })
            {
                var streamRequested = !programStream.BlockStream;

                var pid = programStream.ProgramStream.Pid;
                var streamType = programStream.ProgramStream.StreamType;
                var language = programStream.Language;

                ProgramMap mappedProgram;
                if (_programStreamMap.TryGetValue(pid, out mappedProgram))
                {
                    if (Equals(mappedProgram.StreamType, streamType) && streamRequested)
                        continue;

                    ClearProgram(mappedProgram);
                }

                if (streamRequested)
                {
                    TsPacketizedElementaryStream pes;

                    var key = Tuple.Create(pid, streamType);

                    ProgramMap retiredProgram;
                    if (_retiredProgramStreams.TryGetValue(key, out retiredProgram))
                    {
                        LoggerInstance.Debug("*** TsProgramMapTable.MapProgramStreams(): remapping retired program " + retiredProgram);

                        var removed = _retiredProgramStreams.Remove(key);

                        Debug.Assert(removed, "Unable to remove program from retired");

                        pes = retiredProgram.Stream;

                        _programStreamMap[pid] = retiredProgram;
                    }
                    else
                    {
                        IMediaStreamMetadata mediaStreamMetadata = null;

                        if (null != language)
                            mediaStreamMetadata = new MediaStreamMetadata { Language = language };

                        pes = _decoder.CreateStream(streamType, pid, mediaStreamMetadata);

                        programStream.ProgramStream.Stream = pes;

                        _programStreamMap[pid] = programStream.ProgramStream;
                    }

                    if (pid == _pcrPid)
                    {
                        _foundPcrPid = true;

                        _decoder.RegisterHandler(pid,
                            p =>
                            {
                                AddPcr(p);
                                pes.Add(p);
                            });
                    }
                    else
                        _decoder.RegisterHandler(pid, pes.Add);
                }
                else
                {
                    if (pid == _pcrPid)
                    {
                        _foundPcrPid = true;

                        _decoder.RegisterHandler(pid, AddPcr);
                    }
                }
            }

            _newProgramStreams.Clear();

            if (!_foundPcrPid)
            {
                _foundPcrPid = true;
                _decoder.RegisterHandler(_pcrPid, AddPcr);
            }
        }

        class ProgramMap
        {
            public uint Pid;
            public TsPacketizedElementaryStream Stream;
            public TsDescriptor[] StreamDescriptors;
            public TsStreamType StreamType;

            public override string ToString()
            {
                return string.Format("{0}/{1}", Pid, null == StreamType ? "<unknown type>" : StreamType.Description);
            }
        }
    }
}
