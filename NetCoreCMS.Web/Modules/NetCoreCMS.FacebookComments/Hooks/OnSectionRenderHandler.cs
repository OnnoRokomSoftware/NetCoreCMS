/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/
 
using MediatR;
using Microsoft.Extensions.Logging;
using NetCoreCMS.FacebookComments.Controllers;
using NetCoreCMS.Framework.Core.Events.Themes;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.i18n;
using NetCoreCMS.Modules.FacebookComments.Models;
using Newtonsoft.Json;
using System;

namespace NetCoreCMS.HelloWorld.Hooks
{
    public class OnSectionRenderHandler : IRequestHandler<OnThemeSectionRender, ThemeSection>
    {
        private FacebookCommentsSettings nccFacebookCommentsSettings;
        public OnSectionRenderHandler(NccSettingsService nccSettingsService, ILoggerFactory factory)
        {
            var _logger = factory.CreateLogger<FacebookCommentsHomeController>();
            try
            {
                var tempSettings = nccSettingsService.GetByKey("NccFacebookComments_Settings");
                if (tempSettings != null)
                {
                    nccFacebookCommentsSettings = JsonConvert.DeserializeObject<FacebookCommentsSettings>(tempSettings.Value);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }

        public ThemeSection Handle(OnThemeSectionRender message)
        {
            if (nccFacebookCommentsSettings.IsActive)
            {
                if (message.Data.Name == ThemeSection.Sections.Body)
                {
                    int startIndex = message.Data.Content.IndexOf("<!--CommentsSectionStart-->");
                    int endIndex = message.Data.Content.LastIndexOf("<!--CommentsSectionEnd-->");

                    if (startIndex > 0)
                    {
                        string path = message.Data.HttpContext.Request.Path;
                        if (nccFacebookCommentsSettings.RemoveLanguageParamenter)
                        {
                            foreach (var item in SupportedCultures.Cultures)
                            {
                                string langText = "/" + item.TwoLetterISOLanguageName.ToLower() + "/";
                                if (path.StartsWith(langText))
                                {
                                    path = path.Substring(3);
                                    break;
                                }
                            }
                        }
                        message.Data.Content = message.Data.Content.Substring(0, startIndex) + "<div class='fb-comments' data-href='" + (message.Data.HttpContext.Request.IsHttps ? "https://" : "http://") + message.Data.HttpContext.Request.Host + path + "' data-numposts='" + nccFacebookCommentsSettings.NumberOfPost + "' data-width='100%' data-colorscheme='" + nccFacebookCommentsSettings.ColorScheme + "'></div>" + message.Data.Content.Substring(endIndex);

                    }
                }
                if (message.Data.Name == ThemeSection.Sections.Footer)
                {
                    message.Data.Content += @"<div id='fb-root'></div>
                                            <script>(function(d, s, id) {
                                              var js, fjs = d.getElementsByTagName(s)[0];
                                              if (d.getElementById(id)) return;
                                              js = d.createElement(s); js.id = id;
                                              js.src = 'https://connect.facebook.net/en_GB/sdk.js#xfbml=1&version=v2.11&appId=" + nccFacebookCommentsSettings.FacebookAppId + @"';
                                              fjs.parentNode.insertBefore(js, fjs);
                                            }(document, 'script', 'facebook-jssdk'));</script>";
                }
            }
            return message.Data;
        }
    }
}
