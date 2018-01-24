/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using System.Collections.Generic;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Repository;
using NetCoreCMS.Framework.Core.Mvc.Service;

namespace NetCoreCMS.Framework.Core.Services
{
    public class NccCategoryDetailsService : BaseService<NccCategoryDetails>
    {
        private readonly NccCategoryDetailsRepository _entityRepository;

        public NccCategoryDetailsService(NccCategoryDetailsRepository entityRepository) : base(entityRepository)
        {
            _entityRepository = entityRepository;
        }

        #region New Methods
        public NccCategoryDetails Get(string slug, string language)
        {
            return _entityRepository.Get(slug, language);
        }

        public List<NccCategoryDetails> LoadByParrentId(long parrentId, bool isActive = true)
        {
            return _entityRepository.LoadByParrentId(parrentId, isActive);
        }

        public List<NccCategoryDetails> LoadRecentCategoryDetails(int count, string language = "")
        {
            return _entityRepository.LoadRecentCategoryDetails(count, language);
        }

        public List<NccCategoryDetails> Search(string name, string language = "", int count = 20)
        {
            return _entityRepository.Search(name, language, count);
        } 
        #endregion
    }
}