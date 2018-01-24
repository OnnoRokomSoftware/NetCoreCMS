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
using System.Collections.Generic;
using NetCoreCMS.Framework.Modules;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.ImageSlider.Models.Entities;

namespace NetCoreCMS.Modules.ImageSlider
{
    public class Module : BaseModule, IModule
    {

        public override bool IsMultilangual { get { return false; } }

        public override List<SupportedDatabases> Databases
        {
            get { return new List<SupportedDatabases>() { SupportedDatabases.MySql }; }
        }
        
        public override bool Install(INccSettingsService settingsService, Func<NccDbQueryText, string> executeQuery, Func<Type, bool, int> createUpdateTable)
        {
            try
            {
                createUpdateTable(typeof(NccImageSlider), false);
                createUpdateTable(typeof(NccImageSliderItem), false);
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
                deleteTable(typeof(NccImageSliderItem));
                deleteTable(typeof(NccImageSlider));
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }                
    }
}
