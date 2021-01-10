﻿module LogisticGrid.Model

open System

let fz r c x = r * x * (1 - x) + c

let numberOfPointsOnLine = 128

let genRandomNumbers count1 count2 =
    let rnd = System.Random()
    Array2D.init count1 count2 (fun _ _ -> float(rnd.NextDouble()))

let arr = (genRandomNumbers 128 128)

let deltaX (arr: float[,]) i j =
    let mutable sum = 0.0
    let mutable count = 0
    if i > 0 then
        sum <- sum +  arr.[i - 1, j]
        count <- count + 1
    if i < 127 then
        sum  <- sum + arr.[i + 1, j]
        count <- count + 1
    if j > 0 then
        sum <- sum + arr.[i, j - 1]
        count <- count +  1
    if j < 127 then
        sum <- sum + arr.[i, j + 1]
    sum / (double count)

let calculateNewState(gn: double, cp: double, (arr: float[,])) =
    let copiedArray = Array2D.copy(arr)
    for i in [0..127] do
        for j in [0..127] do
            let dX = deltaX arr i j
            let oldX = arr.[i, j]
            let mutable newValue = 4.0 * gn / 1000.0 * oldX * (1.0-oldX) + cp * (dX - oldX)
            if newValue > 1.0 then newValue <- 1.0
            if newValue < 0.0 then newValue <- 0.0
            copiedArray.[i, j] <- newValue
    copiedArray

            
let toRGB (value: double) =
    match value with
    | value when value <= 1.0/3.0 -> "blue"
    | value when value <= 2.0/3.0 -> "yellow"
    | _ -> "green"
    
let pointsWithColor = Array2D.map (toRGB) arr

let list = [ for i in [0..127] do
                 for j in [0..127] ->  (i, j, pointsWithColor.[i,j])]

let filter color element =
    match element with
    | (_, _, c) -> color = c
    
let list1 = list |> List.filter (filter "blue") |> List.map (fun (x, y, _) -> x, y)

let list2 = list |> List.filter (filter "yellow") |> List.map (fun (x, y, _) -> x, y)

let list3 = list |> List.filter (filter "green") |> List.map (fun (x, y, _) -> x, y)

    