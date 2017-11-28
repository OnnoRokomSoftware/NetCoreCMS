using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Core.Search
{
    public interface SearchModel
    {
        string Title { get; set; }
        string Description { get; set; }
        string SearchFiledOne { get; set; }
        string SearchFiledTwo { get; set; }
        string SearchFiledThree { get; set; }
        string SearchFiledFour { get; set; }
        string SearchFiledFive { get; set; }
        string SearchFiledSix { get; set; }
        string SearchFiledSeven { get; set; }
        string SearchFiledEight { get; set; }
        string SearchFiledNine { get; set; }
        string SearchFiledTen { get; set; }
        
        string GetViewUrl();        
    }
}
