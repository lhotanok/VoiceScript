using System;
using System.Collections.Generic;

namespace VoiceScript.DiagramModel.Commands.LanguageFormats
{
    public class EnglishFormat : LanguageFormat
    {
        readonly static string code = "en-US";

        /// <summary>
        /// Key is default command format, values are all possible formats.
        /// </summary>
        static readonly Dictionary<string, List<string>> commandFormats = new()
        {
            { AddCommand.DefaultFormat, new() { AddCommand.DefaultFormat, "attach", "annex", "insert" } },
            { EditCommand.DefaultFormat, new() { EditCommand.DefaultFormat, "change", "modify", "correct" } },
            { DeleteCommand.DefaultFormat, new() { DeleteCommand.DefaultFormat, "erase", "cut", "remove" } }
        };

        public override Dictionary<string, List<string>> CommandFormats { get => commandFormats; }

        public override string DelimiterFormat => DelimiterWrapper.CommandDefaultFormat;

        public static string GetCode() => code;
        public override string Code { get => code; }
    }
}
