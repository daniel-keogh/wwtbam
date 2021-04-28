# Who Wants to Be a Millionaire?

Y4S2 Gesture-Based UI Voice Recognition Project

## Description

A simple, voice-controlled spin on "*Who Wants to Be a Millionaire?*", built in Unity using [SRGS](https://www.w3.org/TR/speech-grammar/).

The entire application is controllable either by mouse or by speech, except for any scrollable views (e.g. the tutorial & leaderboard), which require the use of a mouse. When the game starts a question is fetched from a JSON API called [OpenTrivia DB](https://opentdb.com/api_config.php), which is shown along with four possible answers. You can select an answer by saying the word "choose" followed by the letter of the answer you wish to select (i.e. "A", "B", "C", or "D"). You can then confirm your choice by saying "Final Answer".

### Using Lifelines

If you don't know the answer to a question, you can use a lifeline. To do so, you can say the word "use" followed by the name of the lifeline (e.g. "use the phone a friend lifeline"). Note that the friend/audience may not always give the correct answer, as the answer given will depend on how difficult the question is (the OpenTriviaDB API gives each question a difficulty rating which determines this).

### Walking Away

If you're out of lifelines and don't know the answer, you can take the money and walk away by saying "take the money" or "walk away", respectively. You will then be prompted to confirm your decision.

## XML Grammars

```sh
tree ./Assets/StreamingAssets -P *.xml
./Assets/StreamingAssets
├── Common.xml
├── Game.xml
├── GameOver.xml
├── Keyboard.xml
├── MainMenu.xml
└── Tutorial.xml
```

- `Game.xml` - This is the grammar file for the application's `GameScene`, with rules for answer selection, confirmation, using lifelines, and quitting the game.

- `MainMenu.xml` - This grammar file controls the application's `MainMenu` scene, with rules for playing a new game, showing the leaderboard screen, or quitting the application outright.

- `GameOver.xml` - Defines the grammar for the `GameOver` scene. It contains rules for replaying the game & quitting to the main menu.

- `Keyboard.xml` - Allows the application to take in user input via speech. It contains a public rule that can return a letter from A-Z whenever the user either says a letter, or a word from the [NATO Phonetic Alphabet](https://en.wikipedia.org/w/index.php?title=NATO_phonetic_alphabet).

- `Common.xml` - This grammar contains a few publicly scoped rules that are referenced by other grammar's in the application. It also defines rules for confirming & cancelling actions (e.g. confirming the user wants to quit the game), as well commands for showing/hiding something (e.g. the leaderboard or tutorial screens).

## C# Implementation

```sh
tree ./Assets/__Scripts/Grammars -P *.cs
./Assets/__Scripts/Grammars
├── Common
│   ├── Helpers.cs
│   └── Rules.cs
├── GameGrammar.cs
├── GameOverGrammar.cs
├── GrammarController.cs
└── MenuGrammar.cs
```

The relevant C# code for using the SRGS grammars is located in the `__Scripts/Grammars` directory. The `GrammarController` is an abstract class defining the base functionality that all the other controllers need, such as reading in the appropriate XML file and listening for the `OnPhraseRecognized` event, etc. The `MenuGrammar` deals with any utterances made in the main menu screen, while the `GameOverGrammar` is responsible for the game over scene. Lastly, the `GameGrammar` deals with any phrases said in the main game scene. Each of these classes emit events to signal which phrase has been said. As you move from one scene to the next, grammar recognition will stop/start so the appropriate XML file is being used at all times.

## Running

*Please Note*: The application uses a REST API & therefore needs access to the internet to function correctly. If building for UWP please ensure the `InternetClient` capability is enabled in the UWP Publishing Settings, in addition to the `Microphone`.

## Screenshots

![wwtbam](https://user-images.githubusercontent.com/37158241/116442972-054edc80-a84b-11eb-812c-8089d2bcd42d.png)

## Credits

- [Open Trivia Database API](https://opentdb.com/api_config.php) – Free JSON API that was used for getting the questions that appear in the game.

- https://millionaire.fandom.com/ - Used for getting the lifeline button icons as well as the audio clips from the show.

- [wildfilmsshorts2000](https://millionairefans.net/thread/3665/international-millionaire-graphics) – Used for getting the buttons that appear in the game.

- https://www.soundboard.com/sb/onemilliondollars - Also used for obtaining some of the sound effects used in the application.
