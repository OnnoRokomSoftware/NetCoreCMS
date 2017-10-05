using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Modules;
using NetCoreCMS.Framework.Modules.Widgets;
using NetCoreCMS.Modules.OnlineExam.Repository;
using NetCoreCMS.Modules.OnlineExam.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace NetCoreCMS.OnlineExam
{
    public class OnlineExamModule : IModule
    {
        List<Widget> _widgets;
        public OnlineExamModule()
        {

        }

        public string ModuleId { get; set; }
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
        public string Path { get; set; }
        public string Folder { get; set; }
        public int ModuleStatus { get; set; }
        public string SortName { get; set; }
        [NotMapped]
        public List<Widget> Widgets { get { return _widgets; } set { _widgets = value; } }

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
            services.AddTransient<OeSubjectRepository>();
            services.AddTransient<OeExamRepository>();
            services.AddTransient<OeUddipokRepository>();
            services.AddTransient<OeQuestionRepository>();
            services.AddTransient<OeStudentRepository>();
            services.AddTransient<OeStudentQuestionSetRepository>();
            services.AddTransient<OeStudentQuestionSetDetailsRepository>();

            services.AddTransient<OeSubjectService>();
            services.AddTransient<OeExamService>();
            services.AddTransient<OeUddipokService>();
            services.AddTransient<OeQuestionService>();
            services.AddTransient<OeStudentService>();
            services.AddTransient<OeStudentQuestionSetService>();
            services.AddTransient<OeStudentQuestionSetDetailsService>();
        }

        public void RegisterRoute(IRouteBuilder routes)
        {

        }

        public bool Install(NccSettingsService settingsService, Func<NccDbQueryText, string> executeQuery)
        {
            var createQuery = @"
                CREATE TABLE IF NOT EXISTS `Ncc_OE_Subject` ( 
                    `Id` BIGINT NOT NULL AUTO_INCREMENT , 
                    `VersionNumber` INT NOT NULL , 
                    `Metadata` varchar(250) COLLATE utf8_unicode_ci NULL,
                    `Name` VARCHAR(250) NOT NULL , 
                    `CreationDate` DATETIME NOT NULL , 
                    `ModificationDate` DATETIME NOT NULL , 
                    `CreateBy` BIGINT NOT NULL , 
                    `ModifyBy` BIGINT NOT NULL , 
                    `Status` INT NOT NULL , 
                PRIMARY KEY (`Id`)) ENGINE = MyISAM;
                insert  into `Ncc_OE_Subject`(`Id`,`VersionNumber`,`Name`,`CreationDate`,`ModificationDate`,`CreateBy`,`ModifyBy`,`Status`) values (1,1,'Bangla I','2017-09-01 00:16:46','2017-09-01 00:16:46',1,1,2),(2,1,'Bangla II','2017-09-01 00:16:56','2017-09-01 00:16:56',1,1,2),(3,1,'English I','2017-09-01 00:17:04','2017-09-01 00:17:04',1,1,2),(4,1,'English II','2017-09-01 00:17:14','2017-09-01 00:17:14',1,1,2),(5,1,'Math I','2017-09-01 00:17:21','2017-09-01 00:17:21',1,1,2),(6,1,'Math II','2017-09-01 00:17:31','2017-09-01 00:17:31',1,1,2),(7,1,'Physics I','2017-09-01 00:17:40','2017-09-01 00:17:40',1,1,2),(8,1,'Physics II','2017-09-01 00:17:47','2017-09-01 00:17:47',1,1,2),(9,1,'Chemistry I','2017-09-01 00:17:55','2017-09-01 00:17:55',1,1,2),(10,1,'Chemistry II','2017-09-01 00:18:02','2017-09-01 00:18:02',1,1,2),(11,1,'Biology I','2017-09-01 00:18:17','2017-09-01 00:18:17',1,1,2),(12,1,'Biology II','2017-09-01 00:18:25','2017-09-01 00:18:25',1,1,2),(13,1,'General Knowledge','2017-09-01 00:18:37','2017-09-01 00:18:37',1,1,2);

                CREATE TABLE IF NOT EXISTS `Ncc_OE_Exam` ( 
                    `Id` BIGINT NOT NULL AUTO_INCREMENT , 
                    `VersionNumber` INT NOT NULL , 
                    `Metadata` varchar(250) COLLATE utf8_unicode_ci NULL,
                    `Name` VARCHAR(250) NOT NULL , 
                    `CreationDate` DATETIME NOT NULL , 
                    `ModificationDate` DATETIME NOT NULL , 
                    `CreateBy` BIGINT NOT NULL , 
                    `ModifyBy` BIGINT NOT NULL , 
                    `Status` INT NOT NULL , 

                    `IsBangla` bit(1) NOT NULL, 
                    `IsEnglish` bit(1) NOT NULL, 
                    `TotalMarks` DOUBLE NULL, 
                    `TotalQuestion` INT NULL, 
                    `MarksPerQuestion` DOUBLE NULL, 
                    `NegativeMarks` DOUBLE NULL, 
                    `TotalUniqueSet` INT NULL, 
                    `TotalTimeInMin` INT NOT NULL, 
                    `IsSubjectWise` bit(1) NOT NULL , 
                    `IsCombinedShuffle` bit(1) NOT NULL ,
                    `IsPublished` bit(1) NOT NULL ,
                    `IsRetake` bit(1) NOT NULL ,
                    `HasDateRange` bit(1) NOT NULL , 
                    `PublishDateTime` DATETIME NULL , 
                    `ExpireDateTime` DATETIME NULL , 
                PRIMARY KEY (`Id`)) ENGINE = MyISAM;
                insert  into `ncc_oe_exam`(`Id`,`VersionNumber`,`Name`,`CreationDate`,`ModificationDate`,`CreateBy`,`ModifyBy`,`Status`,`IsBangla`,`IsEnglish`,`TotalMarks`,`TotalQuestion`,`MarksPerQuestion`,`NegativeMarks`,`TotalUniqueSet`,`TotalTimeInMin`,`IsSubjectWise`,`IsCombinedShuffle`,`IsPublished`,`IsRetake`,`HasDateRange`,`PublishDateTime`,`ExpireDateTime`) values (1,1,'Exam 01','2017-09-10 10:33:52','2017-09-10 10:45:29',1,1,2,'','',10,10,1,0.25,2,10,'\0','\0','','\0','\0','2017-09-10 10:33:00','2017-09-17 10:33:00'), (2,1,'Exam 02','2017-09-10 10:34:17','2017-09-10 10:47:14',1,1,2,'\0','',10,5,2,0.5,2,5,'\0','\0','','\0','\0','2017-09-10 10:33:00','2017-09-17 10:33:00');

                CREATE TABLE IF NOT EXISTS `Ncc_OE_Uddipok` ( 
                    `Id` BIGINT NOT NULL AUTO_INCREMENT , 
                    `VersionNumber` INT NOT NULL , 
                    `Metadata` varchar(250) COLLATE utf8_unicode_ci NULL,
                    `Name` LONGTEXT COLLATE ucs2_unicode_ci ,  
                    `CreationDate` DATETIME NOT NULL , 
                    `ModificationDate` DATETIME NOT NULL , 
                    `CreateBy` BIGINT NOT NULL , 
                    `ModifyBy` BIGINT NOT NULL , 
                    `Status` INT NOT NULL , 

                    `ExamId` BIGINT NOT NULL, 
                    `SubjectId` BIGINT NOT NULL, 
                    `Version` INT NOT NULL, 
                    `UniqueSetNumber` INT NOT NULL, 
                PRIMARY KEY (`Id`)) ENGINE = MyISAM;

                CREATE TABLE IF NOT EXISTS `Ncc_OE_Question` ( 
                    `Id` BIGINT NOT NULL AUTO_INCREMENT , 
                    `VersionNumber` INT NOT NULL , 
                    `Metadata` varchar(250) COLLATE utf8_unicode_ci NULL,
                    `Name` LONGTEXT COLLATE ucs2_unicode_ci ,  
                    `CreationDate` DATETIME NOT NULL , 
                    `ModificationDate` DATETIME NOT NULL , 
                    `CreateBy` BIGINT NOT NULL , 
                    `ModifyBy` BIGINT NOT NULL , 
                    `Status` INT NOT NULL , 

                    `ExamId` BIGINT NOT NULL, 
                    `SubjectId` BIGINT NOT NULL, 
                    `Version` INT NOT NULL, 
                    `UddipokId` BIGINT NULL, 
                    `UniqueSetNumber` INT NOT NULL, 
                    `QuestionSerial` INT NOT NULL, 
                    `OptionA` LONGTEXT COLLATE ucs2_unicode_ci ,  
                    `OptionB` LONGTEXT COLLATE ucs2_unicode_ci ,  
                    `OptionC` LONGTEXT COLLATE ucs2_unicode_ci ,  
                    `OptionD` LONGTEXT COLLATE ucs2_unicode_ci ,  
                    `OptionE` LONGTEXT COLLATE ucs2_unicode_ci ,  
                    `CorrectAnswer` VARCHAR(250) NOT NULL , 
                    `Solve` LONGTEXT COLLATE ucs2_unicode_ci ,  
                    `IsShuffle` bit(1) NOT NULL, 
                PRIMARY KEY (`Id`)) ENGINE = MyISAM;
                insert  into `ncc_oe_question`(`Id`,`VersionNumber`,`Name`,`CreationDate`,`ModificationDate`,`CreateBy`,`ModifyBy`,`Status`,`ExamId`,`SubjectId`,`Version`,`UddipokId`,`UniqueSetNumber`,`QuestionSerial`,`OptionA`,`OptionB`,`OptionC`,`OptionD`,`OptionE`,`CorrectAnswer`,`Solve`,`IsShuffle`) values 
                (1,1,'Unique set 1 Bn Question 1','2017-09-10 12:35:59','2017-09-10 12:35:59',1,1,2,1,2,1,NULL,1,1,'Bn Option 1','Bn Option 2','Bn Option 3','Bn Option 4',NULL,'A','Solve Text',''),
                (2,1,'Unique set 1 En Question 1','2017-09-10 12:35:59','2017-09-10 12:35:59',1,1,2,1,2,2,NULL,1,1,'En Option 1','En Option 2','En Option 3','En Option 4',NULL,'A','Solve Text',''),
                (3,1,'Unique set 1 Bn Question 2','2017-09-10 12:36:12','2017-09-10 12:36:12',1,1,2,1,2,1,NULL,1,2,'Bn Option 1','Bn Option 2','Bn Option 3','Bn Option 4',NULL,'B','Solve Text',''),
                (4,1,'Unique set 1 En Question 2','2017-09-10 12:36:12','2017-09-10 12:36:12',1,1,2,1,2,2,NULL,1,2,'En Option 1','En Option 2','En Option 3','En Option 4',NULL,'B','Solve Text',''),
                (5,1,'Unique set 1 Bn Question 3','2017-09-10 12:36:22','2017-09-10 12:36:22',1,1,2,1,2,1,NULL,1,3,'Bn Option 1','Bn Option 2','Bn Option 3','Bn Option 4',NULL,'C','Solve Text',''),
                (6,1,'Unique set 1 En Question 3','2017-09-10 12:36:22','2017-09-10 12:36:22',1,1,2,1,2,2,NULL,1,3,'En Option 1','En Option 2','En Option 3','En Option 4',NULL,'C','Solve Text',''),
                (7,1,'Unique set 1 Bn Question 4','2017-09-10 12:36:32','2017-09-10 12:36:32',1,1,2,1,2,1,NULL,1,4,'Bn Option 1','Bn Option 2','Bn Option 3','Bn Option 4',NULL,'D','Solve Text',''),
                (8,1,'Unique set 1 En Question 4','2017-09-10 12:36:32','2017-09-10 12:36:32',1,1,2,1,2,2,NULL,1,4,'En Option 1','En Option 2','En Option 3','En Option 4',NULL,'D','Solve Text',''),
                (9,1,'Unique set 1 Bn Question 5','2017-09-10 12:37:19','2017-09-10 12:37:19',1,1,2,1,2,1,NULL,1,5,'Bn Option 1','Bn Option 2','Bn Option 3','Bn Option 4',NULL,'A','Solve Text',''),
                (10,1,'Unique set 1 En Question 5','2017-09-10 12:37:19','2017-09-10 12:37:19',1,1,2,1,2,2,NULL,1,5,'En Option 1','En Option 2','En Option 3','En Option 4',NULL,'A','Solve Text',''),
                (11,1,'Unique set 1 Bn Question 6 No Shuffle','2017-09-10 12:37:52','2017-09-10 12:37:52',1,1,2,1,2,1,NULL,1,6,'Bn Option 1','Bn Option 2','Bn Option 3','Bn Option 4',NULL,'B','Solve Text','\0'),
                (12,1,'Unique set 1 En Question 6 No Shuffle','2017-09-10 12:37:52','2017-09-10 12:37:52',1,1,2,1,2,2,NULL,1,6,'En Option 1','En Option 2','En Option 3','En Option 4',NULL,'B','Solve Text','\0'),
                (13,1,'Unique set 1 Bn Question 7 No Shuffle','2017-09-10 12:38:06','2017-09-10 12:38:06',1,1,2,1,2,1,NULL,1,7,'Bn Option 1','Bn Option 2','Bn Option 3','Bn Option 4',NULL,'C','Solve Text','\0'),
                (14,1,'Unique set 1 En Question 7 No Shuffle','2017-09-10 12:38:06','2017-09-10 12:38:06',1,1,2,1,2,2,NULL,1,7,'En Option 1','En Option 2','En Option 3','En Option 4',NULL,'C','Solve Text','\0'),
                (15,1,'Unique set 1 Bn Question 8','2017-09-10 12:38:19','2017-09-10 12:38:19',1,1,2,1,2,1,NULL,1,8,'Bn Option 1','Bn Option 2','Bn Option 3','Bn Option 4',NULL,'D','Solve Text',''),
                (16,1,'Unique set 1 En Question 8','2017-09-10 12:38:19','2017-09-10 12:38:19',1,1,2,1,2,2,NULL,1,8,'En Option 1','En Option 2','En Option 3','En Option 4',NULL,'D','Solve Text',''),
                (17,1,'Unique set 1 Bn Question 9','2017-09-10 12:38:29','2017-09-10 12:38:29',1,1,2,1,2,1,NULL,1,9,'Bn Option 1','Bn Option 2','Bn Option 3','Bn Option 4',NULL,'A','Solve Text',''),
                (18,1,'Unique set 1 En Question 9','2017-09-10 12:38:29','2017-09-10 12:38:29',1,1,2,1,2,2,NULL,1,9,'En Option 1','En Option 2','En Option 3','En Option 4',NULL,'A','Solve Text',''),
                (19,1,'Unique set 1 Bn Question 10','2017-09-10 12:38:42','2017-09-10 12:38:42',1,1,2,1,2,1,NULL,1,10,'Bn Option 1','Bn Option 2','Bn Option 3','Bn Option 4',NULL,'B','Solve Text',''),
                (20,1,'Unique set 1 En Question 10','2017-09-10 12:38:42','2017-09-10 12:38:42',1,1,2,1,2,2,NULL,1,10,'En Option 1','En Option 2','En Option 3','En Option 4',NULL,'B','Solve Text',''),
                
                (21,1,'Unique set 2 Bn Question 1','2017-09-11 09:05:44','2017-09-11 09:05:44',1,1,2,1,2,1,NULL,2,1,'Bn Option 1','Bn Option 2','Bn Option 3','Bn Option 4',NULL,'A','Solve Text',''),
                (22,1,'Unique set 2 En Question 1','2017-09-11 09:05:44','2017-09-11 09:05:44',1,1,2,1,2,2,NULL,2,1,'En Option 1','En Option 2','En Option 3','En Option 4',NULL,'A','Solve Text',''),
                (23,1,'Unique set 2 Bn Question 2','2017-09-11 09:06:35','2017-09-11 09:06:35',1,1,2,1,2,1,NULL,2,2,'Bn Option 1','Bn Option 2','Bn Option 3','Bn Option 4',NULL,'B','Solve Text',''),
                (24,1,'Unique set 2 En Question 2','2017-09-11 09:06:35','2017-09-11 09:06:35',1,1,2,1,2,2,NULL,2,2,'En Option 1','En Option 2','En Option 3','En Option 4',NULL,'B','Solve Text',''),
                (25,1,'Unique set 2 Bn Question 3','2017-09-11 09:06:50','2017-09-11 09:06:50',1,1,2,1,2,1,NULL,2,3,'Bn Option 1','Bn Option 2','Bn Option 3','Bn Option 4',NULL,'C','Solve Text',''),
                (26,1,'Unique set 2 En Question 3','2017-09-11 09:06:50','2017-09-11 09:06:50',1,1,2,1,2,2,NULL,2,3,'En Option 1','En Option 2','En Option 3','En Option 4',NULL,'C','Solve Text',''),
                (27,1,'Unique set 2 Bn Question 4','2017-09-11 09:07:01','2017-09-11 09:07:01',1,1,2,1,2,1,NULL,2,4,'Bn Option 1','Bn Option 2','Bn Option 3','Bn Option 4',NULL,'D','Solve Text',''),
                (28,1,'Unique set 2 En Question 4','2017-09-11 09:07:01','2017-09-11 09:07:01',1,1,2,1,2,2,NULL,2,4,'En Option 1','En Option 2','En Option 3','En Option 4',NULL,'D','Solve Text',''),
                (29,1,'Unique set 2 Bn Question 5','2017-09-11 09:07:12','2017-09-11 09:07:12',1,1,2,1,2,1,NULL,2,5,'Bn Option 1','Bn Option 2','Bn Option 3','Bn Option 4',NULL,'A','Solve Text',''),
                (30,1,'Unique set 2 En Question 5','2017-09-11 09:07:12','2017-09-11 09:07:12',1,1,2,1,2,2,NULL,2,5,'En Option 1','En Option 2','En Option 3','En Option 4',NULL,'A','Solve Text',''),
                (31,1,'Unique set 2 Bn Question 6','2017-09-11 09:07:22','2017-09-11 09:07:22',1,1,2,1,2,1,NULL,2,6,'Bn Option 1','Bn Option 2','Bn Option 3','Bn Option 4',NULL,'B','Solve Text',''),
                (32,1,'Unique set 2 En Question 6','2017-09-11 09:07:22','2017-09-11 09:07:22',1,1,2,1,2,2,NULL,2,6,'En Option 1','En Option 2','En Option 3','En Option 4',NULL,'B','Solve Text',''),
                (33,1,'Unique set 2 Bn Question 7','2017-09-11 09:07:32','2017-09-11 09:07:32',1,1,2,1,2,1,NULL,2,7,'Bn Option 1','Bn Option 2','Bn Option 3','Bn Option 4',NULL,'C','Solve Text',''),
                (34,1,'Unique set 2 En Question 7','2017-09-11 09:07:32','2017-09-11 09:07:32',1,1,2,1,2,2,NULL,2,7,'En Option 1','En Option 2','En Option 3','En Option 4',NULL,'C','Solve Text',''),
                (35,1,'Unique set 2 Bn Question 8','2017-09-11 09:07:44','2017-09-11 09:07:44',1,1,2,1,2,1,NULL,2,8,'Bn Option 1','Bn Option 2','Bn Option 3','Bn Option 4',NULL,'D','Solve Text',''),
                (36,1,'Unique set 2 En Question 8','2017-09-11 09:07:44','2017-09-11 09:07:44',1,1,2,1,2,2,NULL,2,8,'En Option 1','En Option 2','En Option 3','En Option 4',NULL,'D','Solve Text',''),
                (37,1,'Unique set 2 Bn Question 9','2017-09-11 09:07:56','2017-09-11 09:07:56',1,1,2,1,2,1,NULL,2,9,'Bn Option 1','Bn Option 2','Bn Option 3','Bn Option 4',NULL,'A','Solve Text',''),
                (38,1,'Unique set 2 En Question 9','2017-09-11 09:07:56','2017-09-11 09:07:56',1,1,2,1,2,2,NULL,2,9,'En Option 1','En Option 2','En Option 3','En Option 4',NULL,'A','Solve Text',''),
                (39,1,'Unique set 2 Bn Question 10','2017-09-11 09:08:07','2017-09-11 09:08:07',1,1,2,1,2,1,NULL,2,10,'Bn Option 1','Bn Option 2','Bn Option 3','Bn Option 4',NULL,'B','Solve Text',''),
                (40,1,'Unique set 2 En Question 10','2017-09-11 09:08:07','2017-09-11 09:08:07',1,1,2,1,2,2,NULL,2,10,'En Option 1','En Option 2','En Option 3','En Option 4',NULL,'B','Solve Text',''),

                (41,1,'Unique set 1 En Question 1','2017-09-10 12:39:33','2017-09-10 12:39:33',1,1,2,2,3,2,NULL,1,1,'En Option 1','En Option 2','En Option 3','En Option 4',NULL,'A','Solve Text',''),
                (42,1,'Unique set 1 En Question 2','2017-09-10 12:39:40','2017-09-10 12:39:40',1,1,2,2,3,2,NULL,1,2,'En Option 1','En Option 2','En Option 3','En Option 4',NULL,'B','Solve Text',''),
                (43,1,'Unique set 1 En Question 3','2017-09-10 12:39:48','2017-09-10 12:39:48',1,1,2,2,3,2,NULL,1,3,'En Option 1','En Option 2','En Option 3','En Option 4',NULL,'C','Solve Text',''),
                (44,1,'Unique set 1 En Question 4','2017-09-10 12:39:58','2017-09-10 12:39:58',1,1,2,2,3,2,NULL,1,4,'En Option 1','En Option 2','En Option 3','En Option 4',NULL,'D','Solve Text',''),
                (45,1,'Unique set 1 En Question 5 No Shuffle','2017-09-10 12:40:05','2017-09-10 12:40:05',1,1,2,2,3,2,NULL,1,5,'En Option 1','En Option 2','En Option 3','En Option 4',NULL,'A','Solve Text','\0'),

                (46,1,'Unique set 2 En Question 1','2017-09-10 12:39:33','2017-09-10 12:39:33',1,1,2,2,3,2,NULL,2,1,'En Option 1','En Option 2','En Option 3','En Option 4',NULL,'A','Solve Text',''),
                (47,1,'Unique set 2 En Question 2','2017-09-10 12:39:40','2017-09-10 12:39:40',1,1,2,2,3,2,NULL,2,2,'En Option 1','En Option 2','En Option 3','En Option 4',NULL,'B','Solve Text',''),
                (48,1,'Unique set 2 En Question 3','2017-09-10 12:39:48','2017-09-10 12:39:48',1,1,2,2,3,2,NULL,2,3,'En Option 1','En Option 2','En Option 3','En Option 4',NULL,'C','Solve Text',''),
                (49,1,'Unique set 2 En Question 4','2017-09-10 12:39:58','2017-09-10 12:39:58',1,1,2,2,3,2,NULL,2,4,'En Option 1','En Option 2','En Option 3','En Option 4',NULL,'D','Solve Text',''),
                (50,1,'Unique set 2 En Question 5 No Shuffle','2017-09-10 12:40:05','2017-09-10 12:40:05',1,1,2,2,3,2,NULL,2,5,'En Option 1','En Option 2','En Option 3','En Option 4',NULL,'A','Solve Text','\0'); 




                CREATE TABLE IF NOT EXISTS `Ncc_OE_Student` ( 
                    `Id` BIGINT NOT NULL AUTO_INCREMENT , 
                    `VersionNumber` INT NOT NULL , 
                    `Metadata` varchar(250) COLLATE utf8_unicode_ci NULL,
                    `Name` VARCHAR(250) NOT NULL , 
                    `CreationDate` DATETIME NOT NULL , 
                    `ModificationDate` DATETIME NOT NULL , 
                    `CreateBy` BIGINT NOT NULL , 
                    `ModifyBy` BIGINT NOT NULL , 
                    `Status` INT NOT NULL , 

                    `PrnNo` VARCHAR(250) NOT NULL , 
                    `RegistrationNo` VARCHAR(250) NOT NULL , 
                    `FullName` VARCHAR(250) NOT NULL , 
                PRIMARY KEY (`Id`)) ENGINE = MyISAM;
                insert  into `Ncc_OE_Student`(`Id`,`VersionNumber`,`Name`,`CreationDate`,`ModificationDate`,`CreateBy`,`ModifyBy`,`Status`, `PrnNo`, `RegistrationNo`, `FullName`) values (1,1,'Rahim','2017-09-01 00:16:46','2017-09-01 00:16:46',1,1,2, '12150900001','1000001','Rahimuddin'),(2,1,'Karim','2017-09-01 00:16:56','2017-09-01 00:16:56',1,1,2, '12150900002','1000002','Karim Uddin'),(13,1,'Sumon','2017-09-01 00:18:37','2017-09-01 00:18:37',1,1,2, '12150900003','1000003','Sumon Mollah');

            CREATE TABLE IF NOT EXISTS `Ncc_OE_Student_Question_Set` ( 
                    `Id` BIGINT NOT NULL AUTO_INCREMENT , 
                    `VersionNumber` INT NOT NULL , 
                    `Metadata` varchar(250) COLLATE utf8_unicode_ci NULL,
                    `Name` VARCHAR(250) NULL , 
                    `CreationDate` DATETIME NOT NULL , 
                    `ModificationDate` DATETIME NOT NULL , 
                    `CreateBy` BIGINT NOT NULL , 
                    `ModifyBy` BIGINT NOT NULL , 
                    `Status` INT NOT NULL , 

                    `StudentId` BIGINT NOT NULL, 
                    `ExamId` BIGINT NOT NULL, 
                    `Version` INT NOT NULL, 
                    `Correct` INT NOT NULL, 
                    `Incorrect` BIGINT NULL, 
                    `NotAnswered` INT NOT NULL, 
                    `TotalMarks` DOUBLE NOT NULL, 
                PRIMARY KEY (`Id`)) ENGINE = MyISAM;

                CREATE TABLE IF NOT EXISTS `Ncc_OE_Student_Question_Set_Details` ( 
                    `Id` BIGINT NOT NULL AUTO_INCREMENT , 
                    `VersionNumber` INT NOT NULL , 
                    `Metadata` varchar(250) COLLATE utf8_unicode_ci NULL,
                    `Name` VARCHAR(250) NULL , 
                    `CreationDate` DATETIME NOT NULL , 
                    `ModificationDate` DATETIME NOT NULL , 
                    `CreateBy` BIGINT NOT NULL , 
                    `ModifyBy` BIGINT NOT NULL , 
                    `Status` INT NOT NULL , 
                    
                    `StudentQuestionSetId` BIGINT NOT NULL, 
                    `QuestionId` BIGINT NOT NULL, 
                    `SetSerial` INT NOT NULL, 
                    `StudentAnswer` VARCHAR(255) NULL, 
                    `CorrectAnswer` VARCHAR(255) NULL, 
                    `OptionA` VARCHAR(255) NULL, 
                    `OptionB` VARCHAR(255) NULL, 
                    `OptionC` VARCHAR(255) NULL, 
                    `OptionD` VARCHAR(255) NULL,   
                    `OptionE` VARCHAR(255) NULL,                    
                PRIMARY KEY (`Id`)) ENGINE = MyISAM;
                
            ";

            var nccDbQueryText = new NccDbQueryText() { MySql_QueryText = createQuery };
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
            var deleteQuery = @"
                DROP TABLE IF EXISTS `Ncc_OE_Student_Question_Set_Details`;
                DROP TABLE IF EXISTS `Ncc_OE_Student_Question_Set`;
                DROP TABLE IF EXISTS `Ncc_OE_Student`;
                DROP TABLE IF EXISTS `Ncc_OE_Question`;
                DROP TABLE IF EXISTS `Ncc_OE_Uddipok`;
                DROP TABLE IF EXISTS `Ncc_OE_Exam`; 
                DROP TABLE IF EXISTS `Ncc_OE_Subject`; 
            ;";

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
