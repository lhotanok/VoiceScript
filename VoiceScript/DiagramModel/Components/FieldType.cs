namespace VoiceScript.DiagramModel.Components
{
    public class FieldType : Type
    {
        static readonly string defName = "object";
        public FieldType(Component parent) : this(defName, parent) { }
        public FieldType(string name, Component parent) : base(name, parent)
        {
            defaultName = defName;
        }
        public override Component Clone()
        {
            var clone = new FieldType(Name, Parent);
            CloneChildrenInto(clone);

            return clone;
        }
    }
}
