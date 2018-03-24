(*
Let’s introduce some terminology:

    The set of values that can be used as input to the function is called the domain. In this case, it could be the set of real numbers, but to make life simpler for now, let’s restrict it to integers only.
    The set of possible output values from the function is called the range (technically, the image on the codomain). In this case, it is also the set of integers.
    The function is said to map the domain to the range.
*)
let add1 x = x + 1

(*
Key properties of mathematical functions

Mathematical functions have some properties that are very different from the kinds of functions you are used to in procedural programming.

    A function always gives the same output value for a given input value
    A function has no side effects.
*)
let x = 5
let y = x+1

(*
The power of pure functions

The kinds of functions which have repeatable results and no side effects are called “pure functions”, and you can do some interesting things with them:

    They are trivially parallelizable. I could take all the integers from 1 to 1000, say, and given 1000 different CPUs, I could get each CPU to execute the “add1” function for the corresponding integer at the same time, safe in the knowledge that there was no need for any interaction between them. No locks, mutexes, semaphores, etc., needed.
    I can use a function lazily, only evaluating it when I need the output. I can be sure that the answer will be the same whether I evaluate it now or later.
    I only ever need to evaluate a function once for a certain input, and I can then cache the result, because I know that the same input always gives the same output.
    If I have a number of pure functions, I can evaluate them in any order I like. Again, it can’t make any difference to the final result.
*)

(*
“Unhelpful” properties of mathematical functions

Mathematical functions also have some properties that seem not to be very helpful when used in programming.

    The input and output values are immutable
    A function always has exactly one input and one output
*)

(*
Simple values

Imagine an operation that always returned the integer 5 and didn’t have any input.
*)
let c = 5
//Following are function values? Difference? Map a domain to a range.
let c2 = fun()->5    
let c3() = 5

(*
Naming Values
*)
let derivative f = f //Only for following sample
let f = x
let f' = derivative f
let f'' = derivative f'

let ``is first time customer?`` = true
let ``add gift to order`` = ()
if ``is first time customer?`` then ``add gift to order``

(*
How types work with functions
*)
let intToString x = sprintf "x is %i" x  // format int to string
let stringToInt x = System.Int32.Parse(x)

(* Parameterless functions *)
let print = printfn "Hello"
let print2() = printfn "Hello"

(* Generic Types *)
let onAStick x = x.ToString() + " on a stick"

onAStick 22
onAStick 3.14159
onAStick "hello"

(*
Do you understand?
*)
let testA   = float 2
let testB x = float 2
let testC x = float 2 + x
let testD x = x.ToString().Length
let testE (x:float) = x.ToString().Length
let testF x = printfn "%s" x
let testG x = printfn "%f" x
let testH   = 2 * 2 |> ignore
let testI x = 2 * 2 |> ignore
let testJ (x:int) = 2 * 2 |> ignore
let testK   = "hello"
let testL() = "hello"
let testM x = x=x
let testN x = x 1          // hint: what kind of thing is x?
let testO x:string = x 1   // hint: what does :string modify?

(*
Currying
Breaking multi-parameter functions into smaller one-parameter functions
*)
//normal version
let printTwoParameters x y = 
   printfn "x=%i y=%i" x y

//explicitly curried version
let printTwoParametersCompiled x  =    // only one parameter!
   let subFunction y = 
      printfn "x=%i y=%i" x y  // new function with one param
   subFunction                 // return the subfunction

//normal version
let addTwoParameters x y = 
   x + y

//explicitly curried version
let addTwoParameters' x  =      // only one parameter!
   let subFunction y = 
      x + y                    // new function with one param
   subFunction                 // return the subfunction

// now use it step by step
let a = 6
let b = 99
let intermediateFn = addTwoParameters a  // return fn with
                                         // x "baked in"
let result  = intermediateFn b

(*
One parameter function that returns a function
*)
let add1Param x = (+) x

(* Partial Application *)
let add42 = (+) 42

(* Use with high order functions *)
[1;2;3] |> List.map add42 

(* Explain |> operator *)
let (|>) x f = f x 

// create a "tester" by partial application of "less than"
let twoIsLessThan = (<) 2   // partial application
twoIsLessThan 1
twoIsLessThan 3

// filter each element with the twoIsLessThan function
[1;2;3] |> List.filter twoIsLessThan 

// create a "printer" by partial application of printfn
let printer = printfn "printing param=%i" 

// loop over each element and call the printer function
[1;2;3] |> List.iter printer   

(*
Designing functions for partial application

You can see that the order of the parameters can make a big difference in the ease of use for partial application. 
For example, most of the functions in the List library such as List.map and List.filter have a similar form, namely:

List-function [function parameter(s)] [list]

The list is always the last parameter. Here are some examples of the full form:
*)
List.map    (fun i -> i+1) [0;1;2;3]
List.filter (fun i -> i>1) [0;1;2;3]
List.sortBy (fun i -> -i ) [0;1;2;3]

(*
Wrapping BCL functions for partial application

The .NET base class library functions are easy to access in F#, but are not really designed for use with a functional language like F#. 
For example, most functions have the data parameter first, while with F#, as we have seen, the data parameter should normally come last.

However, it is easy enough to create wrappers for them that are more idiomatic. For example, in the snippet below, the .NET string functions 
are rewritten to have the string target be the last parameter rather than the first:
*)
// create wrappers for .NET string functions
let replace oldStr newStr (s:string) = 
  s.Replace(oldValue=oldStr, newValue=newStr)

let startsWith lookFor (s:string) = 
  s.StartsWith(lookFor)

(* Function associativity *)
let F x y z = x y z

(* Function composition *)
let compose f g x = g ( f(x) )
let (>>) f g x = g ( f(x) )

let twice f = f >> f

let addThenMultiply = (+) 1 >> (*)

(* Composition can also be done backwards using the “<<” operator, if needed. *)
(+) 2 << (*) 3

(* Composition vs. pipeline *)
(* |> versus >> *)

(* Anonymous Functions *)
let add = fun x y -> x + y
let add' x y = x + y
let add'' = (+)

(* Use with high order functions *)
// with separately defined function
let add5 i = i + 5
[1..10] |> List.map add5

// inlined without separately defined function
[1..10] |> List.map (fun i -> i + 5)


(* Pattern matching on parameters *)
type Name = {first:string; last:string} // define a new type
let bob = {first="bob"; last="smith"}   // define a value

// single parameter style
let f1 name =                       // pass in single parameter
   let {first=f; last=l} = name     // extract in body of function
   printfn "first=%s; last=%s" f l

// match in the parameter itself
let f2 {first=f; last=l} =          // direct pattern matching
   printfn "first=%s; last=%s" f l 

// test
f1 bob
f2 bob

(* A common mistake: tuples vs. multiple parameters *)
let funcWithTuple (x,y) = x + y

(* Defining new operators *)
let (.*%) x y = x + y + 1

let res = (.*%) 2 3
let res' = 2 .*% 3 //Exactly 2 parameters

(* Point-free style *)
module PointFree =
    let add x y = x + y    // explicit
    let add' x = (+) x     // point free

    let add1Times2 x = (x + 1) * 2     // explicit
    let add1Times2' = (+) 1 >> (*) 2   // point free

    let sum list = List.reduce (fun sum e -> sum+e) list  // explicit
    let sum' = List.reduce (+)                            // point free

(* Combinators *)
module Combinators =
    let (|>) x f = f x             // forward pipe
    let (<|) f x = f x             // reverse pipe
    let (>>) f g x = g (f x)       // forward composition
    let (<<) g f x = g (f x)       // reverse composition

module CombinatorBirds =
    let I x = x                // identity function, or the Idiot bird
    let K x y = x              // the Kestrel
    let M x = x >> x           // the Mockingbird
    let T x y = y x            // the Thrush (this looks familiar!)
    let Q x y z = y (x z)      // the Queer bird (also familiar!)
    let S x y z = x z (y z)    // The Starling
    // and the infamous...
    let rec Y f x = f (Y f) x  // Y-combinator, or Sage bird

(* Recursive functions *)
let rec fib i = 
   match i with
   | 1 -> 1
   | 2 -> 1
   | n -> fib(n-1) + fib(n-2)

(* Attaching functions to types *)
module Person = 
    type T = {First:string; Last:string} with
       // member defined with type declaration
        member this.FullName = 
            this.First + " " + this.Last

    // constructor
    let create first last = 
        {First=first; Last=last}

    // another member added later
    type T with 
        member this.SortableName = 
            this.Last + ", " + this.First        
// test
let person = Person.create "John" "Doe"
let fullname = person.FullName
let sortableName = person.SortableName

(* Extending system types *)
type System.Int32 with
    member this.IsEven = this % 2 = 0

//test
let i = 20
if i.IsEven then printfn "'%i' is even" i

(*
Hey! Not so fast… The downsides of using methods

If you are coming from an object-oriented background, you might be tempted to use methods everywhere, because that is what you are familiar with. 
But be aware that there some major downsides to using methods as well:

    Methods don’t play well with type inference
    Methods don’t play well with higher order functions
*)

module SomePerson = 
    // type with no members initially
    type T = {First:string; Last:string} 

    // constructor
    let create first last = 
        {First=first; Last=last}

    // standalone function
    let fullName {First=first; Last=last} = 
        first + " " + last

    // function as a member
    type T with 
        member this.FullName = fullName this

open SomePerson

// using standalone function
let printFullName person = 
    printfn "Name is %s" (fullName person) 

    (*
let printFullName2 person = 
    printfn "Name is %s" (person.FullName)    //Does not compile!!!
    *)
(* Methods don’t play well with higher order functions *)
let list = [
    SomePerson.create "Andy" "Anderson";
    SomePerson.create "John" "Johnson"; 
    SomePerson.create "Jack" "Jackson"]

//get all the full names at once
list |> List.map fullName

list |> List.map (fun p -> p.FullName) //With object methods, we have to create special lambdas everywhere



































