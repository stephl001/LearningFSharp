module BowlingKata

type ScoreCard = ScoreCard of int list

let rec private scoreGameIntern frameId points = function
| [10;t1;t2] when frameId=10 -> points + 10 + t1 + t2
| 10::t1::t2::rest -> scoreGameIntern (frameId+1) (points+10+t1+t2) (t1::t2::rest)
| t1::t2::t3::rest when t1+t2=10 -> scoreGameIntern (frameId+1) (points+10+t3) (t3::rest)
| t1::t2::rest -> scoreGameIntern (frameId+1) (points+t1+t2) rest
| _ -> points

let scoreGame (ScoreCard card) = scoreGameIntern 1 0 card





