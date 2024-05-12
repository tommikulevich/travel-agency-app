using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared.Trip.Dtos;

namespace TripService.Models
{
    public class Trip
    {
        [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid? ClientId { get; set; }
        public Guid FlightId { get; set; }
        public Guid HotelId { get; set; }
        public string Name {get; set;}
        public string Country {get; set;}
        public string City {get;set;}
        public string DeparturePlace{get; set;}
        public string ArrivalPlace {get; set;}
        public int NumOfAdults {get; set;}
        public int NumOfKidsTo18 {get; set;}
        public int NumOfKidsTo10 {get; set;}
        public int NumOfKidsTo3 {get; set;}
        public DateTime DepartureDate {get; set;}
        public DateTime ReturnDate {get; set;}
        public string TransportType {get; set;}    // enum
        public double Price {get; set;}
        public string MealsType {get; set;}    // enum
        public string RoomType {get; set;}    // enum
        public double DiscountPercents {get; set;}
        public int NumOfNights {get; set;} 
        public string Features  {get;set;}
        public string Status {get;set;}    

        public void SetFields(TripDto dto)
        {
            this.ClientId = dto.ClientId;
            this.FlightId = dto.FlightId;
            this.HotelId = dto.HotelId;
            this.Name = dto.Name;
            this.Country = dto.Country;
            this.City = dto.City;
            this.DeparturePlace = dto.DeparturePlace;
            this.ArrivalPlace = dto.ArrivalPlace;
            this.NumOfAdults = dto.NumOfAdults;
            this.NumOfKidsTo18 = dto.NumOfKidsTo18;
            this.NumOfKidsTo10 = dto.NumOfKidsTo10;
            this.NumOfKidsTo3 = dto.NumOfKidsTo3;
            this.DepartureDate = dto.DepartureDate;
            this.ReturnDate = dto.ReturnDate;
            this.TransportType = dto.TransportType;
            this.Price = dto.Price;
            this.MealsType = dto.MealsType;
            this.RoomType = dto.RoomType;
            this.DiscountPercents = dto.DiscountPercents;
            this.NumOfNights = dto.NumOfNights;  
            this.Features = dto.Features; 
            this.Status = dto.Status; 
        }

        public TripDto ToTripDto()
        {
            return new TripDto()
            {
                ClientId = this.ClientId,
                FlightId = this.FlightId,
                HotelId = this.HotelId,
                Name = this.Name,
                Country = this.Country,
                City = this.City,
                DeparturePlace = this.DeparturePlace,
                ArrivalPlace = this.ArrivalPlace,
                NumOfAdults = this.NumOfAdults,
                NumOfKidsTo18 = this.NumOfKidsTo18,
                NumOfKidsTo10 = this.NumOfKidsTo10,
                NumOfKidsTo3 = this.NumOfKidsTo3,
                DepartureDate = this.DepartureDate,
                ReturnDate = this.ReturnDate,
                TransportType = this.TransportType,
                Price = this.Price,
                MealsType = this.MealsType,
                RoomType = this.RoomType,
                DiscountPercents = this.DiscountPercents,
                NumOfNights = this.NumOfNights, 
                Features = this.Features,  
                Status = this.Status, 
            };
        }
    }
}