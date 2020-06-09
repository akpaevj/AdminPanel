using AutoMapper;
using AdminPanel.Models;
using AdminPanel.ViewModels;
using AdminPanel.ViewModels.InfoBases;
using AdminPanel.ViewModels.InfoBasesLists;
using Microsoft.CodeAnalysis.FlowAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminPanel.ViewModels.Common;

namespace AdminPanel.Profiles
{
    public class InfoBaseInfoBasesListProfile : Profile
    {
        public InfoBaseInfoBasesListProfile()
        {
            CreateMap<InfoBaseInfoBasesList, InfoBaseInfoBasesListViewModel>();

            CreateMap<InfoBaseInfoBasesListViewModel, InfoBaseInfoBasesList>()
                .ForMember(dest => dest.InfoBase, opt => opt.Ignore())
                .ForMember(dest => dest.InfoBasesList, opt => opt.Ignore());

            CreateMap<InfoBaseInfoBasesList, InfoBaseItemViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.InfoBaseId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.InfoBase.Name));

            CreateMap<InfoBaseItemViewModel, InfoBaseInfoBasesList>()
                .ForMember(dest => dest.InfoBaseId, opt => opt.MapFrom(src => src.Id));

            CreateMap<InfoBaseInfoBasesList, InfoBasesListItemViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.InfoBasesListId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.InfoBasesList.Name));

            CreateMap<InfoBasesListItemViewModel, InfoBaseInfoBasesList>()
                .ForMember(dest => dest.InfoBasesListId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
