using DataLayer.DatabaseModel;
using DataLayer.DataTransferModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class DataservicePersons : IDataservicePersons
    {

        public IList<Persons> GetActors()
        {
            using var db = new PortfolioDBContext();
            var actors = db.NameBasics
                .Select(x => new Persons
                {
                    NConst = x.NConst,
                    Name = x.PrimaryName,
                    BirthYear = x.BirthYear,
                    DeathYear = x.DeathYear,
                    Popularity = x.AVGNameRating,
                    isActor = isActor(x.NConst)

                })
                .Where(x => x.isActor == true)
                .Take(10).ToList();
            if (actors == null) return null;

            return actors;

        }

        public bool isActor(string NConst)
        {
            using var db = new PortfolioDBContext();

            var professions = db.Professions
                .Where(x => x.NConst == NConst)
                .Select(x => new Profession
                {
                    NConst = x.NConst,
                    TProfession = x.TProfession
                });

            foreach (var profession in professions)
            {
                if(profession.TProfession == "actor" || profession.TProfession == "actress")
                {
                    return true;
                }
            }
                

            return false;
        }
    }
}
