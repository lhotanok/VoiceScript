namespace VoiceScript.DiagramModel
{
    class FieldType : Type
    {
        readonly static string defaultName = "object";
        public FieldType(Component parent) : base(defaultName, parent) { }
        public FieldType(string name, Component parent) : base(name, parent) { }

        public override string GetDefaultName() => defaultName;
    }
}
