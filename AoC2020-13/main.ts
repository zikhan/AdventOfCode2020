var rawInput = await Deno.readTextFile("input.txt");

function part1(estTime: number, runningBuses: number[]) : void {
    const calcWait = (busId: number, estTime: number) => busId - (estTime % busId);
    const output = {
        busId: runningBuses[0],
        waitTime: calcWait(runningBuses[0], estTime)
    };

    runningBuses.forEach(bus => {
        const wait = calcWait(bus, estTime);
        if (wait < output.waitTime){
            output.busId = bus;
            output.waitTime = wait;
        }
    });

    console.log("Part 1: ", output.busId * output.waitTime);
}

const estimateStationTime = Number(rawInput.split('\n')[0]);
const runningBuses = rawInput.split('\n')[1].split(',').filter(x => x !== 'x').map(Number);

part1(estimateStationTime, runningBuses);

type busOffset = {
    busId: number;
    offset: number;
}

const busesThatMatter : busOffset[] = [];
rawInput.split('\n')[1].split(',').forEach((x, i) => {
    if (!isNaN(Number(x)))
        busesThatMatter.push({busId: Number(x), offset: i})
});

function part2(busesThatMatter: busOffset[]){
    let keepGoing = true;
    let multiplier = 1;
    let multiplicativeIndex = 1;

    let t = busesThatMatter[0].busId;
    // Needed a bit of help with this one. The internet said to look at the Chinese Remainder Theroem 
    while (keepGoing) {
        const test = t + (busesThatMatter.slice(0,multiplicativeIndex).map( v => v.busId).reduce( (acc, curr) => acc * curr)) * multiplier
        if ((test + busesThatMatter[multiplicativeIndex].offset) % busesThatMatter[multiplicativeIndex].busId === 0) {
            multiplicativeIndex++;
            multiplier = 1;
            t = test;
            if (multiplicativeIndex == busesThatMatter.length) {
                keepGoing = false;
            }
        }
        else
            multiplier++;
    }

    console.log("Part 2: ", t);
}

part2(busesThatMatter);