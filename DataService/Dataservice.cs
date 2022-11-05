
using DataLayer.DatabaseModel;

using System.Collections.Generic;
using DataLayer.DataTransferModel;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace DataLayer
{
    public class DataService : IDataService 
    {

        public IList<TitleBasic> GetTitleBasics()
        {
            using var db = new PortfolioDBContext();

            return db.TitleBasics.ToList();
        }

        /*
        public IList<NameBasic> GetNameBasics()
        {
            using var db = new PortfolioDBContext();
            return db.NameBasics.ToList();
        }
        */
        public IList<TitleAka> GetTitleAkas()
        {
            using var db = new PortfolioDBContext();
            var titleAkaList = db
                .TitleAkas
                .Where(x => x.Ordering == 13)
                .Take(1)
                .ToList();
            
            return titleAkaList;
        }

        public SpecificTitle GetSpecificTitle(string TConst)
        {
            using var db = new PortfolioDBContext();
            var title = db.TitleBasics.FirstOrDefault(x => x.TConst == TConst);
            if (title == null) return null;

            SpecificTitle specificTitle = new SpecificTitle
            {
                TConst = title.TConst,
                Title = title.PrimaryTitle,
                Runtime = title.RuntimeMinutes,
                Year = title.StartYear,
                ActorList = GetActorsForSpecificTitle(TConst),
                DirectorList = GetDirectorsForSpecificTitle(TConst)
            };

            return specificTitle;
        }

        private IList<ActorListElement> GetActorsForSpecificTitle(string TConst)
        {
            /*
            var actors = new List<ActorListElement>();
            using var db = new PortfolioDBContext();
            var characterList = db.Characters
                //.Include(x => x.NameBasic)
                .Where(x => x.TConst == TConst);
            
            if(characterList == null) return actors;

            foreach (var character in characterList)
            {
                ActorListElement actor = new ActorListElement
                {
                    NConst = character.NConst,
                    //Name = character.NameBasic.PrimaryName,
                    Character = character.TCharacter
                };
                actors.Add(actor);
            }
            return actors;
            */
            return null;
        }

        private IList<DirectorListElement> GetDirectorsForSpecificTitle(string TConst)
        {
            var directors = new List<DirectorListElement>();
            using var db = new PortfolioDBContext();

            foreach (var director in db
                         .TitlePrincipals
                         .Where(x => x.TConst == TConst)
                         .Where(x => x.Category == "director"))
            {
                DirectorListElement directorElement = new DirectorListElement
                {
                    NConst = director.NameBasic.NConst
                };
                directors.Add(directorElement);
            }

            return directors;
        }




        /*
        public TitleBasic GetTitleBasics(int id)
        {
            using var db = new PortfolioDBContext();

            return db.TitleBasics.Find(id);
        }*/
    }
}