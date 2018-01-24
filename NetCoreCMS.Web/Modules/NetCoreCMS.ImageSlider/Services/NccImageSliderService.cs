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
using NetCoreCMS.ImageSlider.Repositories;
using NetCoreCMS.ImageSlider.Models.Entities;
using NetCoreCMS.Framework.Core.Mvc.Service;

namespace NetCoreCMS.ImageSlider.Services
{
    public class NccImageSliderService : BaseService<NccImageSlider>
    {
        private readonly NccImageSliderRepository _entityRepository;

        public NccImageSliderService(NccImageSliderRepository entityRepository) : base(entityRepository, new List<string>() { "ImageItems" })
        {
            _entityRepository = entityRepository;
        }
    }
}