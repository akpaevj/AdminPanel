using AdminPanel.ViewModels.Common;
using AdminPanel.ViewModels.InfoBases;
using AdminPanel.ViewModels.InfoBasesLists;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPanel.ViewModels.Users
{
    [Display(Name = "Пользователь")]
    public class UserViewModel
    {
        [HiddenInput]
        public Guid Id { get; set; }
        [HiddenInput]
        public string Sid { get; set; }
        [Display(Name = "Имя")]
        [Required]
        public string Name { get; set; }
        [Display(Name = "Имя AD")]
        [Required]
        public string SamAccountName { get; set; }
        [HiddenInput]
        public Guid? InfoBasesListId { get; set; }
        [Display(Name = "Список информационных баз")]
        public InfoBasesListItemViewModel InfoBasesList { get; set; }
    }
}
