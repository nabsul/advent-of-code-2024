import { readFileSync  } from "node:fs"

const isSafe = (row: number[]) => {
    let prevDiff = 0

    for (let i = 1; i < row.length; i++) {
        const diff = row[i] - row[i-1]
        if (diff * prevDiff < 0) return false;
        prevDiff = diff

        const abs = Math.abs(diff)
        if (abs < 1 || abs > 3) return false;
    }

    return true
}


const data = readFileSync("puzzle2.txt", "utf-8").split("\n").map(row => row.split(" ").map(Number))
const result = data.filter(isSafe).length
console.log(result)
