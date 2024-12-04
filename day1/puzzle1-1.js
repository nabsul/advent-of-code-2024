import { readFileSync } from 'node:fs'
console.log('Hello, world!')
console.log(readFileSync)
const content = readFileSync('puzzle1.txt', 'utf8')

const list1 = []
const list2 = []

content.split('\n').filter(x => x.length > 0).map(x => x.split('   ')
    .map(x => parseInt(x)))
    .map(x => {
        list1.push(x[0])
        list2.push(x[1])
        return 0
    })

list1.sort();
list2.sort();

let total = 0;

for (let i = 0; i < list1.length; i++) {
    total += Math.abs(list1[i] - list2[i])
}

console.log(list1)
console.log(list2)
console.log(total)
