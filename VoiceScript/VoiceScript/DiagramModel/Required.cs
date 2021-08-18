namespace VoiceScript.DiagramModel
{
    class Required : Component
    {
        readonly static string defaultName = "false";
        public Required(Component parent) : base(defaultName, parent) { }
        public bool Value { get; private set; }
        public override string Name { get => base.Name; protected set => SetValue(value); }
        public override string TypeName { get => GetType().Name; }

        void SetValue(string value)
        {
            base.Name = value;
            Value = value == "true";
        }
    }
}
