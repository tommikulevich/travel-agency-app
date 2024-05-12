using TripService.Models;

namespace TripService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using( var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }
        }

        private static void SeedData(AppDbContext context)
        {
            if(!context.Trip.Any())
            {
                Console.WriteLine("--> Seeding data");

                context.Trip.AddRange(
                    new Trip() {
                    FlightId = Guid.Parse("3c3c81e6-46f7-4959-81e0-cb37d0f3dce2"),
                    HotelId = Guid.Parse("d9c48b4f-7e8b-4e8d-9b77-68b8e501d8f7"),
                    Name = "Hurgada",
                    Country = "Egypt",
                    City = "Hurgada",
                    DeparturePlace = "Warsaw",
                    ArrivalPlace = "Hurgada",
                    NumOfAdults = 2,
                    NumOfKidsTo18 = 2,
                    NumOfKidsTo10 = 0,
                    NumOfKidsTo3 = 0,
                    DepartureDate = new DateTime(2024, 5, 9),
                    ReturnDate = new DateTime(2024, 5, 14),
                    TransportType = "Airplane",
                    Price = 1240.50,
                    MealsType = "AllInclusive",
                    RoomType = "Double",
                    DiscountPercents = 0.05,
                    NumOfNights = 5,  
                    Features = "WiFi" ,
                    Status = "Available"},
                    
                    new Trip() {
                    ClientId = Guid.Parse("8fb58b36-9b35-4e6a-8108-f8e6f11bc8d3"),
                    FlightId = Guid.Parse("f1bdaedb-32b6-4656-986a-946f238f14ef"),
                    HotelId = Guid.Parse("f832c2e7-9a9e-491a-82de-5067689cf019"),
                    Name = "Madrid",
                    Country = "Spain",
                    City = "Madrid",
                    DeparturePlace = "Gdansk",
                    ArrivalPlace = "Madrid",
                    NumOfAdults = 2,
                    NumOfKidsTo18 = 1,
                    NumOfKidsTo10 = 1,
                    NumOfKidsTo3 = 0,
                    DepartureDate = new DateTime(2024, 9, 9),
                    ReturnDate = new DateTime(2024, 9, 15),
                    TransportType = "Airplane",
                    Price = 1440.50,
                    MealsType = "AllInclusive",
                    RoomType = "Double",
                    DiscountPercents = 0.00,
                    NumOfNights = 6,
                    Features = "WiFi",   
                    Status = "Reserved"},

                    new Trip() {
                    ClientId = Guid.Parse("ebbd73fb-f202-45aa-a9a4-426ed09f2674"),
                    FlightId = Guid.Parse("ad0e5ae4-dfb9-4c8a-8b7d-af65d692227e"),
                    HotelId = Guid.Parse("2d0032cf-6c6a-4b7b-8424-7e846d4fe5e5"),
                    Name = "Rome",
                    Country = "Italy",
                    City = "Rome",
                    DeparturePlace = "Katowice",
                    ArrivalPlace = "Rome",
                    NumOfAdults = 2,
                    NumOfKidsTo18 = 0,
                    NumOfKidsTo10 = 0,
                    NumOfKidsTo3 = 0,
                    DepartureDate = new DateTime(2024, 7, 11),
                    ReturnDate = new DateTime(2024, 7, 14),
                    TransportType = "Airplane",
                    Price = 620.50,
                    MealsType = "AllInclusive",
                    RoomType = "Single",
                    DiscountPercents = 0.00,
                    NumOfNights = 3,  
                    Features = "WiFi", 
                    Status = "Pending"}
                );

                context.SaveChanges();
                
            }
            else 
            {
                Console.WriteLine("--> We already have data");
            }
        }
    }
}