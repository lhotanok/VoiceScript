using System.Collections.Generic;

namespace VoiceScript.DiagramModel
{
    class Required : Component
    {
        readonly static List<string> validChildTypes = new();
        readonly static string defaultName = "false";
        public Required(Component parent) : base(defaultName, parent, validChildTypes) { }
        public static string TypeName { get => nameof(Required).ToLower(); }

        public bool Value { get; private set; }
        public override string Name { get => base.Name; protected set => SetValue(value); }
        public static string DefaultName { get => defaultName; }
        public override string GetTypeName() => TypeName;

        void SetValue(string value)
        {
            base.Name = value;
            Value = value == "true";
        }
    }
}
