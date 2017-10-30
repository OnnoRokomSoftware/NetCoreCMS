/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using NetCoreCMS.Framework.Core.Auth;
using System;
[assembly: CLSCompliant(true)]
namespace NetCoreCMS.Framework.Core.Mvc.Attributes
{
    /// <summary>
    /// NetCoreCMS default authorization attribute for controller and action.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class NccAuthorize : Attribute
    {
        private string _policyHandler;
        private string _requirement;
        private string _values;
        /// <summary>
        /// Constracture for AuthRequirement
        /// <param name="PolicyHandler">Which handler will handle this requirement. You can implement your own Authorization Handler</param>
        /// <param name="Requirement">Requirement name. Use NccAuthRequirementName class constants.</param>
        /// </summary>        
        public NccAuthorize(string PolicyHandler, string Requirement)
        {
            _policyHandler = PolicyHandler;
            _requirement = Requirement;
        }

        /// <summary>
        /// Use requirement name with value. If requirement name is Roles then pass roles coma seperated role names.
        /// </summary>
        /// <param name="PolicyHandler">Which handler will handle this requirement. You can implement your own Authorization Handler</param>
        /// <param name="Requirement">Requirement name</param>
        /// <param name="Values">If value is require use appropriate coma seperated values.</param>
        public NccAuthorize(string PolicyHandler, string Requirement, string Values)
        {
            _policyHandler = PolicyHandler;
            _requirement = Requirement;
            _values = Values;
        }
    }
}
