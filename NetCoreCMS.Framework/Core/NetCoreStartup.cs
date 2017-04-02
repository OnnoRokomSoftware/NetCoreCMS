using Microsoft.Extensions.DependencyInjection;
using NetCoreCMS.Framework.Utility;

namespace NetCoreCMS.Framework.Core
{
    public class NetCoreStartup
    {
        public void RegisterDatabase(IServiceCollection services)
        {
            if (SetupHelper.IsComplete)
            {
                
            }            
        }
    }
}
