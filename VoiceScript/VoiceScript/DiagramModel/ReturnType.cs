namespace VoiceScript.DiagramModel
{
    class ReturnType : IType
    {
        readonly static string defaultName = "void";
        public ReturnType() : this(defaultName) { }
        public ReturnType(string name = "void")
        {
            Name = name;
            ContainsComponents = false;
        }

        public string Name { get; private set; }
        public string DefaultName { get; }

        public bool ContainsComponents { get; }
    }
}
