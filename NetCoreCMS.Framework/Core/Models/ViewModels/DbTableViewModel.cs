namespace NetCoreCMS.Framework.Core.Models.ViewModels
{
    public class DbTableViewModel
    {
        public string Database { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string ColumnType { get; set; }
        public string ColumnKey { get; set; }
        public string Extra { get; set; }
        public string ColumnComment { get; set; }
    }
}
