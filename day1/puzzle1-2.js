import { readFileSync } from 'node:fs'

const content = readFileSync('puzzle1.txt', 'utf8')

const list1 = []
const list2 = new Map()

content.split('\n').filter(x => x.length > 0).map(x => x.split('   ')
    .map(x => parseInt(x)))
    .map(x => {
        list1.push(x[0])
        const curr = list2.get(x[1]) ?? 0
        list2.set(x[1], curr + 1)
    })

let total = 0;

for (let i = 0; i < list1.length; i++) {
    total += list1[i] * (list2.get(list1[i]) ?? 0)
}

console.log(list1)
console.log(list2)
console.log(total)
