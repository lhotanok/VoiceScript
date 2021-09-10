using NUnit.Framework;

using DiagramModel.Commands;

namespace DiagramModel_UnitTests.CommandParserTests
{
    public class InputWithEmptyLines
    {
        readonly CommandParser parser;

        public InputWithEmptyLines()
        {
            parser = new CommandParser();
        }

        [TestCase("add class person\n add field name")]
        [TestCase("\n\nadd class person\n add method get name\n\nadd field name")]
        public void ParseInputContainingEmptyLines_CheckThatExceptionIsNotThrown(string inputText)
        {
            Assert.DoesNotThrow(() => parser.GetParsedCommands(inputText));
        }

        [TestCase("add class person\n add field name")]
        [TestCase("add class person\nadd field name")]
        [TestCase("add class person\n\n\nadd field name")]
        public void ParseCommandsDividedByLineFeed_ParseExactlyTwoCommands(string inputText)
        {
            var parsedCommands = parser.GetParsedCommands(inputText);

            Assert.AreEqual(2, parsedCommands.Count);
        }

        [TestCase("add class person\nedit class person")]
        public void ParseCommandsDividedByLineFeed_CheckIfParsedCommandsHaveRightType(string inputText)
        {
            var parsedCommands = parser.GetParsedCommands(inputText);

            Assert.IsTrue(parsedCommands[0] is AddCommand);
            Assert.IsTrue(parsedCommands[1] is EditCommand);
        }
    }
}