using System.Collections.Generic;

namespace VoiceScript.DiagramModel
{
    class Parameter : Component
    {
        public Parameter(string name, Component parent) : base(name, parent)
        {
            // set default values
            children.Add(new VariableType(this));
            children.Add(new Required(this));
        }

        public VariableType Type { get => GetFilteredChildren<VariableType>()[0]; }

        public Required Required { get => GetFilteredChildren<Required>()[0]; }
        public override string TypeName { get => GetType().Name; }
    }
}
