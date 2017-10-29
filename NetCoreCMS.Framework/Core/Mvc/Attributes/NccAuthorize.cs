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

namespace NetCoreCMS.Framework.Core.Mvc.Attributes
{
    /// <summary>
    /// NetCoreCMS default authorization attribute for controller and action.
    /// </summary>
    public class NccAuthorize : Attribute
    {
        string _policyId;
        NccAuthRequirement[] _requirements;
        NccAuthRequirementWithValue[] _requirementsWithValue;
        /// <summary>
        /// Constracture for AuthRequirement
        /// </summary>
        /// <param name="policyId">Unique policy id which want to apply on the controller/action</param>
        /// <param name="requirements">Restriction requirements. Built in requirement operations are available at AuthPermission class </param>
        public NccAuthorize(string policyId, params NccAuthRequirement[] requirements)
        {
            _policyId = policyId;
            _requirements = requirements;
        }

        /// <summary>
        /// Constracture for AuthRequirement with value
        /// </summary>
        /// <param name="policyId">Unique policy id which want to apply on the controller/action</param>
        /// <param name="requirements">Restriction requirements. Use instance of NccAuthRequirementWithValue class. At name use AuthOperationName class constant property. And at value field pass your desired value. </param>
        public NccAuthorize(string policyId, params NccAuthRequirementWithValue[] requirements)
        {
            _policyId = policyId;
            _requirementsWithValue = requirements;
        }
    }
}
