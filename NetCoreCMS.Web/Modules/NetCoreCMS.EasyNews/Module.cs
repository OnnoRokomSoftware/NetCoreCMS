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
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Modules;
using NetCoreCMS.EasyNews.Models.Entities;

namespace NetCoreCMS.EasyNews
{
    public class Module : BaseModule, IModule
    {
        public override bool Install(INccSettingsService settingsService, Func<NccDbQueryText, string> executeQuery, Func<Type, bool, int> createUpdateTable)
        {
            try
            {
                createUpdateTable(typeof(Category), false);
                createUpdateTable(typeof(CategoryDetails), false);
                createUpdateTable(typeof(News), false);
                createUpdateTable(typeof(NewsDetails), false);
                createUpdateTable(typeof(NewsCategory), false);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        
        public override bool RemoveTables(INccSettingsService settingsService, Func<NccDbQueryText, string> executeQuery, Func<Type, int> deleteTable)
        {
            try
            {
                deleteTable(typeof(NewsCategory));
                deleteTable(typeof(NewsDetails));
                deleteTable(typeof(News));
                deleteTable(typeof(CategoryDetails));
                deleteTable(typeof(Category));
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
