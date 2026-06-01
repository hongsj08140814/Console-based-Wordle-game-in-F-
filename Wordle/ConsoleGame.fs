module ConsoleGame

open System
open WordleLogic

type GameResult = PlayerWins | PlayerLoses

let private rng = Random()

let private wordFilePath () =
    IO.Path.Combine(AppContext.BaseDirectory, "words.txt")

let private chooseSecret (words: string array) =
    words.[rng.Next(words.Length)]

let private printRules () =
    printfn "Rules:"
    printfn "- Guess the secret 5-letter English word in 6 attempts."
    printfn "- * means the letter is correct and in the correct position."
    printfn "- + means the letter is in the word but in a different position."
    printfn "- - means the letter is not in the word."
    printfn ""

let private readGuess (words: string array) =
    let rec loop () =
        printf "Enter your guess: "
        let guess =
            match Console.ReadLine() with
            | null -> ""
            | text -> normalize text

        if not (isValidLength guess) then
            printfn "Error: your guess must be exactly 5 letters long."
            loop ()
        elif not (hasOnlyLetters guess) then
            printfn "Error: your guess can include letters only."
            loop ()
        elif not (isInWordList words guess) then
            printfn "Error: that word is not in the word list."
            loop ()
        else
            guess

    loop ()

let private writeColored (color: ConsoleColor) (text: string) =
    let previousColor = Console.ForegroundColor
    Console.ForegroundColor <- color
    printf "%s" text
    Console.ForegroundColor <- previousColor

let private formatLetters letters =
    match letters with
    | [] -> "-"
    | _ ->
        letters
        |> List.map (fun letter -> string letter)
        |> String.concat " "

let private printLetterGroup label color letters =
    printf "%s" label
    writeColored color (formatLetters letters)
    printfn ""

let private renderLetterKnowledge board =
    printfn "Letter knowledge:"
    printLetterGroup "letter in correct position: " ConsoleColor.Green (lettersWithState KnownCorrect board)
    printLetterGroup "letter in different position: " ConsoleColor.Yellow (lettersWithState KnownMisplaced board)
    printLetterGroup "letter not in word: " ConsoleColor.DarkGray (lettersWithState KnownWrong board)
    printLetterGroup "unknown yet: " ConsoleColor.Cyan (lettersWithState Unknown board)

let run () : GameResult =
    let words = loadWords (wordFilePath ())
    let secret = chooseSecret words

    printfn "=== Console-based Wordle game in F# ==="
    printRules ()

    let rec loop (board: Board) =
        printfn "Attempt %d of %d" (board.Length + 1) maxAttempts
        render board
        renderLetterKnowledge board
        printfn ""

        let guess = readGuess words
        let row = evaluate secret guess
        let board1 = row :: board

        printfn "Result: %s" (renderRow row)
        printfn ""

        if isWin guess secret then
            printfn "Congratulations! You guessed the word."
            PlayerWins
        elif isFull board1 then
            printfn "Game over. The secret word was: %s" secret
            PlayerLoses
        else
            loop board1

    loop (create ())
