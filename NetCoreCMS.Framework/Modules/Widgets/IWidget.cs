using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreCMS.Framework.Modules.Widgets
{
    public interface IWidget
    {
        string WidgetId { get;}
        string Title { get;}
        string Description { get; }
        string Body { get;}
        string Footer { get;}
        string ConfigJson { get; }
        string ViewFileName { get; }
        /// <summary>
        /// {{%%BODY%%}} use inside your html design so that widget can replace his body content at render time
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        string RenderBody(string html="");
        /// <summary>
        /// {{%%TITLE%%}} use inside your html design so that widget can replace his body content at render time
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        string RenderTitle(string html="");
        /// <summary>
        /// {{%%FOOTER%%}} use inside your html design so that widget can replace his body content at render time
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        string RenderFooter(string html="");
        /// <summary>
        /// Will render complete widget using default design
        /// </summary>
        /// <returns></returns>
        string RenderBody();
        string RenderConfig();
        void Init();
    }
}
