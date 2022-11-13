using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DatabaseModel
{
    public class UserSearch
    {
        public string? Username { get; set; }
        public User? User { get; set; }
        public int SearchId { get; set; }
        public DateOnly Date { get; set; }
        public string? Content { get; set; }
        public string? Category { get; set; }
    }
}
