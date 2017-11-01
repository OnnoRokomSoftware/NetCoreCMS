/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using Microsoft.Extensions.DependencyInjection;
using NetCoreCMS.Framework.Modules;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;
using NetCoreCMS.Framework.Modules.Widgets;
using Microsoft.AspNetCore.Routing;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.Framework.Core.Models.ViewModels;

namespace NetCoreCMS.Modules.DemoData
{
    public class DemoDataModule : IModule
    {
        List<Widget> _widgets;
        public DemoDataModule()
        {
             
        }

        public string ModuleId { get; set; }
        public bool IsCore { get; set; }
        public string ModuleTitle { get; set; }
        public string Author { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string DemoUrl { get; set; }
        public string ManualUrl { get; set; }
        public bool AntiForgery { get; set; }
        public string Version { get; set; }
        public string MinNccVersion { get; set; }
        public string MaxNccVersion { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public List<NccModuleDependency> Dependencies { get; set; }

        [NotMapped]
        public Assembly Assembly { get; set; }
        public string Path { get ; set ; }
        public string Folder { get; set; }
        public int ModuleStatus { get; set; }
        public string SortName { get ; set ; }
        [NotMapped]
        public List<Widget> Widgets { get { return _widgets; } set { _widgets = value; } }
        public List<Menu> Menus { get; set; }
        public bool Activate()
        {
            return true;
        }

        public bool Inactivate()
        {
            return true;
        }

        public void Init(IServiceCollection services)
        {
            
        }

        public void RegisterRoute(IRouteBuilder routes)
        {
            
        }

        public bool Install(NccSettingsService settingsService, Func<NccDbQueryText, string> executeQuery)
        {
            int totalPageCreation = 10;
            int totalPostCreation = 500;

            #region Page Initialization
            string demoTitleEn = "Test Page ";
            string demoSlugEn = "Test-Page-";

            string demoTitleBn = "টেস্ট পৃষ্ঠা ";
            string demoSlugBn = "টেস্ট-পৃষ্ঠা-";

            string pagePreQuery = "insert  into `ncc_page`(`CreateBy`,`CreationDate`,`Layout`,`ModificationDate`,`ModifyBy`,`Name`,`PageOrder`,`PageStatus`,`PageType`,`ParentId`,`PublishDate`,`Status`,`VersionNumber`) values ";
            string pageDetailsPreQuery = "insert  into `ncc_page_details`(`Content`,`CreateBy`,`CreationDate`,`Language`,`MetaDescription`,`MetaKeyword`,`ModificationDate`,`ModifyBy`,`Name`,`PageId`,`Slug`,`Status`,`Title`,`VersionNumber`) values ";
            #endregion
            #region Category Initialization
            string categoryTitleEn = "Test Category ";
            string categorySlugEn = "Test-Category-";

            string categoryTitleBn = "টেস্ট ক্যাটাগরি ";
            string categorySlugBn = "টেস্ট-ক্যাটাগরি-";

            string categoryPreQuery = "insert  into `ncc_category`(`CategoryImage`,`CreateBy`,`CreationDate`,`ModificationDate`,`ModifyBy`,`Name`,`ParentId`,`Status`,`VersionNumber`) values ";
            string categoryDetailsPreQuery = "insert  into `ncc_category_details`(`CategoryId`,`CreateBy`,`CreationDate`,`Language`,`MetaDescription`,`MetaKeyword`,`ModificationDate`,`ModifyBy`,`Name`,`Slug`,`Status`,`Title`,`VersionNumber`) values ";
            #endregion
            #region Tag Initialization
            string tagTitle = "Test Tag ";
            string tagPreQuery = "insert  into `ncc_tag`(`CreateBy`,`CreationDate`,`ModificationDate`,`ModifyBy`,`Name`,`Status`,`VersionNumber`) values ";
            #endregion
            #region Blog Post Initialization
            string postTitleEn = "Test Post ";
            string postSlugEn = "Test-Post-";

            string postTitleBn = "টেস্ট পোস্ট ";
            string postSlugBn = "টেস্ট-পোস্ট-";

            string postPreQuery = "insert  into `ncc_post`(`AllowComment`,`AuthorId`,`CreateBy`,`CreationDate`,`IsFeatured`,`IsStiky`,`Layout`,`ModificationDate`,`ModifyBy`,`Name`,`ParentId`,`PostStatus`,`PostType`,`PublishDate`,`RelatedPosts`,`Status`,`ThumImage`,`VersionNumber`) values ";
            string postDetailsPreQuery = "insert  into `ncc_post_details`(`Content`,`CreateBy`,`CreationDate`,`Language`,`MetaDescription`,`MetaKeyword`,`ModificationDate`,`ModifyBy`,`Name`,`PostId`,`Slug`,`Status`,`Title`,`VersionNumber`) values ";
            #endregion
            #region Post_Category & Post_Tag Initialization            
            string postCategoryPreQuery = "insert  into `ncc_post_category`(`PostId`,`CategoryId`) values ";
            string postTagPreQuery = "insert  into `ncc_post_tag`(`PostId`,`TagId`) values ";
            #endregion

            #region QueryGeneration
            #region Region
            string pageQuery = "";
            string pageDetailsQuery = "";
            string pageName = "Test-Page-";

            string categoryQuery = "";
            string categoryDetailsQuery = "";
            string categoryName = "Test-Category-";

            string tagQuery = "";

            string postQuery = "";
            string postDetailsQuery = "";
            string postName = "Test-Post-";

            string postCategoryQuery = "";
            string postTagQuery = "";
            #endregion
            if (GlobalContext.WebSite.Language == "bn")
            {
                pageName = "টেস্ট-পৃষ্ঠা-";
                categoryName = "টেস্ট-ক্যাটাগরি-";
                tagTitle = "টেস্ট ট্যাগ ";
                postName = "টেস্ট-পোস্ট-";
            }
            for (int i = 1; i <= totalPageCreation; i++)
            {
                if (i == 1)
                {
                    #region Page
                    pageQuery += string.Format("(1,{0},'SiteLayout',{0},1,'{1}',0,2,0,NULL,{0},0,1)", "CURRENT_DATE", pageName + i.ToString());
                    pageDetailsQuery += string.Format("('<div><h1 style=\"text-align:center\">{1}</h1><hr /><h2>লরমেম ইপ্সাম কি?</h2><p><strong>Lorem Ipsum</strong> শুধু মুদ্রণ এবং typesetting শিল্পের ডামি টেক্সট। 1500 এর দশক থেকে লরমেম ইপসাম শিল্পের আদর্শ ডামি টেক্সট হয়ে উঠেছে, যখন একটি অজানা মুদ্রণযন্ত্রটি একটি গলি টাইপ করে এবং এটি একটি টাইপ নমুনা বই তৈরি করতে scrambled। এটা শুধুমাত্র পাঁচ শতাব্দী ধরেই বেঁচে গেছে, কিন্তু ইলেকট্রনিক টাইপসেটিংয়ের মধ্যেও লাফানো, মূলত অপরিবর্তিত রয়েছে। এটি লরমেম ইপ্সাম প্যাসেজগুলির সাথে লেটাসেট পত্রকের মুক্তির সাথে 1960 সালে জনপ্রিয়তা লাভ করে এবং সম্প্রতি ডেস্কটপ পাবলিশিং সফটওয়্যারের মত অ্যালডাস প্যাটারমেকার যেমন লরমেম ইপসামের সংস্করণ সহ।</p></div><div><p><strong>Lorem Ipsum</strong> শুধু মুদ্রণ এবং typesetting শিল্পের ডামি টেক্সট। 1500 এর দশক থেকে লরমেম ইপসাম শিল্পের আদর্শ ডামি টেক্সট হয়ে উঠেছে, যখন একটি অজানা মুদ্রণযন্ত্রটি একটি গলি টাইপ করে এবং এটি একটি টাইপ নমুনা বই তৈরি করতে scrambled। এটা শুধুমাত্র পাঁচ শতাব্দী ধরেই বেঁচে গেছে, কিন্তু ইলেকট্রনিক টাইপসেটিংয়ের মধ্যেও লাফানো, মূলত অপরিবর্তিত রয়েছে। এটি লরমেম ইপ্সাম প্যাসেজগুলির সাথে লেটাসেট পত্রকের মুক্তির সাথে 1960 সালে জনপ্রিয়তা লাভ করে এবং সম্প্রতি ডেস্কটপ পাবলিশিং সফটওয়্যারের মত অ্যালডাস প্যাটারমেকার যেমন লরমেম ইপসামের সংস্করণ সহ।</p></div><div><p><strong>Lorem Ipsum</strong> শুধু মুদ্রণ এবং typesetting শিল্পের ডামি টেক্সট। 1500 এর দশক থেকে লরমেম ইপসাম শিল্পের আদর্শ ডামি টেক্সট হয়ে উঠেছে, যখন একটি অজানা মুদ্রণযন্ত্রটি একটি গলি টাইপ করে এবং এটি একটি টাইপ নমুনা বই তৈরি করতে scrambled। এটা শুধুমাত্র পাঁচ শতাব্দী ধরেই বেঁচে গেছে, কিন্তু ইলেকট্রনিক টাইপসেটিংয়ের মধ্যেও লাফানো, মূলত অপরিবর্তিত রয়েছে। এটি লরমেম ইপ্সাম প্যাসেজগুলির সাথে লেটাসেট পত্রকের মুক্তির সাথে 1960 সালে জনপ্রিয়তা লাভ করে এবং সম্প্রতি ডেস্কটপ পাবলিশিং সফটওয়্যারের মত অ্যালডাস প্যাটারমেকার যেমন লরমেম ইপসামের সংস্করণ সহ।</p></div>', 1, {0}, 'bn', '{1}', '{1}',{0},1,'{1}',{5},'{2}',0,'{1}',1), ('<div><h1 style=\"text-align:center\">{3}</h1><hr /><h2>What is Lorem Ipsum?</h2><p><strong>Lorem Ipsum</strong> is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industries standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.</p></div><div><p><strong>Lorem Ipsum</strong> is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industries standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.</p></div><div><p><strong>Lorem Ipsum</strong> is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industries standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.</p></div>', 1, {0}, 'en', '{3}', '{3}', {0}, 1, '{3}', {5},'{4}',0,'{3}',1)"
                        , "CURRENT_DATE"
                        , demoTitleBn + i.ToString()
                        , demoSlugBn + i.ToString()
                        , demoTitleEn + i.ToString()
                        , demoSlugEn + i.ToString()
                        , i.ToString());
                    #endregion

                    #region Category
                    categoryQuery += string.Format("('/media/Images/2017/06/image-slider-0.jpg',1,{0},{0},1,'{1}',NULL,0,1)", "CURRENT_DATE", categoryName + i.ToString());
                    categoryDetailsQuery += string.Format("({5},1,{0},'bn',NULL,NULL,{0},1,'{1}','{2}',0,'{1}',1), ({5}, 1, {0}, 'en', NULL, NULL, {0}, 1, '{3}', '{4}', 0, '{3}', 1)"
                        , "CURRENT_DATE"
                        , categoryTitleBn + i.ToString()
                        , categorySlugBn + i.ToString()
                        , categoryTitleEn + i.ToString()
                        , categorySlugEn + i.ToString()
                        , i.ToString());
                    #endregion

                    tagQuery += string.Format("(1,'{0}','{0}',1,'{1}',0,1)", "2017-10-01 00:00:00.000000", tagTitle + i.ToString());
                }
                else
                {
                    #region Page
                    pageQuery += string.Format(", (1,{0},'SiteLayout',{0},1,'{1}',0,2,0,NULL,{0},0,1)", "CURRENT_DATE", pageName + i.ToString());
                    pageDetailsQuery += string.Format(", ('<div><h1 style=\"text-align:center\">{1}</h1><hr /><h2>লরমেম ইপ্সাম কি?</h2><p><strong>Lorem Ipsum</strong> শুধু মুদ্রণ এবং typesetting শিল্পের ডামি টেক্সট। 1500 এর দশক থেকে লরমেম ইপসাম শিল্পের আদর্শ ডামি টেক্সট হয়ে উঠেছে, যখন একটি অজানা মুদ্রণযন্ত্রটি একটি গলি টাইপ করে এবং এটি একটি টাইপ নমুনা বই তৈরি করতে scrambled। এটা শুধুমাত্র পাঁচ শতাব্দী ধরেই বেঁচে গেছে, কিন্তু ইলেকট্রনিক টাইপসেটিংয়ের মধ্যেও লাফানো, মূলত অপরিবর্তিত রয়েছে। এটি লরমেম ইপ্সাম প্যাসেজগুলির সাথে লেটাসেট পত্রকের মুক্তির সাথে 1960 সালে জনপ্রিয়তা লাভ করে এবং সম্প্রতি ডেস্কটপ পাবলিশিং সফটওয়্যারের মত অ্যালডাস প্যাটারমেকার যেমন লরমেম ইপসামের সংস্করণ সহ।</p></div><div><p><strong>Lorem Ipsum</strong> শুধু মুদ্রণ এবং typesetting শিল্পের ডামি টেক্সট। 1500 এর দশক থেকে লরমেম ইপসাম শিল্পের আদর্শ ডামি টেক্সট হয়ে উঠেছে, যখন একটি অজানা মুদ্রণযন্ত্রটি একটি গলি টাইপ করে এবং এটি একটি টাইপ নমুনা বই তৈরি করতে scrambled। এটা শুধুমাত্র পাঁচ শতাব্দী ধরেই বেঁচে গেছে, কিন্তু ইলেকট্রনিক টাইপসেটিংয়ের মধ্যেও লাফানো, মূলত অপরিবর্তিত রয়েছে। এটি লরমেম ইপ্সাম প্যাসেজগুলির সাথে লেটাসেট পত্রকের মুক্তির সাথে 1960 সালে জনপ্রিয়তা লাভ করে এবং সম্প্রতি ডেস্কটপ পাবলিশিং সফটওয়্যারের মত অ্যালডাস প্যাটারমেকার যেমন লরমেম ইপসামের সংস্করণ সহ।</p></div><div><p><strong>Lorem Ipsum</strong> শুধু মুদ্রণ এবং typesetting শিল্পের ডামি টেক্সট। 1500 এর দশক থেকে লরমেম ইপসাম শিল্পের আদর্শ ডামি টেক্সট হয়ে উঠেছে, যখন একটি অজানা মুদ্রণযন্ত্রটি একটি গলি টাইপ করে এবং এটি একটি টাইপ নমুনা বই তৈরি করতে scrambled। এটা শুধুমাত্র পাঁচ শতাব্দী ধরেই বেঁচে গেছে, কিন্তু ইলেকট্রনিক টাইপসেটিংয়ের মধ্যেও লাফানো, মূলত অপরিবর্তিত রয়েছে। এটি লরমেম ইপ্সাম প্যাসেজগুলির সাথে লেটাসেট পত্রকের মুক্তির সাথে 1960 সালে জনপ্রিয়তা লাভ করে এবং সম্প্রতি ডেস্কটপ পাবলিশিং সফটওয়্যারের মত অ্যালডাস প্যাটারমেকার যেমন লরমেম ইপসামের সংস্করণ সহ।</p></div>', 1, {0}, 'bn', '{1}', '{1}',{0},1,'{1}',{5},'{2}',0,'{1}',1), ('<div><h1 style=\"text-align:center\">{3}</h1><hr /><h2>What is Lorem Ipsum?</h2><p><strong>Lorem Ipsum</strong> is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industries standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.</p></div><div><p><strong>Lorem Ipsum</strong> is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industries standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.</p></div><div><p><strong>Lorem Ipsum</strong> is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industries standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.</p></div>', 1, {0}, 'en', '{3}', '{3}', {0}, 1, '{3}', {5},'{4}',0,'{3}',1)"
                        , "CURRENT_DATE"
                        , demoTitleBn + i.ToString()
                        , demoSlugBn + i.ToString()
                        , demoTitleEn + i.ToString()
                        , demoSlugEn + i.ToString()
                        , i.ToString());
                    #endregion

                    #region Category
                    categoryQuery += string.Format(", ('/media/Images/2017/06/image-slider-{2}.jpg',1,{0},{0},1,'{1}',NULL,0,1)", "CURRENT_DATE", categoryName + i.ToString(), (i % 4).ToString());
                    categoryDetailsQuery += string.Format(", ({5},1,{0},'bn',NULL,NULL,{0},1,'{1}','{2}',0,'{1}',1), ({5}, 1, {0}, 'en', NULL, NULL, {0}, 1, '{3}', '{4}', 0, '{3}', 1)"
                        , "CURRENT_DATE"
                        , categoryTitleBn + i.ToString()
                        , categorySlugBn + i.ToString()
                        , categoryTitleEn + i.ToString()
                        , categorySlugEn + i.ToString()
                        , i.ToString());
                    #endregion

                    tagQuery += string.Format(", (1,{0},{0},1,'{1}',0,1)", "CURRENT_DATE", tagTitle + i.ToString());
                }
            }

            for (int i = 1; i <= totalPostCreation; i++)
            {
                if (i == 1)
                {
                    #region Post
                    postQuery += string.Format("(TRUE,1,1,{0},FALSE,FALSE,'SiteLayout',{0},1,'{1}',NULL,2,0,{0},NULL,0,'/media/Images/2017/06/image-slider-{2}.jpg',1)", "CURRENT_DATE", postName + i.ToString(), (i % 4).ToString());
                    postDetailsQuery += string.Format("('<div><h1 style=\"text-align:center\">{1}</h1><hr /><h2>লরমেম ইপ্সাম কি?</h2><p><strong>Lorem Ipsum</strong> শুধু মুদ্রণ এবং typesetting শিল্পের ডামি টেক্সট। 1500 এর দশক থেকে লরমেম ইপসাম শিল্পের আদর্শ ডামি টেক্সট হয়ে উঠেছে, যখন একটি অজানা মুদ্রণযন্ত্রটি একটি গলি টাইপ করে এবং এটি একটি টাইপ নমুনা বই তৈরি করতে scrambled। এটা শুধুমাত্র পাঁচ শতাব্দী ধরেই বেঁচে গেছে, কিন্তু ইলেকট্রনিক টাইপসেটিংয়ের মধ্যেও লাফানো, মূলত অপরিবর্তিত রয়েছে। এটি লরমেম ইপ্সাম প্যাসেজগুলির সাথে লেটাসেট পত্রকের মুক্তির সাথে 1960 সালে জনপ্রিয়তা লাভ করে এবং সম্প্রতি ডেস্কটপ পাবলিশিং সফটওয়্যারের মত অ্যালডাস প্যাটারমেকার যেমন লরমেম ইপসামের সংস্করণ সহ।</p></div><div><p><strong>Lorem Ipsum</strong> শুধু মুদ্রণ এবং typesetting শিল্পের ডামি টেক্সট। 1500 এর দশক থেকে লরমেম ইপসাম শিল্পের আদর্শ ডামি টেক্সট হয়ে উঠেছে, যখন একটি অজানা মুদ্রণযন্ত্রটি একটি গলি টাইপ করে এবং এটি একটি টাইপ নমুনা বই তৈরি করতে scrambled। এটা শুধুমাত্র পাঁচ শতাব্দী ধরেই বেঁচে গেছে, কিন্তু ইলেকট্রনিক টাইপসেটিংয়ের মধ্যেও লাফানো, মূলত অপরিবর্তিত রয়েছে। এটি লরমেম ইপ্সাম প্যাসেজগুলির সাথে লেটাসেট পত্রকের মুক্তির সাথে 1960 সালে জনপ্রিয়তা লাভ করে এবং সম্প্রতি ডেস্কটপ পাবলিশিং সফটওয়্যারের মত অ্যালডাস প্যাটারমেকার যেমন লরমেম ইপসামের সংস্করণ সহ।</p></div><div><p><strong>Lorem Ipsum</strong> শুধু মুদ্রণ এবং typesetting শিল্পের ডামি টেক্সট। 1500 এর দশক থেকে লরমেম ইপসাম শিল্পের আদর্শ ডামি টেক্সট হয়ে উঠেছে, যখন একটি অজানা মুদ্রণযন্ত্রটি একটি গলি টাইপ করে এবং এটি একটি টাইপ নমুনা বই তৈরি করতে scrambled। এটা শুধুমাত্র পাঁচ শতাব্দী ধরেই বেঁচে গেছে, কিন্তু ইলেকট্রনিক টাইপসেটিংয়ের মধ্যেও লাফানো, মূলত অপরিবর্তিত রয়েছে। এটি লরমেম ইপ্সাম প্যাসেজগুলির সাথে লেটাসেট পত্রকের মুক্তির সাথে 1960 সালে জনপ্রিয়তা লাভ করে এবং সম্প্রতি ডেস্কটপ পাবলিশিং সফটওয়্যারের মত অ্যালডাস প্যাটারমেকার যেমন লরমেম ইপসামের সংস্করণ সহ।</p></div>',1,{0},'bn','{1}','{1}',{0},1,NULL,{5},'{2}',0,'{1}',1), ('<div><h1 style=\"text-align:center\">{3}</h1><hr /><h2>What is Lorem Ipsum?</h2><p><strong>Lorem Ipsum</strong> is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industries standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.</p></div><div><p><strong>Lorem Ipsum</strong> is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industries standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.</p></div><div><p><strong>Lorem Ipsum</strong> is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industries standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.</p></div>',1,{0},'en','{3}','{3}',{0},1,NULL,{5},'{4}',0,'{3}',1)"
                        , "CURRENT_DATE"
                        , postTitleBn + i.ToString()
                        , postSlugBn + i.ToString()
                        , postTitleEn + i.ToString()
                        , postSlugEn + i.ToString()
                        , i.ToString());
                    #endregion

                    postCategoryQuery += string.Format("({0},{1})", i.ToString(), ((i % 9) + 1).ToString());
                    postTagQuery += string.Format("({0},{1})", i.ToString(), ((i % 9) + 1).ToString());
                }
                else
                {
                    #region Post
                    postQuery += string.Format(", (TRUE,1,1,{0},FALSE,FALSE,'SiteLayout',{0},1,'{1}',NULL,2,0,{0},NULL,0,'/media/Images/2017/06/image-slider-{2}.jpg',1)", "CURRENT_DATE", postName + i.ToString(), (i % 4).ToString());
                    postDetailsQuery += string.Format(", ('<div><h1 style=\"text-align:center\">{1}</h1><hr /><h2>লরমেম ইপ্সাম কি?</h2><p><strong>Lorem Ipsum</strong> শুধু মুদ্রণ এবং typesetting শিল্পের ডামি টেক্সট। 1500 এর দশক থেকে লরমেম ইপসাম শিল্পের আদর্শ ডামি টেক্সট হয়ে উঠেছে, যখন একটি অজানা মুদ্রণযন্ত্রটি একটি গলি টাইপ করে এবং এটি একটি টাইপ নমুনা বই তৈরি করতে scrambled। এটা শুধুমাত্র পাঁচ শতাব্দী ধরেই বেঁচে গেছে, কিন্তু ইলেকট্রনিক টাইপসেটিংয়ের মধ্যেও লাফানো, মূলত অপরিবর্তিত রয়েছে। এটি লরমেম ইপ্সাম প্যাসেজগুলির সাথে লেটাসেট পত্রকের মুক্তির সাথে 1960 সালে জনপ্রিয়তা লাভ করে এবং সম্প্রতি ডেস্কটপ পাবলিশিং সফটওয়্যারের মত অ্যালডাস প্যাটারমেকার যেমন লরমেম ইপসামের সংস্করণ সহ।</p></div><div><p><strong>Lorem Ipsum</strong> শুধু মুদ্রণ এবং typesetting শিল্পের ডামি টেক্সট। 1500 এর দশক থেকে লরমেম ইপসাম শিল্পের আদর্শ ডামি টেক্সট হয়ে উঠেছে, যখন একটি অজানা মুদ্রণযন্ত্রটি একটি গলি টাইপ করে এবং এটি একটি টাইপ নমুনা বই তৈরি করতে scrambled। এটা শুধুমাত্র পাঁচ শতাব্দী ধরেই বেঁচে গেছে, কিন্তু ইলেকট্রনিক টাইপসেটিংয়ের মধ্যেও লাফানো, মূলত অপরিবর্তিত রয়েছে। এটি লরমেম ইপ্সাম প্যাসেজগুলির সাথে লেটাসেট পত্রকের মুক্তির সাথে 1960 সালে জনপ্রিয়তা লাভ করে এবং সম্প্রতি ডেস্কটপ পাবলিশিং সফটওয়্যারের মত অ্যালডাস প্যাটারমেকার যেমন লরমেম ইপসামের সংস্করণ সহ।</p></div><div><p><strong>Lorem Ipsum</strong> শুধু মুদ্রণ এবং typesetting শিল্পের ডামি টেক্সট। 1500 এর দশক থেকে লরমেম ইপসাম শিল্পের আদর্শ ডামি টেক্সট হয়ে উঠেছে, যখন একটি অজানা মুদ্রণযন্ত্রটি একটি গলি টাইপ করে এবং এটি একটি টাইপ নমুনা বই তৈরি করতে scrambled। এটা শুধুমাত্র পাঁচ শতাব্দী ধরেই বেঁচে গেছে, কিন্তু ইলেকট্রনিক টাইপসেটিংয়ের মধ্যেও লাফানো, মূলত অপরিবর্তিত রয়েছে। এটি লরমেম ইপ্সাম প্যাসেজগুলির সাথে লেটাসেট পত্রকের মুক্তির সাথে 1960 সালে জনপ্রিয়তা লাভ করে এবং সম্প্রতি ডেস্কটপ পাবলিশিং সফটওয়্যারের মত অ্যালডাস প্যাটারমেকার যেমন লরমেম ইপসামের সংস্করণ সহ।</p></div>',1,{0},'bn','{1}','{1}',{0},1,NULL,{5},'{2}',0,'{1}',1), ('<div><h1 style=\"text-align:center\">{3}</h1><hr /><h2>What is Lorem Ipsum?</h2><p><strong>Lorem Ipsum</strong> is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industries standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.</p></div><div><p><strong>Lorem Ipsum</strong> is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industries standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.</p></div><div><p><strong>Lorem Ipsum</strong> is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industries standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.</p></div>',1,{0},'en','{3}','{3}',{0},1,NULL,{5},'{4}',0,'{3}',1)"
                        , "CURRENT_DATE"
                        , postTitleBn + i.ToString()
                        , postSlugBn + i.ToString()
                        , postTitleEn + i.ToString()
                        , postSlugEn + i.ToString()
                        , i.ToString());
                    #endregion

                    postCategoryQuery += string.Format(", ({0},{1})", i.ToString(), ((i % 9) + 1).ToString());
                    postTagQuery += string.Format(", ({0},{1})", i.ToString(), ((i % 9) + 1).ToString());
                }
            }
            #endregion
            #region Test Menu Generation 
            string menuQuery = @"INSERT  INTO `ncc_menu`(`CreateBy`,`CreationDate`,`MenuIconCls`,`MenuLanguage`,`MenuOrder`,`Metadata`,`ModificationDate`,`ModifyBy`,`Name`,`Position`,`Status`,`VersionNumber`) VALUES (1, '2017-10-04 09:35:59.374356', NULL, '', 1, 'DemoData', '2017-10-04 09:35:59.374356', 1, 'DemoData Menu', 'Navigation', 0, 1);

                INSERT INTO `ncc_menu_item`(`Action`,`Controller`,`CreateBy`,`CreationDate`,`Data`,`MenuActionType`,`MenuFor`,`MenuIconCls`,`MenuOrder`,`Metadata`,`ModificationDate`,`ModifyBy`,`Module`,`Name`,`NccMenuId`,`NccMenuItemId`,`NccMenuItemId1`,`ParentId`,`Position`,`Status`,`Target`,`Url`,`VersionNumber`) VALUES ('Index', 'Home', 1, CURRENT_DATE, '', 0, 0, NULL, 1, 'DemoData', CURRENT_DATE, 1, '', 'Home', (SELECT MAX(Id) FROM ncc_menu WHERE MetaData = 'DemoData'),NULL,NULL,NULL,0,0,'','/Home/Index',1), ('', '', 1, CURRENT_DATE, '/Post', 4, 0, NULL, 2, 'DemoData', CURRENT_DATE, 1, '', 'Blog Posts', (SELECT MAX(Id) FROM ncc_menu WHERE MetaData = 'DemoData'),NULL,NULL,NULL,0,0,'_self','/Post',1), ('', '', 1, CURRENT_DATE, '/Category', 4, 0, NULL, 3, 'DemoData', CURRENT_DATE, 1, '', 'Blog Categories', (SELECT MAX(Id) FROM ncc_menu WHERE MetaData = 'DemoData'),NULL,NULL,NULL,0,0,'_self','/Category',1), ('', '', 1, CURRENT_DATE, '/Tags', 4, 0, NULL, 4, 'DemoData', CURRENT_DATE, 1, '', 'Post Tags', (SELECT MAX(Id) FROM ncc_menu WHERE MetaData = 'DemoData'),NULL,NULL,NULL,0,0,'_self','/Tags',1); "; 
            #endregion

            var finalQuery = @"";
            finalQuery = pagePreQuery + pageQuery + "; " + pageDetailsPreQuery + pageDetailsQuery + ";"
                + categoryPreQuery + categoryQuery + "; " + categoryDetailsPreQuery + categoryDetailsQuery + ";"
                + tagPreQuery + tagQuery + "; "
                + postPreQuery + postQuery + "; " + postDetailsPreQuery + postDetailsQuery + ";"
                + postCategoryPreQuery + postCategoryQuery + "; " + postTagPreQuery + postTagQuery + ";"
                + menuQuery + " ";

            var nccDbQueryText = new NccDbQueryText() { MySql_QueryText = finalQuery };
            var retVal = executeQuery(nccDbQueryText);
            if (!string.IsNullOrEmpty(retVal))
            {
                


                return true;
            }
            return false;
        }
        public bool Update(NccSettingsService settingsService, Func<NccDbQueryText, string> executeQuery)
        {
            return true;
        }
        public bool Uninstall(NccSettingsService settingsService, Func<NccDbQueryText, string> executeQuery)
        {
            var deleteQuery = @"DELETE FROM ncc_page_details; 
                                ALTER TABLE ncc_page_details AUTO_INCREMENT = 1;
                                DELETE FROM ncc_page;
                                ALTER TABLE ncc_page AUTO_INCREMENT = 1;

                                DELETE FROM ncc_category_details;
                                ALTER TABLE ncc_category_details AUTO_INCREMENT = 1;
                                DELETE FROM ncc_post_category;
                                DELETE FROM ncc_category;
                                ALTER TABLE ncc_category AUTO_INCREMENT = 1;

                                DELETE FROM ncc_post_tag;
                                DELETE FROM ncc_tag;
                                ALTER TABLE ncc_tag AUTO_INCREMENT = 1;

                                DELETE FROM ncc_post_details;
                                ALTER TABLE ncc_post_details AUTO_INCREMENT = 1;                                
                                DELETE FROM ncc_post;
                                ALTER TABLE ncc_post AUTO_INCREMENT = 1;

                                DELETE FROM ncc_menu_item WHERE Metadata = 'DemoData';
                                DELETE FROM ncc_menu WHERE Metadata = 'DemoData';
                            ";

            var nccDbQueryText = new NccDbQueryText() { MySql_QueryText = deleteQuery };
            var retVal = executeQuery(nccDbQueryText);
            if (!string.IsNullOrEmpty(retVal))
            {
                return true;
            }
            return false;

        }
    }
}
