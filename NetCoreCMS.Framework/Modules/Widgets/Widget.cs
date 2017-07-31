using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreCMS.Framework.Modules.Widgets
{
    public abstract class Widget
    {
        public Widget(string widgetId, string title, string description, string footer)
        {
            WidgetId = widgetId;
            Title = title;
            Description = description;
            Footer = footer;
        }

        public string WidgetId { get;}
        public string Title { get; }
        public string DisplayTitle { get; set; }
        public string Description { get; }        
        public string Footer { get; }
        public string ConfigJson { get; }
        public string ConfigHtml { get; set; }
        private string ConfigPrefix { get; set; }
        private string ConfigSuffix { get; set; }
        public long WebSiteWidgetId { get; set; }
        public string ViewFileName { get; set; }
        public string ConfigViewFileName { get; set; }

        public abstract void Init(long websiteWidgetId);
        public abstract string RenderBody();
        public string RenderConfig() {
            
            ConfigPrefix = @"<form id='configForm_" + WebSiteWidgetId + @"'>
                                <div>               
                                    <input class='form-control' type='text' id='title' name='title' value='' />";
            ConfigSuffix = @"       <input id='saveConfig_" + WebSiteWidgetId + @"' type='button' value='Save' />
                                </div>
                            </form>

                            <script>
                                $(document).ready(function () {

                                    $('#saveConfig_" + WebSiteWidgetId + @"').on('click', function (evnt) {
                                        var formJson = $('#configForm_" + WebSiteWidgetId + @"').serializeObject();
                                        
                                        NccUtil.Log(formJson);
                                        var data = JSON.stringify(formJson);
                                        NccUtil.Log(data);

                                        $.ajax({
                                            url: '/CmsWidget/SaveConfig',
                                            method: 'POST',
                                            data: { webSiteWidgetId: " + WebSiteWidgetId + @", data: data},
                                            success: function (rsp) {
                                                if (rsp.isSuccess) {
                                                    NccAlert.ShowSuccess(rsp.message);
                                                }
                                                else {
                                                    NccAlert.ShowError(rsp.message);
                                                }
                                            },
                                            error: function (rsp) {
                                                NccAlert.ShowError('Error! Try again.');
                                            }
                                        });
                                    });

                                    setTimeout(function () {

                                        $.ajax({
                                            url: '/CmsWidget/GetConfig',
                                            method: 'GET',
                                            data: {websiteWidgetId:  " + WebSiteWidgetId + @"},
                                            success: function (rsp) {
                                                if (rsp.isSuccess) {
                                                    if(rsp.data != ''){
                                                        var data = JSON.parse(rsp.data);                                                        
                                                        var dataArr = NccUtil.JsonToArray(data);
                                                        
                                                        for (var key in dataArr) {
                                                            var elem = $('#configForm_" + WebSiteWidgetId + @" [name='+ key + ']');                                                              
                                                            $(elem).val(dataArr[key]);
                                                        }                                                        
                                                    }
                                                }
                                                else {
                                                    NccAlert.ShowError(rsp.message);
                                                }
                                            },
                                            error: function (rsp) {
                                                NccAlert.ShowError('Error! Try again.');
                                            }
                                        });

                                    }, 1000);

                                });
                            </script>
                            ";
            return ConfigPrefix + ConfigHtml + ConfigSuffix;
        }
    }
}
