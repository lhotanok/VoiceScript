using System.Collections.Generic;

namespace DiagramModel.Components
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

        public override string UniqueTypeName { get => TypeName; }
        public override Component Clone()
        {
            var clone = new Type(Name, Parent);
            CloneChildrenInto(clone);

            return clone;
        }
        public override string Name {
            get => base.Name;
            set
            {
                if (TryParseArray(value, out string arrayFormatName))
                {
                    base.Name = arrayFormatName;
                }
                else
                {
                    base.Name = value;
                }
            }
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

        protected override bool IsNameSynonym(string synonym)
        {
            if (TryParseArray(synonym, out string arrayFormatName))
            {
                return arrayFormatName == Name;
            }
            return base.IsNameSynonym(synonym);
        }
    }
}
