using AdminPanel.Models;
using AdminPanel.ViewModels.Common;
using AdminPanel.ViewModels.InfoBasesLists;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPanel.ViewModels.InfoBases
{
    [Display(Name = "Информационная база")]
    public class InfoBaseViewModel
    {
        [HiddenInput]
        public Guid Id { get; set; }
        [Display(Name = "Наименование")]
        [Required(ErrorMessage = "Не заполнено наименование")]
        public string Name { get; set; }
        [Display(Name = "Вариант расположения")]
        [Required(ErrorMessage = "Не указан вариант расположения информационной базы")]
        public InfoBaseConnectionType ConnectionType { get; set; }
        [Display(Name = "Сервер")]
        [Required(ErrorMessage = "Не указан адрес сервера информационной базы")]
        public string Server { get; set; }
        [Display(Name = "Имя информационной базы")]
        [Required(ErrorMessage = "Не заполнено имя информационной базы")]
        public string InfoBaseName { get; set; }
        [Display(Name = "Каталог информационной базы")]
        [Required(ErrorMessage = "Не заполнен каталог информационной базы")]
        public string Path { get; set; }
        [Display(Name = "Адрес информационной базы")]
        [Required(ErrorMessage = "Не заполнен адрес информационной базы")]
        public string URL { get; set; }
        [HiddenInput]
        public string IBasesContent { get; set; }
    }
}
