using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core;
using NetCoreCMS.Framework.Core.Messages;
using NetCoreCMS.Framework.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NetCoreCMS.Framework.Themes
{
    public class ThemeManager
    {
        public static readonly string ThemeInfoFileName = "Theme.json";
        ThemeManager _themeManager;
        ILoggerFactory _loggerFactory;
        ILogger _logger;
 
        public ThemeManager(ILoggerFactory factory)
        {
            ILoggerFactory _loggerFactory = factory;
            _logger = _loggerFactory.CreateLogger<ThemeManager>();
        }

        public List<Theme> ScanThemeDirectory(string path)
        {
            List<Theme> themes = new List<Theme>();
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
                    _logger.LogError(ex.ToString());
                }                
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
                            var themeJson = JsonConvert.SerializeObject(theme);
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
                _logger.LogError(ex.ToString());
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
                        var themeJson = JsonConvert.SerializeObject(theme);
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
                _logger.LogError(ex.ToString());
            }
            return false;
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
