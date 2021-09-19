using System;
using System.Collections.Generic;
using DiagramModel.Commands.LanguageFormats;

namespace DiagramModel.Commands
{
    /// <summary>
    /// Provides interface for creating parsed commands
    /// based on their name used in text command.
    /// </summary>
    public class CommandFactory
    {
        static readonly Dictionary<string, Func<string, string, string, LanguageFormat, Command>> commandCtors = new()
        {
            { AddCommand.DefaultFormat, (name, targetType, targetValue, language) => new AddCommand(name, targetType, targetValue, language) },
            { EditCommand.DefaultFormat, (name, targetType, targetValue, language) => new EditCommand(name, targetType, targetValue, language) },
            { DeleteCommand.DefaultFormat, (name, targetType, targetValue, language) => new DeleteCommand(name, targetType, targetValue, language) }
        };

        /// <summary>
        /// Creates parsed commands from the provided command name, target type and value.
        /// Supports alternative languages that specify various command names. Checks all
        /// possible values of command name and constructs command type corresponding to 
        /// the given name.
        /// </summary>
        /// <param name="commandName">For valid values see <see cref="LanguageFormat.CommandFormats"/>.</param>
        /// <param name="targetType">For valid values see <see cref="LanguageFormat.ComponentNames"/>.</param>
        /// <param name="targetValue">Target component name.</param>
        /// <param name="language">Current language in which the command should be parsed. 
        /// It is needed to properly map commandName on the specific command type.</param>
        /// <returns>Parsed command or null if command can not be created.</returns>
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
