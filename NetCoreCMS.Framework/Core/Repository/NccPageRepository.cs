/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using System;
using System.Linq;
using System.Collections.Generic;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using NetCoreCMS.Framework.Core.Models.ViewModels;
using NetCoreCMS.Framework.Core.Mvc.Models;
using Microsoft.EntityFrameworkCore;
using NetCoreCMS.Framework.Utility;

namespace NetCoreCMS.Framework.Core.Repository
{
    public class NccPageRepository : BaseRepository<NccPage, long>
    {
        public NccPageRepository(NccDbContext context) : base(context)
        {
        }

        public List<NccPage> LoadRecentPages(int count)
        {
            var list = Query()
                .Where(x => x.PageStatus == NccPage.NccPageStatus.Published)
                .OrderByDescending(x => x.CreationDate)
                .Take(count)
                .ToList();
            return list;
        }

        public long Count(bool isActive, string keyword)
        {
            return GetBaseQuery(isActive, keyword).Count();
        }

        public List<NccPage> Load(int from, int total, bool isActive, string keyword, string orderBy, string orderDir)
        {
            var query = GetBaseQuery(isActive, keyword);

            if (orderBy.ToLower() == "name")
            {
                if (orderDir.ToLower() == "asc")
                    query = query.OrderBy(x => x.Name);
                else
                    query = query.OrderByDescending(x => x.Name);
            }

            query = query.OrderByDescending(x => x.CreationDate);
            return query.Skip(from).Take(total).ToList();
        }

        public long SearchCount(string keyword)
        {
            NccDbQueryText query = new NccDbQueryText();
            query.MySql_QueryText = GetBaseSearchQuery(keyword);


            query.MySql_QueryText = "SELECT COUNT(*) Count FROM ( " + query.MySql_QueryText + " ) AS a";
            return ExecuteSqlQuery<CountViewModel>(query).FirstOrDefault().Count;
        }

        public List<NccSearchViewModel> SearchLoad(int from, int total, string keyword)
        {
            NccDbQueryText query = new NccDbQueryText();
            query.MySql_QueryText = GetBaseSearchQuery(keyword);
            query.MySql_QueryText = query.MySql_QueryText + " LIMIT " + from + ", " + total;
            return ExecuteSqlQuery<NccSearchViewModel>(query);
        }

        #region Helper
        private IQueryable<NccPage> GetBaseQuery(bool isActive, string keyword)
        {
            var baseQuery = Query().Include("PageDetails").Include("Parent").Where(x => x.Status != EntityStatus.Deleted);

            if (isActive == true)
            {
                baseQuery = baseQuery.Where(x => x.Status == EntityStatus.Active);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                baseQuery = baseQuery.Where(x => x.Name.Contains(keyword) || x.PageDetails.Any(y => y.Title.Contains(keyword)));
            }
            return baseQuery;
        }

        private string GetBaseSearchQuery(string keyword)
        {
            keyword = keyword.Trim();
            string descriptionField = "MetaDescription";
            string languageField = "'/',`Language`,";
            if (GlobalContext.WebSite.IsMultiLangual == false)
            {
                languageField = "";
            }
            var keywords = keyword.Split(new char[] { ' ', '+', ',' });
            var keywordsAnd = "";
            var keywordsOr = "";
            foreach (var item in keywords)
            {
                keywordsAnd += " AND Title LIKE '%" + keyword + @"%' ";
                if (keywordsOr == "")
                    keywordsOr += " AND ( Title LIKE '%" + keyword + @"%' ";
                else
                    keywordsOr += " OR Title LIKE '%" + keyword + @"%' ";
            }
            keywordsOr += ") ";
            int rank = 1;
            var baseQuery = @"SELECT Title, Url, IFNULL(Description,'') as Description, MIN(Rank) as Rank FROM ( 
                                SELECT Title, CONCAT("+ languageField + @"'/',slug) AS Url, " + descriptionField + @" AS Description, " + (rank++) + @" AS Rank FROM ncc_page_details WHERE `Status` = " + EntityStatus.Active + @" AND Title LIKE '" + keyword + @"'
                                UNION ALL SELECT Title, CONCAT(" + languageField + @"'/Post/',slug) AS Url, " + descriptionField + @" AS Description, " + (rank++) + @" AS Rank FROM ncc_post_details WHERE `Status` = " + EntityStatus.Active + @" AND Title LIKE '" + keyword + @"'

                                UNION ALL SELECT Title, CONCAT("+ languageField + @"'/',slug) AS Url, " + descriptionField + @" AS Description, " + (rank++) + @" AS Rank FROM ncc_page_details WHERE `Status` = " + EntityStatus.Active + @" AND Title LIKE '" + keyword + @"%'
                                UNION ALL SELECT Title, CONCAT("+ languageField + @"'/Post/',slug) AS Url, " + descriptionField + @" AS Description, " + (rank++) + @" AS Rank FROM ncc_post_details WHERE `Status` = " + EntityStatus.Active + @" AND Title LIKE '" + keyword + @"%'

                                UNION ALL SELECT Title, CONCAT("+ languageField + @"'/',slug) AS Url, " + descriptionField + @" AS Description, " + (rank++) + @" AS Rank FROM ncc_page_details WHERE `Status` = " + EntityStatus.Active + @" AND Title LIKE '%" + keyword + @"%'
                                UNION ALL SELECT Title, CONCAT("+ languageField + @"'/Post/',slug) AS Url, " + descriptionField + @" AS Description, " + (rank++) + @" AS Rank FROM ncc_post_details WHERE `Status` = " + EntityStatus.Active + @" AND Title LIKE '%" + keyword + @"%'

                                UNION ALL SELECT Title, CONCAT("+ languageField + @"'/',slug) AS Url, " + descriptionField + @" AS Description, " + (rank++) + @" AS Rank FROM ncc_page_details WHERE `Status` = " + EntityStatus.Active + @" " + keywordsAnd + @"
                                UNION ALL SELECT Title, CONCAT("+ languageField + @"'/Post/',slug) AS Url, " + descriptionField + @" AS Description, " + (rank++) + @" AS Rank FROM ncc_post_details WHERE `Status` = " + EntityStatus.Active + @" " + keywordsAnd + @"

                                UNION ALL SELECT Title, CONCAT("+ languageField + @"'/',slug) AS Url, " + descriptionField + @" AS Description, " + (rank++) + @" AS Rank FROM ncc_page_details WHERE `Status` = " + EntityStatus.Active + @" " + keywordsOr + @"
                                UNION ALL SELECT Title, CONCAT("+ languageField + @"'/Post/',slug) AS Url, " + descriptionField + @" AS Description, " + (rank++) + @" AS Rank FROM ncc_post_details WHERE `Status` = " + EntityStatus.Active + @" " + keywordsOr + @"



                                UNION ALL SELECT Title, CONCAT("+ languageField + @"'/',slug) AS Url, " + descriptionField + @" AS Description, " + (rank++) + @" AS Rank FROM ncc_page_details WHERE `Status` = " + EntityStatus.Active + @" AND Slug LIKE '" + keyword + @"%'
                                UNION ALL SELECT Title, CONCAT("+ languageField + @"'/Post/',slug) AS Url, " + descriptionField + @" AS Description, " + (rank++) + @" AS Rank FROM ncc_post_details WHERE `Status` = " + EntityStatus.Active + @" AND Slug LIKE '" + keyword + @"%'

                                UNION ALL SELECT Title, CONCAT("+ languageField + @"'/',slug) AS Url, " + descriptionField + @" AS Description, " + (rank++) + @" AS Rank FROM ncc_page_details WHERE `Status` = " + EntityStatus.Active + @" AND Slug LIKE '%" + keyword + @"%'
                                UNION ALL SELECT Title, CONCAT("+ languageField + @"'/Post/',slug) AS Url, " + descriptionField + @" AS Description, " + (rank++) + @" AS Rank FROM ncc_post_details WHERE `Status` = " + EntityStatus.Active + @" AND Slug LIKE '%" + keyword + @"%'

                                UNION ALL SELECT Title, CONCAT("+ languageField + @"'/',slug) AS Url, " + descriptionField + @" AS Description, " + (rank++) + @" AS Rank FROM ncc_page_details WHERE `Status` = " + EntityStatus.Active + @" " + keywordsAnd.Replace("Title", "Slug") + @"
                                UNION ALL SELECT Title, CONCAT("+ languageField + @"'/Post/',slug) AS Url, " + descriptionField + @" AS Description, " + (rank++) + @" AS Rank FROM ncc_post_details WHERE `Status` = " + EntityStatus.Active + @" " + keywordsAnd.Replace("Title", "Slug") + @"

                                UNION ALL SELECT Title, CONCAT("+ languageField + @"'/',slug) AS Url, " + descriptionField + @" AS Description, " + (rank++) + @" AS Rank FROM ncc_page_details WHERE `Status` = " + EntityStatus.Active + @" " + keywordsOr.Replace("Title", "Slug") + @"
                                UNION ALL SELECT Title, CONCAT("+ languageField + @"'/Post/',slug) AS Url, " + descriptionField + @" AS Description, " + (rank++) + @" AS Rank FROM ncc_post_details WHERE `Status` = " + EntityStatus.Active + @" " + keywordsOr.Replace("Title", "Slug") + @"



                                UNION ALL SELECT Title, CONCAT("+ languageField + @"'/',slug) AS Url, " + descriptionField + @" AS Description, " + (rank++) + @" AS Rank FROM ncc_page_details WHERE `Status` = " + EntityStatus.Active + @" AND MetaDescription LIKE '" + keyword + @"%'
                                UNION ALL SELECT Title, CONCAT("+ languageField + @"'/Post/',slug) AS Url, " + descriptionField + @" AS Description, " + (rank++) + @" AS Rank FROM ncc_post_details WHERE `Status` = " + EntityStatus.Active + @" AND MetaDescription LIKE '" + keyword + @"%'

                                UNION ALL SELECT Title, CONCAT("+ languageField + @"'/',slug) AS Url, " + descriptionField + @" AS Description, " + (rank++) + @" AS Rank FROM ncc_page_details WHERE `Status` = " + EntityStatus.Active + @" AND MetaDescription LIKE '%" + keyword + @"%'
                                UNION ALL SELECT Title, CONCAT("+ languageField + @"'/Post/',slug) AS Url, " + descriptionField + @" AS Description, " + (rank++) + @" AS Rank FROM ncc_post_details WHERE `Status` = " + EntityStatus.Active + @" AND MetaDescription LIKE '%" + keyword + @"%'

                                UNION ALL SELECT Title, CONCAT("+ languageField + @"'/',slug) AS Url, " + descriptionField + @" AS Description, " + (rank++) + @" AS Rank FROM ncc_page_details WHERE `Status` = " + EntityStatus.Active + @" " + keywordsAnd.Replace("Title", "MetaDescription") + @"
                                UNION ALL SELECT Title, CONCAT("+ languageField + @"'/Post/',slug) AS Url, " + descriptionField + @" AS Description, " + (rank++) + @" AS Rank FROM ncc_post_details WHERE `Status` = " + EntityStatus.Active + @" " + keywordsAnd.Replace("Title", "MetaDescription") + @"

                                UNION ALL SELECT Title, CONCAT("+ languageField + @"'/',slug) AS Url, " + descriptionField + @" AS Description, " + (rank++) + @" AS Rank FROM ncc_page_details WHERE `Status` = " + EntityStatus.Active + @" " + keywordsOr.Replace("Title", "MetaDescription") + @"
                                UNION ALL SELECT Title, CONCAT("+ languageField + @"'/Post/',slug) AS Url, " + descriptionField + @" AS Description, " + (rank++) + @" AS Rank FROM ncc_post_details WHERE `Status` = " + EntityStatus.Active + @" " + keywordsOr.Replace("Title", "MetaDescription") + @"

                            ) AS a
                            GROUP BY Title, Url, Description
                            ORDER BY Rank ASC 
            ";
            return baseQuery;
        }
        #endregion
    }
}
