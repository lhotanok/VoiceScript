using DiagramModel.Commands.LanguageFormats;

namespace DiagramModel.Commands
{
    class DelimiterWrapper
    {
        readonly string escapeCommand;
        public DelimiterWrapper(LanguageFormat languageFormat)
        {
            escapeCommand = languageFormat.DelimiterFormat;
            DelimiterSet = false;
        }
        public string CommandDefaultFormat { get => escapeCommand; }
        public bool IsDelimiter(string word) => word.ToLower() == escapeCommand;

        public bool DelimiterSet { get; set; }
        public bool DelimiterConsumed { get; private set; }
        public void TryConsumeDelimiter()
        {
            DelimiterConsumed = false;

            if (DelimiterSet)
            {
                DelimiterSet = false;
                DelimiterConsumed = true;
            }
        }
    }
}
