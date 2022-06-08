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

            foreach (var command in parsedCommands)
            {
                context.Initialize();
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

    public class RepeatedAdditionOfSameComponent : CommandExecution
    {
        [TestCase("add class person add class person")]
        [TestCase("add class person add field name add class person add method get name")]
        public void TryAddComponentWithSameName_CheckThatExceptionIsNotThrown(string inputText)
        {
            Assert.DoesNotThrow(() => diagram.ConvertTextToDiagram(inputText));
        }

        [TestCase("add class person add class person")]
        [TestCase("add class person add field name add class person add method get name")]
        public void TryAddComponentWithSameName_CheckThatComponentWasNotAdded(string inputText)
        {
            diagram.ConvertTextToDiagram(inputText);

            Assert.AreEqual(1, diagram.GetClasses().Count);
        }

        [TestCase("add class person add class person add method get name")]
        [TestCase("add class person add field name add class person add method get name")]
        public void TryAddComponentWithSameName_CheckThatComponentChildWasAddedToExistingComponent(string inputText)
        {
            diagram.ConvertTextToDiagram(inputText);

            Assert.AreEqual(1, diagram.GetClasses()[0].GetMethods().Count);
        }
    }

    public class AdditionOfArrayTypeComponent : CommandExecution
    {
        [TestCase("add class person add method get phone numbers add parameter phone numbers add type array of integer", "Integer[]")]
        [TestCase("add class person add method get phone numbers add parameter phone numbers add type array integer", "Integer[]")]
        public void AddArrayTypeComponent_CheckThatTypenameWasParsedToArrayStyle(string inputText, string arrayTypename)
        {
            diagram.ConvertTextToDiagram(inputText);

            Assert.AreEqual(arrayTypename, diagram.GetClasses()[0].GetMethods()[0].GetParameters()[0].GetParameterType().Name);
        }

        [TestCase("add class person add method get home addresses add parameter addresses add type array home address", "HomeAddress[]")]
        [TestCase("add class person add method get home addresses add parameter addresses add type array of home address", "HomeAddress[]")]
        public void AddArrayTypeComponent_CheckThatMultipleWordTypenameWasParsedToArrayStyle(string inputText, string arrayTypename)
        {
            diagram.ConvertTextToDiagram(inputText);

            Assert.AreEqual(arrayTypename, diagram.GetClasses()[0].GetMethods()[0].GetParameters()[0].GetParameterType().Name);
        }
    }
}
