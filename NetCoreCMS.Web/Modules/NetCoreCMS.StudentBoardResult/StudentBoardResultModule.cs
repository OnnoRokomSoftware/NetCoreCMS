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
using NetCoreCMS.Modules.StudentBoardResult.Services;
using NetCoreCMS.Modules.StudentBoardResult.Repository;
using NetCoreCMS.Framework.Core.Models;

namespace NetCoreCMS.Modules.StudentBoardResult
{
    public class StudentBoardResultModule : IModule
    {
        List<Widget> _widgets;
        public StudentBoardResultModule()
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
        public string Path { get ; set ; }
        public string Folder { get; set; }
        public int ModuleStatus { get; set; }
        public string SortName { get ; set ; }
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
            services.AddTransient<SbrBoardRepository>();
            services.AddTransient<SbrExamRepository>();
            services.AddTransient<SbrExamSubjectRepository>();
            services.AddTransient<SbrGroupRepository>();
            services.AddTransient<SbrStudentRepository>();
            services.AddTransient<SbrStudentResultRepository>();
            services.AddTransient<SbrStudentResultDetailsRepository>();

            services.AddTransient<SbrBoardService>();
            services.AddTransient<SbrExamService>();
            services.AddTransient<SbrExamSubjectService>();
            services.AddTransient<SbrGroupService>();
            services.AddTransient<SbrStudentService>();
            services.AddTransient<SbrStudentResultService>();
            services.AddTransient<SbrStudentResultDetailsService>();
        }

        public void RegisterRoute(IRouteBuilder routes)
        {
            
        }

        public bool Install(NccSettingsService settingsService, Func<NccDbQueryText, string> executeQuery)
        {
            var createQuery = @"
                CREATE TABLE IF NOT EXISTS `Ncc_Sbr_Board` ( 
                    `Id` BIGINT NOT NULL AUTO_INCREMENT , 
                    `VersionNumber` INT NOT NULL , 
                    `Metadata` varchar(250) COLLATE utf8_unicode_ci NULL,
                    `Name` VARCHAR(250) NOT NULL , 
                    `CreationDate` DATETIME NOT NULL , 
                    `ModificationDate` DATETIME NOT NULL , 
                    `CreateBy` BIGINT NOT NULL , 
                    `ModifyBy` BIGINT NOT NULL , 
                    `Status` INT NOT NULL , 

                    `ShortName` VARCHAR(250) NULL , 
                    `Order` INT NOT NULL , 
                PRIMARY KEY (`Id`)) ENGINE = MyISAM;

                insert  into `ncc_sbr_board`(`Id`,`VersionNumber`,`Name`,`CreationDate`,`ModificationDate`,`CreateBy`,`ModifyBy`,`Status`,`ShortName`,`Order`) values 
                    (1,1,'Barisal','2017-09-23 21:26:38','2017-09-23 21:26:38',1,1,2,'barisal',1),
                    (2,1,'Chittagong','2017-09-23 21:26:38','2017-09-23 21:26:38',1,1,2,'chittagong',2),
                    (3,1,'Comilla','2017-09-23 21:26:38','2017-09-23 21:26:38',1,1,2,'comilla',3),
                    (4,1,'Dhaka','2017-09-23 21:26:38','2017-09-23 21:26:38',1,1,2,'dhaka',4),
                    (5,1,'Dinajpur','2017-09-23 21:26:38','2017-09-23 21:26:38',1,1,2,'dinajpur',5),
                    (6,1,'Jessore','2017-09-23 21:26:38','2017-09-23 21:26:38',1,1,2,'jessore',6),
                    (7,1,'Rajshahi','2017-09-23 21:26:38','2017-09-23 21:26:38',1,1,2,'rajshahi',7),
                    (8,1,'Sylhet','2017-09-23 21:26:38','2017-09-23 21:26:38',1,1,2,'sylhet',8),
                    (9,1,'Madrasah','2017-09-23 21:26:38','2017-09-23 21:26:38',1,1,2,'madrasah',9),
                    (10,1,'Technical','2017-09-23 21:26:38','2017-09-23 21:26:38',1,1,2,'tec',10),
                    (11,1,'DIBS(Dhaka)','2017-09-23 21:26:38','2017-09-23 21:26:38',1,1,2,'dibs',11);



                CREATE TABLE IF NOT EXISTS `Ncc_Sbr_Group` ( 
                    `Id` BIGINT NOT NULL AUTO_INCREMENT , 
                    `VersionNumber` INT NOT NULL , 
                    `Metadata` varchar(250) COLLATE utf8_unicode_ci NULL,
                    `Name` VARCHAR(250) NOT NULL , 
                    `CreationDate` DATETIME NOT NULL , 
                    `ModificationDate` DATETIME NOT NULL , 
                    `CreateBy` BIGINT NOT NULL , 
                    `ModifyBy` BIGINT NOT NULL , 
                    `Status` INT NOT NULL , 

                    `Order` INT NOT NULL , 
                PRIMARY KEY (`Id`)) ENGINE = MyISAM;

                insert  into `ncc_sbr_group`(`Id`,`VersionNumber`,`Name`,`CreationDate`,`ModificationDate`,`CreateBy`,`ModifyBy`,`Status`,`Order`) values 
                    (1,1,'Science','2017-09-23 21:32:28','2017-09-23 21:32:28',1,1,2,1),
                    (2,1,'Arts','2017-09-23 21:32:35','2017-09-23 21:32:35',1,1,2,2),
                    (3,1,'Commerce','2017-09-23 21:32:44','2017-09-23 21:32:44',1,1,2,3);


                CREATE TABLE IF NOT EXISTS `Ncc_Sbr_Exam` ( 
                    `Id` BIGINT NOT NULL AUTO_INCREMENT , 
                    `VersionNumber` INT NOT NULL , 
                    `Metadata` varchar(250) COLLATE utf8_unicode_ci NULL,
                    `Name` VARCHAR(250) NOT NULL , 
                    `CreationDate` DATETIME NOT NULL , 
                    `ModificationDate` DATETIME NOT NULL , 
                    `CreateBy` BIGINT NOT NULL , 
                    `ModifyBy` BIGINT NOT NULL , 
                    `Status` INT NOT NULL , 

                    `Order` INT NOT NULL , 
                PRIMARY KEY (`Id`)) ENGINE = MyISAM;

                insert  into `ncc_sbr_exam`(`Id`,`VersionNumber`,`Name`,`CreationDate`,`ModificationDate`,`CreateBy`,`ModifyBy`,`Status`,`Order`) values 
                    (1,1,'SSC','2017-09-23 21:34:20','2017-09-23 21:34:20',1,1,2,1),
                    (2,1,'HSC','2017-09-23 21:34:28','2017-09-23 21:34:34',1,1,2,2);


                CREATE TABLE IF NOT EXISTS `Ncc_Sbr_Exam_Subject` ( 
                    `Id` BIGINT NOT NULL AUTO_INCREMENT , 
                    `VersionNumber` INT NOT NULL , 
                    `Metadata` varchar(250) COLLATE utf8_unicode_ci NULL,
                    `Name` VARCHAR(250) NOT NULL , 
                    `CreationDate` DATETIME NOT NULL , 
                    `ModificationDate` DATETIME NOT NULL , 
                    `CreateBy` BIGINT NOT NULL , 
                    `ModifyBy` BIGINT NOT NULL , 
                    `Status` INT NOT NULL , 

                    `SubjectCode` VARCHAR(250) NOT NULL , 
                    `Order` INT NOT NULL , 
                    `ExamId` BIGINT NOT NULL , 
                PRIMARY KEY (`Id`)) ENGINE = MyISAM;

                CREATE TABLE IF NOT EXISTS `Ncc_Sbr_Student` ( 
                    `Id` BIGINT NOT NULL AUTO_INCREMENT , 
                    `VersionNumber` INT NOT NULL , 
                    `Metadata` varchar(250) COLLATE utf8_unicode_ci NULL,
                    `Name` VARCHAR(250) NOT NULL , 
                    `CreationDate` DATETIME NOT NULL , 
                    `ModificationDate` DATETIME NOT NULL , 
                    `CreateBy` BIGINT NOT NULL , 
                    `ModifyBy` BIGINT NOT NULL , 
                    `Status` INT NOT NULL , 
                    
                    `FatherName` VARCHAR(250) NOT NULL , 
                    `MotherName` VARCHAR(250) NOT NULL , 
                    `Gender` INT NOT NULL , 
                    `Religion` INT NOT NULL , 
                    `MobileNumber` VARCHAR(250) NULL , 
                    `DateOfBirth` DATETIME NULL ,   
                    `ImagePath` VARCHAR(500) NULL ,                    
                PRIMARY KEY (`Id`)) ENGINE = MyISAM;

                insert  into `ncc_sbr_student`(`Id`,`VersionNumber`,`Name`,`CreationDate`,`ModificationDate`,`CreateBy`,`ModifyBy`,`Status`,`FatherName`,`MotherName`,`Gender`,`Religion`,`DateOfBirth`) values 
                    (1,1,'Rahimuddin','2017-09-25 09:29:08','2017-09-25 09:34:51',1,1,2,'Rahimuddin\'s father','Rahimuddin\'s mother',0,1,'2001-02-01 00:00:00'),
                    (2,1,'Karimuddin','2017-09-25 09:29:08','2017-09-25 09:34:51',1,1,2,'Karimuddin\'s father','Karimuddin\'s mother',0,1,'2002-05-01 00:00:00'),
                    (3,1,'Nasir uddin','2017-09-25 09:29:08','2017-09-25 09:34:51',1,1,2,'Nasir uddin\'s father','Nasir uddin\'s mother',0,1,'2003-01-01 00:00:00'),
                    (4,1,'Amena begam','2017-09-25 09:29:08','2017-09-25 09:34:51',1,1,2,'Amena begam\'s father','Amena begam\'s mother',1,1,'2004-01-05 00:00:00'),
                    (5,1,'Rohima Khatun','2017-09-25 09:29:08','2017-09-25 09:34:51',1,1,2,'Rohima Khatun\'s father','Rohima Khatun\'s mother',1,1,'2005-08-04 00:00:00');


                CREATE TABLE IF NOT EXISTS `Ncc_Sbr_Student_Result` ( 
                    `Id` BIGINT NOT NULL AUTO_INCREMENT , 
                    `VersionNumber` INT NOT NULL , 
                    `Metadata` varchar(250) COLLATE utf8_unicode_ci NULL,
                    `Name` VARCHAR(250) NULL , 
                    `CreationDate` DATETIME NOT NULL , 
                    `ModificationDate` DATETIME NOT NULL , 
                    `CreateBy` BIGINT NOT NULL , 
                    `ModifyBy` BIGINT NOT NULL , 
                    `Status` INT NOT NULL , 
                                       
                    `RollNo` VARCHAR(250) NOT NULL , 
                    `RegistrationNo` VARCHAR(250) NOT NULL , 
                    `Year` VARCHAR(250) NOT NULL , 
                    `Gpa` double NOT NULL ,  
                    `GpaWithout4th` double NOT NULL ,  
                    `Grade` VARCHAR(250) NOT NULL , 

                    `StudentId` BIGINT NOT NULL , 
                    `ExamId` BIGINT NOT NULL , 
                    `BoardId` BIGINT NOT NULL , 
                    `GroupId` BIGINT NOT NULL , 
                PRIMARY KEY (`Id`)) ENGINE = MyISAM;

                insert  into `ncc_sbr_student_result`(`Id`,`VersionNumber`,`CreationDate`,`ModificationDate`,`CreateBy`,`ModifyBy`,`Status`,`RollNo`,`RegistrationNo`,`Year`,`Gpa`,`GpaWithout4th`,`Grade`,`StudentId`,`ExamId`,`BoardId`,`GroupId`) values 
                    (1,1,'2017-09-25 09:29:08','2017-09-25 09:34:51',1,1,2,'123451','1234567890','2015',5,5,'A+',1,1,4,1),
                    (2,1,'2017-09-25 09:29:08','2017-09-25 09:34:51',1,1,2,'123451','1234567890','2017',5,5,'A+',1,2,4,1),
                    (3,1,'2017-09-25 09:29:08','2017-09-25 09:34:51',1,1,2,'123452','1234567891','2015',5,5,'A+',2,1,4,1),
                    (4,1,'2017-09-25 09:29:08','2017-09-25 09:34:51',1,1,2,'123452','1234567891','2017',5,5,'A+',2,2,4,1),
                    (5,1,'2017-09-25 09:29:08','2017-09-25 09:34:51',1,1,2,'123453','1234567892','2015',5,5,'A+',3,1,4,1),
                    (6,1,'2017-09-25 09:29:08','2017-09-25 09:34:51',1,1,2,'123453','1234567892','2017',5,5,'A+',3,2,4,1),
                    (7,1,'2017-09-25 09:29:08','2017-09-25 09:34:51',1,1,2,'123454','1234567893','2015',5,5,'A+',4,1,4,1),
                    (8,1,'2017-09-25 09:29:08','2017-09-25 09:34:51',1,1,2,'123454','1234567893','2017',5,5,'A+',4,2,4,1),
                    (9,1,'2017-09-25 09:29:08','2017-09-25 09:34:51',1,1,2,'123455','1234567894','2015',5,5,'A+',5,1,4,1),
                    (10,1,'2017-09-25 09:29:08','2017-09-25 09:34:51',1,1,2,'123455','1234567894','2017',5,5,'A+',5,2,4,1);



                CREATE TABLE IF NOT EXISTS `Ncc_Sbr_Student_Result_Details` ( 
                    `Id` BIGINT NOT NULL AUTO_INCREMENT , 
                    `VersionNumber` INT NOT NULL , 
                    `Metadata` varchar(250) COLLATE utf8_unicode_ci NULL,
                    `Name` VARCHAR(250) NULL , 
                    `CreationDate` DATETIME NOT NULL , 
                    `ModificationDate` DATETIME NOT NULL , 
                    `CreateBy` BIGINT NOT NULL , 
                    `ModifyBy` BIGINT NOT NULL , 
                    `Status` INT NOT NULL , 
                                       
                    `StudentResultId` BIGINT NOT NULL , 
                    `ExamSubjectId` BIGINT NOT NULL , 
                    `Gpa` double NOT NULL ,  
                    `Grade` VARCHAR(250) NOT NULL , 
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
                DROP TABLE IF EXISTS `Ncc_SBR_Student_Result_Details`;
                DROP TABLE IF EXISTS `Ncc_SBR_Student_Result`;
                DROP TABLE IF EXISTS `Ncc_SBR_Student`; 
                DROP TABLE IF EXISTS `Ncc_SBR_Exam_Subject`; 
                DROP TABLE IF EXISTS `Ncc_SBR_Exam`; 
                DROP TABLE IF EXISTS `Ncc_SBR_Group`; 
                DROP TABLE IF EXISTS `Ncc_SBR_Board`; 
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
