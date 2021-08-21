using System;
using System.Collections.Generic;

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

        static readonly string delimiterFormat = "přepni";

        public override Dictionary<string, List<string>> CommandFormats { get => commandFormats; }
        public override string DelimiterFormat => delimiterFormat;
    }
}
