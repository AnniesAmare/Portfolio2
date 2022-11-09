using DataLayer.DataTransferModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public interface IDataserviceSpecificPerson
    {
        
        //SPECIFIC PERSON METHODS
        SpecificPerson GetSpecificPersonByName(string name);
        SpecificPerson GetSpecificPerson(string nConst);

    }
}
