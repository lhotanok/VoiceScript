using System.Collections.Generic;

using DiagramModel.Components;

namespace DiagramModel.Commands.LanguageFormats
{
    public class CzechFormat : LanguageFormat
    {
        static readonly string code = "cs-CZ";

        /// <summary>
        /// Key is default command format, values are all possible formats.
        /// </summary>
        static readonly Dictionary<string, List<string>> commandFormats = new()
        {
            { AddCommand.DefaultFormat, new() { AddCommand.DefaultFormat, "přidej", "vlož", "připoj", "vytvoř" } },
            { EditCommand.DefaultFormat, new() { EditCommand.DefaultFormat, "uprav", "oprav", "změň", "edituj" } },
            { DeleteCommand.DefaultFormat, new() { DeleteCommand.DefaultFormat, "smaž", "vymaž", "odstraň", "vystřihni" } }
        };

        static readonly Dictionary<string, string> componentNames = new()
        {
            { "diagram", Diagram.TypeName },

            { "třídu", Class.TypeName},

            { "člen", Field.TypeName },
            { "atribut", Field.TypeName },

            { "metodu", Method.TypeName },
            { "funkci", Method.TypeName },

            { "typ", Type.TypeName },
            { "druh", Type.TypeName },
            { "tip", Type.TypeName }, // Google's speech-to-text often chooses "tip" over "typ" in this context 

            { "parametr", Parameter.TypeName },
            { "parameter", Parameter.TypeName },

            { "povinnost", Required.TypeName },

            { "viditelnost", Visibility.TypeName },
            { "ochranu", Visibility.TypeName },

            { "rodiče", Parent.TypeName },
            { "předka", Parent.TypeName }
        };

        static readonly Dictionary<string, string> valuesToReplace = new()
        {
            { "veřejná", "public" },
            { "interní", "internal" },
            { "chráněná", "protected" },
            { "privátní", "private" },

            { "pole", "array" },

            { "pravda", "true" },
            { "ano", "true" },
            { "nepravda", "false" },
            { "lež", "false" },
            { "ne", "false" },

            { "řetězec", "string" },
        };

        static readonly string delimiterFormat = "přepni";

        public override Dictionary<string, List<string>> CommandFormats { get => commandFormats; }
        public override Dictionary<string, string> ComponentNames => componentNames;
        public override string DelimiterFormat => delimiterFormat;
        public static string GetCode() => code;
        public override string Code { get => code; }
        public override string ComponentNameFormat { get => "jméno"; }

        public override Dictionary<string, string> TargetValuesToReplace { get => valuesToReplace; }
    }
}
