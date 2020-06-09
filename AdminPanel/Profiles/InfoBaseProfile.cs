using AutoMapper;
using AdminPanel.Models;
using AdminPanel.ViewModels.InfoBases;
using AdminPanel.ViewModels.InfoBasesLists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminPanel.ViewModels.Common;

namespace AdminPanel.Profiles
{
    public class InfoBaseProfile : Profile
    {
        public InfoBaseProfile()
        {
            CreateMap<InfoBase, InfoBase>()
                .ForMember(dest => dest.InfoBaseInfoBasesLists, opt => opt.Ignore());

            CreateMap<InfoBase, InfoBaseViewModel>()
                .ReverseMap();
        }
    }
}
