var rawInput = await Deno.readTextFile("input.txt");
var input = rawInput.split('\n').map((v: string) => v.split(''));

function applyRules(currentSeating:string[][], currentSeat: Seat): string {
    const currentSeatValue = currentSeating[currentSeat.row][currentSeat.column];

    let adjacentOccupied = 0;
    
    // Part 1 Solution
    // var adjSeats = adjacentSeatNumbers(currentSeat, currentSeating.length, currentSeating[currentSeat.row].length);
    // for (let row = adjSeats.lowest.row; row <= adjSeats.highest.row; row++) {
    //     for (let column = adjSeats.lowest.column; column <= adjSeats.highest.column; column++) {
    //         if (row === currentSeat.row && column === currentSeat.column)
    //             continue;
            
    //         switch (currentSeating[row][column]) {
    //             case "#":
    //                 adjacentOccupied++;
    //                 break;
    //             case "L":
    //             case ".":
    //             default:
    //                 break;
    //         }
    //     }
    // }

    // if (currentSeatValue === "L" && adjacentOccupied === 0)
    //     return "#";

    // if (currentSeatValue === "#" && adjacentOccupied >= 4)
    //     return "L";

    // return currentSeatValue;


    enum WhatToDo {
        continue = 0,
        break = 1,
        plusAndBreak = 2
    }

    const a = function (currentSeatValue: string): WhatToDo {
        switch (currentSeatValue) {
            case ".":
            default:
                return WhatToDo.continue;
            case "L":
                return WhatToDo.break;
            case "#":
                return WhatToDo.plusAndBreak;
        }
    }

    // check top of current seat
    for (let row = currentSeat.row - 1; row >= 0; row--) {
        const ಠ_ಠ = a(currentSeating[row][currentSeat.column]);
        if (ಠ_ಠ === WhatToDo.break)
            break;
        if (ಠ_ಠ === WhatToDo.continue)
            continue;
        if (ಠ_ಠ === WhatToDo.plusAndBreak) {
            adjacentOccupied++;
            break;
        }
    }

    // check top-left of current seat
    for (let row = currentSeat.row - 1, column = currentSeat.column - 1; row >= 0 && column >= 0; row--,column--) {
        const ಠ_ಠ = a(currentSeating[row][column]);
        if (ಠ_ಠ === WhatToDo.break)
            break;
        if (ಠ_ಠ === WhatToDo.continue)
            continue;
        if (ಠ_ಠ === WhatToDo.plusAndBreak) {
            adjacentOccupied++;
            break;
        }
    }

    // check left of current seat
    for (let column = currentSeat.column - 1; column >= 0; column--) {
        const ಠ_ಠ = a(currentSeating[currentSeat.row][column]);
        if (ಠ_ಠ === WhatToDo.break)
            break;
        if (ಠ_ಠ === WhatToDo.continue)
            continue;
        if (ಠ_ಠ === WhatToDo.plusAndBreak) {
            adjacentOccupied++;
            break;
        }
    }

    // check bottom-left of current seat
    for (let column = currentSeat.column - 1, row = currentSeat.row + 1; column >= 0 && row < currentSeating.length; column--, row++) {
        const ಠ_ಠ = a(currentSeating[row][column]);
        if (ಠ_ಠ === WhatToDo.break)
            break;
        if (ಠ_ಠ === WhatToDo.continue)
            continue;
        if (ಠ_ಠ === WhatToDo.plusAndBreak) {
            adjacentOccupied++;
            break;
        }
    }
    
    // check bottom of current seat
    for (let row = currentSeat.row + 1; row < currentSeating.length; row++) {
        const ಠ_ಠ = a(currentSeating[row][currentSeat.column]);
        if (ಠ_ಠ === WhatToDo.break)
            break;
        if (ಠ_ಠ === WhatToDo.continue)
            continue;
        if (ಠ_ಠ === WhatToDo.plusAndBreak) {
            adjacentOccupied++;
            break;
        }
    }

    // check bottom-right of current seat
    for (let column = currentSeat.column + 1, row = currentSeat.row + 1; column < currentSeating[currentSeat.row].length && row < currentSeating.length; column++, row++) {
        const ಠ_ಠ = a(currentSeating[row][column]);
        if (ಠ_ಠ === WhatToDo.break)
            break;
        if (ಠ_ಠ === WhatToDo.continue)
            continue;
        if (ಠ_ಠ === WhatToDo.plusAndBreak) {
            adjacentOccupied++;
            break;
        }
    }

    // check right of current seat
    for (let column = currentSeat.column + 1; column < currentSeating[currentSeat.row].length ; column++) {
        const ಠ_ಠ = a(currentSeating[currentSeat.row][column]);
        if (ಠ_ಠ === WhatToDo.break)
            break;
        if (ಠ_ಠ === WhatToDo.continue)
            continue;
        if (ಠ_ಠ === WhatToDo.plusAndBreak) {
            adjacentOccupied++;
            break;
        }
    }

    // check bottom-right of current seat
    for (let column = currentSeat.column + 1, row = currentSeat.row - 1; column < currentSeating[currentSeat.row].length && row >= 0; column++, row--) {
        const ಠ_ಠ = a(currentSeating[row][column]);
        if (ಠ_ಠ === WhatToDo.break)
            break;
        if (ಠ_ಠ === WhatToDo.continue)
            continue;
        if (ಠ_ಠ === WhatToDo.plusAndBreak) {
            adjacentOccupied++;
            break;
        }
    }

    if (currentSeatValue === "L" && adjacentOccupied === 0)
        return "#";

    if (currentSeatValue === "#" && adjacentOccupied >= 5)
        return "L";

    return currentSeatValue;
}

class Seat {
    row: number;
    column: number;
    constructor(row: number, column: number){
        this.row = row;
        this.column = column;
    }

    toArray(): number[] { return [this.row, this.column]}
}

function adjacentSeatNumbers(currentSeat : Seat, maxRows: number, maxColumns: number) : {lowest: Seat, highest: Seat} {
    const lowest: Seat = new Seat(0,0);
    const highest: Seat = new Seat(0,0);
    lowest.row = currentSeat.row === 0 ? 0 : currentSeat.row - 1;
    lowest.column = currentSeat.column === 0 ? 0 : currentSeat.column - 1;

    highest.row = currentSeat.row + 1 === maxRows ? currentSeat.row : currentSeat.row + 1;
    highest.column = currentSeat.column + 1 === maxColumns ? currentSeat.column : currentSeat.column + 1;

    return {lowest: lowest, highest: highest};
}

const finalSeating: string[][] = input;
let currentSeating: string[][] = [];

while (JSON.stringify(finalSeating) != JSON.stringify(currentSeating)) {
    currentSeating = JSON.parse(JSON.stringify(finalSeating));

    for (let row = 0; row < currentSeating.length; row++) {
        for (let column = 0; column < currentSeating[row].length; column++) {
            if (currentSeating[row][column] === ".")
                continue;

            const currentSeat = new Seat(row, column);
            finalSeating[row][column] = applyRules(currentSeating, currentSeat);
        }
    }
}

let occupied = 0;
finalSeating.forEach(element => {
    occupied += element.filter(x => x === "#").length
});
console.log("Final Occupied Seats: ", occupied);

