using System.IO;
using System.Threading.Tasks;
using MediatR.Pipeline;

namespace NetCoreCMS.Framework.Core.Events.Behaviours
{
    public class GenericRequestPreProcessor<TRequest> : IRequestPreProcessor<TRequest>
    {
        public GenericRequestPreProcessor()
        {
            
        }

        public Task Process(TRequest request)
        {
            return Task.FromResult(0);
        }
    }
}