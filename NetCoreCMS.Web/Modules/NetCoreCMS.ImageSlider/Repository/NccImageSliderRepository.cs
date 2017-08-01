using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Mvc.Repository;
using NetCoreCMS.ImageSlider.Models;

namespace NetCoreCMS.ImageSlider.Repository
{
    public class NccImageSliderRepository : BaseRepository<NccImageSlider, long>
    {
        public NccImageSliderRepository(NccDbContext context) : base(context)
        {
        }
    }
}
