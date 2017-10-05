using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using NetCoreCMS.ImageSlider.Models;

namespace NetCoreCMS.ImageSlider.Repository
{    
    public class NccImageSliderItemRepository : BaseRepository<NccImageSliderItem, long>
    {
        public NccImageSliderItemRepository(NccDbContext context) : base(context)
        {
        }
    }
}
