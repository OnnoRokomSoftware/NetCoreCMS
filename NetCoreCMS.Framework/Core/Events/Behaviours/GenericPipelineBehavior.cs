using MediatR;
using System.IO;
using System.Threading.Tasks;

namespace NetCoreCMS.Framework.Core.Events.Behaviours
{
    public class GenericPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        public GenericPipelineBehavior()
        {
            
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next)
        {            
            var response = await next();            
            return response;
        }
    }
}
