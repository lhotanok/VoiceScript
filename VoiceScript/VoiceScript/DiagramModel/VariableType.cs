using System.Collections.Generic;

namespace VoiceScript.DiagramModel
{
    class VariableType : Component
    {
        readonly static List<string> validChildTypes = new();
        readonly static string defaultName = "object";
        public VariableType(Component parent) : this(defaultName, parent) { }
        public static string TypeName { get => "type"; }

        public VariableType(string name, Component parent) : base(name, parent, validChildTypes) { }

        public static string DefaultName { get => defaultName; }
        public override string GetTypeName() => TypeName;
    }
}
