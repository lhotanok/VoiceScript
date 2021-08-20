namespace VoiceScript.DiagramModel
{
    class ParameterType : Type
    {
        public ParameterType(Component parent) : this(defaultName, parent) { }
        public ParameterType(string name, Component parent) : base(name, parent)
        {
            defaultName = "object";
        }
    }
}
