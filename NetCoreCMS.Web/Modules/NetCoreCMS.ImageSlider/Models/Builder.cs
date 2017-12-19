/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using Microsoft.EntityFrameworkCore;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.ImageSlider.Models.Entity;

namespace NetCoreCMS.ImageSlider.Models
{
    public class Builder : IModelBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NccImageSlider>().ToTable(GlobalContext.GetTableName<NccImageSlider>());
            modelBuilder.Entity<NccImageSliderItem>().ToTable(GlobalContext.GetTableName<NccImageSliderItem>());
        }
    }
}
