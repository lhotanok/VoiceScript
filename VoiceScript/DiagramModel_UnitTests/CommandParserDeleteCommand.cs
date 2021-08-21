using NUnit.Framework;

using VoiceScript.DiagramModel.Commands;

namespace DiagramModel_UnitTests
{
    [TestFixture("add class person delete class person")]
    [TestFixture("add class person Delete class person")]
    [TestFixture("unrecognized input add class person delete class person")]
    [TestFixture("add class person delete class escape person")]
    public class CommandParser_CreateDeleteCommand
    {
        readonly CommandParser parser;
        readonly string input;

        public CommandParser_CreateDeleteCommand(string inputText)
        {
            parser = new CommandParser();
            input = inputText;
        }

        [Test]
        public void ParseClassDeletionCommand_ParseExactlyOneDeleteCommandAfterAddCommand()
        {
            var parsedCommands = parser.GetParsedCommands(input);

            Assert.AreEqual(2, parsedCommands.Count);
        }

        [Test]
        public void ParseClassDeletionCommand_CheckIfSecondParsedCommandIsDeleteCommand()
        {
            var parsedCommands = parser.GetParsedCommands(input);

            Assert.IsTrue(parsedCommands[1] is DeleteCommand);
        }

        [Test]
        public void ParseClassDeletionCommand_CommandHasCorrectContent()
        {
            var parsedCommands = parser.GetParsedCommands(input);
            var command = parsedCommands[1];

            Assert.AreEqual("class", command.TargetType);
            Assert.AreEqual("Person", command.TargetValue);
        }
    }
}