using System;
using Gsafety.PTMS.Media.TransportStream.TsParser.Descriptor;

namespace Gsafety.PTMS.Media.TransportStream.TsParser
{
    public class TsTransportStreamDescriptionTable : TsProgramSpecificInformation
    {
        const int MinimumDescriptionTableLength = 5;
        readonly ITsDescriptorFactory _descriptorFactory;

        public TsTransportStreamDescriptionTable(ITsDescriptorFactory descriptorFactory)
            : base(TsTableId.TS_description_section)
        {
            if (null == descriptorFactory)
                throw new ArgumentNullException("descriptorFactory");

            _descriptorFactory = descriptorFactory;
        }

        protected override void ParseSection(TsPacket packet, int offset, int length)
        {
            if (length < MinimumDescriptionTableLength)
                return;

            var i = offset;
            var buffer = packet.Buffer;
            var sectionEnd = i + length;

            i += 2; // Skip reserved

            var version_number = buffer[i++];

            var current_next_indicator = version_number & 1;

            version_number = (byte)((version_number >> 1) & 0x1f);

            var section_number = buffer[i++];

            var last_section_number = buffer[i++];

            var descriptors = _descriptorFactory.Parse(buffer, i, sectionEnd - i);
        }
    }
}
