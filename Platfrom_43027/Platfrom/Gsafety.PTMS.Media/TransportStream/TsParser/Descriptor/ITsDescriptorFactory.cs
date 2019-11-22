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

namespace Gsafety.PTMS.Media.TransportStream.TsParser.Descriptor
{
    public interface ITsDescriptorFactory
    {
        TsDescriptor Create(byte code, byte[] buffer, int offset, int length);
    }
}
