namespace VoiceScript.DiagramModel.Commands
{
    class DelimiterWrapper
    {
        readonly static string command = "escape";
        bool delimiterSet, delimiterPreviouslySet;
        public DelimiterWrapper()
        {
            (delimiterSet, delimiterPreviouslySet) = (false, false);
        }
        public static string CommandFormat { get => command; }
        public static bool IsDelimiter(string word) => word.ToLower() == command;

        public bool DelimiterSet {
            get => delimiterSet;

            private set {
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
