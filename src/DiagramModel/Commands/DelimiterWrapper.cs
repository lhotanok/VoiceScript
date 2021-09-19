using DiagramModel.Commands.LanguageFormats;

namespace DiagramModel.Commands
{
    /// <summary>
    /// Represents current state of the delimiter during
    /// the command parsing process. To be used while parsing
    /// target name as it can consist of multiple words (command
    /// name and target type are always 1-word only).
    /// </summary>
    class DelimiterWrapper
    {
        readonly string escapeCommand;
        public DelimiterWrapper(LanguageFormat languageFormat)
        {
            escapeCommand = languageFormat.DelimiterFormat;
            DelimiterSet = false;
        }

        /// <summary>
        /// Holds the name of the delimiter command with
        /// respect to the current language context.
        /// </summary>
        public string CommandFormat { get => escapeCommand; }

        /// <summary>
        /// Checks whether the given word is an escape command.
        /// Handles both upper case and lower case formats or 
        /// their combination.
        /// </summary>
        /// <param name="word">String to check against delimiter command name.</param>
        /// <returns>The result of word and delimiter command name comparison.</returns>
        public bool IsDelimiter(string word) => word.ToLower() == escapeCommand;

        /// <summary>
        /// Specifies whether a delimiter is currently set or not.
        /// </summary>
        public bool DelimiterSet { get; set; }

        /// <summary>
        /// Specifies whether a delimiter was consumed.
        /// Holds true if delimiter was set during command parsing
        /// and the word following the delimiter command still belongs
        /// to the currently parsed command.
        /// </summary>
        public bool DelimiterConsumed { get; private set; }

        /// <summary>
        /// Updates delimiter context for the newly parsed word.
        /// </summary>
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
