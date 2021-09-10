using System.Drawing;

namespace VoiceScript.CommandDesign
{
    public class CommandColor
    {
        static readonly Color name = Color.DodgerBlue;
        static readonly Color targetType = Color.Orchid;
        static readonly Color targetValue = Color.Teal;

        public static Color NameColor { get => name; }
        public static Color TargetTypeColor { get => targetType; }
        public static Color TargetValueColor { get => targetValue; }
    }

}
