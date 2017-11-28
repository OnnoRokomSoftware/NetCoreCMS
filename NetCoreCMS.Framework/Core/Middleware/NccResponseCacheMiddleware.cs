using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using NetCoreCMS.Framework.Core.Mvc.Cache;

namespace NetCoreCMS.Framework.Core.Middleware
{
    public class NccResponseCacheMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly NccResponseCacheClient _cache;
        private readonly ILogger _logger;

        public NccResponseCacheMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, NccResponseCacheClient cache)
        {
            _next = next;
            _cache = cache;
            _logger = loggerFactory.CreateLogger<NccResponseCacheMiddleware>();            
        }
        
        public async Task Invoke(HttpContext context)
        {
            var key = BuildCacheKey(context);

            if (_cache.TryGet(key, out CachedPage page))
            {
                await WriteResponse(context, page);

                return;
            }

            ApplyClientHeaders(context);

            if (IsNotServerCachable(context))
            {
                await _next.Invoke(context);

                return;
            }

            page = await CaptureResponse(context);

            if (page != null)
            {
                var serverCacheDuration = GetCacheDuration(context, NccCacheKeys.ServerDuration);

                if (serverCacheDuration.TotalSeconds > 0)
                {
                    var tags = GetCacheTags(context, NccCacheKeys.Tags);
                    _cache.Set(key, page, serverCacheDuration, tags);
                }
            }
        }

        private string GetCacheTags(HttpContext context, string tags)
        {
            throw new NotImplementedException();
        }

        private TimeSpan GetCacheDuration(HttpContext context, string serverDuration)
        {
            throw new NotImplementedException();
        }

        private bool IsNotServerCachable(HttpContext context)
        {
            throw new NotImplementedException();
        }

        private string BuildCacheKey(HttpContext context)
        {
            throw new NotImplementedException();
        }

        private async Task WriteResponse(HttpContext context, CachedPage page)
        {
            foreach (var header in page.Headers)
            {
                context.Response.Headers.Add(header);
            }

            await context.Response.Body.WriteAsync(page.Content, 0, page.Content.Length);
        }

        private async Task<CachedPage> CaptureResponse(HttpContext context)
        {
            var responseStream = context.Response.Body;

            using (var buffer = new MemoryStream())
            {
                try
                {
                    context.Response.Body = buffer;

                    await _next.Invoke(context);
                }
                finally
                {
                    context.Response.Body = responseStream;
                }

                if (buffer.Length == 0) return null;

                var bytes = buffer.ToArray(); // you could gzip here

                responseStream.Write(bytes, 0, bytes.Length);

                if (context.Response.StatusCode != 200) return null;

                return BuildCachedPage(context, bytes);
            }
        }

        private CachedPage BuildCachedPage(HttpContext context, byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public void ApplyClientHeaders(HttpContext context)
        {
            context.Response.OnStarting(() =>
            {
                var clientCacheDuration = GetCacheDuration(context, NccCacheKeys.ClientDuration);

                if (clientCacheDuration.TotalSeconds > 0 && context.Response.StatusCode == 200)
                {
                    if (clientCacheDuration == TimeSpan.Zero)
                    {
                        context.Response.GetTypedHeaders().CacheControl = new CacheControlHeaderValue
                        {
                            NoCache = true,
                            NoStore = true,
                            MustRevalidate = true
                        };
                        context.Response.Headers["Expires"] = "0";
                    }
                    else
                    {
                        context.Response.GetTypedHeaders().CacheControl = new CacheControlHeaderValue
                        {
                            Public = true,
                            MaxAge = clientCacheDuration
                        };
                    }
                }

                return Task.CompletedTask;
            });
        }
    }

    internal class CachedPage
    {
        public byte[] Content { get; private set; }
        public List<KeyValuePair<string, StringValues>> Headers { get; private set; }

        public CachedPage(byte[] content)
        {
            Content = content;
            Headers = new List<KeyValuePair<string, StringValues>>();
        }
    }

}
