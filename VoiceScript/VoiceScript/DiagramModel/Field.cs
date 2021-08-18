using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceScript.DiagramModel
{
    class Field : Component
    {
        public static List<string> ValidChildTypes = new() { Visibility.TypeName, VariableType.TypeName };
        public Field(string name, Component parent) : base(name, parent, ValidChildTypes)
        {
            children.Add(new VariableType(this));
            children.Add(new Visibility(this));
        }
        public static string TypeName { get => nameof(Field).ToLower(); }
        public Visibility Visibility { get => GetTypeFilteredChildren<Visibility>()[0]; }

        public VariableType Type { get => GetTypeFilteredChildren<VariableType>()[0]; }
        public override string GetTypeName() => TypeName;
    }
}
