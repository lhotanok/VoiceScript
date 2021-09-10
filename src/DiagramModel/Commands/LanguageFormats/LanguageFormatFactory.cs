using System;
using System.Collections.Generic;

namespace DiagramModel.Commands.LanguageFormats
{
    class LanguageFormatFactory
    {
        static readonly Dictionary<string, Func<LanguageFormat>> formatCtors = new()
        {
            { EnglishFormat.GetCode(), () => new EnglishFormat() },
            { CzechFormat.GetCode(), () => new CzechFormat() }
        };

        public static LanguageFormat CreateLanguageFormat(string languageCode)
        {
            return formatCtors.ContainsKey(languageCode)
                ? formatCtors[languageCode]()
                : null;
        }

        public static IList<string> GetSupportedFormats()
        {
            var supportedFormats = new List<string>();

            foreach (var languageCode in formatCtors.Keys)
            {
                supportedFormats.Add(languageCode);
            }

            return supportedFormats;
        }
    }
}
