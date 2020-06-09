using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPanel.Models
{
    public class InfoBaseInfoBasesList
    {
        public Guid InfoBaseId { get; set; }
        public virtual InfoBase InfoBase { get; set; }
        public Guid InfoBasesListId { get; set; }
        public virtual InfoBasesList InfoBasesList { get; set; }
    }
}
