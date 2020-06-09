using AdminPanel.ViewModels.InfoBases;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdminPanel.ViewModels.InfoBasesLists
{
    [Display(Name = "Списки информационных баз")]
    public class InfoBasesListIndexViewModel
    {
        [Display(Name = "Текущая страница")]
        public int CurrentPage { get; set; } = 1;
        [Display(Name = "Количество")]
        public int PagesAmount { get; set; }
        [Display(Name = "Списки информационных баз")]
        public List<InfoBasesListViewModel> Items { get; set; } = new List<InfoBasesListViewModel>();
    }
}
