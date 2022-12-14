const {readFileSync} = require('fs');

let contents = readFileSync("input.txt", 'utf-8');
let arr = contents.split(/\r?\n/);
let arrReduced = []
let arrParsed = []

arr.forEach(e => 
    {
        if(e.length != 0){
            arrReduced.push(e
            )
        }
    })

arrReduced.forEach(e => arrParsed.push(eval(e)))

let pairCount = 1;
let count = 0;

for (let i = 0; i < arrReduced.length; i += 2)
{
    let value = GetValue(arrParsed[i], arrParsed[i+1])
    if(value == 1)
    {
        count += pairCount;
    }
    pairCount++;
}

console.log("Part 1 = " + count)

function GetValue(a1, a2)
{
    let a1IsArray = Array.isArray(a1)
    let a2IsArray = Array.isArray(a2)
    if(!a1IsArray && !a2IsArray)
    {
        if (a1 < a2) return 1
        if (a1 > a2) return -1
        return 0
    }
    else if(a1IsArray && a2IsArray)
    {
        for(let i = 0; i < a1.length; i++)
        {
            if(i == a2.length) return -1;
            var result = GetValue(a1[i], a2[i])
            if(result != 0) return result
        }
        return GetValue(a1.length, a2.length)
    }
    else if(!a1IsArray && a2IsArray)
    {
        return GetValue([a1], a2)
    }
    else if(a1IsArray && !a2IsArray)
    {
        return GetValue(a1, [a2])
    }
}

arrParsed.push([[2]])
arrParsed.push([[6]])

arrParsed = arrParsed.sort((a,b) => GetValue(a,b)).reverse()

let index1 = arrParsed.findIndex(e => e[0] == 2) + 1
let index2 = arrParsed.findIndex(e => e[0] == 6) + 1
let part2 = index1 * index2;
console.log("Part 2 = " + part2)



