namespace VoiceScript.DiagramModel
{
    class ReturnType : Type
    {
        public ReturnType(Component parent) : this(defaultName, parent) { }
        public ReturnType(string name, Component parent) : base(name, parent)
        {
            defaultName = "void";
        }
    }
}
