var rawInput = await Deno.readTextFile("input.txt");

class Rule {
    public Operation = "";
    public HowMuch = 0;

    constructor(fullRule: string){
        this.Operation = fullRule[0];
        this.HowMuch = Number(fullRule.slice(1))
    }
}

var input = rawInput.split('\n').map(v => new Rule(v));

enum Direction {
    "North" = 0,
    "East" = 1,
    "South" = 2,
    "West" = 3
}

type ShipStatus = {
    North: number; // Negative is South
    East: number;  // Negative is West
    Direction: Direction;
}

function ExecuteRulePt1(currentStatus: ShipStatus, rule: Rule) : void {
    switch (rule.Operation) {
        case "N":
            currentStatus.North += rule.HowMuch;
            break;
        case "S":
            currentStatus.North -= rule.HowMuch;
            break;
        case "E":
            currentStatus.East += rule.HowMuch;
            break;
        case "W":
            currentStatus.East -= rule.HowMuch;
            break;
        case "L":
            // (Math.abs(4 - HM / 90) + CurrentDirection) % 4
            currentStatus.Direction = (Math.abs(4 - (rule.HowMuch / 90)) + currentStatus.Direction) % 4
            break;
        case "R":
            // (HM / 90 + CurrentDirection) % 4
            currentStatus.Direction = (rule.HowMuch / 90 + currentStatus.Direction) % 4
            break;
        case "F":
            switch (currentStatus.Direction) {
                case Direction.North:
                    currentStatus.North += rule.HowMuch;
                    break;
                case Direction.South:
                    currentStatus.North -= rule.HowMuch;
                    break;
                case Direction.East:
                    currentStatus.East += rule.HowMuch;
                    break;
                case Direction.West:
                    currentStatus.East -= rule.HowMuch;
                    break;
            }
            break;
    }
}

function Part1(input:Rule[]) : void {
    const status: ShipStatus = {North: 0, East: 0, Direction: 1};

    input.forEach((rule: Rule) =>
        ExecuteRulePt1(status, rule));
    
    console.log("Part 1: ", Math.abs(status.North) + Math.abs(status.East));
}

Part1(input);

type ShipPosition = {
    North: number; // Negative is South
    East: number; // Negative is West
    Waypoint: {
        North: number; // Negative is South
        East: number; // Negative is West
    };
}

function ExecuteRulePt2(currentPosition: ShipPosition, rule: Rule ) : void {
    switch (rule.Operation) {
        case "N":
            currentPosition.Waypoint.North += rule.HowMuch;
            break;
        case "S":
            currentPosition.Waypoint.North -= rule.HowMuch;
            break;
        case "E":
            currentPosition.Waypoint.East += rule.HowMuch;
            break;
        case "W":
            currentPosition.Waypoint.East -= rule.HowMuch;
            break;
        case "F":
            currentPosition.North += currentPosition.Waypoint.North * rule.HowMuch;
            currentPosition.East += currentPosition.Waypoint.East * rule.HowMuch;
            break;
        case "L":
            for (let i = 0; i < rule.HowMuch / 90; i++)
            {
                const oldNorth = currentPosition.Waypoint.North;
                currentPosition.Waypoint.North = currentPosition.Waypoint.East      // Flip North to positive East
                currentPosition.Waypoint.East = oldNorth * -1 // Flip East to negative North
            }
            break;
        case "R":
            for (let i = 0; i < rule.HowMuch / 90; i++)
            {
                const oldNorth = currentPosition.Waypoint.North;
                currentPosition.Waypoint.North = currentPosition.Waypoint.East * -1; // Flip North to negative East
                currentPosition.Waypoint.East = oldNorth; // Flip East to positive north
            }
            break;
    }
}

function part2(input: Rule[]) : void {
    const position: ShipPosition =
    {
        North: 0,
        East: 0,
        Waypoint: { North: 1, East: 10 },
    };

    input.forEach(rule => {
        ExecuteRulePt2(position, rule);
    });

    console.log("Part 2: ", Math.abs(position.East) + Math.abs(position.North));
}

part2(input);
