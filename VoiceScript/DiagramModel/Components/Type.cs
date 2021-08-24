using System;
using System.Collections.Generic;

namespace VoiceScript.DiagramModel.Components
{
    public class Type : Component
    {
        readonly static List<string> validChildTypes = new();
        readonly static string typeName = "type";

        protected string defaultName = "object";
        public static string TypeName { get => typeName; }

        public Type(string name, Component parent) : base(name, parent, validChildTypes) { }

        public string DefaultName { get => defaultName; }
        public override string GetUniqueTypeName() => typeName;
        public override Component Clone()
        {
            var clone = new Type(Name, Parent);
            CloneChildrenInto(clone);

            return clone;
        }
    }
}
