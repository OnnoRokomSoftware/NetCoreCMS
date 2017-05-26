using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Modules.Widgets
{
    public interface IWidget
    {
        string WidgetID { get;}
        string Title { get;}
        string Body { get;}
        string Footer { get;}
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
        string Render();
        void Prepare();
    }
}
