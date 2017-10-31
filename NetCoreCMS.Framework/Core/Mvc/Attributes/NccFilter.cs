/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using System;

namespace NetCoreCMS.Framework.Core.Mvc.Attributes
{
    /// <summary>
    /// Attribute for identifying NccAuthRequireHandler
    /// </summary>
    public class NccFilter : Attribute
    {
        private readonly int order;

        /// <summary>
        /// Takes unique id and display name of the handler. Which display name will show at admin panel user's permission giveing page.
        /// </summary>
        /// <param name="Order">Execution order</param>
        public NccFilter(int Order)
        {
            order = Order;
        }
    } 
}
