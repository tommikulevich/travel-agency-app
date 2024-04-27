using OffersServise.Models;

namespace OffersServise.Data
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
            if(!context.Offers.Any())
            {
                Console.WriteLine("--> Seeding data");

                context.Offers.AddRange(
                    new Offer() {Name="Dot Net", Country="Microsoft", Cost="Free"},
                    new Offer() {Name="SQL Server Express", Country="Microsoft", Cost="Free"},
                    new Offer() {Name="Kubernetes", Country="Cloud Native Computing", Cost="Free"}
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