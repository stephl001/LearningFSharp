﻿namespace LearningFSharp
 
module TexasHoldemKata =
 
    [<Literal>]
    let PokerHandCardsCount = 5
 
    type Suit = Spade | Club | Diamond | Heart
    type Rank =
        | Two | Three | Four | Five | Six | Seven | Eight | Nine | Ten
        | Jack | Queen | King | Ace
        static member (-) (r1, r2) =
            let rankVal = function
            | Two -> 2 | Three -> 3 | Four -> 4 | Five -> 5 | Six -> 6 | Seven -> 7 | Eight -> 8 
            | Nine -> 9 | Ten -> 10 | Jack -> 11 | Queen -> 12 | King -> 13 | Ace -> 14
            rankVal r1 - rankVal r2
    type Card = Card of Rank*Suit
 
    type CommunityCards =
        | Flop of Card*Card*Card
        | Turn of Card*Card*Card*Card
        | River of Card*Card*Card*Card*Card
        | Empty
 
    type Pair = Card*Card
    type Triple = Card*Card*Card
    type Quads = Card*Card*Card*Card
    type Suite = Card*Card*Card*Card*Card
 
    type SingleKicker = Card
    type DoubleKicker = Card*Card
    type TripleKicker = Card*Card*Card
    type QuadKicker = Card*Card*Card*Card
 
    type PokerHand =
        | HighCard of Card * QuadKicker
        | OnePair  of Pair * TripleKicker
        | DoublePair  of Pair * Pair * SingleKicker
        | ThreeOfaKind of Triple * DoubleKicker
        | Straight of Suite
        | Flush of Suite
        | FullHouse of Triple * Pair
        | FourOfaKind of Quads * SingleKicker
        | StraightFlush of Suite
        | Invalid
 
    type Hole = Hole of Card*Card
    type Hand = Hand of Hole*CommunityCards
    type UnusedCards = UnusedCards of Card*Card
    type ScoredHand = ScoredHand of PokerHand*UnusedCards
 
    type HandState =
        | Folded of Hand
        | Loosing of ScoredHand
        | Winning of ScoredHand
     
        