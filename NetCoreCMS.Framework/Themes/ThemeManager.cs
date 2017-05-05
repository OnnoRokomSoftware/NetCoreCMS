using Microsoft.Extensions.Logging;
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
        ILogger _logger;

        public ThemeManager(ILogger logger)
        {
            _logger = logger;
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
                        GlobalMessageRegistry.RegisterMessage(
                            new GlobalMessage()
                            {
                                Registrater = typeof(ThemeManager).Name,
                                Text = "Theme config file Theme.json not found",
                                Type = GlobalMessage.MessageType.Error,
                                For = GlobalMessage.MessageFor.Admin
                            },
                            new TimeSpan(0, 1, 0)
                        );
                    }                    

                }
                catch(Exception ex)
                {
                    GlobalMessageRegistry.RegisterMessage(
                           new GlobalMessage()
                           {
                               Registrater = typeof(ThemeManager).Name,
                               Text = ex.Message,
                               Type = GlobalMessage.MessageType.Fatal,
                               For = GlobalMessage.MessageFor.Both
                           },
                           new TimeSpan(0, 1, 0)
                       );
                    _logger.LogError(ex.ToString());
                }                
            }
            return themes;
        }
    }
}
