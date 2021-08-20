namespace VoiceScript.DiagramModel
{
    class FieldType : Type
    {
        public FieldType(Component parent) : this(defaultName, parent) { }
        public FieldType(string name, Component parent) : base(name, parent)
        {
            defaultName = "object";
        }
    }
}
