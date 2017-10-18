using MediatR;
using NetCoreCMS.Framework.Core.Models;

namespace NetCoreCMS.Framework.Core.Events.Page
{
    public class OnPageShow : IRequest<NccPage>
    {
        public NccPage Page { get; set; }
        public OnPageShow(NccPage page)
        {
            Page = page;
        }
    }
}
