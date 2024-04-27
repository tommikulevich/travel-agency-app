using OffersServise.Models;

namespace OffersServise.Data
{
    public interface IOfferRepo
    {
        bool SaveChanges();

        IEnumerable<Offer> GetAllOffers();
        Offer GetOfferById(int id);
        void CreateOffer(Offer offer);
    }
}