using System;
using System.Collections.Generic;

namespace VoiceScript.DiagramModel
{
    class Class : Component
    {
        public static List<string> ValidChildTypes = new() { Field.TypeName, Method.TypeName };
        public Class(string name, Component parent) : base(name, parent, ValidChildTypes ) { }
        public static string TypeName { get => nameof(Class).ToLower(); }

        public IEnumerable<Field> GetFields() => GetTypeFilteredChildren<Field>();

        public IEnumerable<Method> GetMethods() => GetTypeFilteredChildren<Method>();

        public override string GetTypeName() => TypeName;

    }
}
