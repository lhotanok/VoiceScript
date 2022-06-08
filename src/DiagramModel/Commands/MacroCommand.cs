using DiagramModel.Commands.LanguageFormats;
using DiagramModel.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagramModel.Commands
{
    public class MacroCommand : ICommand
    {
        private readonly IEnumerable<ICommand> commands;
        public MacroCommand(IEnumerable<ICommand> commands)
        {
            this.commands = commands;
        }
        public void Execute(CommandExecutionContext context)
        {
            var currentCommandNumber = 1;

            try
            {
                foreach (var command in commands)
                {
                    context.Initialize();
                    command.Execute(context);

                    currentCommandNumber++;
                }
            }
            catch (Exception ex)
            {
                throw new CommandExecutionException($"Error while executing {currentCommandNumber}. command.\n\n" + ex.Message,
                    currentCommandNumber);
            }
        }

        public void Undo()
        {
            foreach (var command in commands.Reverse())
            {
                command.Undo();
            }
        }
    }
}
