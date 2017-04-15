using NetCoreCMS.Framework.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetCoreCMS.Modules.Cms.Models.ViewModels
{
    public class NccPageViewModel : NccPage
    {
        [MaxLength(10000)]
        public string PageContent {
            get {
                if(Content != null)
                {
                    return Encoding.UTF8.GetString(Content);
                }
                return string.Empty;
            }
            set {
                if (!string.IsNullOrEmpty(value))
                {
                    Content = Encoding.UTF8.GetBytes(value);
                }
                
            }
        }
        
    }
}
