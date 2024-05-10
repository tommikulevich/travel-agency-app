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
                    new Trip() {OfferId = "154",
                    ClientId = "124",
                    FlightId = "212",
                    Name = "Hurgada",
                    Country = "Egypt",
                    NumOfAdults = 2,
                    NumOfKids = 2,
                    DepartureDate = new DateTime(2024, 5, 9),
                    ReturnDate = new DateTime(2024, 5, 14),
                    TransportType = "Airplane",
                    Price = 1240.50,
                    MealsType = "AllInclusive",
                    RoomType = "Double",
                    DiscountPercents = 0.05,
                    NumOfNights = 5,   
                    Status = "Available"},
                    
                    new Trip() {OfferId = "2114",
                    ClientId = "4897",
                    FlightId = "214",
                    Name = "Madrit",
                    Country = "Spain",
                    NumOfAdults = 2,
                    NumOfKids = 0,
                    DepartureDate = new DateTime(2024, 9, 9),
                    ReturnDate = new DateTime(2024, 9, 15),
                    TransportType = "Airplane",
                    Price = 1440.50,
                    MealsType = "AllInclusive",
                    RoomType = "Double",
                    DiscountPercents = 0.00,
                    NumOfNights = 6,   
                    Status = "Reserved"},

                    new Trip() {OfferId = "7895",
                    ClientId = "984",
                    FlightId = "1234",
                    Name = "Rome",
                    Country = "Italy",
                    NumOfAdults = 1,
                    NumOfKids = 0,
                    DepartureDate = new DateTime(2024, 7, 11),
                    ReturnDate = new DateTime(2024, 7, 14),
                    TransportType = "Airplane",
                    Price = 620.50,
                    MealsType = "AllInclusive",
                    RoomType = "Single",
                    DiscountPercents = 0.00,
                    NumOfNights = 3,   
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