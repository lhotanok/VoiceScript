using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceScript.DiagramModel
{
    class ComponentFactory
    {
        static readonly Dictionary<string, Func<string, Component, Component>> componentCtors = new()
        {
            { Class.TypeName, (childName, parent) => new Class(childName, parent) },
            { Field.TypeName, (childName, parent) => new Field(childName, parent) },
            { Method.TypeName, (childName, parent) => new Method(childName, parent) },
            { Parameter.TypeName, (childName, parent) => new Parameter(childName, parent) },
            { Required.TypeName, (childName, parent) => new Required(parent) },
            { FieldType.TypeName, (childName, parent) => new FieldType(childName, parent) },
            { Visibility.TypeName, (childName, parent) => new Visibility(childName, parent) }
        };
        public static bool CanCreateComponent(string componentType) => componentCtors.ContainsKey(componentType);
        public static Func<string, Component, Component> GetComponentCtor(string componentType) => componentCtors[componentType];
    }
}
