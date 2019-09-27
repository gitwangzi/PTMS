using System;
using System.Text;
using Gsafety.PTMS.Media.Utility.TextEncodings;

namespace Gsafety.PTMS.Media.TransportStream.TsParser.Descriptor
{
    public class TsIso639LanguageDescriptor : TsDescriptor
    {
        public static readonly TsDescriptorType DescriptorType = new TsDescriptorType(10, "ISO 639 language");
        readonly Language[] _languages;

        public TsIso639LanguageDescriptor(Language[] languages)
            : base(DescriptorType)
        {
            if (null == languages)
                throw new ArgumentNullException("languages");

            _languages = languages;
        }

        public Language[] Languages
        {
            get { return _languages; }
        }

        enum AudioType
        {
            // ISO/IEC 13818-1:2007 Table 2-60
            Undefined = 0,
            Clean_effects = 1,
            Hearing_impaired = 2,
            Visual_impaired_commentary = 3
        }

        public class Language
        {
            readonly byte _audioType;
            readonly string _iso639;

            public Language(string iso639, byte audioType)
            {
                if (iso639 == null)
                    throw new ArgumentNullException("iso639");

                _iso639 = iso639;
                _audioType = audioType;
            }

            public string Iso639_2
            {
                get { return _iso639; }
            }

            public byte AudioType
            {
                get { return _audioType; }
            }
        }
    }

    public class TsIso639LanguageDescriptorFactory : TsDescriptorFactoryInstanceBase
    {
        const int BlockLength = 4;
        readonly Encoding _latin1;

        public TsIso639LanguageDescriptorFactory(ISmEncodings smEncodings)
            : base(TsIso639LanguageDescriptor.DescriptorType)
        {
            if (null == smEncodings)
                throw new ArgumentNullException("smEncodings");

            _latin1 = smEncodings.Latin1Encoding;
        }

        public override TsDescriptor Create(byte[] buffer, int offset, int length)
        {
            if (length < BlockLength)
                return null;

            var i = offset;
            var languages = new TsIso639LanguageDescriptor.Language[length / BlockLength];
            var languageIndex = 0;

            while (length >= BlockLength)
            {
                var language = _latin1.GetString(buffer, i, 3);

                var audio_type = buffer[3];

                languages[languageIndex++] = new TsIso639LanguageDescriptor.Language(language, audio_type);

                i += BlockLength;
                length -= BlockLength;
            }

            return new TsIso639LanguageDescriptor(languages);
        }
    }
}
