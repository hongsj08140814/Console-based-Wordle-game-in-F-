# Console-based Wordle game in F#

A command-line Wordle game built with **F# / .NET 10**.

The game reads possible answers from the external file [words.txt](words.txt), chooses one secret 5-letter word randomly, and gives the player 6 attempts to guess it.

The word list contains 9,983 entries: alphabetic 5-letter words extracted from the local system dictionary (`/usr/share/dict/words`), plus `kaist` and `taxes` to preserve the proposal's example interaction.

## How to Run

Follow these steps from a terminal or command prompt.

### Prerequisite

Install the .NET 10 SDK first. You can check whether it is installed by running:

```bash
dotnet --version
```

The command should print a version number starting with `10`.

### Step 1: Download the repository

Clone the repository from GitHub, or download it as a ZIP file and unzip it.

If you use Git, run:

```bash
git clone <repository-url>
```

Replace `<repository-url>` with the actual URL of this repository.

### Step 2: Run the game

On macOS or Linux, run:

```bash
chmod +x run.sh
./run.sh
```

On Windows, run:

```bat
run.bat
```

You can also run the project directly on any platform with:

```bash
dotnet run
```

### Step 3: Play

When the game starts, you should see:

```text
=== Console-based Wordle game in F# ===
Rules:
- Guess the secret 5-letter English word in 6 attempts.
```

Then type a 5-letter word and press Enter. For example:

```text
Enter your guess: kaist
```

## Rules

- Guess the secret 5-letter English word in 6 attempts.
- Each guess must be exactly 5 letters.
- Each guess must contain letters only.
- Each guess must exist in `words.txt`.

## Feedback

| Symbol | Meaning |
| `*` | Correct: the letter is in the correct position |
| `+` | Misplaced: the letter is in the word but in a different position |
| `-` | Wrong: the letter is not in the word |

After each guess, the game also groups the full alphabet into:

- `letter in correct position:`
- `letter in different position:`
- `letter not in word:`
- `unknown yet:`

The groups are shown with console colors when the terminal supports them.

**Example Interaction**: If the secret word is `kaist` and the user enters `taxes`, the system prints `+ * - - +`. Then the system prints out the `Previous guesses` and the alphabet knowledge summary:

Enter your guess: taxes
Result: + * - - +

Attempt 2 of 6
Previous guesses:
1. taxes  ->  + * - - +
letter in correct position: a
letter in different position: t
letter not in word: e s x
unknown yet: b c d f g h i j k l m n o p q r u v w y z

Then it waits for the next input.
```

## Requirement changes made from the original proposal

In the original proposal, the game only showed the feedback result for each individual guess through the symbols '*', '+', and '-'. I extended this feature in the actual game for the convenience of the player. The game now also groups alphabets(from a to z) to 'letter in correct position', 'letter in different position', 'letter not in word', 'unknown yet', and displays it after each guess. This helps the player to track all alphabets easily while playing.

## Experience Using an LLM

I used an LLM to help develop this project. It was mainly used for generating the word list(words.txt) from the dictionary, generating the initial structure and organizing the project into separate modules, and implementing the Wordle feedback logic. It also partially assisted in writing README.md and requirements.md. The main point that the LLM was not able to do correctly was matching the exact output I wanted. It helped implement basic wordle rules and feedback correctly, but needed further prompts and additional fixing to make the interface cleaner and to match well with the proposal.

## Project Structure

```text
wordle-fsharp/
├── Wordle.fsproj
├── words.txt
├── run.sh
├── run.bat
├── README.md
├── requirements.md
└── Wordle/
    ├── WordleLogic.fs
    ├── ConsoleGame.fs
    └── Main.fs
```