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
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.TagHelpers;
using NetCoreCMS.Framework.i18n;
using NetCoreCMS.Framework.Utility;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace NetCoreCMS.Framework.Core.Mvc.Views.TagHelpers
{

    [HtmlTargetElement("a")]
    [HtmlTargetElement("a", Attributes = "asp-controller")]
    [HtmlTargetElement("a", Attributes = "asp-action")]
    [HtmlTargetElement("a", Attributes = "asp-route-id")]
    public class LanguageEnabledAnchorTagHelper : TagHelper
    {
        IHttpContextAccessor _httpContextAccessor;
        NccLanguageDetector _nccLanguageDetector;
        public override int Order { get; } = int.MaxValue;

        [HtmlAttributeName("asp-for")]
        public string For { get; set; }        
        [HtmlAttributeName("asp-action")]
        public string Action { get; set; }
        [HtmlAttributeName("asp-controller")]
        public string Controller { get; set; }        
        [HtmlAttributeName("href")]
        public string Href { get; set; }

        public LanguageEnabledAnchorTagHelper(IHttpContextAccessor httpContextAccessor, NccLanguageDetector nccLanguageDetector)
        {
            _httpContextAccessor = httpContextAccessor;
            _nccLanguageDetector = nccLanguageDetector;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);
            TagHelperAttribute href;
            var req = _httpContextAccessor.HttpContext.Request;

            if (GlobalContext.WebSite != null && GlobalContext.WebSite.IsMultiLangual)
            {
                var lang = _nccLanguageDetector.GetCurrentLanguage();
                if (context.AllAttributes["asp-action"] != null && context.AllAttributes["asp-controller"] != null)
                {
                    output.TagName = "a";
                    output.Attributes.TryGetAttribute("href", out href);

                    if (href != null)
                    {
                        output.Attributes.Remove(href);
                    }

                    var finalUrl = "/" + lang + "/" + Controller + "/" + Action;
                    var queryString = GetQueryString(context);

                    if (!string.IsNullOrEmpty(queryString))
                    {
                        finalUrl += "/?" + queryString;
                    }

                    if (href.Value.ToString().StartsWith("~/"))
                    {
                        finalUrl = href.Value.ToString().Substring(2);
                        finalUrl = req.PathBase.Value.ToString() + finalUrl;
                    }

                    output.Attributes.Add(new TagHelperAttribute("href", finalUrl));
                }
                else
                {
                    if (Href != null && !IsExternalUrl(Href))
                    {
                        bool langFound = false;
                        foreach (var item in SupportedCultures.Cultures)
                        {
                            if (Href.StartsWith("/" + item.TwoLetterISOLanguageName) || Href.StartsWith(item.TwoLetterISOLanguageName))
                            {
                                langFound = true;
                                break;
                            }
                        }

                        if (langFound == false)
                        {
                            var finalUrl = Href;

                            if (Href.StartsWith("~/"))
                            {
                                finalUrl = Href.Substring(1);
                                finalUrl = req.PathBase.Value.ToString() + "/" + lang + finalUrl;
                            }
                            else if (Href.StartsWith("/") == false)
                            {
                                Href = "/" + Href;
                                finalUrl = req.PathBase.Value.ToString() + "/" + lang + Href;
                                
                            }
                            else
                            {
                                finalUrl = "/" + lang + Href;
                            }

                            output.Attributes.SetAttribute("href", finalUrl);
                        }
                    }
                }
            }
            else
            {
                var finalUrl = Href;
                if (string.IsNullOrEmpty(Href) == false)
                {
                    if (Href.StartsWith("~/"))
                    {
                        finalUrl = Href.Substring(2);
                        finalUrl = req.PathBase.Value.ToString() + "/" + finalUrl;
                        output.Attributes.SetAttribute("href", finalUrl);
                    }                    
                }
            }

            output.Attributes.TryGetAttribute("href", out href);

            if (href == null)
            {
                output.Attributes.Add(new TagHelperAttribute("href", Href??""));
            }
        }

        private string GetQueryString(TagHelperContext context)
        {
            var queryString = "";
            foreach (var item in context.AllAttributes)
            {
                if (item.Name.StartsWith("asp-route"))
                {
                    var parts = item.Name.Split("-".ToArray(), StringSplitOptions.RemoveEmptyEntries);
                    if(parts.Length >= 3)
                    {
                        queryString += parts[2] + "=" + item.Value + "&";
                    }
                }                
            }

            if(queryString.EndsWith("&")){
                queryString = queryString.Remove(queryString.Length - 1);
            }
            return queryString;
        }
         
        private static int GetMaxLength(IReadOnlyList<object> validatorMetadata)
        {
            for (var i = 0; i < validatorMetadata.Count; i++)
            {
                if (validatorMetadata[i] is StringLengthAttribute stringLengthAttribute && stringLengthAttribute.MaximumLength > 0)
                {
                    return stringLengthAttribute.MaximumLength;
                }

                if (validatorMetadata[i] is MaxLengthAttribute maxLengthAttribute && maxLengthAttribute.Length > 0)
                {
                    return maxLengthAttribute.Length;
                }
            }
            return 0;
        }

        private static bool IsExternalUrl(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                string pattern = @"^(http|https|ftp|)\://|[a-zA-Z0-9\-\.]+\.[a-zA-Z](:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*[^\.\,\)\(\s]$";
                Regex reg = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                return reg.IsMatch(url);
            }
            return false;
        }
    }

}
