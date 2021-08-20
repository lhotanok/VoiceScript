using System;
using System.Collections.Generic;

namespace VoiceScript.DiagramModel
{
    abstract class Type : Component
    {
        protected static string defaultName = "object";
        readonly static List<string> validChildTypes = new();
        public static string TypeName { get => "type"; }

        public Type(string name, Component parent) : base(name, parent, validChildTypes) { }

        public static string DefaultName { get => defaultName; }
        public override string GetTypeName() => TypeName;
    }
}
