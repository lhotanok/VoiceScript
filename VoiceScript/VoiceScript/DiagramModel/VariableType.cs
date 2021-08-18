namespace VoiceScript.DiagramModel
{
    class VariableType : Component
    {
        readonly static string defaultName = "object";
        public VariableType(Component parent) : base(defaultName, parent) { }
        public VariableType(string name, Component parent) : base(name, parent) { }

        public string DefaultName { get => defaultName; }
        public override string TypeName { get => GetType().Name; }
    }
}
