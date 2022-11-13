using DataLayer.DataTransferModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public interface IDataservicePersons
    {

        public IList<Persons> GetActors(int page, int pageSize);
        public int GetNumberOfActors(int page, int pageSize);
    }
}
