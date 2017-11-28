/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using Microsoft.AspNetCore.Mvc;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using System;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.i18n;
using NetCoreCMS.Framework.Core.Services;
using Newtonsoft.Json;
using System.Linq;
using NetCoreCMS.Framework.Core.Network;
using NetCoreCMS.Framework.Core.Mvc.Attributes;
using Microsoft.AspNetCore.Identity;
using NetCoreCMS.Framework.Core.Models;
using System.Threading.Tasks;
using NetCoreCMS.Framework.Utility;
using static NetCoreCMS.Framework.Core.Models.NccPage;
using System.Collections.Generic;
using static NetCoreCMS.Framework.Core.Models.NccPost;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Modules.DemoData;

namespace NetCoreCMS.DemoData.Controllers
{
    [AdminMenu(Name = "Demo Data", Order = 100)]
    [NccAuthorize]
    public class DemoDataHomeController : NccController
    {
        #region Initialization
        private NccSettingsService _nccSettingsService;

        RoleManager<NccRole> _roleManager;
        UserManager<NccUser> _userManager;
        NccUserService _nccUserService;
        NccPermissionService _nccPermissionService;

        NccPageService _pageService;

        NccCategoryService _categoryService;
        NccTagService _tagService;
        NccPostService _postService;
        NccCommentsService _commentsService;

        static readonly Random rnd = new Random();

        public DemoDataHomeController(ILoggerFactory factory, NccSettingsService nccSettingsService, RoleManager<NccRole> roleManager, UserManager<NccUser> userManager, NccUserService nccUserService, NccPageService pageService, NccCategoryService categoryService, NccTagService tagService, NccPostService postService, NccCommentsService commentsService, NccPermissionService nccPermissionService)
        {
            _logger = factory.CreateLogger<DemoDataHomeController>();
            _nccSettingsService = nccSettingsService;

            _roleManager = roleManager;
            _userManager = userManager;
            _nccUserService = nccUserService;
            _nccPermissionService = nccPermissionService;

            _pageService = pageService;

            _categoryService = categoryService;
            _tagService = tagService;
            _postService = postService;
            _commentsService = commentsService;
        }
        #region Demo Text
        string enDemoContent = @"<h2>What is Lorem Ipsum?</h2>
<p><strong>Lorem Ipsum</strong> is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industries standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.</p>
<p><strong>Lorem Ipsum</strong> is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industries standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.</p>
<p><strong>Lorem Ipsum</strong> is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industries standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.</p>
<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Magna eget est lorem ipsum dolor sit amet consectetur. Pellentesque nec nam aliquam sem. Ut sem nulla pharetra diam sit. Sit amet aliquam id diam maecenas. Aliquet eget sit amet tellus cras adipiscing enim eu turpis. Viverra justo nec ultrices dui sapien eget mi proin sed. Et ligula ullamcorper malesuada proin. Quis viverra nibh cras pulvinar. Nam at lectus urna duis convallis convallis tellus id. Id porta nibh venenatis cras sed felis eget.</p>
<p>Phasellus faucibus scelerisque eleifend donec pretium vulputate sapien nec sagittis.Sed vulputate odio ut enim blandit volutpat. Vitae congue mauris rhoncus aenean vel. Dui vivamus arcu felis bibendum.Purus gravida quis blandit turpis cursus in. Nunc consequat interdum varius sit amet mattis.Ipsum dolor sit amet consectetur adipiscing elit ut. Odio morbi quis commodo odio aenean sed adipiscing diam donec. Quis hendrerit dolor magna eget est lorem.In dictum non consectetur a.Amet luctus venenatis lectus magna.</p>
<p>Nunc mattis enim ut tellus.Dapibus ultrices in iaculis nunc sed augue lacus.Semper eget duis at tellus at urna condimentum. Placerat duis ultricies lacus sed turpis. Habitant morbi tristique senectus et netus et.Sollicitudin ac orci phasellus egestas tellus rutrum tellus. Nunc consequat interdum varius sit amet mattis vulputate enim.Massa tempor nec feugiat nisl pretium fusce id velit.Aliquam purus sit amet luctus venenatis. Fermentum posuere urna nec tincidunt.Scelerisque mauris pellentesque pulvinar pellentesque.Est placerat in egestas erat imperdiet.Ut eu sem integer vitae justo eget magna fermentum iaculis. Venenatis tellus in metus vulputate eu scelerisque felis.Ullamcorper eget nulla facilisi etiam dignissim diam quis enim lobortis. Amet purus gravida quis blandit turpis cursus in.</p>
<p>At imperdiet dui accumsan sit amet. Pretium vulputate sapien nec sagittis aliquam malesuada.Enim nunc faucibus a pellentesque sit amet porttitor. Bibendum neque egestas congue quisque egestas. Nunc pulvinar sapien et ligula.Odio aenean sed adipiscing diam donec adipiscing tristique risus nec. Ut pharetra sit amet aliquam id diam maecenas. Nulla aliquet porttitor lacus luctus.Scelerisque viverra mauris in aliquam sem fringilla.Sit amet commodo nulla facilisi nullam vehicula ipsum a arcu. Diam maecenas sed enim ut sem.</p>
<p>Ipsum dolor sit amet consectetur.Sed viverra ipsum nunc aliquet bibendum enim facilisis gravida.Nisl suscipit adipiscing bibendum est ultricies integer quis. Sed augue lacus viverra vitae congue eu.Nulla pellentesque dignissim enim sit amet venenatis urna. Et magnis dis parturient montes nascetur ridiculus mus mauris.Aliquet bibendum enim facilisis gravida neque convallis.At risus viverra adipiscing at in tellus integer. Dignissim convallis aenean et tortor at. Vulputate odio ut enim blandit volutpat maecenas.Nullam vehicula ipsum a arcu.Ac turpis egestas sed tempus urna. Faucibus vitae aliquet nec ullamcorper.</p>";
        string bnDemoContent = @"<h2>লরমেম ইপ্সাম কি?</h2>
<p><strong>Lorem Ipsum</strong> শুধু মুদ্রণ এবং typesetting শিল্পের ডামি টেক্সট। 1500 এর দশক থেকে লরমেম ইপসাম শিল্পের আদর্শ ডামি টেক্সট হয়ে উঠেছে, যখন একটি অজানা মুদ্রণযন্ত্রটি একটি গলি টাইপ করে এবং এটি একটি টাইপ নমুনা বই তৈরি করতে scrambled। এটা শুধুমাত্র পাঁচ শতাব্দী ধরেই বেঁচে গেছে, কিন্তু ইলেকট্রনিক টাইপসেটিংয়ের মধ্যেও লাফানো, মূলত অপরিবর্তিত রয়েছে। এটি লরমেম ইপ্সাম প্যাসেজগুলির সাথে লেটাসেট পত্রকের মুক্তির সাথে 1960 সালে জনপ্রিয়তা লাভ করে এবং সম্প্রতি ডেস্কটপ পাবলিশিং সফটওয়্যারের মত অ্যালডাস প্যাটারমেকার যেমন লরমেম ইপসামের সংস্করণ সহ।</p>
<p><strong>Lorem Ipsum</strong> শুধু মুদ্রণ এবং typesetting শিল্পের ডামি টেক্সট। 1500 এর দশক থেকে লরমেম ইপসাম শিল্পের আদর্শ ডামি টেক্সট হয়ে উঠেছে, যখন একটি অজানা মুদ্রণযন্ত্রটি একটি গলি টাইপ করে এবং এটি একটি টাইপ নমুনা বই তৈরি করতে scrambled। এটা শুধুমাত্র পাঁচ শতাব্দী ধরেই বেঁচে গেছে, কিন্তু ইলেকট্রনিক টাইপসেটিংয়ের মধ্যেও লাফানো, মূলত অপরিবর্তিত রয়েছে। এটি লরমেম ইপ্সাম প্যাসেজগুলির সাথে লেটাসেট পত্রকের মুক্তির সাথে 1960 সালে জনপ্রিয়তা লাভ করে এবং সম্প্রতি ডেস্কটপ পাবলিশিং সফটওয়্যারের মত অ্যালডাস প্যাটারমেকার যেমন লরমেম ইপসামের সংস্করণ সহ।</p>
<p><strong>Lorem Ipsum</strong> শুধু মুদ্রণ এবং typesetting শিল্পের ডামি টেক্সট। 1500 এর দশক থেকে লরমেম ইপসাম শিল্পের আদর্শ ডামি টেক্সট হয়ে উঠেছে, যখন একটি অজানা মুদ্রণযন্ত্রটি একটি গলি টাইপ করে এবং এটি একটি টাইপ নমুনা বই তৈরি করতে scrambled। এটা শুধুমাত্র পাঁচ শতাব্দী ধরেই বেঁচে গেছে, কিন্তু ইলেকট্রনিক টাইপসেটিংয়ের মধ্যেও লাফানো, মূলত অপরিবর্তিত রয়েছে। এটি লরমেম ইপ্সাম প্যাসেজগুলির সাথে লেটাসেট পত্রকের মুক্তির সাথে 1960 সালে জনপ্রিয়তা লাভ করে এবং সম্প্রতি ডেস্কটপ পাবলিশিং সফটওয়্যারের মত অ্যালডাস প্যাটারমেকার যেমন লরমেম ইপসামের সংস্করণ সহ।</p>
<p>তাদের কর্মক্ষেত্রে, তাদের কর্মক্ষেত্রে নিয়োগের জন্য নির্ধারিত, এবং তাদের কাজের জন্য কিছু সময় এবং একটি বড় মাপের হিসাবে প্রদর্শিত হবে। Magna ইস্তাম্বুল জন্য একটি সভায় উত্সাহিত করা হয় প্লেয়ার আপনি এখানে একটি সম্পূর্ণরূপে বন্ধ করুন আপনি যদি আমাদের ডিরেক্টরির মধ্যে প্রদর্শিত আগ্রহী? সহজেই আপনি একটি ইন্টিগ্রেটেড প্রোগ্রামের জন্য কাজ করতে পারেন কিন্তু আপনি যদি আমাদের ডিরেক্টরির মধ্যে প্রদর্শিত আগ্রহী? এবং ল্যাব কিসের নিখুঁত পলভনারের নিখরচায় লন্ডন এবং লুইস ক্রিস্টালস ক্যাথিডিসের ডাক নাম আইডি পোর্ট নিম্নাভিত্তিক ভ্যানিটাস ক্রস এন্ড ফ্যালিস ইজট।</p>
<p>Phasellus শয়তান শয়তান একটি শয়তান শয়তান শয়তান এর ভাঁজ বোঁচকা ঘোড়দৌড় গলাধঃকরণ জন্য বোঁচকা বায়ু চার্জার কাঁধের বোঁচকা আরাম ভার্জিন দ্বীপপুঞ্জ পুরোপুরি সুগন্ধি বোতল ফেনা দ্বারা চালিত করা হয়। এপস এন্ড ডেভেলপমেন্ট ডেভেলপমেন্ট অডিও মার্কেটপ্লেসটি যখন আপনি আপনার কাজটি করার জন্য এটি তৈরি করতে পারবেন আপনি কি আমাদেরঅবস্থান ও শর্তাবলীবুঝতে পেরেছেন? উচ্চারণে কোন পারিশ্রমিক নেই অ্যালুমিনিয়াম আন্ডার টেপ ম্যাগাজিন।</p>
<p>এখন আপনি কি করতে চান? আইপ্যাড এবং নমনীয় আইপ্যাড মিনি সেগরি প্যাটার্নে দাপিয়ে বেড়ানোর সময় এগুলি তৈরি করা হয়। প্ল্যাটফর্মে দুটি উচ্চ গতিসম্পন্ন ট্র্যাক বাস্তবসম্মত মস্তিষ্কের চশমা এবং পরিচিত এবং স্বেচ্ছাসেবক সংস্থা বা আধ্যাত্মিক ফৌজদারী মামলা দায়ের করা হয়। এখন পর্যন্ত এটি জন্য একটি চটকদার জন্য এটি একটি ভাল চর্চা আছে। মাথার সাময়িক ভ্যাকুয়ামের জন্য এটি ব্যবহৃত হয় একটি নির্দিষ্ট সময়সীমার জন্য টেনিস প্যাটার্ন গর্ত ইত্যাদি বোজানো স্কলারিকিউ ম্যরিস পল্লিঙ্কুল পলভিনের পিটারসেক এটি আগে কখনও কখনও ছিল না। শুধুমাত্র একটি পূর্ণাঙ্গ তালিকা থেকে শুরু করতে পারেন ভ্যালেনটাইনস ভ্যালুগুয়েট অফ দ্য স্কলারস ফ্লেসিস Ullamcorper ইস্তাম্বুল এমনকি সহজে জন্য সহজে স্থানান্তর করা হয়। আমন্ত্রিত প্রসূতির বুদ্ধিমত্তার কারন।</p>
<p>দুর্নীতিপরায়ণ প্ল্যাটফর্মের উপর আম্পায়ার কোন পুরুষ কোন পুরুষ বা স্ত্রী পুরুষ আমি এখন একটি পোর্টেনিপেটর এবং পোর্টেটার সরবরাহকারী একটি নমনীয়। Bibendum এবং কখনও কখনও কখনও কখনও কখনও কখনও নুনকির চিতা আপনি যদি আমাদের পণ্য কোন মডেল আগ্রহী, আপনি যদি আমাদের ডিরেক্টরির মধ্যে প্রদর্শিত আগ্রহী? আপনি যদি আমাদের ডিরেক্টরির মধ্যে প্রদর্শিত আগ্রহী? স্ক্রোলার স্ক্রিনে কোনও ফাঁকা জায়গা সাইন আপ করার জন্য সড়ক ঘোড়দৌড় চলাচলের জন্য সহজেই একটি গাড়ি আপনি যদি আমাদের ডিরেক্টরির মধ্যে প্রদর্শিত আগ্রহী?</p>
<p>ইপসাম ডিজাইন আমরা এখন সহজেই আমাদের জন্য সহজেই একটি বিকাশের জন্য প্রস্তুত করা হয়। Nil সম্ভাব্য পরামিতি উচ্চতর সংখ্যা পূর্ণসংখ্যা হয় আমরা আপনার কাছ থেকে আসা এবং আপনি আপনার ইচ্ছার জন্য অনুসন্ধান করা যেতে পারে। নল্লা পল্লীগিরির জন্য এটি নিখরচায় স্থানান্তরিত হয়। এবং মস্তিষ্কের মস্তিষ্কের মস্তিষ্কের মস্তিষ্কের মস্তিস্ক অভিযান এবং অভিযান পূর্ণসংখ্যায় পূর্ণ সংখ্যা হিসাবে এটি চালু হয় ডাইনোসর এবং অন্যান্য উপর আক্রমন অভিযুক্ত ভলিউমেটিক মাতৃভাষার বিকাশের জন্য মজাদার রাস্তার রাস্তার উপর একটি রাস্তা এসি শুরু কিন্তু সময় সময় এটি। শয়তান তার কল্পনাশূন্যতা সম্পর্কে জিজ্ঞাসা।</p>";
        string commentsDemoContent = @"Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industries standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum. 
Lorem Ipsum শুধু মুদ্রণ এবং typesetting শিল্পের ডামি টেক্সট। 1500 এর দশক থেকে লরমেম ইপসাম শিল্পের আদর্শ ডামি টেক্সট হয়ে উঠেছে, যখন একটি অজানা মুদ্রণযন্ত্রটি একটি গলি টাইপ করে এবং এটি একটি টাইপ নমুনা বই তৈরি করতে scrambled। এটা শুধুমাত্র পাঁচ শতাব্দী ধরেই বেঁচে গেছে, কিন্তু ইলেকট্রনিক টাইপসেটিংয়ের মধ্যেও লাফানো, মূলত অপরিবর্তিত রয়েছে। এটি লরমেম ইপ্সাম প্যাসেজগুলির সাথে লেটাসেট পত্রকের মুক্তির সাথে 1960 সালে জনপ্রিয়তা লাভ করে এবং সম্প্রতি ডেস্কটপ পাবলিশিং সফটওয়্যারের মত অ্যালডাস প্যাটারমেকার যেমন লরমেম ইপসামের সংস্করণ সহ।";
        #endregion
        #endregion

        [AdminMenuItem(Name = "Generate", Url = "/DemoDataHome/Index", SubActions = new string[] { "Generate", "GenerateUser", "GeneratePage", "GenerateCategory", "GenerateTag", "GeneratePost", "GenerateComments" }, Order = 1)]

        public ActionResult Index()
        {
            var nccTranslator = new NccTranslator(CurrentLanguage);

            ViewBag.TotalUser = _userManager.Users.Count();

            ViewBag.TotalPublishedPage = _pageService.LoadAllByPageStatus(NccPage.NccPageStatus.Published).Count();
            ViewBag.TotalPage = _pageService.LoadAll(true).Count();


            ViewBag.TotalCategory = _categoryService.LoadAll(true).Count();
            ViewBag.TotalTag = _tagService.LoadAll(true).Count();
            ViewBag.TotalPublishedPost = _postService.Count(true, true, true, true);
            ViewBag.TotalPost = _postService.LoadAll(true).Count();
            ViewBag.TotalComments = _commentsService.LoadAll(true).Count();

            return View();
        }

        [HttpPost]
        public ActionResult Generate(int totalUser, int totalPage, int totalCategory, int totalTag, int totalPost, int totalComment, DateTime dateFrom, DateTime dateTo)
        {
            CreateUser(totalUser);
            CreatePage(totalPage, dateFrom, dateTo);
            CreateCategory(totalCategory);
            CreateTag(totalTag);
            CreatePost(totalPost, dateFrom, dateTo);
            CreateComments(totalComment);

            ShowMessage("Demo data created successfully", Framework.Core.Mvc.Views.MessageType.Success, false, true);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult GenerateUser(int totalCount)
        {
            if (CreateUser(totalCount) == true)
            {
                ShowMessage(totalCount + " User created successfully", Framework.Core.Mvc.Views.MessageType.Success);
            }
            else
            {
                ShowMessage("Error", Framework.Core.Mvc.Views.MessageType.Error);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult GeneratePage(int totalCount, DateTime dateFrom, DateTime dateTo)
        {
            if (CreatePage(totalCount, dateFrom, dateTo) == true)
            {
                ShowMessage(totalCount + " Page created successfully", Framework.Core.Mvc.Views.MessageType.Success);
            }
            else
            {
                ShowMessage("Error", Framework.Core.Mvc.Views.MessageType.Error);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult GenerateCategory(int totalCount)
        {
            if (CreateCategory(totalCount) == true)
            {
                ShowMessage(totalCount + " Category created successfully", Framework.Core.Mvc.Views.MessageType.Success);
            }
            else
            {
                ShowMessage("Error", Framework.Core.Mvc.Views.MessageType.Error);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult GenerateTag(int totalCount)
        {
            if (CreateTag(totalCount) == true)
            {
                ShowMessage(totalCount + " Tags created successfully", Framework.Core.Mvc.Views.MessageType.Success);
            }
            else
            {
                ShowMessage("Error", Framework.Core.Mvc.Views.MessageType.Error);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult GeneratePost(int totalCount, DateTime dateFrom, DateTime dateTo)
        {
            if (CreatePost(totalCount, dateFrom, dateTo) == true)
            {
                ShowMessage(totalCount + " Post created successfully", Framework.Core.Mvc.Views.MessageType.Success);
            }
            else
            {
                ShowMessage("Error", Framework.Core.Mvc.Views.MessageType.Error);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult GenerateComments(int totalCount)
        {
            if (CreateComments(totalCount) == true)
            {
                ShowMessage(totalCount + " Comments created successfully", Framework.Core.Mvc.Views.MessageType.Success);
            }
            else
            {
                ShowMessage("Error", Framework.Core.Mvc.Views.MessageType.Error);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete()
        {
            #region Comments Delete
            var comments = _commentsService.LoadAll(false);
            List<NccComment> commentsDeleteList = new List<NccComment>();
            foreach (var item in comments)
            {
                if (item.Metadata == "DEMODATA")
                {
                    commentsDeleteList.Add(item);
                }
            }
            foreach (var item in commentsDeleteList)
            {
                _commentsService.DeletePermanently(item.Id);
            }
            #endregion
            #region Post Delete
            var post = _postService.LoadAll(false);
            List<NccPost> postDeleteList = new List<NccPost>();
            foreach (var item in post)
            {
                if (item.Metadata == "DEMODATA")
                {
                    postDeleteList.Add(item);
                }
            }
            foreach (var item in postDeleteList)
            {
                _postService.DeletePermanently(item.Id);
            }
            #endregion
            #region Tag Delete
            var tag = _tagService.LoadAll(false);
            List<NccTag> tagDeleteList = new List<NccTag>();
            foreach (var item in tag)
            {
                if (item.Metadata == "DEMODATA")
                {
                    tagDeleteList.Add(item);
                }
            }
            foreach (var item in tagDeleteList)
            {
                _tagService.DeletePermanently(item.Id);
            }
            #endregion
            #region Category Delete
            var category = _categoryService.LoadAll(false);
            List<NccCategory> categoryDeleteList = new List<NccCategory>();
            foreach (var item in category)
            {
                if (item.Metadata == "DEMODATA")
                {
                    categoryDeleteList.Add(item);
                }
            }
            foreach (var item in categoryDeleteList)
            {
                _categoryService.DeletePermanently(item.Id);
            }
            #endregion
            #region Page Delete
            var page = _pageService.LoadAll(false);
            List<NccPage> pageDeleteList = new List<NccPage>();
            foreach (var item in page)
            {
                if (item.Metadata == "DEMODATA")
                {
                    pageDeleteList.Add(item);
                }
            }
            foreach (var item in pageDeleteList)
            {
                _pageService.DeletePermanently(item.Id);
            }
            #endregion
            ShowMessage("Demo data deleted successfully", Framework.Core.Mvc.Views.MessageType.Success, false, true);

            return RedirectToAction("Index");
        }
        #region Helper
        private bool CreateUser(int totalCount)
        {
            int currentUserCount = _userManager.Users.Count();
            for (int i = 1; i <= totalCount; i++)
            {
                string userName = "Test_user_" + (currentUserCount + i).ToString();
                string password = "123456#";
                var user = new NccUser { UserName = userName, Email = userName + "@site.com" };
                var result = _userManager.CreateAsync(user, password).Result;
                if (result.Succeeded)
                {
                    //result = _userManager.AddToRoleAsync(user, GlobalContext.WebSite.NewUserRole).Result;
                    user.Permissions.Add(new NccUserPermission() { User = user, Permission = GetRandomPermission() });
                    //_logger.LogInformation("User created a new account with password.");

                    if (result.Succeeded)
                    {
                        TempData[""] = "Success";
                    }
                }

            }
            return true;
        }
        public bool CreatePage(int totalCount, DateTime dateFrom, DateTime dateTo)
        {
            int currentCount = _pageService.LoadAll().Count();
            string itemName = "Test-Page-";
            string enDemoTitle = "Test Page ";
            string enDemoSlug = "Test-Page-";
            string bnDemoTitle = "টেস্ট পৃষ্ঠা ";
            string bnDemoSlug = "টেস্ট-পৃষ্ঠা-";

            for (int i = 1; i <= totalCount; i++)
            {
                //Declare new page
                NccPage page = new NccPage();
                page.Metadata = "DEMODATA";
                page.PublishDate = GetRandomDate(dateFrom, dateTo);
                page.PageStatus = NccPageStatus.Published;
                page.PageType = NccPageType.Public;
                page.Layout = "SiteLayout";
                page.CreateBy = page.ModifyBy = GetRandomUserId();
                page.Name = itemName + (currentCount + i).ToString();

                //Create other language details
                foreach (var item in SupportedCultures.Cultures)
                {
                    var count = page.PageDetails.Where(x => x.Language == item.TwoLetterISOLanguageName).Count();
                    if (count <= 0)
                    {
                        NccPageDetails _nccPageDetails = new NccPageDetails();
                        _nccPageDetails.Language = item.TwoLetterISOLanguageName;
                        if (item.TwoLetterISOLanguageName == "en")
                        {
                            _nccPageDetails.Title = enDemoTitle + (currentCount + i).ToString();
                            _nccPageDetails.Slug = enDemoSlug + (currentCount + i).ToString();
                            _nccPageDetails.Name = enDemoSlug + (currentCount + i).ToString();
                            _nccPageDetails.Content = "<h1 style=\"text-align:center\">" + enDemoTitle + (currentCount + i).ToString() + "</h1><hr />" + enDemoContent;
                            _nccPageDetails.MetaDescription = enDemoTitle + (currentCount + i).ToString() + " " + enDemoContent;
                            _nccPageDetails.MetaDescription = _nccPageDetails.MetaDescription.Substring(0, 160);
                        }
                        else if (item.TwoLetterISOLanguageName == "bn")
                        {
                            _nccPageDetails.Title = bnDemoTitle + (currentCount + i).ToString();
                            _nccPageDetails.Slug = bnDemoSlug + (currentCount + i).ToString();
                            _nccPageDetails.Name = bnDemoSlug + (currentCount + i).ToString();
                            _nccPageDetails.Content = "<h1 style=\"text-align:center\">" + bnDemoTitle + (currentCount + i).ToString() + "</h1><hr />" + bnDemoContent;
                            _nccPageDetails.MetaDescription = bnDemoTitle + (currentCount + i).ToString() + " " + bnDemoContent;
                            _nccPageDetails.MetaDescription = _nccPageDetails.MetaDescription.Substring(0, 160);
                        }
                        page.PageDetails.Add(_nccPageDetails);
                    }
                }

                _pageService.Save(page);
            }

            return true;
        }
        public bool CreateCategory(int totalCount)
        {
            int currentCount = _categoryService.LoadAll().Count();
            string itemName = "Test-Category-";
            string enDemoTitle = "Test Category ";
            string enDemoSlug = "Test-Category-";
            string bnDemoTitle = "টেস্ট ক্যাটাগরি ";
            string bnDemoSlug = "টেস্ট-ক্যাটাগরি-";
            for (int i = 1; i <= totalCount; i++)
            {
                NccCategory item = new NccCategory();
                item.Metadata = "DEMODATA";
                item.Name = itemName + (currentCount + i).ToString();
                item.CategoryImage = "/media/Images/2017/06/image-slider-" + (i % 4).ToString() + ".jpg";
                //Create other language details
                foreach (var lang in SupportedCultures.Cultures)
                {
                    var count = item.CategoryDetails.Where(x => x.Language == lang.TwoLetterISOLanguageName).Count();
                    if (count <= 0)
                    {
                        NccCategoryDetails _nccDetails = new NccCategoryDetails();
                        _nccDetails.Language = lang.TwoLetterISOLanguageName;
                        if (lang.TwoLetterISOLanguageName == "en")
                        {
                            _nccDetails.Title = enDemoTitle + (currentCount + i).ToString();
                            _nccDetails.Slug = enDemoSlug + (currentCount + i).ToString();
                            _nccDetails.Name = enDemoSlug + (currentCount + i).ToString();
                        }
                        else if (lang.TwoLetterISOLanguageName == "bn")
                        {
                            _nccDetails.Title = bnDemoTitle + (currentCount + i).ToString();
                            _nccDetails.Slug = bnDemoSlug + (currentCount + i).ToString();
                            _nccDetails.Name = bnDemoSlug + (currentCount + i).ToString();
                        }
                        item.CategoryDetails.Add(_nccDetails);
                    }
                }
                _categoryService.Save(item);
                //ShowMessage(totalCount + " Category created successfully", Framework.Core.Mvc.Views.MessageType.Success);
            }

            return true;
        }
        public bool CreateTag(int totalCount)
        {
            int currentCount = _tagService.LoadAll().Count();
            string itemName = "Test Tag ";
            //string enDemoTitle = "Test Tag ";
            //string enDemoSlug = "Test-Tag-";
            for (int i = 1; i <= totalCount; i++)
            {
                NccTag item = new NccTag();
                item.Metadata = "DEMODATA";
                item.Name = itemName + (currentCount + i).ToString();

                _tagService.Save(item);
                //ShowMessage(totalCount + " Tag(s) created successfully", Framework.Core.Mvc.Views.MessageType.Success);
            }

            return true;
        }
        public bool CreatePost(int totalCount, DateTime dateFrom, DateTime dateTo)
        {
            int currentCount = _postService.LoadAll().Count();
            string itemName = "Test-Post-";
            string enDemoTitle = "Test Post ";
            string enDemoSlug = "Test-Post-";
            string bnDemoTitle = "টেস্ট পোস্ট ";
            string bnDemoSlug = "টেস্ট-পোস্ট-";
            var categoryList = _categoryService.LoadAll();
            var tagList = _tagService.LoadAll();
            for (int i = 1; i <= totalCount; i++)
            {
                //Declare new post
                NccPost post = new NccPost();
                post.Metadata = "DEMODATA";
                post.PublishDate = GetRandomDate(dateFrom, dateTo);
                post.PostStatus = NccPostStatus.Published;
                post.PostType = NccPostType.Public;
                post.Layout = "SiteLayout";
                post.CreateBy = post.ModifyBy = GetRandomUserId();
                post.Name = itemName + (currentCount + i).ToString();
                post.AllowComment = true;

                //Create other languost details
                foreach (var item in SupportedCultures.Cultures)
                {
                    var count = post.PostDetails.Where(x => x.Language == item.TwoLetterISOLanguageName).Count();
                    if (count <= 0)
                    {
                        NccPostDetails _nccPostDetails = new NccPostDetails();
                        _nccPostDetails.Language = item.TwoLetterISOLanguageName;
                        if (item.TwoLetterISOLanguageName == "en")
                        {
                            _nccPostDetails.Title = enDemoTitle + (currentCount + i).ToString();
                            _nccPostDetails.Slug = enDemoSlug + (currentCount + i).ToString();
                            _nccPostDetails.Name = enDemoSlug + (currentCount + i).ToString();
                            _nccPostDetails.Content = "<h1 style=\"text-align:center\">" + enDemoTitle + (currentCount + i).ToString() + "</h1><hr />" + enDemoContent;
                            _nccPostDetails.MetaDescription = enDemoTitle + (currentCount + i).ToString() + " " + enDemoContent;
                            _nccPostDetails.MetaDescription = _nccPostDetails.MetaDescription.Substring(0, 160);
                        }
                        else if (item.TwoLetterISOLanguageName == "bn")
                        {
                            _nccPostDetails.Title = bnDemoTitle + (currentCount + i).ToString();
                            _nccPostDetails.Slug = bnDemoSlug + (currentCount + i).ToString();
                            _nccPostDetails.Name = bnDemoSlug + (currentCount + i).ToString();
                            _nccPostDetails.Content = "<h1 style=\"text-align:center\">" + bnDemoTitle + (currentCount + i).ToString() + "</h1><hr />" + bnDemoContent;
                            _nccPostDetails.MetaDescription = bnDemoTitle + (currentCount + i).ToString() + " " + bnDemoContent;
                            _nccPostDetails.MetaDescription = _nccPostDetails.MetaDescription.Substring(0, 160);
                        }
                        post.PostDetails.Add(_nccPostDetails);
                    }
                }

                _postService.Save(post);
                try
                {
                    post = _postService.Get(post.Id, true);
                    //assign random number of category
                    int catLen = (categoryList.Count - 1) > 3 ? 3 : (categoryList.Count - 1);
                    int rndCatNumber = rnd.Next(1, catLen);
                    post.Categories = new List<NccPostCategory>();
                    for (int j = 0; j < rndCatNumber; j++)
                    {
                        var temp = GetRandomCategory(categoryList);
                        if (post.Categories.Where(x => x.CategoryId == temp.Id).Count() <= 0)
                        {
                            post.Categories.Add(new NccPostCategory() { Post = post, CategoryId = temp.Id });
                        }
                    }
                    _postService.Update(post);
                }
                catch (Exception ex) { }
                try
                {
                    post = _postService.Get(post.Id, true);
                    //assign random number of tag
                    int catLen = (tagList.Count - 1) > 4 ? 4 : (tagList.Count - 1);
                    int rndTagNumber = rnd.Next(1, catLen);
                    post.Tags = new List<NccPostTag>();
                    for (int j = 0; j < rndTagNumber; j++)
                    {
                        var temp = GetRandomTag(tagList);
                        if (post.Tags.Where(x => x.TagId == temp.Id).Count() <= 0)
                        {
                            post.Tags.Add(new NccPostTag() { Post = post, TagId = temp.Id });
                        }
                    }
                    _postService.Update(post);
                }
                catch (Exception ex) { }
                //ShowMessage(totalCount + " Post created successfully", Framework.Core.Mvc.Views.MessageType.Success);
            }

            return true;
        }
        public bool CreateComments(int totalCount)
        {
            int currentCount = _commentsService.LoadAll().Count();
            var postList = _postService.Load(0, 1000, true, false, true, true);
            string itemName = "Test-Comment-";
            for (int i = 1; i <= totalCount; i++)
            {
                NccComment item = new NccComment();
                item.Metadata = "DEMODATA";
                item.Name = itemName + (currentCount + i).ToString();
                item.Content = item.Name + " -> " + commentsDemoContent;
                item.CommentStatus = NccComment.NccCommentStatus.Approved;
                item.Post = GetRandomPost(postList);

                _commentsService.Save(item);
                //ShowMessage(totalCount + " Comments created successfully", Framework.Core.Mvc.Views.MessageType.Success);
            }

            return true;
        }
        private static DateTime GetRandomDate(DateTime from, DateTime to)
        {
            var range = to - from;
            var randTimeSpan = new TimeSpan((long)(rnd.NextDouble() * range.Ticks));
            return from + randTimeSpan;
        }
        private long GetRandomUserId()
        {
            var users = _nccUserService.LoadAll();
            int r = rnd.Next(users.Count);
            return users[r].Id;
        }
        private NccPermission GetRandomPermission()
        {
            var itemList = _nccPermissionService.LoadAll();
            int r = rnd.Next(itemList.Count);
            return itemList[r];
        }
        private NccCategory GetRandomCategory(List<NccCategory> itemList)
        {
            int r = rnd.Next(itemList.Count);
            return itemList[r];
        }
        private NccTag GetRandomTag(List<NccTag> itemList)
        {
            int r = rnd.Next(itemList.Count);
            return itemList[r];
        }
        private NccPost GetRandomPost(List<NccPost> itemList)
        {
            int r = rnd.Next(itemList.Count);
            return itemList[r];
        }
        #endregion
    }
}