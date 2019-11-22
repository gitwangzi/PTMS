using Gsafety.PTMS.Media.Common.Loggers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Gsafety.PTMS.Media.TransportStream.TsParser.Descriptor
{
    public static class TsDescriptors
    {
        public static IEnumerable<TsDescriptor> Parse(this ITsDescriptorFactory factory, byte[] buffer, int offset, int length)
        {
            while (length > 0)
            {
                if (length < 2)
                {
                    LoggerInstance.Debug("Unused buffer " + length);
                    break;
                }

                var code = buffer[offset];
                var descriptorLength = buffer[offset + 1];

                offset += 2;
                length -= 2;

                if (length < descriptorLength)
                {
                    LoggerInstance.Debug(" " + descriptorLength + " exceeds buffer (" + length + " remaining)");
                    break;
                }

                var descriptor = factory.Create(code, buffer, offset, descriptorLength);

                if (null != descriptor)
                    yield return descriptor;

                length -= descriptorLength;
                offset += descriptorLength;
            }
        }

        public static void WriteDescriptors(TextWriter writer, byte[] buffer, int offset, int length)
        {
            while (length > 0)
            {
                if (length < 2)
                {
                    writer.WriteLine("Unused buffer " + length);
                    break;
                }

                var code = buffer[offset];
                var descriptorLength = buffer[offset + 1];

                offset += 2;
                length -= 2;

                var type = TsDescriptorTypes.GetDescriptorType(code);

                if (null == type)
                    writer.Write(code + ":Unknown");
                else
                    writer.Write(type);

                if (length < descriptorLength)
                {
                    writer.WriteLine(" " + descriptorLength + " exceeds buffer (" + length + " remaining)");
                    break;
                }

                length -= descriptorLength;
                offset += descriptorLength;
            }
        }

        [Conditional("DEBUG")]
        public static void DebugWrite(byte[] buffer, int offset, int length)
        {
            using (var sw = new StringWriter())
            {
                WriteDescriptors(sw, buffer, offset, length);

                LoggerInstance.Debug(sw.ToString());
            }
        }

        public static string GetDefaultLanguage(this IEnumerable<TsDescriptor> descriptors)
        {
            if (null == descriptors)
                return null;

            var languageDescriptor = descriptors.OfType<TsIso639LanguageDescriptor>().FirstOrDefault();

            if (null == languageDescriptor)
                return null;

            if (languageDescriptor.Languages.Length < 1)
                return null;

            return languageDescriptor.Languages[0].Iso639_2;
        }
    }
}
