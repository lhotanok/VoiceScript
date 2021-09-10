using NUnit.Framework;
using DiagramModel.Commands;

namespace DiagramModel_UnitTests.CommandExecutionTests
{
    [TestFixture]
    public class SingleEditCommand : CommandExecution
    {
        [TestCase("add class person edit class person")]
        [TestCase("add class person add field name add method get name edit class person")]
        public void ExecuteEditClassCommand_CheckThatClassContextWasEntered(string inputText)
        {
            ExecuteCommandsManually(inputText);

            Assert.AreEqual(context.TargetComponent, diagram.GetClasses()[0]);
        }

        [TestCase("add class person edit name employee")]
        [TestCase("add class person edit Name employee")]
        public void ExecuteEditNameCommand_CheckThatNameHasChanged(string inputText)
        {
            diagram.ConvertTextToDiagram(inputText);

            Assert.AreEqual("Employee", diagram.GetClasses()[0].Name);
        }

        [TestCase("edit class person")]
        [TestCase("edit field name")]
        [TestCase("add class person edit field name")]
        [TestCase("edit undefined name")]
        [TestCase("add class person add field name add type string add method get name add visibility public edit type string")]
        public void TryExecuteEditCommandWithInvalidTargetTypeInCurrentContext_CheckThatExceptionIsThrown(string inputText)
        {
            Assert.Throws<CommandExecutionException>(() => diagram.ConvertTextToDiagram(inputText));
        }
    }
}
