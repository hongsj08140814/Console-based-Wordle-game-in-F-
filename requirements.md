**Project Title**: Console-based Wordle game in F#

**Overview**: This project implements Wordle as a command-line F# game. The player must guess a secret 5-letter English word in 6 attempts. After each valid guess, the game prints text feedback showing which letters are correct, misplaced, or wrong.

**Requirements**:

1. At the start of the game, the system picks one five-letter word randomly from a predefined external word list file named `words.txt`.
2. The system shows a welcome message and explains the rules of the game.
3. Each turn, the system displays the current attempt number and receives the user's input.
4. If the input is invalid, the system prints an error message that matches the situation:
   - Not exactly 5 letters long.
   - Includes non-letter characters.
   - Is not in the word list.
5. After a valid guess, the system evaluates each letter as one of three results:
   - `*` Correct: the letter is in the correct position.
   - `+` Misplaced: the letter is in the word but not in that position.
   - `-` Wrong: the letter is not in the word.
6. The system displays the result of each previous guess through text.
7. After each guess, the system displays the full alphabet grouped into four colored categories:
   - `letter in correct position:`
   - `letter in different position:`
   - `letter not in word:`
   - `unknown yet:`
8. If the player's guess exactly matches the answer word, the system prints a congratulation message and the game ends.
9. If the player uses all 6 attempts without guessing the word, the system prints a game-over message including the secret word and the game ends.

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
