using System;
using System.Collections.Generic;

namespace VoiceScript.DiagramModel
{
    class Type : Component
    {
        readonly static List<string> validChildTypes = new();
        readonly static string typeName = "type";

        protected string defaultName = "object";
        public static string TypeName { get => typeName; }

        public Type(string name, Component parent) : base(name, parent, validChildTypes) { }

        public string DefaultName { get => defaultName; }
        public override string GetTypeName() => typeName;
    }
}
