/*************************************************************  
 *          Project: NetCoreCMS                              *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *           Mobile: +88 017 08 166 003                      *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace NetCoreCMS.Framework.i18n
{
    /// <summary>
    /// An <see cref="IStringLocalizerFactory"/> that creates instances of <see cref="ResourceManagerStringLocalizer"/>
    /// and will properly handle the resources of ClassLibraries.
    /// </summary>
    public class ClassLibraryStringLocalizerFactory : ResourceManagerStringLocalizerFactory
    {
        private IReadOnlyDictionary<string, string> _resourcePathMappings;

        public ClassLibraryStringLocalizerFactory(
            IOptions<LocalizationOptions> localizationOptions,
            IOptions<ClassLibraryLocalizationOptions> classLibraryLocalizationOptions,
            ILoggerFactory loggerFactory)
                : base(localizationOptions, loggerFactory)
        {
            _resourcePathMappings = classLibraryLocalizationOptions.Value.ResourcePaths;
        }

        protected override string GetResourcePrefix(TypeInfo typeInfo)
        {
            var assemblyName = typeInfo.Assembly.GetName().Name;
            return GetResourcePrefix(typeInfo, assemblyName, GetResourcePath(assemblyName));
        }

        protected override string GetResourcePrefix(TypeInfo typeInfo, string baseNamespace, string resourcesRelativePath)
        {
            var assemblyName = new AssemblyName(typeInfo.Assembly.FullName);
            return base.GetResourcePrefix(typeInfo, baseNamespace, GetResourcePath(assemblyName.Name));
        }

        private string GetResourcePath(string assemblyName)
        {
            string resourcePath;
            if (!_resourcePathMappings.TryGetValue(assemblyName, out resourcePath))
            {
                //throw new KeyNotFoundException("Attempted to access an assembly which doesn't have a resourcePath set.");
                return "";
            }

            if (!string.IsNullOrEmpty(resourcePath))
            {
                resourcePath = resourcePath.Replace(Path.AltDirectorySeparatorChar, '.')
                    .Replace(Path.DirectorySeparatorChar, '.') + ".";
            }

            return resourcePath;
        }
    }
}
