using Google.Cloud.Speech.V1;

namespace VoiceScript
{
    abstract class Language
    {
        public abstract string LanguageCode { get; }
        public abstract string Name { get; }
        public override string ToString() => Name;
    }

    class English : Language
    {
        public override string LanguageCode => LanguageCodes.English.UnitedStates;
        public override string Name => "English";
    }
    class Czech : Language
    {
        public override string LanguageCode => LanguageCodes.Czech.CzechRepublic;
        public override string Name => "Czech";
    }
    class German : Language
    {
        public override string LanguageCode => LanguageCodes.German.Germany;
        public override string Name => "German";
    }
}
