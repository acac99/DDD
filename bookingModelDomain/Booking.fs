namespace Booking

open System

module Booking =

  // common types
  type Result<'Success, 'Failure> = 
    | Ok of 'Success
    | Error of 'Failure

  let map f aResult = 
    match aResult with
      | Ok success -> Ok (f success)
      | Error failure -> Error failure


  // simple types
  type BookingId = BookingId of Guid
  type ValidationError = ValidationError of string
  type Status = 
    | CREATED 
    | ACCEPTED    

  // compound types
  type BookingTimes = private {
    bookingFrom: DateTime
    bookingTo: DateTime
  }
  module BookingTimes = 
    let create (bookingFrom: DateTime, bookingTo: DateTime) =
      if (bookingTo - bookingFrom).Hours < 1 then
        Result.Error (ValidationError "Booking cannot be less than 1 hour")
      else if (bookingTo - bookingFrom).Hours > 24 then
        Result.Error (ValidationError "Booking cannot be greater than 24 hours")
      else 
        let bookingTimes = {
          bookingFrom = bookingFrom
          bookingTo = bookingTo
        }
        Result.Ok bookingTimes


    let value bookingTimes: BookingTimes = bookingTimes

  type BookingCreateRequest = {
    bookingFrom: DateTime
    bookingTo: DateTime
  }
  
  type Booking = {
    Id: BookingId
    bookingTimes: BookingTimes
    status: Status
  }


  let CreateBooking req = 
    let bookingTimesResult = BookingTimes.create (req.bookingFrom, req.bookingTo)
    match bookingTimesResult with
      | Result.Ok bookingTimes -> 
        let createdBooking = {
          Id = (BookingId Guid.Empty)
          bookingTimes = bookingTimes
          status = Status.CREATED
        }
        Result.Ok createdBooking
      | Result.Error error -> Result.Error error
   