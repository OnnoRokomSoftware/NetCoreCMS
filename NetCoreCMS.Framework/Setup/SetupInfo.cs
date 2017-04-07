using NetCoreCMS.Framework.Core.Data;

namespace NetCoreCMS.Framework.Setup
{
    public class SetupInfo
    {
        public string AdminUserName { get; set; }
        public string Email { get; set; }
        public string AdminPassword { get; set; }
        public DatabaseEngine Database { get; set; }
        public string ConnectionString { get; set; }
    }
}
