using NUnit.Framework;

using VoiceScript.DiagramModel.Commands;

namespace DiagramModel_UnitTests.CommandParserTests
{
    [TestFixture("add class person edit class person")]
    [TestFixture("add class person Edit class person")]
    [TestFixture("unrecognized input add class person edit class person")]
    [TestFixture("add class person edit class escape person")]
    public class SingleEditCommand
    {
        readonly CommandParser parser;
        readonly string input;

        public SingleEditCommand(string inputText)
        {
            parser = new CommandParser();
            input = inputText;
        }

        [Test]
        public void ParseEditationCommand_ParseExactlyOneEditCommandAfterAddCommand()
        {
            var parsedCommands = parser.GetParsedCommands(input);

            Assert.AreEqual(2, parsedCommands.Count);
        }

        [Test]
        public void ParseEditationCommand_CheckIfSecondParsedCommandIsEditCommand()
        {
            var parsedCommands = parser.GetParsedCommands(input);

            Assert.IsTrue(parsedCommands[1] is EditCommand);
        }

        [Test]
        public void ParseEditationCommand_CommandHasCorrectContent()
        {
            var parsedCommands = parser.GetParsedCommands(input);
            var command = parsedCommands[1];

            Assert.AreEqual("class", command.TargetType);
            Assert.AreEqual("Person", command.TargetValue);
        }
    }
}