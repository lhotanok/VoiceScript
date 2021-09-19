using System.Collections.Generic;

namespace DiagramModel.Commands.LanguageFormats
{
    /// <summary>
    /// Represents language settings for conversion of commands into diagram model.
    /// </summary>
    public abstract class LanguageFormat
    {
        /// <summary>
        /// Language standardized code.
        /// </summary>
        public abstract string Code { get; }

        /// <summary>
        /// Represents mapping of default command name on alternative command names.
        /// Key is represented by command's default format (see <see cref="AddCommand.DefaultFormat"/>,
        /// <see cref="EditCommand.DefaultFormat"/> and <see cref="DeleteCommand.DefaultFormat"/>).
        /// </summary>
        public abstract Dictionary<string, List<string>> CommandFormats { get; }

        /// <summary>
        /// Format of delimiter command.
        /// </summary>
        public abstract string DelimiterFormat { get; }

        /// <summary>
        /// Represents component name for accessing <see cref="Components.Component.Name"/>
        /// from commands. It is needed for edit name command. Is "name" by default.
        /// </summary>
        public virtual string ComponentNameFormat { get => "name"; }

        /// <summary>
        /// Various component names mapped on default component names.
        /// Keys are valid component names that can be used for component 
        /// representation in the individual commands and values are default
        /// component names corresponding to key names.
        /// </summary>
        public abstract Dictionary<string, string> ComponentNames { get; }

        /// <summary>
        /// Special values used in target value parts of the command that should
        /// be replaced during command parsing phase. The original values get
        /// replaced by these values and they are no longer accessible from 
        /// the parsed commands. Keys stand for the actual values used in commands
        /// and values are replacement strings.
        /// </summary>
        public virtual Dictionary<string, string> TargetValuesToReplace { get => new(); }

        /// <summary>
        /// Get list of possible command formats.
        /// </summary>
        /// <param name="commandDefault"></param>
        /// <returns>List of formats or null
        /// if command's default format is unknown.</returns>
        public List<string> GetFormats(string commandDefault)
        {
            return CommandFormats.ContainsKey(commandDefault)
                    ? CommandFormats[commandDefault]
                    : null;
        }

        /// <summary>
        /// Extracts all possible command names that can be used
        /// for commands representation. Merges all command types.
        /// </summary>
        /// <returns></returns>
        public IList<string> GetAllCommandFormats()
        {
            var commandFormats = new List<string>();

            var commandFormatValues = CommandFormats.Values;

            foreach (var commandFormat in commandFormatValues)
            {
                commandFormats.AddRange(commandFormat);
            }

            return commandFormats;
        }
    }
}
