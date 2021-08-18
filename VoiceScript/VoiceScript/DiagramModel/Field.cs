using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceScript.DiagramModel
{
    class Field : Component
    {
        public Field(string name, Component parent) : base(name, parent)
        {
            children.Add(new VariableType(this));
            children.Add(new Visibility(this));
        }
        public Visibility Visibility { get => GetFilteredChildren<Visibility>()[0]; }

        public VariableType Type { get => GetFilteredChildren<VariableType>()[0]; }
        public override string TypeName { get => GetType().Name; }
    }
}
