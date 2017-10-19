/*************************************************************  
 *          Project: NetCoreCMS                              *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *           Mobile: +88 017 08 166 003                      *
 *          License: BSD-3-Clause                            *
 *************************************************************/
 
using MediatR;

namespace NetCoreCMS.Framework.Core.Events.Modules
{
    public class OnModuleActivity : IRequest<ModuleActivity>
    {
        public ModuleActivity Data { get; set; }
        public OnModuleActivity(ModuleActivity data)
        {
            Data = data;
        }
    }
}
