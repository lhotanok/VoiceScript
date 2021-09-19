using System;
using System.Collections.Generic;

namespace DiagramModel.Commands
{
    /// <summary>
    /// Thrown if an error is detected during command parsing phase.
    /// </summary>
    public class CommandParseException : InvalidOperationException
    {
        public CommandParseException(string message, List<Command> parsedCommands, List<string> unparsedWords) : base(message)
        {
            ParsedCommands = parsedCommands;
            UnparsedWords = unparsedWords;
        }

        public CommandParseException(string message) : this(message, new List<Command>(), new List<string>()) { }

        /// <summary>
        /// Contains successfully parsed commands. Can be used to show
        /// which commands got parsed before a parsing error occurred. 
        /// </summary>
        public List<Command> ParsedCommands { get; }

        /// <summary>
        /// Contains collection of words that remained unparsed into commands.
        /// It is designed to be used with <see cref="ParsedCommands"/>. 
        /// Successfully parsed commands might be shown to the user compiled already
        /// and the unparsed words might be placed below these compiled commands
        /// to emphasise the place where an error was detected. 
        /// </summary>
        public List<string> UnparsedWords { get; }
    }
}
