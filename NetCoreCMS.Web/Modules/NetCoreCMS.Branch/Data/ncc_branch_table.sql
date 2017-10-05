CREATE TABLE `ncc_branch` ( 
	`Id` BIGINT NOT NULL AUTO_INCREMENT, 
	`VersionNumber` INT NOT NULL, 
	`Name` VARCHAR(250) NOT NULL, 
	`CreationDate` DATETIME NOT NULL, 
	`ModificationDate` DATETIME NOT NULL, 
	`CreateBy` BIGINT NOT NULL, 
	`ModifyBy` BIGINT NOT NULL, 
	`Status` INT NOT NULL, 
	
	`Address` VARCHAR(1000), 
	`Description` TEXT, 
	`Phone` VARCHAR(250), 
	`LatLon` VARCHAR(250), 
	`GoogleLocation` VARCHAR(1000), 
	`ZoomLevel` INT NOT NULL, 
	`IsAdmissionOnly` bit(1) NOT NULL, 
	`Order` INT NOT NULL, 
	PRIMARY KEY (`Id`)
) ENGINE = MyISAM; 