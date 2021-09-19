using System.Collections.Generic;

namespace DiagramModel.Components
{
    public class Required : Component
    {
        readonly static List<string> validChildTypes = new();
        readonly static string defaultName = "true";
        public Required(Component parent) : this(defaultName, parent) { }
        public Required(string name, Component parent) : base(name.ToLower(), parent, validChildTypes) { }
        public static string TypeName { get => nameof(Required).ToLower(); }

        public bool Value { get => Name.ToLower() == "true"; }
        public override string Name { get => base.Name; set => base.Name = value.ToLower(); }
        public static string DefaultName { get => defaultName; }
        public override string UniqueTypeName { get => TypeName; }

        public override Component Clone()
        {
            var clone = new Required(Name, Parent);
            CloneChildrenInto(clone);

            return clone;
        }
    }
}
