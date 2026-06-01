module WordleLogic

open System

type LetterResult = Correct | Misplaced | Wrong

type LetterKnowledge = KnownCorrect | KnownMisplaced | KnownWrong | Unknown

type GuessLetter = {
    Letter: char
    Result: LetterResult
}

type GuessRow = GuessLetter array

type Board = GuessRow list

let wordLength = 5
let maxAttempts = 6

let create () : Board = []

let alphabet = [|'a' .. 'z'|]

let normalize (text: string) =
    text.Trim().ToLowerInvariant()

let loadWords (path: string) =
    if not (IO.File.Exists path) then
        failwithf "Word list file not found: %s" path

    IO.File.ReadAllLines path
    |> Array.map normalize
    |> Array.filter (fun word ->
        word.Length = wordLength && word |> Seq.forall Char.IsLetter)
    |> Array.distinct

let isValidLength (guess: string) =
    guess.Length = wordLength

let hasOnlyLetters (guess: string) =
    guess |> Seq.forall Char.IsLetter

let isInWordList (words: string array) (guess: string) =
    words |> Array.contains guess

let private resultSymbol = function
    | Correct -> "*"
    | Misplaced -> "+"
    | Wrong -> "-"

let renderRow (row: GuessRow) =
    row
    |> Array.map (fun item -> resultSymbol item.Result)
    |> String.concat " "

let render (board: Board) =
    if board.IsEmpty then
        printfn "No guesses yet."
    else
        printfn "Previous guesses:"
        board
        |> List.rev
        |> List.iteri (fun index row ->
            let guess =
                row
                |> Array.map (fun item -> string item.Letter)
                |> String.concat ""
            printfn "%d. %s  ->  %s" (index + 1) guess (renderRow row))

let private rank = function
    | KnownCorrect -> 3
    | KnownMisplaced -> 2
    | KnownWrong -> 1
    | Unknown -> 0

let private resultKnowledge = function
    | Correct -> KnownCorrect
    | Misplaced -> KnownMisplaced
    | Wrong -> KnownWrong

let letterKnowledge (board: Board) =
    let states: Map<char, LetterKnowledge> =
        alphabet
        |> Array.map (fun letter -> letter, Unknown)
        |> Map.ofArray

    board
    |> List.collect Array.toList
    |> List.fold (fun (current: Map<char, LetterKnowledge>) item ->
        let next = resultKnowledge item.Result
        let previous = current |> Map.find item.Letter

        if rank next > rank previous then
            current |> Map.add item.Letter next
        else
            current) states

let lettersWithState state board =
    letterKnowledge board
    |> Map.toList
    |> List.choose (fun (letter, knowledge) ->
        if knowledge = state then Some letter else None)
    |> List.sort

let evaluate (secret: string) (guess: string) : GuessRow =
    let secretChars = secret.ToCharArray()
    let guessChars = guess.ToCharArray()
    let results = Array.create wordLength Wrong
    let remaining = Collections.Generic.Dictionary<char, int>()

    for i in 0 .. wordLength - 1 do
        if guessChars.[i] = secretChars.[i] then
            results.[i] <- Correct
        else
            let count =
                match remaining.TryGetValue(secretChars.[i]) with
                | true, value -> value
                | false, _ -> 0
            remaining.[secretChars.[i]] <- count + 1

    for i in 0 .. wordLength - 1 do
        if results.[i] <> Correct then
            match remaining.TryGetValue(guessChars.[i]) with
            | true, count when count > 0 ->
                results.[i] <- Misplaced
                remaining.[guessChars.[i]] <- count - 1
            | _ -> ()

    Array.init wordLength (fun i -> {
        Letter = guessChars.[i]
        Result = results.[i]
    })

let isWin (guess: string) (secret: string) =
    guess = secret

let isFull (board: Board) =
    board.Length >= maxAttempts
