// Learn more about F# at http://fsharp.org

open System
open Booking
open Booking.Booking

[<EntryPoint>]
let main argv =
    let bookingRequest: BookingCreateRequest = {
      bookingFrom = DateTime.Now
      bookingTo = DateTime.Now.AddHours(2.0)
    }
    let result = Booking.CreateBooking bookingRequest 
    match result with
      | Result.Ok booking ->
        printfn "Booking: %A" booking
        0
      | Result.Error error ->
        printfn "Error: %A" error
        0
