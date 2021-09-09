﻿using System;
using System.Collections.Generic;
using VoiceScript.DiagramModel.Components;

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

        readonly Dictionary<string, string> componentNames = new()
        {
            { Diagram.TypeName, Diagram.TypeName },
            { Class.TypeName, Class.TypeName },

            { Field.TypeName, Field.TypeName },
            { "member", Field.TypeName },
            { "attribute", Field.TypeName },

            { Method.TypeName, Method.TypeName },
            { "function", Method.TypeName },

            { Components.Type.TypeName, Components.Type.TypeName },
            { Parameter.TypeName, Parameter.TypeName },

            { Required.TypeName, Required.TypeName },
            { "mandatory", Required.TypeName },

            { Visibility.TypeName, Visibility.TypeName },
            { "protection", Visibility.TypeName }
        };

        static readonly Dictionary<string, string> constantValues = new()
        {
            { "true", "true" },
            { "yes", "true" },
            { "false", "false" },
            { "no", "false" },
        };

        public override Dictionary<string, List<string>> CommandFormats { get => commandFormats; }

        public override Dictionary<string, string> ComponentNames => componentNames;

        public override string DelimiterFormat => "escape";

        public static string GetCode() => code;
        public override string Code { get => code; }

        public override string ComponentNameFormat { get => "name"; }

        public override Dictionary<string, string> ValueConstants { get => constantValues; }
    }
}
