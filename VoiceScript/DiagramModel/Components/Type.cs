using System;
using System.Collections.Generic;

namespace VoiceScript.DiagramModel.Components
{
    public class Type : Component
    {
        readonly static List<string> validChildTypes = new();
        readonly static string typeName = "type";
        public static string TypeName { get => typeName; }

        public Type(string name, Component parent) : base(name, parent, validChildTypes)
        {
            if (TryParseArray(name, out string arrayFormatName))
            {
                Name = arrayFormatName;
            }
        }

        public override string GetUniqueTypeName() => typeName;
        public override Component Clone()
        {
            var clone = new Type(Name, Parent);
            CloneChildrenInto(clone);

            return clone;
        }

        static bool TryParseArray(string name, out string arrayFormatName)
        {
            arrayFormatName = string.Empty;
            var prefix = "array";
            var lowerName = name.ToLower();

            if (lowerName.Contains(prefix))
            {
                if (lowerName.Contains("arrayof")) prefix = "arrayof";

                var typename = name[prefix.Length..];

                arrayFormatName = typename + "[]";

                return true;
            }

            return false;
        }
    }
}
