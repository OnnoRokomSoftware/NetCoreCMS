using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Core.Search
{
    public interface ISearchDataSource
    {
        List<SearchModel> LoadAll();
    }
}
