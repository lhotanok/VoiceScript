using System;
using System.Collections.Generic;

namespace DiagramModel.Commands
{
    public class CommandParseException : InvalidOperationException
    {
        public CommandParseException(string message, List<Command> parsedCommands, List<string> unparsedWords) : base(message)
        {
            ParsedCommands = parsedCommands;
            UnparsedWords = unparsedWords;
        }

        public CommandParseException(string message) : this(message, new List<Command>(), new List<string>()) { }

        public List<Command> ParsedCommands { get; }
        public List<string> UnparsedWords { get; }
    }
}
