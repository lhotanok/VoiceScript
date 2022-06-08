namespace DiagramModel.Commands
{
    public interface ICommand
    {
        void Execute(CommandExecutionContext context);
        void Undo();
    }
}
