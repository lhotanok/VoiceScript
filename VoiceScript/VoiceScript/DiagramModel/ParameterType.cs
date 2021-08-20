namespace VoiceScript.DiagramModel
{
    class ParameterType : Type
    {
        readonly static string defaultName = "object";
        public ParameterType(Component parent) : base(defaultName, parent) { }
        public ParameterType(string name, Component parent) : base(name, parent) { }

        public override string GetDefaultName() => defaultName;
    }
}
