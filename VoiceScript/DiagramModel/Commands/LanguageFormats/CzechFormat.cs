using System.Collections.Generic;

using VoiceScript.DiagramModel.Components;

namespace VoiceScript.DiagramModel.Commands.LanguageFormats
{
    public class CzechFormat : LanguageFormat
    {
        static readonly string code = "cs-CZ";
        /// <summary>
        /// Key is default command format, values are all possible formats.
        /// </summary>
        static readonly Dictionary<string, List<string>> commandFormats = new()
        {
            { AddCommand.DefaultFormat, new() { "přidej", "vlož", "připoj", "vytvoř" } },
            { EditCommand.DefaultFormat, new() { "uprav", "oprav", "změň", "edituj" } },
            { DeleteCommand.DefaultFormat, new() { "smaž", "vymaž", "odstraň", "vystřihni" } }
        };

        static readonly Dictionary<string, string> componentNames = new()
        {
            { "diagram", Diagram.TypeName },
            { "třídu", Class.TypeName},
            { "člen", Field.TypeName },
            { "metodu", Method.TypeName },
            { "druh", Type.TypeName },
            { "parametr", Parameter.TypeName },
            { "povinnost", Required.TypeName },
            { "viditelnost", Visibility.TypeName }
        };

        static readonly Dictionary<string, string> boolValues = new()
        {
            { "pravda", "true" },
            { "ano", "true" },
            { "nepravda", "false" },
            { "ne", "false" },
        };

        static readonly string delimiterFormat = "přepni";

        public override Dictionary<string, List<string>> CommandFormats { get => commandFormats; }
        public override Dictionary<string, string> ComponentNames => componentNames;
        public override string DelimiterFormat => delimiterFormat;
        public static string GetCode() => code;
        public override string Code { get => code; }
        public override string ComponentNameFormat { get => "jméno"; }

        public override Dictionary<string, string> BoolValues { get => boolValues; }
    }
}
