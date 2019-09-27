using System;
using System.Collections.Generic;
using System.Diagnostics;
using Gsafety.PTMS.Media.Metadata;
using Gsafety.PTMS.Media.TransportStream.TsParser;
using Gsafety.PTMS.Media.Common.Loggers;

namespace Gsafety.PTMS.Media.Pes
{
    public sealed class PesHandlers : IPesHandlers
    {
        readonly IPesHandlerFactory _handlerFactory;
        readonly Func<PesStreamParameters> _parameterFactory;

        readonly Dictionary<uint, PesStreamHandler> _handlers = new Dictionary<uint, PesStreamHandler>();
        readonly Dictionary<byte, Func<uint, TsStreamType, Action<TsPesPacket>>> _pesStreamHandlerFactory =
            new Dictionary<byte, Func<uint, TsStreamType, Action<TsPesPacket>>>();

        public PesHandlers(IPesHandlerFactory handlerFactory, Func<PesStreamParameters> parameterFactory)
        {
            if (null == handlerFactory)
                throw new ArgumentNullException("handlerFactory");
            if (null == parameterFactory)
                throw new ArgumentNullException("parameterFactory");

            _handlerFactory = handlerFactory;
            _parameterFactory = parameterFactory;
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            CleanupHandlers();
        }

        public PesStreamHandler GetPesHandler(TsStreamType streamType, uint pid, IMediaStreamMetadata mediaStreamMetadata, Action<TsPesPacket> nextHandler)
        {
            PesStreamHandler handler;

            if (_handlers.TryGetValue(pid, out handler))
                LoggerInstance.Debug("Found PES {0} stream ({1}) with PID {2}", streamType.Contents, streamType.Description, pid);
            else
            {
                LoggerInstance.Debug("Create PES {0} stream ({1}) with PID {2}", streamType.Contents, streamType.Description, pid);

                var parameters = _parameterFactory();

                parameters.Pid = pid;
                parameters.StreamType = streamType;
                parameters.NextHandler = nextHandler;
                parameters.MediaStreamMetadata = mediaStreamMetadata;

                handler = _handlerFactory.CreateHandler(parameters);

                _handlers[pid] = handler;
            }

            return handler;
        }

        public void Initialize()
        {
            CleanupHandlers();
        }

        void CleanupHandlers()
        {
            if (null == _handlers)
                return;

            _handlers.Clear();
        }

        public void RegisterHandlerFactory(byte streamType, Func<uint, TsStreamType, Action<TsPesPacket>> handlerFactory)
        {
            _pesStreamHandlerFactory[streamType] = handlerFactory;
        }
    }
}
