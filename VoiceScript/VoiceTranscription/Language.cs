using Google.Cloud.Speech.V1;

namespace VoiceScript.VoiceTranscription
{
    public abstract class Language
    {
        public abstract string LanguageCode { get; }
        public abstract string Name { get; }
        public override string ToString() => Name;
    }

    public class English : Language
    {
        public override string LanguageCode => LanguageCodes.English.UnitedStates;
        public override string Name => "English";
    }
    public class Czech : Language
    {
        public override string LanguageCode => LanguageCodes.Czech.CzechRepublic;
        public override string Name => "Czech";
    }
    public class German : Language
    {
        public override string LanguageCode => LanguageCodes.German.Germany;
        public override string Name => "German";
    }
}
