namespace DiagramModel.Components
{
    public class ReturnType : Type
    {
        public ReturnType(string name, Component parent) : base(name, parent) { }
        public override Component Clone()
        {
            var clone = new ReturnType(Name, Parent);
            CloneChildrenInto(clone);

            return clone;
        }
        public override string Name { get => base.Name; set => base.Name = value.ToLower() == "void" ? "void" : value; }
    }
}
