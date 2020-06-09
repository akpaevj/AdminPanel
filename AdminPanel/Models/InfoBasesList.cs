using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPanel.Models
{
    public class InfoBasesList
    {
        public Guid Id { get; set; }
        public Guid ListId { get; set; }
        public string Name { get; set; }
        public virtual List<InfoBaseInfoBasesList> InfoBaseInfoBasesLists { get; set; } = new List<InfoBaseInfoBasesList>();
        public virtual List<User> Users { get; set; } = new List<User>();

        public string GetIBasesContent()
        {
            var data = "";

            foreach (var item in InfoBaseInfoBasesLists)
            {
                data += item.InfoBase.IBasesContent + "\n";
            }

            return data;
        }
    }
}
