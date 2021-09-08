using System.Collections.Generic;

namespace VoiceScript.DiagramModel.Components
{
    public class Required : Component
    {
        readonly static List<string> validChildTypes = new();
        readonly static string defaultName = "true";
        public Required(Component parent) : this(defaultName, parent) { }
        public Required(string name, Component parent) : base(name, parent, validChildTypes) { }
        public static string TypeName { get => nameof(Required).ToLower(); }

        public bool Value { get; private set; }
        public override string Name { get => base.Name; set => SetValue(value); }
        public static string DefaultName { get => defaultName; }
        public override string GetUniqueTypeName() => TypeName;

        void SetValue(string value)
        {
            base.Name = value;
            Value = value.ToLower() == "true";
        }
        public override Component Clone()
        {
            var clone = new Required(Name, Parent);
            CloneChildrenInto(clone);

            return clone;
        }
    }
}
