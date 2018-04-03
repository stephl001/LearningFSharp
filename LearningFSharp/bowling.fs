namespace LearningFSharp
 
module BowlingKata =
 
    type PartialThrow =
        | Gutter
        | One | Two | Three | Four | Five | Six | Seven | Eight | Nine
    type SpareFirstThrow = SpareFirstThrow of PartialThrow
 
    let private partialThrowToInt = function
    | Gutter -> 0 | One -> 1 | Two -> 2 | Three -> 3 | Four -> 4
    | Five -> 5 | Six -> 6 | Seven -> 7 | Eight -> 8 | Nine -> 9
    
    type Frame =
        private
        | Strike
        | Spare of SpareFirstThrow
        | Open of PartialThrow*PartialThrow
    type BonusDoubleThrow =
        private
        | DoubleStrike
        | Spare of SpareFirstThrow
        | Open of PartialThrow*PartialThrow
    type BonusSingleThrow =
        private
        | Strike
        | Open of PartialThrow
    type LastFrame =
        private
        | Strike of BonusDoubleThrow
        | Spare of SpareFirstThrow*BonusSingleThrow
        | Open of PartialThrow*PartialThrow

    type Frame with
        static member CreateStrike = Frame.Strike
        static member CreateSpare = Frame.Spare
        static member CreateOpen (t1,t2) = 
            let (ti1,ti2) = (partialThrowToInt t1,partialThrowToInt t2)
            if ti1+ti2<10 
            then Frame.Open (t1,t2)
            else failwith "Wrong throws"
 
    type EmptyCard = private | Empty
    type InProgressCard = private { RegularFrames: Frame list }
    type AllFramesButLastCard = private { RegularFrames: Frame list }
    type CompleteCard = private { RegularFrames: Frame list; LastFrame: LastFrame }
 
    type ScoreCard =
        private
        | Empty of EmptyCard
        | InProgress of InProgressCard
        | AlmostComplete of AllFramesButLastCard
        | Complete of CompleteCard
 
    let private addToEmptyCard frame =
        InProgress { RegularFrames=[frame] }
    let private addToInProgressCard (card:InProgressCard) frame =
        if card.RegularFrames.Length < 9
        then InProgress { RegularFrames=card.RegularFrames@[frame] }
        else AlmostComplete { RegularFrames=card.RegularFrames@[frame] }
    let private addLastFrame (card:AllFramesButLastCard) lastFrame =
        Complete { RegularFrames=card.RegularFrames; LastFrame=lastFrame }
 
    type EmptyCard with
        member this.AddFrame = addToEmptyCard
    type InProgressCard with
        member this.AddFrame = addToInProgressCard this
    type AllFramesButLastCard with
        member this.AddLastFrame = addLastFrame this
 
    let private spareThrows (SpareFirstThrow firstThrow) =
        let pinsFirstThrow = firstThrow |> partialThrowToInt
        (pinsFirstThrow,10-pinsFirstThrow)
   
    let private frameToThrow = function
    | Frame.Strike -> [10]
    | Frame.Spare firstThrow ->
        let (ft,st) = spareThrows firstThrow
        [ft;st]
    | Frame.Open (t1,t2) -> [partialThrowToInt t1; partialThrowToInt t2]
 
    let private lastFrameToThrow = function
    | LastFrame.Strike bonus ->
        match bonus with
        | DoubleStrike -> [10;10;10]
        | BonusDoubleThrow.Spare firstThrow ->
            let (t1,t2) = spareThrows firstThrow
            [t1;t2]
        | BonusDoubleThrow.Open (t1,t2) ->
            [partialThrowToInt t1; partialThrowToInt t2]
    | LastFrame.Spare (firstThrow,bonus) ->
        let (t1,t2) = spareThrows firstThrow
        match bonus with
        | BonusSingleThrow.Strike ->
            [t1;t2;10]
        | BonusSingleThrow.Open t ->
            [t1;t2;partialThrowToInt t]
    | LastFrame.Open (t1,t2) ->
        [partialThrowToInt t1; partialThrowToInt t2]

    let private addFrame card frame =
        match card with
        | Empty c -> c.AddFrame frame
        | InProgress c -> c.AddFrame frame
        | AlmostComplete _ | Complete _ -> failwith "You cannot do this"

    let private addCardLastFrame card frame =
        match card with
        | AlmostComplete c -> c.AddLastFrame frame
        | InProgress _ | Empty _ | Complete _ -> failwith "You cannot do this"

    type ScoreCard with
        static member CreateNew = Empty EmptyCard.Empty
        member this.AddRegularFrame = addFrame this
        member this.AddLastFrame = addCardLastFrame this

    let private transformCard = function
    | Complete {RegularFrames=frames; LastFrame=lastFrame} ->
        let regularThrows = frames |> List.collect frameToThrow
        let lastFrameThrows = lastFrameToThrow lastFrame
        regularThrows @ lastFrameThrows
    | Empty _ -> failwith "You cannot score an empty card"
    | InProgress { RegularFrames=frames } -> failwith (sprintf "A complete card has 10 frames. This one has only %d." frames.Length)
    | AlmostComplete _ -> failwith "The last frame is still missing for this card"
                
    let rec private scoreGameIntern frameId points = function
    | [10;t1;t2] when frameId=10 ->
        points + 10 + t1 + t2
    | 10::(t1::t2::_ as remainingCard) ->
        scoreGameIntern (frameId+1) (points+10+t1+t2) remainingCard
    | t1::t2::(t3::_ as remainingCard) when t1+t2=10 ->
        scoreGameIntern (frameId+1) (points+10+t3) remainingCard
    | t1::t2::rest ->
        scoreGameIntern (frameId+1) (points+t1+t2) rest
    | _ -> points
 
    let scoreGame = transformCard >> scoreGameIntern 1 0