using System;
using System.Collections.Generic;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using NetCoreCMS.Modules.Analytics.Models;
using Microsoft.EntityFrameworkCore;

namespace NetCoreCMS.Modules.Analytics.Repository
{
    public class AnalyticsRepository : BaseRepository<AnalyticsModel, long>
    {
        public AnalyticsRepository(NccDbContext context) : base(context)
        {
        }

        public List<ViewAnalyticsModel> LoadAllWithCount()
        {
            NccDbQueryText query = new NccDbQueryText();
            query.MySql_QueryText = @"
                SELECT 
                    Id, VersionNumber, `Name`, CreationDate, ModificationDate, CreateBy, ModifyBy, `Status`, `Key`
                    , SUM(total) TotalLog, SUM(today) TotalLogToday, SUM(last24) TotalLogLast24
                FROM (
	                SELECT DISTINCT a.*, COUNT(*) total, 0 AS today, 0 last24
	                FROM `ncc_analytics` AS a
	                LEFT JOIN `ncc_analytics_log` AS l ON a.`Id` = l.`AnalyticsModelId`
	                GROUP BY a.Id
	                UNION ALL
	                SELECT DISTINCT a.*, 0 AS total, COUNT(*) today, 0 last24
	                FROM `ncc_analytics` AS a
	                LEFT JOIN `ncc_analytics_log` AS l ON a.`Id` = l.`AnalyticsModelId`
	                WHERE l.CreationDate > CURDATE()
	                GROUP BY a.Id
	                UNION ALL
	                SELECT DISTINCT a.*, 0 AS total, 0 AS today, COUNT(*) last24
	                FROM `ncc_analytics` AS a
	                LEFT JOIN `ncc_analytics_log` AS l ON a.`Id` = l.`AnalyticsModelId`
	                WHERE l.CreationDate > (DATE_ADD(CURDATE(),INTERVAL -24 HOUR))
	                GROUP BY a.Id
                ) AS a
                GROUP BY a.Id
            ";
            return ExecuteSqlQuery<ViewAnalyticsModel>(query);
        }
    }
}