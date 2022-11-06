﻿using DataLayer.DatabaseModel;
using DataLayer.DataTransferModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public interface IDataService
    {
        IList<TitleBasic> GetTitleBasics();
        //IList<NameBasics> GetNameBasics();

        //SPECIFIC TITLE METHODS
        SpecificTitle GetSpecificTitleByName(string name);
        SpecificTitle GetSpecificTitle(string tConst);

        //SPECIFIC PERSON METHODS
        SpecificPerson GetSpecificPersonByName(string name);
        SpecificPerson GetSpecificPerson(string nConst);


    }
}
