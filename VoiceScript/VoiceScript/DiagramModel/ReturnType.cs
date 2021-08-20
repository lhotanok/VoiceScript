namespace VoiceScript.DiagramModel
{
    class ReturnType : Type
    {
        readonly static string defaultName = "void";
        public ReturnType(Component parent) : base(defaultName, parent) { }
        public ReturnType(string name, Component parent) : base(name, parent) { }

        public override string GetDefaultName() => defaultName;
    }
}
