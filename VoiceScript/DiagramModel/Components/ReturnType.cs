namespace VoiceScript.DiagramModel.Components
{
    public class ReturnType : Type
    {
        static readonly string defName = "void";
        public ReturnType(Component parent) : this(defName, parent) { }
        public ReturnType(string name, Component parent) : base(name, parent)
        {
            defaultName = defName;
        }
        public override Component Clone()
        {
            var clone = new ReturnType(Name, Parent);
            CloneChildrenInto(clone);

            return clone;
        }
        public static new string DefaultName => defName;

        public override string Name { get => base.Name; set => base.Name = value.ToLower() == "void" ? "void" : value; }
    }
}
