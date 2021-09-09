using System;
using System.Collections.Generic;
using VoiceScript.DiagramModel.Commands.LanguageFormats;

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

        public static Command CreateCommand(string commandName, string targetType, string targetValue, LanguageFormat language)
        {
            var possibleCommands = language.CommandFormats;

            foreach (var command in possibleCommands)
            {
                if (commandCtors.ContainsKey(command.Key))
                {
                    if (command.Value.Contains(commandName))
                    {
                        return commandCtors[command.Key](commandName, targetType, targetValue);
                    }
                }
            }

            return null;
        }
    }
}
