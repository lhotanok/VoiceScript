namespace VoiceScript.DiagramModel
{
    class ReturnType : Type
    {
        static readonly string defName = "void";
        public ReturnType(Component parent) : this(defName, parent) { }
        public ReturnType(string name, Component parent) : base(name, parent)
        {
            defaultName = defName;
        }
    }
}
