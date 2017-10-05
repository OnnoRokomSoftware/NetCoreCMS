namespace NetCoreCMS.Framework.Core.Models
{
    public class NccPostCategory
    {
        public long PostId { get; set; }        
        public long CategoryId { get; set; }

        public NccPost Post { get; set; }
        public NccCategory Category { get; set; }
    }
}
