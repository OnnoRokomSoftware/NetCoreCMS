using Microsoft.Extensions.DependencyInjection;
using NetCoreCMS.Framework.Setup;

namespace NetCoreCMS.Framework.Core
{
    public class NetCoreStartup
    {
        public void RegisterDatabase(IServiceCollection services)
        {
            if (SetupHelper.IsDbCreateComplete)
            {
                
            }            
        }
    }
}
