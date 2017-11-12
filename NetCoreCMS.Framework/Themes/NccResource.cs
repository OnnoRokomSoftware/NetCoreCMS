/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/
 
namespace NetCoreCMS.Framework.Themes
{
    public class NccResource
    {
        public static string JQuery { get { return "JQuery"; } }
        public static string Bootstrap { get { return "Bootstrap"; } }
        public static string BootstrapDateTimePicker { get { return "BootstrapDateTimePicker"; } }
        public static string CkEditor { get { return "CkEditor"; } }
        public static string DataTable { get { return "DataTable"; } }
        public static string DataTableResponsive { get { return "DataTableResponsive"; } }
        public static string DataTableFixedColumn { get { return "DataTableFixedColumn"; } }

        public NccResource()
        {
            UseMinify = true;            
        }
        
        public string Version { get; set; }
        public string FilePath { get; set; }        
        public bool UseMinify { get; set; }
        public ResourceType Type { get; set; }
        public int Order { get; set; }
        public IncludePosition Position{ get; set; }
        
        public enum ResourceType
        {
            JsFile,
            JsContent,
            CssFile,
            CssContent,
            SassFile,
            LcssFile
        }

        public enum IncludePosition
        {
            Header,
            Footer
        }
    }
}
