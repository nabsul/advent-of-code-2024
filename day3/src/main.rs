use std::fs::File;
use std::io::Read;
use regex::Regex;

fn main() {
    let mut file = File::open("data.txt").expect("Unable to open file");
    let mut contents: String = String::new();
    file.read_to_string(&mut contents).expect("Unable to read file");


    println!("Part1: {}", calculate(&contents, false));
    println!("Part2: {}", calculate(&contents, true));
}

fn calculate(contents: &String, check_do_dont: bool) -> i32 {
    let re: Regex = Regex::new(r"mul\((\d+)\,(\d+)\)|do\(\)|don\'t\(\)").expect("Invalid regex");
    let mut doing: bool = true;

    let mut sum: i32 = 0;
    for cap in re.captures_iter(&contents) {
        if &cap[0] == "do()" {
            doing = true;
            continue;
        }

        if &cap[0] == "don't()" {
            doing = false;
            continue;
        }

        if check_do_dont && !doing {
            continue;
        }

        let left: &i32 = &cap[1].parse::<i32>().expect("Invalid number");
        let right: &i32 = &cap[2].parse::<i32>().expect("Invalid number");
        sum += left * right;
    }

    sum
}