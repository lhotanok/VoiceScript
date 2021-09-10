using NUnit.Framework;
using DiagramModel.Commands;

namespace DiagramModel_UnitTests.CommandExecutionTests
{
    [TestFixture]
    public class SingleDeleteCommand : CommandExecution
    {
        [TestCase("add class person delete class person")]
        [TestCase("add class person add field name add method get name delete class person")]
        public void ExecuteDeleteClassCommand_CheckThatClassWasDeleted(string inputText)
        {
            diagram.ConvertTextToDiagram(inputText);

            Assert.AreEqual(0, diagram.GetClasses().Count);
        }

        [TestCase("delete class person")]
        [TestCase("delete field name")]
        [TestCase("add class person delete field name")]
        [TestCase("delete undefined name")]
        [TestCase("add class person add field name add type string add method get name add visibility public delete type string")]
        public void TryExecuteDeleteCommandWithInvalidTargetTypeInCurrentContext_CheckThatExceptionIsThrown(string inputText)
        {
            Assert.Throws<CommandExecutionException>(() => diagram.ConvertTextToDiagram(inputText));
        }
    }
}
