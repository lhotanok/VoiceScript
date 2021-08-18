namespace VoiceScript.DiagramModel
{
    class ReturnType : Component
    {
        readonly static string defaultName = "void";
        public ReturnType(Component parent) : base(defaultName, parent) { }
        public ReturnType(string name, Component parent) : base(name, parent) { }
        public string DefaultName { get; }
        public override string TypeName { get => GetType().Name; }
    }
}
