using System.Drawing;

namespace CodeGeneration
{
    /// <summary>
    /// Wrapper for colors of individual components in C# code.
    /// </summary>
    public class CSharpCodeColor : ICodeColor
    {
        static readonly Color keyword = Color.CornflowerBlue;
        static readonly Color throwKeyword = Color.Plum;
        static readonly Color classType = Color.MediumAquamarine;
        static readonly Color method = Color.PeachPuff;
        static readonly Color parameter = Color.SkyBlue;
        static readonly Color defaultText = Color.WhiteSmoke;

        public static Color KeywordColor { get => keyword; }
        public static Color ThrowKeywordColor { get => throwKeyword; }
        public static Color ClassTypeColor { get => classType; }
        public static Color MethodColor { get => method; }
        public static Color ParameterColor { get => parameter; }
        public static Color Default { get => defaultText; }
    }

}
