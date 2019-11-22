using System.Collections.Generic;
using System.Linq;

namespace Gsafety.PTMS.Media.TransportStream.TsParser.Descriptor
{
    public class TsDescriptorFactory : ITsDescriptorFactory
    {
        readonly ITsDescriptorFactoryInstance[] _factories;

        public TsDescriptorFactory(IEnumerable<ITsDescriptorFactoryInstance> factories)
        {
            var allFactories = factories.OrderBy(f => f.Type.Code).ToArray();

            var maxIndex = allFactories.Max(f => f.Type.Code);

            _factories = new ITsDescriptorFactoryInstance[maxIndex + 1];

            foreach (var factory in allFactories)
                _factories[factory.Type.Code] = factory;
        }

        public TsDescriptor Create(byte code, byte[] buffer, int offset, int length)
        {
            if (code >= _factories.Length)
                return null;

            var factory = _factories[code];

            if (null == factory)
                return null;

            return factory.Create(buffer, offset, length);
        }
    }
}
