/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

/*************************************************************   
 *          Project: NetCoreCMS                              *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *           Mobile: +88 017 08 166 003                      *
 *************************************************************/

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
