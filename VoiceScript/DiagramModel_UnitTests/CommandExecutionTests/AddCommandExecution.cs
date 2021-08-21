using System;
using System.Collections.Generic;
using NUnit.Framework;

using VoiceScript.DiagramModel.Commands;
using VoiceScript.DiagramModel.Components;

namespace DiagramModel_UnitTests.CommandExecutionTests
{
    [TestFixture]
    public class AddCommand_ExecuteOneAddCommand
    {
        Diagram diagram;
        CommandParser parser;
        CommandExecutionContext context;

        [SetUp]
        public void SetUp()
        {
            diagram = new Diagram();
            parser = new CommandParser();
            context = new CommandExecutionContext();
        }

        [TestCase("add class person")]
        [TestCase("Add class person")]
        public void ExecuteAddClassCommand_CheckThatDiagramHasExactlyOneClass(string inputText)
        {
            var parsedCommands = parser.GetParsedCommands(inputText);
            InitializeCommandExecutionContext();
            
            parsedCommands[0].Execute(context);

            Assert.AreEqual(1, diagram.Children.Count);
        }

        [TestCase("add class person")]
        [TestCase("Add class person")]
        public void TryExecuteAddCommandWithInvalidTargetTypeInCurrentContext_CheckThatExceptionIsThrown(string inputText)
        {
            var parsedCommands = parser.GetParsedCommands(inputText);
            InitializeCommandExecutionContext();

            parsedCommands[0].Execute(context);

            Assert.AreEqual(1, diagram.Children.Count);
        }

        void InitializeCommandExecutionContext()
        {
            context.CurrentComponent = diagram;
            context.TargetComponent = diagram;
            context.CommandExecuted = false;

        }
    }
}
