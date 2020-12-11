var rawInput = await Deno.readTextFile("input.txt");

var input = rawInput.split('\n').map((value, _index, _arr) => Number(value)).sort((a,b) => a-b);

// Part 1 find the sequence of numbers that are |previous - current| >= [1|2|3] and multiply the count of 1 diffs and 3 diffs
// Current voltage starts at zero
/*
let currentVolatage = 0;
let singleDistance = 0;
let tripleDistance = 0;

input.forEach(adaptor => {
    const distance = adaptor - currentVolatage;
    switch (distance) {
        case 1:
            singleDistance++;
            break;
        case 2:
            break;
        case 3:
            tripleDistance++;
            break;
        default:
            break;
    }
    currentVolatage = adaptor;
});

console.log(`Solution: ${singleDistance * (tripleDistance + 1)}`);
*/
// Part 2 find every distinct arrangement that fulfills the |previous - current| >= 1|2|3
// My possible solution, build a graph of all possible adaptors, Count all paths that end at the highest number
function findNextPossibleNumbers(arr: number[], currentJoltage: number): number[] {
    const output: number[] = [];
    for (let index = 0; index < 3; index++) {
        switch (arr[index] - currentJoltage) {
            case 1:
            case 2:
            case 3:
                output.push(arr[index]);
                break;
            default:
                break;
        }
    }
    return output;
}

function buildGraph(inputArr: number[]) {
    // deno-lint-ignore no-explicit-any
    let graph: any = {[0]: findNextPossibleNumbers(inputArr, 0)};
    inputArr.forEach((val: number, index: number, array: number[])=>{
        graph = {...graph, [val]: (findNextPossibleNumbers(array.slice(index + 1), val))};
    });
    return graph;
}

// deno-lint-ignore no-explicit-any
function countAllPaths (graph: any, currentNode: number) : number {
    const allNodes = graph[currentNode];
    if (allNodes === undefined)
        return 0;
    if (allNodes.length === 0)
        return 1;
    let count = 0;
    allNodes.forEach((val: number) => {
        count += countAllPaths(graph, val);
    });
    return count;
}

const graph = buildGraph(input);
console.log(countAllPaths(graph, 0))