namespace DiagramModel.Components
{
    public class ParameterType : Type
    {
        public ParameterType(string name, Component parent) : base(name, parent) { }
        public override Component Clone()
        {
            var clone = new ParameterType(Name, Parent);
            CloneChildrenInto(clone);

            return clone;
        }
    }
}
