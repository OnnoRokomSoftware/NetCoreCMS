/// <summary>
/// Authorization related classes are decleared inside this namespace.
/// </summary>
namespace NetCoreCMS.Framework.Core.Auth
{
    /// <summary>
    /// Permission type of a Policy. Use this names at NccAuthPolicy attribute
    /// </summary>
    public static class AuthPermission
    {
        /// <summary>
        /// Will provide data and file creation permission.
        /// </summary>
        public static readonly NccAuthRequirement Create     =  new NccAuthRequirement (AuthRequirementName.Create);
        /// <summary>
        /// Will allow read data and file content.
        /// </summary>
        public static NccAuthRequirement Read       = new NccAuthRequirement (AuthRequirementName.Read);
        /// <summary>
        /// Will allow download any resources.
        /// </summary>
        public static NccAuthRequirement Download   = new NccAuthRequirement (AuthRequirementName.Download);
        /// <summary>
        /// Update a record.
        /// </summary>
        public static NccAuthRequirement Update     = new NccAuthRequirement (AuthRequirementName.Update);
        /// <summary>
        /// Delete record or file.
        /// </summary>
        public static NccAuthRequirement Delete     = new NccAuthRequirement (AuthRequirementName.Delete);
        /// <summary>
        /// Approve any pending approval.
        /// </summary>
        public static NccAuthRequirement Approve    = new NccAuthRequirement (AuthRequirementName.Approve);
        /// <summary>
        /// Reject pending request.
        /// </summary>
        public static NccAuthRequirement Reject     = new NccAuthRequirement (AuthRequirementName.Reject);
        /// <summary>
        /// Will check is accessing data is created by logged user?
        /// </summary>
        public static NccAuthRequirement DataOwner  = new NccAuthRequirement (AuthRequirementName.IsDataOwner); 
    }
}
