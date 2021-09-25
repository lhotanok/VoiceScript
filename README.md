# VoiceScript <img src=".\images\icon.png" width="80" height="80" />

`WinForms` `Google Cloud Platform`  `Speech-to-Text`

Desktop application providing voice commands conversion into UML diagrams and the corresponding code signatures.

1. [About the project](#about)
   - [Built With](#builtWith)
2. [Getting Started](#gettingStarted)
   - [Prerequisites](#prerequisites)
   - [Installation](#installation)
3. [Usage](#usage)
   - [Components](#components)
   - [Commands](#commands)
   - [Compilation](#compilation)
   - [Diagram and Code Visualization](#visualization)
   - [Languages](#languages)
   - [Examples](#examples)
     - [Class Diagram](#diagram)
     - [Inheritance](#inheritance)
     - [Traversing Diagram Structure](#traversing)
4. [License](#license)

## About VoiceScript project <a name="about"></a>

How many words can you say out loud in a few seconds versus how many words are you able to write during the same period using your keyboard? Having the powerful tool for speech-to-text translation by your side, you can effectively use your voice to generate text content and increase your speed significantly if compared with manual writing. VoiceScript uses one of the strongest solutions available for voice transcription and that is Google's [Speech-to-Text](https://cloud.google.com/speech-to-text) AI technology. Thanks to using this API, VoiceScript can expand and support the same number of languages that are provided by Google's API.  

Speak to VoiceScript using simple strictly defined commands and visualize the result in the form of colorful UML diagram. Create new voice record directly in the application, trigger the transcription once you stop recording and wait for the raw transcribed text. If you detect any flaws in the transcribed text, you can rewrite the faulty parts manually in the text box. The individual diagram classes are designed in the form of boxes whose colors are generated randomly. 

## Built With <a name="builtWith"></a>

- Google Cloud [Speech To Text](https://cloud.google.com/speech-to-text)
- [NAudio](https://github.com/naudio/NAudio)
- Microsoft [Automatic Graph Layout](https://github.com/microsoft/automatic-graph-layout)

## Getting Started <a name="gettingStarted"></a>

### Prerequisites<a name="prerequisites"></a>

1. Create Google Cloud Platform project at [GCP](https://cloud.google.com/).
2. Enable billing for [Speech-to-Text](https://cloud.google.com/speech-to-text) service. 
   - If you're new to Google Cloud Platform, once you'll register you'll get free credits for an account trial. You can use these to test out Speech-to-Text functionality.
   - For billing details see Google's [pricing table](https://cloud.google.com/speech-to-text/pricing).
3. Create and/or assign one or more service accounts to Speech-to-Text.
4. Download a service account credential API key.

> **_NOTE:_**  You can use VoiceScript without speech-to-text API key, if you only want to convert text commands into UML diagrams and code. The transcription won't work with no valid key provided but you can write your commands directly to the transcription textbox and convert them.

### Installation<a name="installation"></a>

``` bash
git clone https://github.com/lhotanok/VoiceScript.git
```

Create folder Keys in project's root directory and copy your json API key in this directory. The whole directory is ignored by git so that an api key wouldn't get exposed if any changes are published in a public repository. 

> **_NOTE:_**  You can skip this step as VoiceScript prompts you to add your api key at the first start. It opens a file dialog where you can easily navigate to your json key and you don't need to worry about it its directory. It is copied to Keys directory automatically. This way is preferred considering future code changes.

If used with VisualStudio, you can build and launch VoiceScript project right after this quick setup. Otherwise you'll need to install package dependencies manually.  All packages can be installed using nuget:

```bash
nuget install package_name
```

#### VoiceScript project

Uses packages:

- [AutomaticGraphLayout.GraphViewerGDI](https://github.com/Microsoft/automatic-graph-layout)
- [Google.Cloud.Speech.V1](https://github.com/googleapis/google-cloud-dotnet)

#### VoiceTranscription project

Uses packages:

- [NAudio](https://github.com/naudio/NAudio)

- [Google.Cloud.Speech.V1](https://github.com/googleapis/google-cloud-dotnet)
- [Google.Cloud.Storage.V1](https://github.com/googleapis/google-cloud-dotnet)

## Usage <a name="usage"></a>

Create a new voice record using the `Record` button and convert it into transcribed text by clicking the `Convert` button.

> **_NOTE:_**  You can switch on the real-time transcription during the recording process but it is currently an experimental feature. Its precision needs to be boosted as VoiceScript requires very strict commands format.

All commands follow the same simplified format:

COMMAND_NAME	TARGET_TYPE	TARGET_VALUE

### Components <a name="components"></a>

UML diagram components represent parts of the diagram and they are organized in the tree structure. Root component is the diagram itself and its only valid children are class components. As for class components, they can only hold fields and methods as their child components. You can inspect the whole component structure in [DiagramModel API](https://lhotanok.github.io/VoiceScript/api/api-diagram/DiagramModel.Components.html).

The component class type corresponds to the TARGET_TYPE part of the command. This type name is always 1-word only to keep command parsing unambiguous.

Each component holds `Name` property which identifies its name (could be interpreted as component's value as well depending on the context). The value of this property is accessed through TARGET_VALUE part of the command.

### Commands<a name="commands"></a>

VoiceScript supports **Add**, **Edit** and **Delete** commands at the moment. These commands can be used to manipulate components of the diagram model. They are evaluated in the context of the last accessed component. If the given command is not valid in the last component's context, it tries to execute itself in the context of the last component's parent. It continues traversing in the root component's direction and when it's not valid even at the top-most context, an error is shown.

#### Add command

Inserts new component in the current context and it enters the created component's context. 

#### Edit command

Enters context of the specified component. Checks if the component of the provided type and name exists in the current context and if so, it enters this component. 

Edit command is designed to be used for entering specific component's context and then modify this component. Component's name can be changed using edit command only (supposing we're in the right context already): `edit name new name`. We can also combine `edit` command usage with `add` and `delete` commands. First, we navigate to the desired component using `edit` command and then we can add new component or delete an existing one in the context of our specified component.

#### Delete command

Removes component from the current context. If no matching component exists in the current context, it generates an error.

#### Delimiter

There's one more special command and that is the **Escape** command. You can use this one the same way as the `\` delimiter is used in code. A typical use case is that you need to use command name in the name of your component. For example, you want to create class Person with method AddName. AddName contains Add which is also keyword for VoiceScript command. To let VoiceScript know you want to specify name and not start a new command, add `escape` word before using the keyword. You can also have two escape words in one command - everything that goes after escape is considered as name part. This way, you can easily create names containing even escape word itself.

**Examples**

- add method escape escape word ✔
  - method EscapeWord
- add method escape add name ✔
  - method AddName
- add method add name ❌
  - incomplete command target error

##### Auto-escaping

VoiceScript comes with one extra feature to simplify commands escaping. It requires usage of command keyword synonyms and it's only designed to escape first word in command's target name. Say we want to create method AddName as mentioned in the previous section. We already know that command `add method add name` can't be used as VoiceScript evaluates `add` word as the start of the new command. But there's a trick how to keep command short and get the desired effect at the same time. We choose an arbitrary [synonym](#languages) of `add` command and use it to start the main command. When specifying first word of method's name, only the same synonym which was used to start the current command is considered a new command starter. Other commands or synonyms of `add` command are auto-escaped and can be used as the first word of target name without using an explicit delimiter.

Auto-escaping is 

**Examples**

- insert method add name ✔
  - method AddName
- add method edit name ✔
  - method EditName
- change method edit name ✔
  - context of EditName method is entered

### Compilation<a name="compilation"></a>

Once you're done with specifying commands, click `Compile` button and if all commands are valid, they are parsed and executed eventually which generates UML diagram along with the corresponding code signatures.

Design of the commands in the left-most textbox gets boosted at this step. Command parts are grouped so that there would be one command per one line and they are colorized as well. If there is an error detected while parsing the individual commands, all commands that were parsed successfully are grouped and colorized, the rest of the commands is shown below as raw text and an error message is shown. You can correct the remaining text and click `Compile` again. Note that text corrections need to be handled manually. Alternatively, the whole text might be erased, new voice commands recorded and transcribed.

### Diagram and Code Visualization<a name="visualization"></a>

If command parsing phase succeeds, all commands are executed. Again, there might be a few errors that are discovered during the execution phase. It is typically caused by trying to execute command in the wrong context. 

When all commands are executed successfully, an UML diagram is shown in the middle of the screen and code block is generated in the right-most textbox.

You can play with the generated diagram, reorganize its class boxes, change the overall size or even generate new color schema by clicking `Compile` button repeatedly. Once you're happy with your diagram, you can save it for later visualization or even export it in a bitmap format (JPG, PNG, BMP, GIF, SVG).

**_NOTE:_**  While saving diagram as an image, prefer higher image size. If you save the image with low resolution, it might not render correctly and the result may slightly differ from the one you were looking at inside VoiceScript application. Simply go for the highest size if possible to avoid rendering problems.

To add more commands, you can either keep the old ones in the transcript textbox and add the new ones below them or you can erase the old commands and add new ones to the empty textbox. Both ways, diagram and code are updated and all previous changes are persisted. If you want to clear the entire diagram and the corresponding code, press `Clear` button explicitly. 

### Languages

VoiceScript currently supports English and Czech voice commands. With both languages, there are **synonyms** you can use for the individual commands. It is mainly because of the voice transcription feature - you might test which commands are recognized better from the Speech-To-Text service. Or you can combine them all and have funnier conversation with VoiceScript 🎙.

#### English format

##### Commands

Default command keywords are: **add**, **edit**, **delete**, **escape**. Apart from these keywords, you can use the corresponding synonyms (bold variants are default names, synonyms are specified after '=' if any):

- **add** = insert, append, attach, annex
- **edit** = change, modify, correct
- **delete** = erase, remove, cut
- **escape**

##### Components

Diagram components you can manipulate by using their names in commands are:

- **class**
- **field** = member, attribute
- **method** = function
- **type**
- **parameter**
- **required** = mandatory
- **visibility** = protection
- **parent** = ancestor

#### Czech format

##### Commands

Again, **add**, **edit** and **delete** commands are implemented. Use one of their Czech translations or command's default name if you prefer. See dictionary below:

- **add** = přidej, vytvoř, vlož, připoj
- **edit** = uprav, změň, edituj, oprav
- **delete** = smaž, vymaž, odstraň, vystřihni

For delimiter command, use Czech translation only:

- **escape** = přepni

##### Components

To manipulate components using Czech commands, don't use default component keywords (class, field etc.) and use translated component keywords instead:

- **class** = třídu
- **field** = člen, atribut
- **method** = metodu, funkci
- **type** = typ, druh
- **parameter** = parametr, parameter
- **required** = povinnost
- **visibility** = viditelnost, ochranu
- **parent** = rodiče, předka

##### Name values

VoiceScript translates specific values from Czech in English automatically to provide full Czech commands compatibility. You can use the following values in your commands as name parts, they get translated in English and the corresponding diagram and code get generated properly. If you prefer using these values directly in English, feel free to use the English equivalents instead of Czech translations.

- **public** = veřejná
- **internal** = interní
- **protected** = chráněná
- **private** = privátní

- **array** = pole
- **true** = pravda, ano
- **false** = nepravda, lež, ne
- **string** = řetězec

### Examples<a name="examples"></a>

#### Class Diagram<a name="diagram"></a>

Let's create our first 1-class diagram with `Person` class which contains public string field Name and 2 public methods: `GetName()` method which returns `string` type and `SetName(string name)` method which is void and takes `name` parameter of `string` type. 

Fill the only visible textbox with text commands either by recording voice commands and transcribing them into text commands or by writing commands manually.

**_NOTE:_**  Don't forget to set the correct language. Language settings are used by both voice transcription and commands parsing so ensure you set the right language even if you're not using speech-to-text feature.

**Transcribed commands**

```markdown
add class person add field name add type string add method get name add type string add method set name add parameter name add type string
```

<img src=".\images\transcript_example.png"/>

Now let's click the `Compile` button and see if all commands are valid. 

<img src=".\images\diagram_code_example.png"/>

It seems like we hit the right commands and the diagram was generated along with the C# code signatures!

#### Inheritance<a name="inheritance"></a>

Okay, let's try out something a little bit more complicated. UML diagrams are typically used for visualization of object-oriented design which goes hand in hand with the concept of class inheritance. So let's add two more classes and use some inheritance as well.

**Transcribed commands**

```markdown
add class person add field name add type string add method get name add type string add method set name add parameter name add type string

add class student add parent person add field teachers add type array teacher add visibility private add method get teachers add type array teacher

add class teacher add parent person add field students add type array student add visibility protected add method get students add type array student
```

<img src=".\images\inheritance_example.png"/>

#### Traversing Diagram Structure<a name="traversing"></a>

We'll start with a diagram from the previous [inheritance example](#inheritance). So we have class `Person` which is parent of `Teacher` and `Student` classes. We realized we didn't want the `Teacher` class to be called `Teacher` and we'd like to rename it to `Tutor`. We'd also like to rename all its occurrences in `Student` class. 

And to finish with, we want to get rid of the `SetName` method in `Person` class entirely as we plan to make this class immutable and accept name only through class constructor.

So, let's turn our idea into concrete commands and generate our new diagram.

**Transcribed commands**

```
add class person add field name add type string add method get name add type string add method set name add parameter name add type string

add class student add parent person add field teachers add type array teacher add visibility private add method get teachers add type array teacher

add class teacher add parent person add field students add type array student add visibility protected add method get students add type array student

edit class teacher edit name tutor edit class student edit field teachers edit name tutors edit type array teacher edit name array tutor edit method get teachers edit name get tutors edit type array teacher edit name array tutor

edit class person delete method set name
```

<img src=".\images\traversing_example.png"/>

See [API documentation](https://lhotanok.github.io/VoiceScript/) for more details about project's structure.

## License <a name="license"></a>

Distributed under the MIT License. 

