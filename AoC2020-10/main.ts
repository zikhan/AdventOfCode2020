var rawInput = await Deno.readTextFile("input.txt");

var input = rawInput.split('\n').map((value, _index, _arr) => Number(value)).sort((a,b) => a-b);

// Part 1 find the sequence of numbers that are |previous - current| >= [1|2|3] and multiply the count of 1 diffs and 3 diffs
// Current voltage starts at zero
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
    const graph: any = {[0]: findNextPossibleNumbers(inputArr, 0)};
    inputArr.forEach((val: number, index: number, array: number[])=>{
        graph[val]= (findNextPossibleNumbers(array.slice(index + 1), val));
    });
    return graph;
}

// THANKS @corsen-olbin FOR THE TIP TO HELP ME THINK OF THE SOLUTION!
// deno-lint-ignore no-explicit-any
function countAllPaths (inputArr: number[], graph: any) : number {
    // deno-lint-ignore no-explicit-any
    const permutations: any = {};
    for (let index = inputArr.length - 1; index >= 0; index--) {
        let pointValue = 0;
        const currentJoltage = inputArr[index];
        if (graph[currentJoltage].length === 0) {
            pointValue = 1;
        }
        else
            graph[currentJoltage].forEach((next: string | number) => {
                pointValue += permutations[next]
            });
        permutations[currentJoltage] = pointValue
    }
    return graph["0"]
           .map((v: string|number) => permutations[v]) // Find all the permutations for the starting points
           .reduce((total: number, current: number) => total + current); // Sum all starting point permutations
}

const graph = buildGraph(input);
console.log("Total Permutations: ", countAllPaths(input, graph))