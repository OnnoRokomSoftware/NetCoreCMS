/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/
 
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Collections.Generic;

using NetCoreCMS.Framework.Core;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.Framework.Core.Messages;
using NetCoreCMS.Framework.Modules.Widgets;

using Microsoft.CodeAnalysis;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;
namespace NetCoreCMS.Framework.Themes
{
    public class ThemeManager
    {
        public static readonly string ThemeInfoFileName = "Theme.json";        
        private List<Assembly> _themeDlls;

        public ThemeManager()
        {
            _themeDlls = new List<Assembly>();
        }
        
        public List<Theme> ScanThemeDirectory(string path)
        {
            var themes = new List<Theme>();
            var directoryInfo = new DirectoryInfo(path);
            foreach (var themeDir in directoryInfo.EnumerateDirectories())
            {
                try
                {
                    var configFileLocation = Path.Combine(themeDir.FullName, ThemeInfoFileName);
                    if (File.Exists(configFileLocation))
                    {
                        var themeInfoFileContent = File.ReadAllText(configFileLocation);
                        var theme = JsonConvert.DeserializeObject<Theme>(themeInfoFileContent);
                        theme.Folder = themeDir.Name;
                        theme.ConfigFilePath = configFileLocation;
                        
                        if (Directory.Exists(themeDir.FullName + "\\Bin\\Debug\\netcoreapp2.0"))
                        {
                            theme.ResourceFolder    = themeDir.FullName + "\\Bin\\Debug\\netcoreapp2.0\\Resources";
                            theme.AssemblyPath      = themeDir.FullName + "\\Bin\\Debug\\netcoreapp2.0\\" + theme.ThemeName + ".dll";
                        }
                        else if(Directory.Exists(themeDir.FullName + "\\Bin\\Debug\\netcoreapp2.0"))
                        {
                            theme.AssemblyPath      = themeDir.FullName + "\\Bin\\Release\\netcoreapp2.0\\" + theme.ThemeName + ".dll";
                            theme.ResourceFolder    = themeDir.FullName + "\\Bin\\Release\\netcoreapp2.0\\Resources";
                        }
                        
                        if (string.IsNullOrEmpty(theme.AssemblyPath) == false && File.Exists(theme.AssemblyPath))
                        {                            
                            var themeAssembly = Assembly.LoadFile(theme.AssemblyPath);
                            themes.Add(theme);
                            if (theme.IsActive)
                            {
                                ThemeHelper.ActiveTheme     = theme;                                
                            }
                            GlobalContext.Themes.Add(theme);
                        }
                    }
                    else
                    {
                        RegisterErrorMessage("Theme config file Theme.json not found");
                    }                    

                }
                catch(Exception ex)
                {
                    RegisterErrorMessage(ex.Message);
                    throw ex;
                }                
            }

            if(ThemeHelper.ActiveTheme == null)
            {
                ActivateDefaultTheme();
            }

            return themes;
        }

        public bool ActivateTheme(string themeName)
        {
            try
            {
                var infoFileLocation = Path.Combine(GlobalContext.ContentRootPath, NccInfo.ThemeFolder, themeName,ThemeInfoFileName);
                if (File.Exists(infoFileLocation))
                {
                    var themeInfoFileContent = File.ReadAllText(infoFileLocation);
                    var theme = JsonConvert.DeserializeObject<Theme>(themeInfoFileContent);
                    
                    if (theme.IsActive == false)
                    {
                        if (InactivateTheme(ThemeHelper.ActiveTheme.ThemeName))
                        {
                            theme.IsActive = true;                           
                            var themeJson = JsonConvert.SerializeObject(theme,Formatting.Indented);
                            File.WriteAllText(infoFileLocation, themeJson);                            
                            ThemeHelper.ActiveTheme     = theme;
                        }
                        else
                        {
                            RegisterErrorMessage("Previous theme inactivation failed.");
                        } 
                    }

                    return true;
                }
                else
                {
                    RegisterErrorMessage("Theme config file Theme.json not found");
                }

            }
            catch (Exception ex)
            {
                RegisterErrorMessage(ex.Message);
                throw ex;
            }
            return false;
        }
        
        public bool InactivateTheme(string themeName)
        {
            try
            {
                var infoFileLocation = Path.Combine(GlobalContext.ContentRootPath, NccInfo.ThemeFolder, themeName, ThemeInfoFileName);
                if (File.Exists(infoFileLocation))
                {
                    var themeInfoFileContent = File.ReadAllText(infoFileLocation);
                    var theme = JsonConvert.DeserializeObject<Theme>(themeInfoFileContent);

                    if (theme.IsActive == true)
                    {
                        theme.IsActive = false;
                        //GlobalConfig.ActiveTheme = theme;
                        var themeJson = JsonConvert.SerializeObject(theme, Formatting.Indented);
                        File.WriteAllText(infoFileLocation, themeJson);
                    }

                    return true;
                }
                else
                {
                    RegisterErrorMessage("Theme config file Theme.json not found");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }

        public bool ActivateDefaultTheme()
        {
            try
            {
                var infoFileLocation = Path.Combine(GlobalContext.ContentRootPath, NccInfo.ThemeFolder, "Default", ThemeInfoFileName);
                if (File.Exists(infoFileLocation))
                {
                    var themeInfoFileContent = File.ReadAllText(infoFileLocation);
                    var theme = JsonConvert.DeserializeObject<Theme>(themeInfoFileContent);
                    theme.IsActive = true;                    
                    var themeJson = JsonConvert.SerializeObject(theme, Formatting.Indented);
                    File.WriteAllText(infoFileLocation, themeJson);                    
                    ThemeHelper.ActiveTheme     = theme;
                    return true;
                }
                else
                {
                    RegisterErrorMessage("Theme config file Theme.json not found");
                }

            }
            catch (Exception ex)
            {
                RegisterErrorMessage(ex.Message);
                throw ex;
            }
            return false;
        }

        public void RegisterThemes(IMvcBuilder mvcBuilder, IServiceCollection services, IServiceProvider serviceProvider, IDirectoryContents themes )
        {
            _themeDlls = new List<Assembly>();
            foreach (var themeFolder in themes.Where(x => x.IsDirectory))
            {
                try
                {
                    var binFolder = new DirectoryInfo(Path.Combine(themeFolder.PhysicalPath, "bin"));
                    if (!binFolder.Exists)
                    {
                        continue;
                    }

                    foreach (var file in binFolder.GetFileSystemInfos("*.dll", SearchOption.AllDirectories))
                    {
                        Assembly assembly;
                        try
                        {
                            assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(file.FullName);
                        }
                        catch (FileLoadException ex)
                        {
                            continue;
                        }
                        catch (BadImageFormatException ex)
                        {
                            continue;
                        }

                        if (assembly.FullName.Contains(themeFolder.Name))
                        {
                            _themeDlls.Add(assembly);
                            if (ThemeHelper.ActiveTheme.Folder == themeFolder.Name)
                            {
                                mvcBuilder.AddApplicationPart(assembly);
                                var widgetTypeList = assembly.GetTypes().Where(x => typeof(Widget).IsAssignableFrom(x)).ToList();
                                foreach (var widgetType in widgetTypeList)
                                {         
                                    services.AddTransient(widgetType);                                    
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Could not load theme from " + themeFolder);
                }
            } 
            
            mvcBuilder.AddRazorOptions(o =>
            {
                foreach (var theme in _themeDlls)
                {
                    o.AdditionalCompilationReferences.Add(MetadataReference.CreateFromFile(theme.Location));
                }
            });
        }

        public List<Widget> RegisterThemeWidgets(IMvcBuilder mvcBuilder, IServiceCollection services, IServiceProvider serviceProvider, IDirectoryContents themes)
        {
            var widgets = new List<Widget>();

            foreach (var themeFolder in themes.Where(x => x.IsDirectory))
            {
                if (ThemeHelper.ActiveTheme.Folder == themeFolder.Name)
                {
                    var assembly = _themeDlls.Where(x => x.ManifestModule.Name == themeFolder.Name+".dll").FirstOrDefault();
                    if(assembly != null)
                    {
                        var widgetTypeList = assembly.GetTypes().Where(x => typeof(Widget).IsAssignableFrom(x)).ToList();
                        foreach (var widgetType in widgetTypeList)
                        {                            
                            //var widgetInstance = (IWidget)Activator.CreateInstance(widgetType);                            
                            var widgetInstance = (Widget)serviceProvider.GetService(widgetType);
                            widgets.Add(widgetInstance);
                            ThemeHelper.ActiveTheme.Widgets.Add(widgetInstance);
                            GlobalContext.Widgets.Add(widgetInstance);
                        }
                    }
                }
            }
            return widgets;
        } 

        private void RegisterErrorMessage(string message)
        {
            GlobalMessageRegistry.RegisterMessage(
                new GlobalMessage()
                {
                    Registrater = typeof(ThemeManager).Name,
                    Text = message,
                    Type = GlobalMessage.MessageType.Error,
                    For = GlobalMessage.MessageFor.Admin
                },
                new TimeSpan(0, 1, 0)
            );
        }
    }
}
