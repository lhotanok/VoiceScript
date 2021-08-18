using System.Collections.Generic;

namespace VoiceScript.DiagramModel
{
    class Parameter : Component
    {
        readonly static List<string> validChildTypes = new() { VariableType.TypeName, Required.TypeName };
        public Parameter(string name, Component parent) : base(name, parent, validChildTypes)
        {
            // set default values
            children.Add(new VariableType(this));
            children.Add(new Required(this));
        }
        public static string TypeName { get => nameof(Parameter).ToLower(); }
        public VariableType Type { get => GetTypeFilteredChildren<VariableType>()[0]; }

        public Required IsRequired() => GetTypeFilteredChildren<Required>()[0];
        public override string GetTypeName() => TypeName;
    }
}
