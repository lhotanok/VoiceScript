using System;
using System.Text;
using System.Collections.Generic;
using VoiceScript.DiagramModel.Commands.LanguageFormats;

namespace VoiceScript.DiagramModel.Commands
{
    public class CommandParser
    {
        readonly LanguageFormat language;

        List<string> parsedWords;
        int parsedOffset;

        public CommandParser(string languageCode = null)
        {
            if (languageCode == null) languageCode = EnglishFormat.GetCode();

            language = LanguageFormatFactory.CreateLanguageFormat(languageCode);

            #region Handle invalid language code
            if (language == null)
            {
                var exceptionMessage = $"Unsupported language format: {languageCode}. Supported formats are: ";
                var supportedFormats = LanguageFormatFactory.GetSupportedFormats();
                for (int i = 0; i < supportedFormats.Count; i++)
                {
                    if (i != 0) exceptionMessage += ", ";
                    exceptionMessage += supportedFormats[i];
                }
                throw new CommandParseException(exceptionMessage);
            }
            #endregion
        }

        public IList<Command> GetParsedCommands(string inputText)
        {
            ParseIntoWords(inputText);

            var parsedCommands = new List<Command>();

            var offsetBeforeCommand = parsedOffset;
            var command = GetNextCommand();

            while (command != null)
            {
                parsedCommands.Add(command);

                try
                {
                    offsetBeforeCommand = parsedOffset;
                    command = GetNextCommand();
                }
                catch (Exception ex)
                {
                    ThrowParseException(ex, parsedCommands, offsetBeforeCommand);
                }
                
            }

            return parsedCommands;
        }

        public static string ParseCamelCase(string pascalCaseWord)
        {
            if (pascalCaseWord.Length == 0) return pascalCaseWord;

            var firstLetter = pascalCaseWord[0].ToString().ToLower();
            var remainingLetters = pascalCaseWord[1..];

            return firstLetter + remainingLetters;
        }

        public static string ParsePascalCase(IEnumerable<string> wordFragments)
        {
            var pascalCaseWord = new StringBuilder();

            foreach (var fragment in wordFragments)
            {
                var firstLetter = fragment[0].ToString().ToUpper();
                pascalCaseWord.Append(firstLetter + fragment[1..]);
            }

            return pascalCaseWord.ToString();
        }

        void ParseIntoWords(string inputText)
        {
            parsedWords = new List<string>();

            var splitWords = inputText.Split(' ');

            foreach (var word in splitWords)
            {
                var fragments = word.Split('\n');
                foreach (var fragment in fragments)
                {
                    if (fragment != string.Empty)
                    {
                        parsedWords.Add(fragment);
                    }
                }
            }

            parsedOffset = 0;
        }

        void ThrowParseException(Exception ex, List<Command> parsedCommands, int successParsedOffset)
        {
            var unparsedWords = new List<string>();
            for (int i = successParsedOffset; i < parsedWords.Count; i++)
            {
                unparsedWords.Add(parsedWords[i]);
            }

            var message = $"Successfully parsed {parsedCommands.Count} commands.\n\n" + ex.Message;
            throw new CommandParseException(message, parsedCommands, unparsedWords);
        }

        Command GetNextCommand()
        {
            var commandName = GetCommandName();
            parsedOffset++;

            if (!IsCommandKeyword(commandName)) return null;

            var targetType = GetNextWord().ToLower();
            parsedOffset++;

            var targetName = GetTargetName();

            if (IncompleteCommandTarget(targetType, targetName))
                throw new CommandParseException("Incomplete command target in next command."
                     + "\nCommand format is: NAME  TARGET_TYPE  TARGET_VALUE");

            var command = CommandFactory.CreateCommand(commandName, targetType, targetName, language);

            if (command == null)
                throw new CommandParseException($"Unsupported command name: {commandName}.");

            return command;
        }

        string GetCommandName()
        {
            var word = GetNextWord();

            while (word != string.Empty && !IsCommandKeyword(word))
            {
                parsedOffset++;
                word = GetNextWord();
            }

            return word.ToLower();
        }

        string GetTargetName()
        {
            var nameParts = new List<string>();
            var delimiter = new DelimiterWrapper(language);

            var word = GetNextWord();

            while (word != string.Empty)
            {
                if (IsCommandKeyword(word))
                {
                    delimiter.UpdateDelimiterContext(word);
                    if (!delimiter.Escape() && !delimiter.DelimiterSet) break;

                    if (delimiter.Escape()) AddWordToNameParts(word, nameParts);
                }
                else
                {
                    AddWordToNameParts(word, nameParts);
                }

                parsedOffset++;
                word = GetNextWord();
            }

            return ParsePascalCase(nameParts);
        }

        void AddWordToNameParts(string word, List<string> nameParts)
        {
            var lowerWord = word.ToLower();
            if (language.ValuesToReplace.ContainsKey(lowerWord))
            {
                word = language.ValuesToReplace[lowerWord];
            }

            nameParts.Add(word);
        }

        string GetNextWord() => parsedOffset < parsedWords.Count ? parsedWords[parsedOffset] : string.Empty;

        bool IsCommandKeyword(string word)
        {
            var lowerWord = word.ToLower();

            return language.GetAllCommandFormats().Contains(lowerWord)
                || language.DelimiterFormat == lowerWord;
        }

        static bool IncompleteCommandTarget(string targetType, string targetName)
            => targetType == string.Empty || targetName == string.Empty;
    }
}
