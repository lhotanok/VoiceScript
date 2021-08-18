using System.Collections.Generic;

namespace VoiceScript.DiagramModel
{
    class ReturnType : Component
    {
        public readonly static List<string> ValidChildTypes = new();
        readonly static string defaultName = "void";
        public ReturnType(Component parent) : this(defaultName, parent) { }
        public ReturnType(string name, Component parent) : base(name, parent, ValidChildTypes) { }
        public static string TypeName { get => "return"; }

        public static string DefaultName { get => defaultName; }
        public override string GetTypeName() => TypeName;
    }
}
