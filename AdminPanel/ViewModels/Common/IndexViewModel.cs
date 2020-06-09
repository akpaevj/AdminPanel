using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPanel.ViewModels.Common
{
    public class IndexViewModel<T>
    {
        public int CurrentPage { get; set; } = 1;
        public int PagesAmount { get; set; }
        public List<T> Items { get; set; } = new List<T>();
    }
}
