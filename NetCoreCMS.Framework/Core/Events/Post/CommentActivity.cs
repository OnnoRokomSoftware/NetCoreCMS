using NetCoreCMS.Framework.Core.Models;
namespace NetCoreCMS.Framework.Core.Events.Post
{
    public class CommentActivity
    {
        public NccPost Post{ get; set; }
        public NccComment Comment { get; set; }
        public NccUser Author { get; set; }
        public Type ActionType { get; set; }

        public enum Type
        {
            NewComment,
            CommentApproved,
            CommentRejected,
            CommentDeleted
        }
    }
}
