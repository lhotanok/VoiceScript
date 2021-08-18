using System.Collections.Generic;

namespace VoiceScript.DiagramModel
{
    class Visibility : Component
    {
        readonly static List<string> validChildTypes = new();
        readonly List<string> validNames;
        readonly static string defaultName = "public";
        public Visibility(Component parent) : this(defaultName, parent) { }
        public Visibility(string name, Component parent) : base(name, parent, validChildTypes)
        {
            validNames = new List<string>() { "public", "private", "protected", "internal" };
        }
        public static string TypeName { get => nameof(Visibility).ToLower(); }

        public static string DefaultName { get => defaultName; }

        public IEnumerable<string> ValidNames { get => validNames; }

        public override string GetTypeName() => TypeName;
    }
}
