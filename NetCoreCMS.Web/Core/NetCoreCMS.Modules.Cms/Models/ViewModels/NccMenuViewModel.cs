using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Modules.Cms.Models.ViewModels
{
    public class NccMenuViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public List<NccMenuItemViewModel> Items { get; set; }
    }

    public class NccMenuItemViewModel
    {
        public long Id { get; set; }
        public NccMenuItemViewModel Parent { get; set; }
        public long ParentId { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Data { get; set; }
        public string url { get; set; }
        public string Target { get; set; }
        public string Order { get; set; }
        public List<NccMenuItemViewModel> Childrens { get; set; }        
    }
}
