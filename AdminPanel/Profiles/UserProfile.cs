using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminPanel.Models;
using AdminPanel.ViewModels.Users;
using AdminPanel.ViewModels.Common;

namespace AdminPanel.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserViewModel>();

            CreateMap<UserViewModel, User>()
                .ForMember(dest => dest.InfoBasesList, opt => opt.Ignore());

            CreateMap<User, ItemViewModel>();

            CreateMap<ItemViewModel, User>()
                .ForMember(dest => dest.Name, opt => opt.Ignore());
        }
    }
}
