using Microsoft.EntityFrameworkCore;
using NetCoreCMS.Framework.Core.Data;

namespace NetCoreCMS.AdvancedPermission.Models
{
    public class AdvancedPermissionModelBuilder : INccModuleBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NccUserPermission>().ToTable("Ncc_User_Permission");
        }
    } 
}
