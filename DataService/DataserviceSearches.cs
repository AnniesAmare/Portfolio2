﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DataLayer.DatabaseModel;
using DataLayer.DatabaseModel.SearchModel;
using DataLayer.DataServiceInterfaces;
using DataLayer.DataTransferModel;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;


namespace DataLayer
{
    public class DataserviceSearches : IDataserviceSearches
    {
        public IList<PersonSearch> GetSearchResultActors(string username, string search)
        {
            using var db = new PortfolioDBContext();
            var searchResult = db.PersonSearches.FromSqlInterpolated($"select * from string_search_name({search})").ToList();
            try
            {
                //add user search
                db.Database.ExecuteSqlInterpolated($"select save_search({username},{search},{nameof(GetSearchResultActors)})");
                return searchResult;
            }
            catch
            {
                return null;
            }
        }

        public (IList<PersonSearch>? searchResult, int total) GetSearchResultActors(string username, string search, int page, int pageSize)
        {
            var allActors = GetSearchResultActors(username, search);
            var pagedActors = allActors
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToList();
            return (pagedActors, allActors.Count);
        }

        public IList<TitleSearch> GetSearchResultTitles(string username, string search)
        {
            using var db = new PortfolioDBContext();
            string[] searchArray = search.Split(" ");

            var searchResult = db.TitleSearches.FromSqlInterpolated($"select * from best_match({searchArray})").ToList();
            try
            {
                //add user search
                db.Database.ExecuteSqlInterpolated($"select save_search({username},{search},{nameof(GetSearchResultTitles)})");
                return searchResult;
            }
            catch
            {
                return null;
            }
        }

        public (IList<TitleSearch>? searchResult, int total) GetSearchResultTitles(string username, string search, int page, int pageSize)
        {
            var allTitles = GetSearchResultTitles(username, search);
            var pagedTitles = allTitles
                .OrderByDescending(x => x.Rank)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToList();
            return (pagedTitles, allTitles.Count);
        }

        public IList<TitleSearch> GetSearchResultGenres(string username, string search)
        {
            using var db = new PortfolioDBContext();
            string[] searchArray = search.RemoveSpaces().Split(",");
            var searchResult = db.TitleSearches.FromSqlInterpolated($"select * from best_match_genre({searchArray})").ToList();
            try
            {
                //add user search
                db.Database.ExecuteSqlInterpolated($"select save_search({username},{search},{nameof(GetSearchResultGenres)})");
                return searchResult;
            }
            catch
            {
                return null;
            }
        }

        public (IList<TitleSearch>? searchResult, int total) GetSearchResultGenres(string username, string search, int page, int pageSize)
        {
            var allTitles = GetSearchResultGenres(username, search);
            var pagedTitles = allTitles
                .OrderByDescending(x => x.Rank)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToList();
            return (pagedTitles, allTitles.Count);
        }


        public bool ClearSearchHistory(string username)
        {
            using var db = new PortfolioDBContext();
            var searches = db.UserSearches.Where(x => x.Username == username);
            if (searches != null)
            {
                foreach (var search in searches)
                {
                    db.UserSearches.Remove(search);
                }
                db.SaveChanges();
                return true;
            }
            return false;
        }
    }
}