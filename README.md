# VoiceScript <img src=".\images\icon.png" width="80" height="80" />

`WinForms` `Google Cloud Platform`  `Speech-to-Text`

Desktop application providing voice commands conversion into UML diagrams and the corresponding code signatures.

1. [About the project](#about)
   - [Built With](#builtWith)
2. [Getting Started](#gettingStarted)
   - [Prerequisites](#prerequisites)
   - [Installation](#installation)
3. [Usage](#usage)
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
   - If you're new to Google Cloud Platform, once you register you'll get free credits for an account trial. You can use these to test out Speech-to-Text functionality.
   - For billing details see Google's [pricing table](https://cloud.google.com/speech-to-text/pricing).
3. Create and/or assign one or more service accounts to Speech-to-Text.
4. Download a service account credential API key.

> **_NOTE:_**  You can use VoiceScript without speech-to-text API key, if you only want to convert text commands into UML diagrams and code. The transcription won't work with no valid key provided but you can write your commands directly to the transcription textbox and convert them.

### Installation<a name="installation"></a>

``` bash
git clone https://github.com/lhotanok/VoiceScript.git
```

Create folder Keys in project's root directory and copy your json API key in this directory. The whole directory is not versed from git so that your api key wouldn't get exposed if you published your changes in a public repository. 

> **_NOTE:_**  You can skip this step as VoiceScript prompts you to add your api key at the first start. It opens a file dialog where you can easily navigate to your json key and don't worry about it its directory. It is copied to Keys directory automatically. This way is preferred considering future code changes.

If used with VisualStudio, you can build and launch VoiceScript project right after this quick setup. Else you'll need to install package dependencies manually.  All packages can be installed using nuget:

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

### Components

UML diagram components represent parts of the diagram and they are organized in the tree structure. Root component is the diagram itself and its only valid children are class components. As for class components, they can only hold fields and methods as their child components. You can inspect the whole component structure in [DiagramModel API](https://lhotanok.github.io/VoiceScript/api/api-diagram/DiagramModel.Components.html).

The component class type corresponds to the TARGET_TYPE part of the command. This type name is always 1-word only to keep command parsing unambiguous.

Each component holds `Name` property which identifies its name (could be interpreted as their value as well depending on the context). The value of this property is accessed through TARGET_VALUE part of the command.

### Commands

VoiceScript supports **Add**, **Edit** and **Delete** commands at the moment. These commands can be used to manipulate components of the diagram model. They are evaluated in the context of the last accessed component. If the given command is not valid in the last component's context, it tries to execute itself in the context of last component's parent. It continues traversing in the root component's direction and when it's not valid even at the top-most context, an error is shown. 

See [API documentation](https://lhotanok.github.io/VoiceScript/) for more details about project's structure.

## License <a name="license"></a>

Distributed under the MIT License. 

