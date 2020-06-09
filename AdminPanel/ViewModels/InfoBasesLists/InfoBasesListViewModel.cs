using AdminPanel.Models;
using AdminPanel.ViewModels.Common;
using AdminPanel.ViewModels.InfoBases;
using AdminPanel.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPanel.ViewModels.InfoBasesLists
{
    [Display(Name = "Список информационных баз")]
    public class InfoBasesListViewModel
    {
        [HiddenInput]
        public Guid Id { get; set; }
        [HiddenInput]
        public Guid ListId { get; set; }
        [Display(Name = "Наименование")]
        [Required( ErrorMessage = "Не заполнено наименование")]
        public string Name { get; set; }
    }
}
