using System;
using System.Collections.Generic;
using System.Drawing;

namespace VoiceScript.CodeGeneration
{
    class CodeColor
    {
        static readonly Color keyword = Color.CornflowerBlue;
        static readonly Color type = Color.MediumAquamarine;
        static readonly Color method = Color.PeachPuff;
        static readonly Color parameter = Color.PowderBlue;
        static readonly Color defaultText = Color.WhiteSmoke;

        public static Color KeywordColor { get => keyword; }
        public static Color TypeColor { get => type; }
        public static Color MethodColor { get => method; }
        public static Color ParameterColor { get => parameter; }
        public static Color Default { get => defaultText; }
    }

}
