using Google.Cloud.Speech.V1;

namespace VoiceScript.VoiceTranscription
{
    /// <summary>
    /// Wrapper for language info.
    /// Contains language code and human-readable name.
    /// </summary>
    public abstract class Language
    {
        /// <summary>
        /// Language code corresponding to the unified standard.
        /// </summary>
        public abstract string LanguageCode { get; }

        /// <summary>
        /// Language full name. 
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Overrides default <see cref="object.ToString()"/>
        /// and returns language name directly.
        /// Useful for integration with WinForms designer.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Name;
    }

    /// <summary>
    /// English language representation.
    /// </summary>
    public class English : Language
    {
        public override string LanguageCode => LanguageCodes.English.UnitedStates;
        public override string Name => "English";
    }

    /// <summary>
    /// Czech language representation.
    /// </summary>
    public class Czech : Language
    {
        public override string LanguageCode => LanguageCodes.Czech.CzechRepublic;
        public override string Name => "Czech";
    }
}
