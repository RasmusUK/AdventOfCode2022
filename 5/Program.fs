let lines = List.ofSeq (System.IO.File.ReadLines(__SOURCE_DIRECTORY__ + "\\input.txt"))

let count = 
    let rec findEmpty lst = 
        match lst with
        | x :: xs when x <> "" -> 1 + findEmpty xs
        | _ -> 0
    findEmpty lines

let input = List.map (fun x -> Seq.toList x) (List.take count lines)

let rec reduceList (lst : string list) =
    match lst with 
    | x :: xs ->        
        let split = x.Split(" ")
        (int (Array.item 1 split), 
        (int (Array.item 3 split), 
        int (Array.item 5 split))) :: reduceList xs 
    | _ -> []

let inline charToInt c = int c - int '0'

let rec initialSetup lst (map : Map<int, char list>) index =
    match lst with 
    | x :: xs when System.Char.IsDigit x -> 
        let xInt = charToInt x
        initialSetup xs (map.Add(xInt, (List.fold (fun a b -> 
            let (v : char) = List.item index  (List.item b input)
            if(v <> ' ') then v::a else a
        ) [] (List.rev (([0..(count-2)])))))) (index+1)
    | _ :: xs -> initialSetup xs map (index+1)
    | [] -> map

let inital = initialSetup (List.item (count-1) input) Map.empty 0
let actions = reduceList (List.skip (count+1) lines)

let rec part1Actions actlst (map : Map<int, char list>) = 
    match actlst with
    | x :: xs -> 
        part1Actions xs (
        List.fold (fun a _ -> 
            let fromIndex = (fst (snd x))
            let toIndex = (snd (snd x))
            let fromList = a.Item fromIndex
            let toList = a.Item toIndex
            a.Add(fromIndex, fromList.Tail).Add(toIndex, fromList.Head :: toList)
        ) map [0..(fst x)-1])
    | [] -> map

let part1 = part1Actions actions inital
let part1Answer = System.String.Concat(Array.ofList(List.map List.head (List.ofSeq part1.Values)))

printfn "Part1 = %s" part1Answer

let rec part2Actions actlst (map : Map<int, char list>) = 
    match actlst with
    | x :: xs -> 
        let count = fst x
        part2Actions xs (
            let fromIndex = (fst (snd x))
            let toIndex = (snd (snd x))
            let fromList = map.Item fromIndex
            let toList = map.Item toIndex
            map.Add(fromIndex, List.skip count fromList).Add(toIndex, (List.take count fromList) @ toList))
    | [] -> map

let part2 = part2Actions actions inital
let part2Answer = System.String.Concat(Array.ofList(List.map List.head (List.ofSeq part2.Values)))

printfn "Part2 = %s" part2Answer