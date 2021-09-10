namespace DiagramModel.Components
{
    public class FieldType : Type
    {
        public FieldType(string name, Component parent) : base(name, parent) { }
        public override Component Clone()
        {
            var clone = new FieldType(Name, Parent);
            CloneChildrenInto(clone);

            return clone;
        }
    }
}
