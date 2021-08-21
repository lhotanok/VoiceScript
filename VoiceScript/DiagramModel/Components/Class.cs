﻿using System;
using System.Collections.Generic;

namespace VoiceScript.DiagramModel.Components
{
    public class Class : Component
    {
        readonly static List<string> validChildTypes = new() { Field.TypeName, Method.TypeName };
        public Class(string name, Component parent) : base(name, parent, validChildTypes ) { }
        public static string TypeName { get => nameof(Class).ToLower(); }

        public IEnumerable<Field> GetFields() => GetTypeFilteredChildren<Field>();

        public IEnumerable<Method> GetMethods() => GetTypeFilteredChildren<Method>();

        public override string GetTypeName() => TypeName;
        public override Component Clone()
        {
            var clone = new Class(Name, Parent);
            CloneChildrenInto(clone);

            return clone;
        }
    }
}
