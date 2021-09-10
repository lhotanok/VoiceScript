using System;
using System.Collections.Generic;

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
            { Visibility.TypeName, (childName, parent) => new Visibility(childName, parent) },
            { Parent.TypeName, (childName, parent) => new Parent(childName, parent) }
        };

        public static bool CanCreateComponent(string type) => componentCtors.ContainsKey(type);

        public static Component CreateComponent(string type, string name, Component parent) => componentCtors[type](name, parent);

    }
}
