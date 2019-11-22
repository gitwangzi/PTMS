using Gsafety.PTMS.Media.TransportStream.TsParser.Descriptor;
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.Media.TransportStream.TsParser
{
    public class TsProgramMapTableFactory : ITsProgramMapTableFactory
    {
        readonly ITsDescriptorFactory _descriptorFactory;

        public TsProgramMapTableFactory(ITsDescriptorFactory descriptorFactory)
        {
            if (null == descriptorFactory)
                throw new ArgumentNullException("descriptorFactory");

            _descriptorFactory = descriptorFactory;
        }

        public TsProgramMapTable Create(ITsDecoder decoder, int programNumber, uint pid, Action<IProgramStreams> streamFilter)
        {
            return new TsProgramMapTable(decoder, _descriptorFactory, programNumber, pid, streamFilter);
        }
    }
}
