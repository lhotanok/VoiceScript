using System;
using System.Collections.Generic;

namespace VoiceScript.DiagramModel.Commands
{
    public class CommandFactory
    {
        static readonly Dictionary<string, Func<string, string, string, Command>> commandCtors = new()
        {
            { AddCommand.DefaultFormat, (name, targetType, targetValue) => new AddCommand(name, targetType, targetValue) },
            { EditCommand.DefaultFormat, (name, targetType, targetValue) => new EditCommand(name, targetType, targetValue) },
            { DeleteCommand.DefaultFormat, (name, targetType, targetValue) => new DeleteCommand(name, targetType, targetValue) }
        };
        public static bool CanCreateCommand(string commandName) => commandCtors.ContainsKey(commandName);
        public static Func<string, string, string, Command> GetCommandCtor(string commandName) => commandCtors[commandName];
    }
}
