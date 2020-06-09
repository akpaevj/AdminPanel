using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPanel.ViewModels.Common
{
    public class ItemViewModel
    {
        [HiddenInput]
        public Guid Id { get; set; }
        [Display(Name = "Наименование")]
        public string Name { get; set; }
    }
}
