using OffersServise.Models;

namespace OffersServise.Data
{
    public class OfferRepo : IOfferRepo
    {
        private readonly AppDbContext _context;
        public OfferRepo(AppDbContext context)
        {
            _context = context;
        }

        public void CreateOffer(Offer offer)
        {
            if (offer == null) 
            {
                throw new ArgumentNullException(nameof(offer));
            }
            _context.Offers.Add(offer);
        }

        public IEnumerable<Offer> GetAllOffers()
        {
            return _context.Offers.ToList();
        }

        public Offer GetOfferById(int id)
        {
            return _context.Offers.FirstOrDefault(p => p.Id == id);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}