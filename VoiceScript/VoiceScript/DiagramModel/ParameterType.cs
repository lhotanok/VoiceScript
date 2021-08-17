namespace VoiceScript.DiagramModel
{
    class ParameterType : IType
    {
        readonly static string defaultName = "object";
        public ParameterType() : this(defaultName) { }
        public ParameterType(string name)
        {
            Name = name;
            ContainsComponents = false;
        }
        public string Name { get; private set; }

        public string DefaultName { get; }

        public bool ContainsComponents { get; }
    }
}
