using System.Collections.Generic;

using VoiceScript.DiagramModel.Components;

namespace VoiceScript.DiagramModel.Commands.LanguageFormats
{
    public abstract class LanguageFormat
    {
        readonly Dictionary<string, string> componentNames = new()
        {
            { Diagram.TypeName, Diagram.TypeName },
            { Class.TypeName, Class.TypeName },
            { Field.TypeName, Field.TypeName },
            { Method.TypeName, Method.TypeName },
            { Type.TypeName, Type.TypeName },
            { Parameter.TypeName, Parameter.TypeName },
            { Required.TypeName, Required.TypeName },
            { Visibility.TypeName, Visibility.TypeName }
        };
        public abstract string Code { get; }
        public abstract Dictionary<string, List<string>> CommandFormats { get; }
        public abstract string DelimiterFormat { get; }
        public abstract string ComponentNameFormat { get; }
        public abstract Dictionary<string, string> BoolValues { get; }
        public virtual Dictionary<string, string> ComponentNames { get => componentNames; }

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
