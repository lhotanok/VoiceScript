using System.Collections.Generic;

using VoiceScript.DiagramModel.Components;

namespace VoiceScript.DiagramModel.Commands.LanguageFormats
{
    class CzechFormat : Format
    {
        /// <summary>
        /// Key is default command format, values are all possible formats.
        /// </summary>
        static readonly Dictionary<string, List<string>> commandFormats = new()
        {
            { AddCommand.DefaultFormat, new() { "přidej", "vlož", "připoj" } },
            { EditCommand.DefaultFormat, new() { "uprav", "oprav", "změň", "edituj" } },
            { DeleteCommand.DefaultFormat, new() { "smaž", "vymaž", "odstraň", "vystřihni" } }
        };

        static readonly Dictionary<string, string> componentNames = new()
        {
            { Diagram.TypeName, "diagram" },
            { Class.TypeName, "tříd"},
            { Field.TypeName, "člen" },
            { Method.TypeName, "metod" },
            { Type.TypeName, "typ" },
            { Parameter.TypeName, "parametr" },
            { Required.TypeName, "povinnost" },
            { Visibility.TypeName, "viditelnost" }
        };

        static readonly string delimiterFormat = "přepni";

        public override Dictionary<string, List<string>> CommandFormats { get => commandFormats; }
        public override Dictionary<string, string> ComponentNames => componentNames;
        public override string DelimiterFormat => delimiterFormat;
    }
}
