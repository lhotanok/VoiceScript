using System;
using System.Collections.Generic;

namespace VoiceScript.DiagramModel.Commands
{
    public class CommandFactory
    {
        static readonly Dictionary<string, Func<string, string, Command>> commandCtors = new()
        {
            { AddCommand.DefaultFormat, (targetType, targetValue) => new AddCommand(targetType, targetValue) },
            { EditCommand.DefaultFormat, (targetType, targetValue) => new EditCommand(targetType, targetValue) },
            { DeleteCommand.DefaultFormat, (targetType, targetValue) => new DeleteCommand(targetType, targetValue) }
        };
        public static bool CanCreateCommand(string commandName) => commandCtors.ContainsKey(commandName);
        public static Func<string, string, Command> GetCommandCtor(string commandName) => commandCtors[commandName];
    }
}
