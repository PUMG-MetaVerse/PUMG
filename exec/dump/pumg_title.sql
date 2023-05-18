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
-- Table structure for table `title`
--

DROP TABLE IF EXISTS `title`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `title` (
  `idx` bigint NOT NULL AUTO_INCREMENT,
  `created_at` datetime(6) DEFAULT NULL,
  `updated_at` datetime(6) DEFAULT NULL,
  `description` varchar(255) COLLATE utf8mb3_bin DEFAULT NULL,
  `title` varchar(255) COLLATE utf8mb3_bin DEFAULT NULL,
  PRIMARY KEY (`idx`)
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `title`
--

LOCK TABLES `title` WRITE;
/*!40000 ALTER TABLE `title` DISABLE KEYS */;
INSERT INTO `title` VALUES (1,'2023-05-10 07:05:07.000000','2023-05-10 07:05:07.000000','타겟을 정확히 맞추는 당신은 명사수','[백발백중하는 명사수]'),(2,'2023-05-10 07:06:46.000000','2023-05-10 07:06:46.000000','만점을 얻은 자','[저 뉴비인데요]'),(3,'2023-05-10 07:26:59.000000','2023-05-10 07:26:59.000000','전투기 속도 정도는 느리다','[遅い(오소이)]'),(4,'2023-05-10 08:39:49.000000','2023-05-10 08:39:49.000000','처음 접속시 획득','[Hello, World!]'),(5,'2023-05-14 10:40:14.000000','2023-05-14 10:40:14.000000','여기가 어디죠?','[여기가 어디죠_]'),(6,'2023-05-14 10:40:21.000000','2023-05-14 10:40:21.000000','2층을 탈출한 자','[JUNIOR ESCAPE MAN]'),(7,'2023-05-14 10:40:21.000000','2023-05-14 10:40:21.000000','1층을 탈출한 자','[SENIOR ESCAPE MAN]'),(8,'2023-05-14 10:40:21.000000','2023-05-14 10:40:21.000000','모든 층을 탈출한 자','[MASTER ESCAPE MAN]'),(9,'2023-05-14 10:40:21.000000','2023-05-14 10:40:21.000000','모닥불 발견','[불멍좌]'),(10,'2023-05-14 10:40:21.000000','2023-05-14 10:40:21.000000','수영을 해보자','[음~파!]'),(11,'2023-05-14 10:40:21.000000','2023-05-14 10:40:21.000000','클러버','[둠칫둠칫]'),(12,'2023-05-14 10:40:21.000000','2023-05-14 10:40:21.000000','하늘에서 풍경을 본 자','[양손에 목숨  가득 쥐고]');
/*!40000 ALTER TABLE `title` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-05-18 19:43:08