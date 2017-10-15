using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Themes
{
    public class NccResource
    {
        public NccResource()
        {
            UseMinify = true;
            Dependencies = new List<NccResource>();
        }

        public string Name { get; set; }
        public string Version { get; set; }
        public string FilePath { get; set; }
        public string Content { get; set; }
        public bool UseMinify { get; set; }
        public ResourceType Type { get; set; }
        public int Order { get; set; }
        public IncludeLocation Location{ get; set; }
        public List<NccResource> Dependencies { get; set; }

        public enum ResourceType
        {
            JsFile,
            JsContent,
            CssFile,
            CssContent,
            SassFile,
            LcssFile
        }

        public enum IncludeLocation
        {
            Header,
            Footer
        }
    }
}
