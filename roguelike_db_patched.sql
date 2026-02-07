-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Feb 07, 2026 at 06:51 PM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.0.30

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `roguelike_db`
--

-- --------------------------------------------------------

--
-- Table structure for table `characters`
--

CREATE TABLE `characters` (
  `id` int(11) NOT NULL,
  `name` varchar(255) NOT NULL,
  `hp` int(11) NOT NULL,
  `atk` int(11) NOT NULL,
  `spd` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `characters`
--

INSERT INTO `characters` (`id`, `name`, `hp`, `atk`, `spd`) VALUES
(1, 'Hero', 100, 15, 15),
(2, 'Stranger', 62, 42, 26),
(3, 'Traveler', 101, 6, 23),
(4, 'Rogue', 57, 29, 44),
(5, 'Wizard', 65, 52, 13);

-- --------------------------------------------------------

--
-- Table structure for table `enemies`
--

CREATE TABLE `enemies` (
  `id` int(11) NOT NULL,
  `name` varchar(255) NOT NULL,
  `hp` int(11) NOT NULL,
  `atk` int(11) NOT NULL,
  `spd` int(11) NOT NULL,
  `rarity` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `enemies`
--

INSERT INTO `enemies` (`id`, `name`, `hp`, `atk`, `spd`, `rarity`) VALUES
(1, 'Slime', 5, 7, 4, 1),
(2, 'Goblin', 8, 7, 6, 1),
(3, 'Ork', 10, 9, 7, 1),
(4, 'Giant Spider', 13, 16, 10, 2),
(5, 'Ogre', 16, 15, 11, 2),
(6, 'Ice Golem', 13, 18, 16, 2),
(7, 'Evil Druid', 21, 20, 22, 3),
(8, 'Giant Bat', 20, 22, 30, 3),
(9, 'Wind elemental', 29, 23, 21, 3),
(10, 'Centaur', 36, 36, 31, 4),
(11, 'Chimera', 37, 33, 31, 4),
(12, 'Dream Demon', 30, 35, 38, 4),
(13, 'Fire Dragon', 45, 45, 40, 5),
(14, 'Ice Dragon', 42, 42, 46, 5),
(15, 'Vampire Baron', 41, 46, 41, 5);

-- --------------------------------------------------------

--
-- Table structure for table `enemylootpool`
--

CREATE TABLE `enemylootpool` (
  `id` bigint(20) UNSIGNED NOT NULL,
  `enemy_rarity` int(11) DEFAULT NULL,
  `item_and_weapon_id` int(11) DEFAULT NULL,
  `drop_chance` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `enemylootpool`
--

INSERT INTO `enemylootpool` (`id`, `enemy_rarity`, `item_and_weapon_id`, `drop_chance`) VALUES
(1, 1, 101, 30),
(2, 1, 102, 20),
(3, 1, 103, 10),
(4, 1, 1, 20),
(5, 1, 2, 20),
(6, 1, 3, 20),
(7, 2, 101, 40),
(8, 2, 102, 30),
(9, 2, 103, 20),
(10, 2, 104, 10),
(11, 2, 105, 5),
(12, 2, 1, 30),
(13, 2, 2, 30),
(14, 2, 3, 30),
(15, 2, 8, 20),
(16, 2, 9, 20),
(17, 3, 103, 40),
(18, 3, 104, 30),
(19, 3, 105, 20),
(20, 3, 106, 10),
(21, 3, 107, 5),
(22, 3, 1, 40),
(23, 3, 2, 40),
(24, 3, 3, 40),
(25, 3, 8, 30),
(26, 3, 9, 30),
(27, 3, 4, 20),
(28, 3, 5, 20),
(29, 4, 105, 40),
(30, 4, 106, 30),
(31, 4, 107, 20),
(32, 4, 108, 10),
(33, 4, 109, 5),
(34, 4, 1, 50),
(35, 4, 2, 50),
(36, 4, 3, 50),
(37, 4, 8, 40),
(38, 4, 9, 40),
(39, 4, 4, 30),
(40, 4, 5, 30),
(41, 4, 6, 20),
(42, 4, 7, 20),
(43, 5, 107, 40),
(44, 5, 108, 30),
(45, 5, 109, 20),
(46, 5, 110, 10),
(47, 5, 1, 60),
(48, 5, 2, 60),
(49, 5, 3, 60),
(50, 5, 8, 50),
(51, 5, 9, 50),
(52, 5, 4, 40),
(53, 5, 5, 40),
(54, 5, 6, 30),
(55, 5, 7, 30),
(56, 5, 10, 20);

-- --------------------------------------------------------

--
-- Table structure for table `items`
--

CREATE TABLE `items` (
  `id` int(11) NOT NULL,
  `name` varchar(30) DEFAULT NULL,
  `stat` varchar(30) DEFAULT NULL,
  `inc` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Dumping data for table `items`
--

INSERT INTO `items` (`id`, `name`, `stat`, `inc`) VALUES
(1, 'Strength Potion', 'atk', 20),
(2, 'Health Potion', 'hp', 15),
(3, 'Speed Potion', 'spd', 20),
(4, 'Unknown Potion', 'atk', 30),
(5, 'Unknown Potion', 'atk', -20),
(6, 'Gladiator helmet', 'hp', 10),
(7, 'Adventurer Boots', 'spd', 15),
(8, 'Beef Steak', 'hp', 30),
(9, 'Scruffy Adventurer Boots', 'spd', 10),
(10, 'Unknown Potion', 'hp', -10);

-- --------------------------------------------------------

--
-- Table structure for table `weapons`
--

CREATE TABLE `weapons` (
  `id` int(11) NOT NULL,
  `name` varchar(255) NOT NULL,
  `atk` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `weapons`
--

INSERT INTO `weapons` (`id`, `name`, `atk`) VALUES
(101, 'Slingshot', 5),
(102, 'Rusty dagger', 10),
(103, 'Iron Sword', 15),
(104, 'Battle axe', 20),
(105, 'Greatsword', 25),
(106, 'Magic wand', 30),
(107, 'Rune Bow', 35),
(108, 'The Magic Mace of the North', 40),
(109, 'The Spear of the Thunder Bringer', 45),
(110, 'The Excalibur', 50);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `characters`
--
ALTER TABLE `characters`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `enemies`
--
ALTER TABLE `enemies`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `enemylootpool`
--
ALTER TABLE `enemylootpool`
  ADD PRIMARY KEY (`id`),
  ADD KEY `enemy_id` (`enemy_rarity`);

--
-- Indexes for table `items`
--
ALTER TABLE `items`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `weapons`
--
ALTER TABLE `weapons`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `enemylootpool`
--
ALTER TABLE `enemylootpool`
  MODIFY `id` bigint(20) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=57;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `enemylootpool`
--
ALTER TABLE `enemylootpool`
  ADD CONSTRAINT `enemylootpool_ibfk_1` FOREIGN KEY (`enemy_rarity`) REFERENCES `enemies` (`id`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
