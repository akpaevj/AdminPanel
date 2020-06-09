using AdminPanel.ViewModels.InfoBases;
using AdminPanel.ViewModels.InfoBasesLists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPanel.ViewModels
{
    public class InfoBaseInfoBasesListViewModel
    {
        public Guid InfoBaseId { get; set; }
        public InfoBaseViewModel InfoBase { get; set; }
        public Guid InfoBasesListId { get; set; }
        public InfoBasesListViewModel InfoBasesList { get; set; }
    }
}
