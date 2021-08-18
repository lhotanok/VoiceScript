using System.Collections.Generic;

namespace VoiceScript.DiagramModel
{
    class ReturnType : Component
    {
        readonly static List<string> validChildTypes = new();
        readonly static string defaultName = "void";
        public ReturnType(Component parent) : this(defaultName, parent) { }
        public ReturnType(string name, Component parent) : base(name, parent, validChildTypes) { }
        public static string TypeName { get => "return"; }

        public static string DefaultName { get => defaultName; }
        public override string GetTypeName() => TypeName;
    }
}
