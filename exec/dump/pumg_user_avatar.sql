-- MySQL dump 10.13  Distrib 8.0.31, for Win64 (x86_64)
--
-- Host: k8b108.p.ssafy.io    Database: pumg
-- ------------------------------------------------------
-- Server version	8.0.33

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `user_avatar`
--

DROP TABLE IF EXISTS `user_avatar`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user_avatar` (
  `idx` bigint NOT NULL AUTO_INCREMENT,
  `created_at` datetime(6) DEFAULT NULL,
  `updated_at` datetime(6) DEFAULT NULL,
  `avatar_idx` bigint DEFAULT NULL,
  `user_idx` bigint DEFAULT NULL,
  PRIMARY KEY (`idx`),
  KEY `FKbnoakw937mfnijquk05v2fbus` (`avatar_idx`),
  KEY `FK3v7r1eewuaxg8dh8svrrgouxd` (`user_idx`),
  CONSTRAINT `FK3v7r1eewuaxg8dh8svrrgouxd` FOREIGN KEY (`user_idx`) REFERENCES `user` (`idx`),
  CONSTRAINT `FKbnoakw937mfnijquk05v2fbus` FOREIGN KEY (`avatar_idx`) REFERENCES `avatar` (`idx`)
) ENGINE=InnoDB AUTO_INCREMENT=200 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_avatar`
--

LOCK TABLES `user_avatar` WRITE;
/*!40000 ALTER TABLE `user_avatar` DISABLE KEYS */;
INSERT INTO `user_avatar` VALUES (24,'2023-04-25 19:26:53.783187','2023-04-25 19:26:53.783190',2,41),(25,'2023-04-25 19:31:14.901198','2023-04-25 19:31:14.901201',1,43),(26,'2023-04-25 19:31:24.395539','2023-04-25 19:31:24.395541',1,44),(27,'2023-04-25 20:51:21.616245','2023-04-25 20:51:21.616248',2,45),(28,'2023-04-26 10:18:11.790727','2023-04-26 10:18:11.790727',1,46),(29,'2023-04-26 10:38:58.074676','2023-04-26 10:38:58.074678',1,47),(30,'2023-04-26 15:07:45.878554','2023-04-26 15:07:45.878557',1,48),(31,'2023-04-27 14:42:30.508326','2023-04-27 14:42:30.508328',1,49),(32,'2023-04-27 15:47:08.489486','2023-04-27 15:47:08.489487',1,50),(33,'2023-05-01 10:15:44.226733','2023-05-01 10:15:44.226735',1,52),(34,'2023-05-03 13:52:56.571935','2023-05-03 13:52:56.571937',2,53),(35,'2023-05-08 00:02:52.013200','2023-05-08 00:02:52.013201',1,54),(36,'2023-05-08 00:18:50.855897','2023-05-08 00:18:50.855898',1,56),(37,'2023-05-08 14:53:06.558808','2023-05-08 14:53:06.558810',1,57),(39,'2023-05-08 21:03:59.736948','2023-05-08 21:03:59.736949',1,59),(40,'2023-05-08 21:22:39.739727','2023-05-08 21:22:39.739728',1,61),(41,'2023-05-08 21:46:35.692104','2023-05-08 21:46:35.692106',1,62),(52,'2023-05-09 00:14:29.117882','2023-05-09 00:14:29.117884',1,67),(57,'2023-05-09 00:21:04.978826','2023-05-09 00:21:04.978827',1,68),(58,'2023-05-09 10:03:49.733983','2023-05-09 10:03:49.733984',1,76),(59,'2023-05-09 10:36:50.191221','2023-05-09 10:36:50.191222',1,77),(60,'2023-05-09 10:38:54.090267','2023-05-09 10:38:54.090268',1,78),(61,'2023-05-09 11:06:31.822770','2023-05-09 11:06:31.822771',1,81),(62,'2023-05-09 11:06:32.884237','2023-05-09 11:06:32.884238',1,79),(63,'2023-05-09 13:08:56.104316','2023-05-09 13:08:56.104317',1,82),(64,'2023-05-09 13:23:18.989183','2023-05-09 13:23:18.989185',1,83),(65,'2023-05-09 13:29:16.912079','2023-05-09 13:29:16.912080',1,84),(66,'2023-05-09 13:59:48.918906','2023-05-09 13:59:48.918907',1,86),(67,'2023-05-09 14:13:59.762824','2023-05-09 14:13:59.762826',1,87),(68,'2023-05-09 14:21:50.010774','2023-05-09 14:21:50.010775',1,90),(69,'2023-05-09 15:27:51.383826','2023-05-09 15:27:51.383827',1,91),(70,'2023-05-09 17:22:08.653639','2023-05-09 17:22:08.653641',1,93),(71,'2023-05-10 10:10:15.425920','2023-05-10 10:10:15.425921',1,94),(72,'2023-05-10 13:32:06.072109','2023-05-10 13:32:06.072110',1,95),(73,'2023-05-10 16:52:26.714578','2023-05-10 16:52:26.714579',1,97),(74,'2023-05-11 11:12:28.104763','2023-05-11 11:12:28.104765',1,99),(75,'2023-05-11 12:52:35.110395','2023-05-11 12:52:35.110397',1,100),(76,'2023-05-11 16:47:20.690887','2023-05-11 16:47:20.690889',1,101),(77,'2023-05-12 10:02:48.396492','2023-05-12 10:02:48.396494',1,102),(78,'2023-05-15 08:49:46.293192','2023-05-15 08:49:46.293193',1,104),(79,'2023-05-15 14:02:49.885351','2023-05-15 14:02:49.885353',1,109),(81,'2023-05-15 14:16:52.156407','2023-05-15 14:16:52.156409',1,110),(82,'2023-05-15 14:19:05.731331','2023-05-15 14:19:05.731332',1,111),(84,'2023-05-15 15:31:59.725793','2023-05-15 15:31:59.725794',1,112),(85,'2023-05-15 16:14:13.312450','2023-05-15 16:14:13.312452',1,113),(88,'2023-05-15 16:33:55.850854','2023-05-15 16:33:55.850855',1,87),(90,'2023-05-15 16:41:14.969093','2023-05-15 16:41:14.969094',1,114),(91,'2023-05-15 16:41:32.650913','2023-05-15 16:41:32.650915',2,90),(92,'2023-05-15 17:08:30.157768','2023-05-15 17:08:30.157769',1,113),(94,'2023-05-15 17:22:14.145685','2023-05-15 17:22:14.145686',1,115),(96,'2023-05-15 17:55:13.590880','2023-05-15 17:55:13.590881',5,43),(97,'2023-05-15 20:40:39.830174','2023-05-15 20:40:39.830175',2,117),(98,'2023-05-15 21:07:29.525032','2023-05-15 21:07:29.525033',1,118),(100,'2023-05-15 21:32:41.294672','2023-05-15 21:32:41.294673',1,119),(102,'2023-05-15 21:38:24.282597','2023-05-15 21:38:24.282598',1,120),(104,'2023-05-15 21:51:19.858132','2023-05-15 21:51:19.858133',1,121),(105,'2023-05-15 21:51:34.190096','2023-05-15 21:51:34.190097',1,122),(106,'2023-05-15 23:45:11.073958','2023-05-15 23:45:11.073959',1,123),(108,'2023-05-15 23:53:39.070147','2023-05-15 23:53:39.070148',5,45),(109,'2023-05-16 15:37:40.049929','2023-05-16 15:37:40.049932',1,124),(110,'2023-05-16 21:42:41.907551','2023-05-16 21:42:41.907552',1,125),(111,'2023-05-16 21:56:17.955357','2023-05-16 21:56:17.955359',1,126),(112,'2023-05-16 22:06:39.831546','2023-05-16 22:06:39.831548',1,127),(113,'2023-05-16 22:14:14.712348','2023-05-16 22:14:14.712349',1,128),(114,'2023-05-16 23:15:06.641589','2023-05-16 23:15:06.641591',1,130),(115,'2023-05-17 10:47:46.963158','2023-05-17 10:47:46.963159',1,131),(123,'2023-05-17 11:10:28.577758','2023-05-17 11:10:28.577759',1,136),(128,'2023-05-17 11:14:42.708223','2023-05-17 11:14:42.708224',1,132),(129,'2023-05-17 13:23:57.045968','2023-05-17 13:23:57.045970',1,138),(131,'2023-05-17 13:25:14.825771','2023-05-17 13:25:14.825772',1,137),(132,'2023-05-17 13:42:23.825932','2023-05-17 13:42:23.825933',1,139),(133,'2023-05-17 15:23:59.549421','2023-05-17 15:23:59.549422',1,142),(134,'2023-05-17 15:24:22.264587','2023-05-17 15:24:22.264589',2,143),(135,'2023-05-17 16:13:20.298173','2023-05-17 16:13:20.298175',2,144),(136,'2023-05-17 17:17:59.021045','2023-05-17 17:17:59.021046',1,145),(137,'2023-05-17 17:35:52.970091','2023-05-17 17:35:52.970093',1,146),(138,'2023-05-17 17:56:31.999988','2023-05-17 17:56:31.999989',2,148),(139,'2023-05-17 19:26:33.147115','2023-05-17 19:26:33.147116',2,149),(140,'2023-05-17 19:35:05.508247','2023-05-17 19:35:05.508249',2,150),(141,'2023-05-17 19:38:57.246400','2023-05-17 19:38:57.246402',2,152),(142,'2023-05-17 19:39:27.107648','2023-05-17 19:39:27.107649',5,152),(143,'2023-05-17 20:10:28.181706','2023-05-17 20:10:28.181708',2,153),(144,'2023-05-17 20:20:24.999050','2023-05-17 20:20:24.999051',2,154),(145,'2023-05-17 20:44:10.298945','2023-05-17 20:44:10.298946',8,43),(146,'2023-05-17 20:45:13.099144','2023-05-17 20:45:13.099146',2,155),(147,'2023-05-17 20:45:53.319846','2023-05-17 20:45:53.319848',8,155),(150,'2023-05-17 20:46:54.692672','2023-05-17 20:46:54.692673',2,156),(151,'2023-05-17 20:51:25.996556','2023-05-17 20:51:25.996558',2,157),(152,'2023-05-17 20:56:35.335058','2023-05-17 20:56:35.335060',5,154),(153,'2023-05-17 20:59:36.164673','2023-05-17 20:59:36.164674',1,158),(154,'2023-05-17 21:21:15.023328','2023-05-17 21:21:15.023329',5,139),(155,'2023-05-17 21:34:13.232040','2023-05-17 21:34:13.232041',5,48),(156,'2023-05-17 22:09:24.840307','2023-05-17 22:09:24.840309',5,62),(157,'2023-05-17 22:09:27.344839','2023-05-17 22:09:27.344841',5,156),(158,'2023-05-18 08:31:10.861392','2023-05-18 08:31:10.861393',5,104),(159,'2023-05-18 08:31:42.925990','2023-05-18 08:31:42.925992',8,158),(160,'2023-05-18 09:44:27.530065','2023-05-18 09:44:27.530067',2,162),(161,'2023-05-18 09:45:07.577049','2023-05-18 09:45:07.577051',8,162),(162,'2023-05-18 10:48:21.138058','2023-05-18 10:48:21.138060',2,163),(163,'2023-05-18 10:48:47.548617','2023-05-18 10:48:47.548619',7,163),(164,'2023-05-18 10:49:59.309509','2023-05-18 10:49:59.309511',6,52),(165,'2023-05-18 11:16:26.184659','2023-05-18 11:16:26.184661',1,164),(166,'2023-05-18 11:16:52.035346','2023-05-18 11:16:52.035348',7,164),(167,'2023-05-18 11:49:00.176922','2023-05-18 11:49:00.176923',1,165),(168,'2023-05-18 11:49:23.509733','2023-05-18 11:49:23.509735',8,165),(169,'2023-05-18 11:58:00.462100','2023-05-18 11:58:00.462102',1,168),(170,'2023-05-18 11:58:39.087452','2023-05-18 11:58:39.087454',5,168),(171,'2023-05-18 12:25:16.827441','2023-05-18 12:25:16.827443',1,169),(172,'2023-05-18 12:25:37.787081','2023-05-18 12:25:37.787083',6,169),(173,'2023-05-18 12:57:20.911194','2023-05-18 12:57:20.911195',1,171),(174,'2023-05-18 13:21:55.982816','2023-05-18 13:21:55.982818',2,172),(175,'2023-05-18 15:46:06.433694','2023-05-18 15:46:06.433696',1,74),(177,'2023-05-18 15:46:57.206268','2023-05-18 15:46:57.206269',2,174),(178,'2023-05-18 15:55:50.887712','2023-05-18 15:55:50.887713',5,174),(179,'2023-05-18 16:27:01.040047','2023-05-18 16:27:01.040048',6,97),(180,'2023-05-18 16:31:28.895858','2023-05-18 16:31:28.895859',5,102),(182,'2023-05-18 17:09:53.787435','2023-05-18 17:09:53.787436',2,175),(183,'2023-05-18 17:14:15.036650','2023-05-18 17:14:15.036651',5,175),(184,'2023-05-18 17:15:39.518145','2023-05-18 17:15:39.518146',1,176),(185,'2023-05-18 17:17:33.570653','2023-05-18 17:17:33.570655',6,176),(186,'2023-05-18 17:29:53.489587','2023-05-18 17:29:53.489588',2,178),(187,'2023-05-18 17:31:54.591851','2023-05-18 17:31:54.591852',8,178),(188,'2023-05-18 17:34:07.878595','2023-05-18 17:34:07.878596',1,179),(189,'2023-05-18 17:37:18.557055','2023-05-18 17:37:18.557057',1,181),(190,'2023-05-18 17:37:38.148041','2023-05-18 17:37:38.148042',5,181),(191,'2023-05-18 17:39:47.180662','2023-05-18 17:39:47.180663',1,182),(192,'2023-05-18 17:40:14.065010','2023-05-18 17:40:14.065011',5,182),(193,'2023-05-18 17:50:38.213684','2023-05-18 17:50:38.213686',1,183),(194,'2023-05-18 17:51:01.704031','2023-05-18 17:51:01.704032',5,183),(195,'2023-05-18 17:54:47.091157','2023-05-18 17:54:47.091158',5,49),(196,'2023-05-18 17:57:11.690455','2023-05-18 17:57:11.690457',2,184),(197,'2023-05-18 17:57:36.162930','2023-05-18 17:57:36.162931',5,184),(198,'2023-05-18 18:02:34.383966','2023-05-18 18:02:34.383971',1,185),(199,'2023-05-18 18:03:06.456526','2023-05-18 18:03:06.456530',6,185);
/*!40000 ALTER TABLE `user_avatar` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-05-18 19:43:10