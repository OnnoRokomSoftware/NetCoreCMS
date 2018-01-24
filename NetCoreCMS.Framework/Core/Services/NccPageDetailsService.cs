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
    public class NccPageDetailsService : BaseService<NccPageDetails>
    {
        private readonly NccPageDetailsRepository _entityRepository;

        public NccPageDetailsService(NccPageDetailsRepository entityRepository) : base(entityRepository)
        {
            _entityRepository = entityRepository;
        }

        #region New Method
        public NccPageDetails Get(string slug, string language)
        {
            return _entityRepository.Get(slug, language);
        }

        public List<NccPageDetails> LoadRecentPageDetails(int limit, string language = "")
        {
            return _entityRepository.LoadRecentPageDetails(limit, language);
        }

        public List<NccPageDetails> LoadAllByStatus(NccPage.NccPageStatus status)
        {
            return _entityRepository.LoadAllByStatus(status);
        }

        public List<NccPageDetails> Search(string name, string language = "")
        {
            return _entityRepository.Search(name, language);
        } 
        #endregion
    }
}
