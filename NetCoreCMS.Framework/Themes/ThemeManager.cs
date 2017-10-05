using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core;
using NetCoreCMS.Framework.Core.Messages;
using NetCoreCMS.Framework.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.CodeAnalysis;
using NetCoreCMS.Framework.Modules.Widgets;

namespace NetCoreCMS.Framework.Themes
{
    public class ThemeManager
    {
        public static readonly string ThemeInfoFileName = "Theme.json";        
        static List<Assembly> _themeDlls;
        
        public ThemeManager(){}

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
                        theme.ResourceFolder = themeDir.FullName + "\\Bin\\Debug\\netcoreapp2.0\\Resources";

                        var themeAssemblyPath = themeDir.FullName + "\\Bin\\Debug\\netcoreapp2.0\\" + theme.ThemeName + ".dll";
                        var themeAssembly = Assembly.LoadFile(themeAssemblyPath);

                        if (RuntimeUtil.IsRelease(themeAssembly)) 
                        {
                            theme.ResourceFolder = themeDir.FullName + "\\Bin\\Release\\netcoreapp2.0\\Resources";
                        }

                        themes.Add(theme);
                        
                        if (theme.IsActive)
                        {
                            GlobalConfig.ActiveTheme = theme;
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

            if(GlobalConfig.ActiveTheme == null)
            {
                ActivateDefaultTheme();
            }
            return themes;
        }

        public bool ActivateTheme(string themeName)
        {
            try
            {
                var infoFileLocation = Path.Combine(GlobalConfig.ContentRootPath, NccInfo.ThemeFolder, themeName,ThemeInfoFileName);
                if (File.Exists(infoFileLocation))
                {
                    var themeInfoFileContent = File.ReadAllText(infoFileLocation);
                    var theme = JsonConvert.DeserializeObject<Theme>(themeInfoFileContent);
                    
                    if (theme.IsActive == false)
                    {
                        if (InactivateTheme(GlobalConfig.ActiveTheme.ThemeName))
                        {
                            theme.IsActive = true;
                            GlobalConfig.ActiveTheme = theme;
                            var themeJson = JsonConvert.SerializeObject(theme,Formatting.Indented);
                            File.WriteAllText(infoFileLocation, themeJson);
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
                var infoFileLocation = Path.Combine(GlobalConfig.ContentRootPath, NccInfo.ThemeFolder, themeName, ThemeInfoFileName);
                if (File.Exists(infoFileLocation))
                {
                    var themeInfoFileContent = File.ReadAllText(infoFileLocation);
                    var theme = JsonConvert.DeserializeObject<Theme>(themeInfoFileContent);

                    if (theme.IsActive == true)
                    {
                        theme.IsActive = false;
                        GlobalConfig.ActiveTheme = theme;
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
                var infoFileLocation = Path.Combine(GlobalConfig.ContentRootPath, NccInfo.ThemeFolder, "Default", ThemeInfoFileName);
                if (File.Exists(infoFileLocation))
                {
                    var themeInfoFileContent = File.ReadAllText(infoFileLocation);
                    var theme = JsonConvert.DeserializeObject<Theme>(themeInfoFileContent);
                    theme.IsActive = true;
                    GlobalConfig.ActiveTheme = theme;
                    var themeJson = JsonConvert.SerializeObject(theme, Formatting.Indented);
                    File.WriteAllText(infoFileLocation, themeJson); 
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
                            if (GlobalConfig.ActiveTheme.Folder == themeFolder.Name)
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
                if (GlobalConfig.ActiveTheme.Folder == themeFolder.Name)
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
                            GlobalConfig.ActiveTheme.Widgets.Add(widgetInstance);
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
