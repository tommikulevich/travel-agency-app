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
            if(!context.Trips.Any())
            {
                Console.WriteLine("--> Seeding data");

                context.Trips.AddRange(
                    new Trip() {OfferId = Guid.Parse("7d4653b2-8a2b-4d7a-b8ff-6e8e11a5f485"),
                    ClientId = Guid.Parse("6e98f972-df4d-4ecb-bc3a-9f7a1d05ac56"),
                    FlightId = Guid.Parse("3c3c81e6-46f7-4959-81e0-cb37d0f3dce2"),
                    Name = "Hurgada",
                    Country = "Egypt",
                    City = "Hurgada",
                    DeparturePlace = "Warsaw",
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
                    
                    new Trip() {OfferId = Guid.Parse("be95e30b-9eaf-4bb5-92d4-d42a23d1a672"),
                    ClientId = Guid.Parse("8fb58b36-9b35-4e6a-8108-f8e6f11bc8d3"),
                    FlightId = Guid.Parse("f1bdaedb-32b6-4656-986a-946f238f14ef"),
                    Name = "Madrid",
                    Country = "Spain",
                    City = "Madrid",
                    DeparturePlace = "Gdansk",
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

                    new Trip() {OfferId = Guid.Parse("4a6f6368-dab8-4d03-8ec7-7a2764c1d15c"),
                    ClientId = Guid.Parse("ebbd73fb-f202-45aa-a9a4-426ed09f2674"),
                    FlightId = Guid.Parse("ad0e5ae4-dfb9-4c8a-8b7d-af65d692227e"),
                    Name = "Rome",
                    Country = "Italy",
                    City = "Rome",
                    DeparturePlace = "Katowice",
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