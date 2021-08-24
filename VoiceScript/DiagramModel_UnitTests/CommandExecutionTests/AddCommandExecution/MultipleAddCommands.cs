using NUnit.Framework;

namespace DiagramModel_UnitTests.CommandExecutionTests
{
    [TestFixture("add class person add field name add method get name")]
    [TestFixture("add class person add method get name add field name")]
    public class MultipleAddCommands : CommandExecution
    {
        readonly string input;

        public MultipleAddCommands(string inputText)
        {
            input = inputText;
        }

        [Test]
        public void ExecuteAddClassCommandManually_CheckThatClassWithTwoChildrenWasCreated()
        {
            ExecuteCommandsManually(input);

            Assert.AreEqual(2, diagram.GetClasses()[0].Children.Count);
        }

        [Test]
        public void ExecuteAddClassCommandManually_CheckThatClassHasOneMethodAndOneField()
        {
            var parsedCommands = parser.GetParsedCommands(input);
            InitializeCommandExecutionContext(diagram);

            foreach (var command in parsedCommands)
            {
                InitializeCommandExecutionContext(context.TargetComponent);
                command.Execute(context);
            }

            var createdClass = diagram.GetClasses()[0];

            Assert.AreEqual(1, createdClass.GetFields().Count);
            Assert.AreEqual(1, createdClass.GetMethods().Count);
        }

        [Test]
        public void ExecuteAddClassCommandUsingDiagramAPI_CheckThatClassWithTwoChildrenWasCreated()
        {
            diagram.ConvertTextToDiagram(input);

            Assert.AreEqual(2, diagram.GetClasses()[0].Children.Count);
        }

        public void ExecuteAddClassCommandManuallyUsingDiagramAPI_CheckThatClassHasOneMethodAndOneField()
        {
            diagram.ConvertTextToDiagram(input);

            var createdClass = diagram.GetClasses()[0];

            Assert.AreEqual(1, createdClass.GetFields().Count);
            Assert.AreEqual(1, createdClass.GetMethods().Count);
        }
    }
}
