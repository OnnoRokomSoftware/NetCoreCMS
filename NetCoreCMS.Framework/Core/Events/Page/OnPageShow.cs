using MediatR;
using NetCoreCMS.Framework.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

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
