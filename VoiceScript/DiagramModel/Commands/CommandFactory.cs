using System;
using System.Collections.Generic;
using VoiceScript.DiagramModel.Commands.LanguageFormats;

namespace VoiceScript.DiagramModel.Commands
{
    public class CommandFactory
    {
        static readonly Dictionary<string, Func<string, string, string, LanguageFormat, Command>> commandCtors = new()
        {
            { AddCommand.DefaultFormat, (name, targetType, targetValue, language) => new AddCommand(name, targetType, targetValue, language) },
            { EditCommand.DefaultFormat, (name, targetType, targetValue, language) => new EditCommand(name, targetType, targetValue, language) },
            { DeleteCommand.DefaultFormat, (name, targetType, targetValue, language) => new DeleteCommand(name, targetType, targetValue, language) }
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
                        return commandCtors[command.Key](commandName, targetType, targetValue, language);
                    }
                }
            }

            return null;
        }
    }
}
