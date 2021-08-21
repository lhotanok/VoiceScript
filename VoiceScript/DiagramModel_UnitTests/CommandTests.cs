using NUnit.Framework;

using VoiceScript.DiagramModel.Commands;

namespace DiagramModel_UnitTests
{
    [TestFixture("add class person")]
    [TestFixture("Add class person")]
    [TestFixture("ADD Class Person")]
    [TestFixture("unrecognized input add class person")]
    [TestFixture("add class escape person")]
    public class CommandParser_CreateAddCommand
    {
        readonly CommandParser parser;
        readonly string input;

        public CommandParser_CreateAddCommand(string inputText)
        {
            parser = new CommandParser();
            input = inputText;
        }

        [Test]
        public void ParseClassAditionCommand_ParseExactlyOneCommand()
        {
            var parsedCommands = parser.GetParsedCommands(input);

            Assert.AreEqual(1, parsedCommands.Count);
        }

        [Test]
        public void ParseClassAditionCommand_EachParsedCommandIsAddCommand()
        {
            var parsedCommands = parser.GetParsedCommands(input);

            foreach (var command in parsedCommands)
            {
                Assert.IsTrue(command is AddCommand);
            }
        }

        [Test]
        public void ParseClassAditionCommand_CommandHasCorrectContent()
        {
            var parsedCommands = parser.GetParsedCommands(input);

            foreach (var command in parsedCommands)
            {
                Assert.AreEqual("class", command.TargetType);
                Assert.AreEqual("Person", command.TargetValue);
            }
        }
    }
}