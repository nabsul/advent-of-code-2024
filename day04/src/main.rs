use std::fs::File;
use std::io::{self, BufRead};
use std::path::Path;

const SEARCH: &str = "XMAS";
const DIRECTIONS: [i32; 3] = [-1, 0, 1];
const OFFSETS: [i32; 2] = [-1, 1];

fn main() {
    let data = read_data();

    println!("Part 1: {}", find_part1(&data));
    println!("Part 2: {}", find_part2(&data));
}

fn read_data() -> Vec<Vec<char>> {
    let path = Path::new("data.txt");
    let file = File::open(&path).expect("Unable to open file");
    let reader = io::BufReader::new(file);

    let mut data: Vec<Vec<char>> = Vec::new();

    for line in reader.lines() {
        let line = line.expect("Unable to read line");
        let chars: Vec<char> = line.chars().collect();
        data.push(chars);
    }

    return data;
}

fn find_part1(data: &Vec<Vec<char>>) -> i32 {
    let mut count = 0;
    
    for r in DIRECTIONS {
        for c in DIRECTIONS {
            if r == 0 && c == 0 {
                continue;
            }

            count += find(&data, r, c);
        }
    }

    count
}

fn find(data: &Vec<Vec<char>>, dr: i32, dc: i32) -> i32 {
    let mut count = 0;

    for r in 0..data.len() {
        for c in 0..data[0].len() {
            if check(data, r as i32, c as i32, dr, dc) {
                count += 1;
            }
        }
    }

    return count
}

fn check(data: &Vec<Vec<char>>, r: i32, c: i32, dr: i32, dc: i32) -> bool {
    for i in 0..SEARCH.len() {
        let r = r + ((i as i32) * dr);
        let c = c + ((i as i32) * dc);

        if r < 0 || r >= data.len() as i32 || c < 0 || c >= data[0].len() as i32 {
            return false;
        }

        if data[r as usize][c as usize] != SEARCH.chars().nth(i).unwrap() {
            return false;
        }
    }

    return true;
}

fn find_part2(data: &Vec<Vec<char>>) -> i32 {
    let mut count = 0;

    for r in 1..data.len() - 1 {
        for c in 1..data[0].len() - 1 {
            if check_for_x(data, r as i32, c as i32) {
                count += 1;
            }
        }
    }

    count
}

fn check_for_x(data: &Vec<Vec<char>>, r: i32, c: i32) -> bool {
    if data[r as usize][c as usize] != 'A' {
        return false;
    }

    for dr in OFFSETS {
        for dc in OFFSETS {
            let c = data[(r + dr) as usize][(c + dc) as usize];
            if c != 'S' && c != 'M' {
                return false;
            }
        }
    }

    if data[(r-1) as usize][(c-1) as usize] == data[(r+1) as usize][(c+1) as usize] {
        return false;
    }

    if data[(r-1) as usize][(c+1) as usize] == data[(r+1) as usize][(c-1) as usize] {
        return false;
    }

    return true;
}