# SQL Manager Lite for MySQL 5.3.1.7
# ---------------------------------------
# Host     : localhost
# Port     : 3306
# Database : Bookstore


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

SET FOREIGN_KEY_CHECKS=0;

CREATE DATABASE `Bookstore`
    CHARACTER SET 'utf8'
    COLLATE 'utf8_general_ci';

USE `bookstore`;

#
# Structure for the `authors` table : 
#

CREATE TABLE `authors` (
  `AuthorId` INTEGER(10) NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(50) COLLATE utf8_general_ci NOT NULL,
  PRIMARY KEY USING BTREE (`AuthorId`) COMMENT ''
)ENGINE=InnoDB
AUTO_INCREMENT=8 AVG_ROW_LENGTH=2340 CHARACTER SET 'utf8' COLLATE 'utf8_general_ci'
COMMENT=''
;

#
# Structure for the `books` table : 
#

CREATE TABLE `books` (
  `BookId` INTEGER(10) NOT NULL AUTO_INCREMENT,
  `Title` VARCHAR(100) COLLATE utf8_general_ci NOT NULL,
  `AuthorId` INTEGER(10) NOT NULL,
  `PublishDate` DATE DEFAULT NULL,
  `ISBN` INTEGER(10) DEFAULT NULL,
  PRIMARY KEY USING BTREE (`BookId`) COMMENT '',
   INDEX `AuthorId` USING BTREE (`AuthorId`) COMMENT '',
  CONSTRAINT `books_fk1` FOREIGN KEY (`AuthorId`) REFERENCES `authors` (`AuthorId`) ON UPDATE CASCADE
)ENGINE=InnoDB
AUTO_INCREMENT=10 AVG_ROW_LENGTH=2048 CHARACTER SET 'utf8' COLLATE 'utf8_general_ci'
COMMENT=''
;

#
# Data for the `authors` table  (LIMIT -492,500)
#

INSERT INTO `authors` (`AuthorId`, `Name`) VALUES

  (1,'Pesho'),
  (2,'Gosho'),
  (3,'Tosho'),
  (4,'Sasho'),
  (5,'Mustafa'),
  (6,'Asen'),
  (7,'AZ!');
COMMIT;

#
# Data for the `books` table  (LIMIT -491,500)
#

INSERT INTO `books` (`BookId`, `Title`, `AuthorId`, `PublishDate`, `ISBN`) VALUES

  (2,'Az sum Pesho',1,'2013-07-16',1111111111),
  (3,'Az sum Pesho, no veche razlichen',1,'2013-07-16',11111),
  (4,'Az sum Gosho',2,'2013-07-16',22222),
  (5,'Az sum Tosho',3,'2013-07-16',33333),
  (6,'Az sum Sasho',4,'2013-07-16',44444),
  (7,'Az sum Mustafa',5,'2013-07-16',55555),
  (8,'Az sum Asen',6,'2013-07-16',66666),
  (9,'Az sum AZ!',7,'2013-07-16',77777);
COMMIT;



/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;