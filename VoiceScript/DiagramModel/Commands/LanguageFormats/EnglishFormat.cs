using System;
using System.Collections.Generic;

namespace VoiceScript.DiagramModel.Commands.LanguageFormats
{
    class EnglishFormat : Format
    {
        /// <summary>
        /// Key is default command format, values are all possible formats.
        /// </summary>
        static readonly Dictionary<string, List<string>> commandFormats = new()
        {
            { AddCommand.DefaultFormat, new() { AddCommand.DefaultFormat, "attach", "annex" } },
            { EditCommand.DefaultFormat, new() { EditCommand.DefaultFormat, "change", "modify", "correct" } },
            { DeleteCommand.DefaultFormat, new() { DeleteCommand.DefaultFormat, "erase", "cut" } }
        };

        public override Dictionary<string, List<string>> CommandFormats { get => commandFormats; }

        public override string DelimiterFormat => DelimiterWrapper.CommandDefaultFormat;
    }
}
