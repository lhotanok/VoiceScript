namespace VoiceScript.DiagramModel
{
    class ParameterType : Type
    {
        static readonly string defName = "object";
        public ParameterType(Component parent) : this(defName, parent) { }
        public ParameterType(string name, Component parent) : base(name, parent)
        {
            defaultName = defName;
        }
    }
}
