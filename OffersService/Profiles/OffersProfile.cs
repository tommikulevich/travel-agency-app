using AutoMapper;
using OffersServise.Dtos;
using OffersServise.Models;

namespace OfferServise.Profiles
{
    public class OffersProfile  : Profile
    {
        public void OfferProfile()
        {
            // Source -> Target
            CreateMap<Offer, OfferReadDto>();
            CreateMap<OfferCreateDto, Offer>();
        }
    }
}