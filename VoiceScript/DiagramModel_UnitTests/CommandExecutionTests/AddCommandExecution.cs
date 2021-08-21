using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace DiagramModel_UnitTests.CommandExecutionTests
{
    [TestFixture]
    public class SingleAddCommand : CommandExecution
    {
        [TestCase("add class person")]
        [TestCase("Add class person")]
        public void ExecuteAddClassCommandManually_CheckThatDiagramHasExactlyOneClass(string inputText)
        {
            var parsedCommands = parser.GetParsedCommands(inputText);
            InitializeCommandExecutionContext(diagram);
            
            parsedCommands[0].Execute(context);

            Assert.AreEqual(1, diagram.Children.Count);
        }

        [TestCase("add class person")]
        [TestCase("Add class person")]
        public void ExecuteAddClassCommandUsingDiagramAPI_CheckThatDiagramHasExactlyOneClass(string inputText)
        {
            diagram.ConvertTextToDiagram(inputText);

            Assert.AreEqual(1, diagram.Children.Count);
        }

        [TestCase("add field name")]
        [TestCase("Add parameter age")]
        [TestCase("Add diagram my diagram")]
        public void TryExecuteAddCommandManuallyWithInvalidTargetTypeInCurrentContext_CheckThatExceptionIsThrown(string inputText)
        {
            var parsedCommands = parser.GetParsedCommands(inputText);
            InitializeCommandExecutionContext(diagram);

            Assert.Throws<InvalidOperationException>(() => parsedCommands[0].Execute(context));
        }

        [TestCase("add field name")]
        [TestCase("Add parameter age")]
        [TestCase("Add diagram my diagram")]
        public void TryExecuteAddCommandUsingDiagramAPIWithInvalidTargetTypeInCurrentContext_CheckThatExceptionIsThrown(string inputText)
        {
            Assert.Throws<InvalidOperationException>(() => diagram.ConvertTextToDiagram(inputText));
        }
    }

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
            var parsedCommands = parser.GetParsedCommands(input);
            InitializeCommandExecutionContext(diagram);

            foreach (var command in parsedCommands)
            {
                InitializeCommandExecutionContext(context.TargetComponent);
                command.Execute(context);
            }

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
