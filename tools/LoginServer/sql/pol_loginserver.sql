# HeidiSQL Dump 
#
# --------------------------------------------------------
# Host:                 127.0.0.1
# Database:             pol_loginserver
# Server version:       5.0.51a
# Server OS:            Win32
# Target-Compatibility: MySQL 5.1 and above
# Extended INSERTs:     Y
# max_allowed_packet:   1048576
# HeidiSQL version:     3.0 Revision: 572
# --------------------------------------------------------

/*!40100 SET CHARACTER SET latin1*/;


#
# Database structure for database 'pol_loginserver'
#

CREATE DATABASE `pol_loginserver` /*!40100 DEFAULT CHARACTER SET latin1 */;

USE `pol_loginserver`;


DROP TABLE IF EXISTS `accounts`;

#
# Table structure for table 'accounts'
#

CREATE TABLE `accounts` (
  `username` varchar(50) NOT NULL,
  `password` text NOT NULL,
  `enabled` tinyint(3) unsigned default '1',
  `banned` tinyint(3) unsigned default '0',
  `expansion` varchar(10) NOT NULL default 'T2A',
  `email` varchar(50) default NULL,
  `defaultprivs` varchar(50) default NULL,
  `defaultcmdlevel` varchar(50) default NULL,
  `last_ip` varchar(50) default NULL,
  `last_login` timestamp NOT NULL default CURRENT_TIMESTAMP on update CURRENT_TIMESTAMP,
  PRIMARY KEY  (`username`),
  UNIQUE KEY `username` (`username`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1 COMMENT='User Account Details';



#
# Dumping data for table 'accounts'
#

/*!40000 ALTER TABLE `accounts` DISABLE KEYS*/;
LOCK TABLES `accounts` WRITE;
UNLOCK TABLES;
/*!40000 ALTER TABLE `accounts` ENABLE KEYS*/;
