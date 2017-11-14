using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Auth.Handlers;
using NetCoreCMS.Framework.Core.Mvc.Attributes;
using NetCoreCMS.Framework.Core.Mvc.Controllers;

namespace NetCoreCMS.HelloWorld.Controllers
{ 
    [AdminMenu(Name = "Hello Module", Order = 100)]
    [SiteMenu(Name = "Hello Auth Site Menu", Order = 100)]
    public class HelloAuthController : NccController
    {
        public HelloAuthController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<HelloHomeController>();
        }

        [AdminMenuItem(Name = "Index", Url = "/HelloAuth/Index", Order = 1)]
        [SiteMenuItem(Name = "Hello world Home", Url = "/HelloAuth/Index", Order = 1)]
        [NccAuthorize()]
        [NccAuthorize(NccAuthRequirementName.Create)]        
        [NccAuthorize(HandlerClassName = "HrAuthHandler", RequirementList = new string[] { "Brunches" }, ValueList = new string[] { "Firmget,Malibag,Mirpur1,Mirpur2" })]
        [NccAuthorize(HandlerClassName = "HrAuthHandler", RequirementList = new string[] { "Organizations" }, ValueList = new string[] { "OnnorokomGroup,OnnorokomSoftware,Rokomari,Electronics,OnnoRokomRobots" })]
        public ActionResult Index()
        {
            return View();
        }

        [AdminMenuItem(Name = "Custom Permission", Url = "/HelloAuth/CustomPermission", Order = 2)]
        [SiteMenuItem(Name = "Hello Custom Permission", Url = "/HelloHome/CustomPermission", Order = 2)]
        [NccAuthorize(HandlerClassName = "HrAuthHandler", RequirementList = new string[] { "Brunches" })]
        [NccAuthorize(HandlerClassName = "HrAuthHandler", RequirementList = new string[] { "Organizations" })]
        public ActionResult CustomPermission(string[] Brunches, string[] Organizations)
        {
            return View();
        }

        public ActionResult SubAction()
        {
            return View();
        } 
    }
}
