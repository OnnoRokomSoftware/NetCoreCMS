/*
 * Author: Xonaki
 * Website: http://xonaki.com
 * Copyright (c) xonaki.com
 * License: BSD (3 Clause)
*/
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
