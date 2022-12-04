let lines = List.ofSeq (System.IO.File.ReadLines(__SOURCE_DIRECTORY__ + "\\input.txt"))

let split (s : string) =
    let half = s.Split(',')
    let left = half[0].Split('-')
    let right = half[1].Split('-')
    ((int left[0], int left[1]),(int right[0], int right[1]))

let reduced = List.map split lines

let contains = function
    | s1, s2, s3, s4 when s1 <= s3 && s2 >= s4 -> true
    | s1, s2, s3, s4 when s3 <= s1 && s4 >= s2 -> true
    | _ -> false

let overlaps = function
    | s1, s2, s3, s4 -> 
        let list = [s1..s2] @ [s3..s4]
        List.length (List.distinct list) < (List.length list)

let rec part1 = function
    | ((s1, s2),(s3,s4)) :: xs ->       
        if contains (s1, s2, s3, s4) then 1 + part1 xs
        else 0 + part1 xs
    | _ -> 0

let rec part2 = function
    | ((s1, s2),(s3,s4)) :: xs ->       
        if overlaps (s1, s2, s3, s4) then 1 + part2 xs
        else 0 + part2 xs
    | _ -> 0    

printfn "Part 1 = %i" (part1 reduced)
printfn "Part 2 = %i" (part2 reduced)
