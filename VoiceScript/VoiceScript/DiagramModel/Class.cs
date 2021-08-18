using System;
using System.Collections.Generic;

namespace VoiceScript.DiagramModel
{
    class Class : Component
    {
        public Class(string name, Component parent) : base(name, parent) { }

        public IEnumerable<Field> GetFields() => GetFilteredChildren<Field>();

        public IEnumerable<Method> GetMethods() => GetFilteredChildren<Method>();


        public override string TypeName { get => GetType().Name; }

    }
}
