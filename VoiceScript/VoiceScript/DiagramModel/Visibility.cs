using System.Collections.Generic;

namespace VoiceScript.DiagramModel
{
    class Visibility : IVisibility
    {
        readonly List<string> validNames;
        readonly static string defaultName = "public";
        public Visibility() : this(defaultName) { }
        public Visibility(string name)
        {
            Name = name;
            ContainsComponents = false;
            validNames = new List<string>() { "public", "private", "protected", "internal" };
        }

        public IEnumerable<string> ValidNames { get => validNames; }

        public string Name { get; private set; }
        public string DefaultName { get => defaultName; }

        public bool ContainsComponents { get; }
    }
}
