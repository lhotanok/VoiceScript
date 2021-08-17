namespace VoiceScript.DiagramModel
{
    interface IComponent
    {
        string Name { get; }
    }
    abstract class Component : IComponent
    {
        public string Name { get; }
        public Component(string name)
        {
            Name = name;
        }
    }
}
