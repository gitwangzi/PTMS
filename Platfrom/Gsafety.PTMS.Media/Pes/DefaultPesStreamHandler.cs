using System;
using Gsafety.PTMS.Media.Configuration;
using Gsafety.PTMS.Media.TransportStream.TsParser;

namespace Gsafety.PTMS.Media.Pes
{
    public class DefaultPesStreamHandler : PesStreamHandler
    {
        readonly Action<TsPesPacket> _nextHandler;

        public DefaultPesStreamHandler(PesStreamParameters parameters)
            : base(parameters)
        {
            if (null == parameters)
                throw new ArgumentNullException("parameters");
            if (null == parameters.NextHandler)
                throw new ArgumentException("NextHandler cannot be null", "parameters");

            _nextHandler = parameters.NextHandler;
        }

        public override IConfigurationSource Configurator
        {
            get { return null; }
        }

        public override void PacketHandler(TsPesPacket packet)
        {
            base.PacketHandler(packet);

            _nextHandler(packet);
        }
    }
}
