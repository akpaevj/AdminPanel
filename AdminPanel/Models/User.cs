using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPanel.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Sid { get; set; }
        public string Name { get; set; }
        public string SamAccountName { get; set; }
        public Guid? InfoBasesListId { get; set; }
        public virtual InfoBasesList InfoBasesList { get; set; }

        public override bool Equals(object obj)
        {
            return obj is User user &&
                   Sid == user.Sid &&
                   Name == user.Name &&
                   SamAccountName == user.SamAccountName;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Sid, Name, SamAccountName);
        }
    }
}
