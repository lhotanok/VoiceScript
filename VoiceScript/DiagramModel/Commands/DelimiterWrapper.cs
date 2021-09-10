using VoiceScript.DiagramModel.Commands.LanguageFormats;

namespace VoiceScript.DiagramModel.Commands
{
    class DelimiterWrapper
    {
        readonly string command;
        bool delimiterSet, delimiterPreviouslySet;
        public DelimiterWrapper(LanguageFormat languageFormat)
        {
            (delimiterSet, delimiterPreviouslySet) = (false, false);
            command = languageFormat.DelimiterFormat;
        }
        public string CommandDefaultFormat { get => command; }
        public bool IsDelimiter(string word) => word.ToLower() == command;

        public bool DelimiterSet {
            get => delimiterSet;

            set {
                delimiterPreviouslySet = delimiterSet;
                delimiterSet = value;
            }
        }

        public bool Escape() => delimiterPreviouslySet;

        public void UpdateDelimiterContext(string word)
        {
            if (IsDelimiter(word) && !DelimiterSet)
            {
                DelimiterSet = true; // set delimiter for the next run
            }
            else if (DelimiterSet)
            {
                DelimiterSet = false; // consume delimiter
            }
        }
    }
}
