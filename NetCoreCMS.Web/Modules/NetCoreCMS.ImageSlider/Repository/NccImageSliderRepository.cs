using System;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using NetCoreCMS.ImageSlider.Models;
using System.Linq;
using NetCoreCMS.Framework.Core.Mvc.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NetCoreCMS.ImageSlider.Repository
{
    public class NccImageSliderRepository : BaseRepository<NccImageSlider, long>
    {
        public NccImageSliderRepository(NccDbContext context) : base(context)
        {
        }
    }
}
