using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Setup;
using System.ComponentModel.DataAnnotations;

namespace NetCoreCMS.Modules.Setup.Models.ViewModels
{
    public class SetupViewModel : DatabaseInfo
    { 
        public DatabaseEngine Database { get; set; }
        
    }
}
