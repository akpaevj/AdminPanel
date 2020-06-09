using AutoMapper;
using AdminPanel.Models;
using AdminPanel.ViewModels.InfoBases;
using AdminPanel.ViewModels.InfoBasesLists;
using AdminPanel.ViewModels.Users;
using System.Linq;
using AdminPanel.ViewModels.Common;

namespace AdminPanel.Profiles
{
    public class InfoBasesListProfile : Profile
    {
        public InfoBasesListProfile()
        {
            CreateMap<InfoBasesList, InfoBasesList>()
                .ForMember(dest => dest.InfoBaseInfoBasesLists, opt => opt.Ignore())
                .ForMember(dest => dest.Users, opt => opt.Ignore());

            CreateMap<InfoBasesList, InfoBasesListViewModel>().ReverseMap();

            CreateMap<InfoBasesList, InfoBasesListItemViewModel>().ReverseMap();
        }
    }
}
