using AdminPanel.ViewModels.InfoBases;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdminPanel.ViewModels.InfoBases
{
    [Display(Name = "Информационные базы")]
    public class InfoBaseIndexViewModel
    {
        [Display(Name = "Текущая страница")]
        public int CurrentPage { get; set; } = 1;
        [Display(Name = "Количество")]
        public int PagesAmount { get; set; }
        [Display(Name = "Информационные базы")]
        public List<InfoBaseViewModel> Items { get; set; } = new List<InfoBaseViewModel>();
    }
}
