namespace VoiceScript.DiagramModel
{
    class FieldType : Type
    {
        static readonly string defName = "object";
        public FieldType(Component parent) : this(defName, parent) { }
        public FieldType(string name, Component parent) : base(name, parent)
        {
            defaultName = defName;
        }
    }
}
