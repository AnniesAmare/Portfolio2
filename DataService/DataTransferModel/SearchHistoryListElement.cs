using DataLayer.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DataTransferModel
{
    public class SearchHistoryListElement
    {
        public DateTime Date { get; set; }
        public string? Content { get; set; }
        public string? Category { get; set; }
    }
}
