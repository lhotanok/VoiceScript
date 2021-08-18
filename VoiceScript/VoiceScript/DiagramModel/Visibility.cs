using System.Collections.Generic;

namespace VoiceScript.DiagramModel
{
    class Visibility : Component
    {
        readonly List<string> validNames;
        readonly static string defaultName = "public";
        public Visibility(Component parent) : base(defaultName, parent) { }
        public Visibility(string name, Component parent) : base(name, parent)
        {
            validNames = new List<string>() { "public", "private", "protected", "internal" };
        }

        public IEnumerable<string> ValidNames { get => validNames; }

        public string DefaultName { get => defaultName; }
        public override string TypeName { get => GetType().Name; }
    }
}
