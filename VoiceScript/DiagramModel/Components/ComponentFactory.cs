using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceScript.DiagramModel.Components
{
    class ComponentFactory
    {
        static readonly Dictionary<string, Func<string, Component, Component>> componentCtors = new()
        {
            { Diagram.TypeName, (childName, parent) => new Diagram(childName, null) },
            { Class.TypeName, (childName, parent) => new Class(childName, parent) },
            { Field.TypeName, (childName, parent) => new Field(childName, parent) },
            { Method.TypeName, (childName, parent) => new Method(childName, parent) },
            { Parameter.TypeName, (childName, parent) => new Parameter(childName, parent) },
            { Required.TypeName, (childName, parent) => new Required(childName, parent) },
            { Type.TypeName, (childName, parent) => new Type(childName, parent) },
            { Visibility.TypeName, (childName, parent) => new Visibility(childName, parent) }
        };
        public static bool CanCreateComponent(string componentType) => componentCtors.ContainsKey(componentType);
        public static Func<string, Component, Component> GetComponentCtor(string componentType) => componentCtors[componentType];
    }
}
