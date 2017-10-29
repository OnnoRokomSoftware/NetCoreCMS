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
    public class NccAuthPolicy : Attribute
    {
        string _policyId;
        string _displayName;

        /// <summary>
        /// Takes unique id and display name of the handler. Which display name will show at admin panel user's permission giveing page.
        /// </summary>
        /// <param name="policyId">Unique Id of handler</param>
        /// <param name="displayName">Readable display name</param>
        public NccAuthPolicy(string policyId, string displayName)
        {
            _policyId = policyId;
            _displayName = displayName;
        }
    } 
}
