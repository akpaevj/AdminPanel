using AdminPanel.Models;
using AdminPanel.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPanel.ViewModels.Users
{
    [Display(Name = "Пользователи")]
    public class UserIndexViewModel
    {
        [Display(Name = "Текущая страница")]
        public int CurrentPage { get; set; } = 1;
        [Display(Name = "Количество")]
        public int PagesAmount{ get; set; }
        [Display(Name = "Пользователи")]
        public List<UserViewModel> Items { get; set; } = new List<UserViewModel>();
    }
}
