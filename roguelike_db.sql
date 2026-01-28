-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1
-- Létrehozás ideje: 2026. Jan 28. 14:14
-- Kiszolgáló verziója: 10.4.32-MariaDB
-- PHP verzió: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Adatbázis: `roguelike_db`
--

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `characteritems`
--

CREATE TABLE `characteritems` (
  `character_id` int(11) NOT NULL,
  `item_id` int(11) NOT NULL,
  `item_order` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- A tábla adatainak kiíratása `characteritems`
--

INSERT INTO `characteritems` (`character_id`, `item_id`, `item_order`) VALUES
(1, 101, 0),
(1, 102, 1);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `characters`
--

CREATE TABLE `characters` (
  `id` int(11) NOT NULL,
  `name` varchar(255) NOT NULL,
  `hp` int(11) NOT NULL,
  `atk` int(11) NOT NULL,
  `spd` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- A tábla adatainak kiíratása `characters`
--

INSERT INTO `characters` (`id`, `name`, `hp`, `atk`, `spd`) VALUES
(1, 'Hero', 100, 15, 10);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `enemies`
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
-- A tábla adatainak kiíratása `enemies`
--

INSERT INTO `enemies` (`id`, `name`, `hp`, `atk`, `spd`, `rarity`) VALUES
(1, 'Slime', 20, 5, 2, 1);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `enemylootpool`
--

CREATE TABLE `enemylootpool` (
  `id` bigint(20) UNSIGNED NOT NULL,
  `enemy_id` int(11) NOT NULL,
  `pool_index` int(11) NOT NULL,
  `item_id` int(11) NOT NULL,
  `item_value` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- A tábla adatainak kiíratása `enemylootpool`
--

INSERT INTO `enemylootpool` (`id`, `enemy_id`, `pool_index`, `item_id`, `item_value`) VALUES
(1, 1, 0, 101, 50),
(2, 1, 0, 102, 25);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `weapons`
--

CREATE TABLE `weapons` (
  `id` int(11) NOT NULL,
  `name` varchar(255) NOT NULL,
  `atk` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- A tábla adatainak kiíratása `weapons`
--

INSERT INTO `weapons` (`id`, `name`, `atk`) VALUES
(1, 'Iron Sword', '10-15');

--
-- Indexek a kiírt táblákhoz
--

--
-- A tábla indexei `characteritems`
--
ALTER TABLE `characteritems`
  ADD PRIMARY KEY (`character_id`,`item_order`);

--
-- A tábla indexei `characters`
--
ALTER TABLE `characters`
  ADD PRIMARY KEY (`id`);

--
-- A tábla indexei `enemies`
--
ALTER TABLE `enemies`
  ADD PRIMARY KEY (`id`);

--
-- A tábla indexei `enemylootpool`
--
ALTER TABLE `enemylootpool`
  ADD PRIMARY KEY (`id`),
  ADD KEY `enemy_id` (`enemy_id`);

--
-- A tábla indexei `weapons`
--
ALTER TABLE `weapons`
  ADD PRIMARY KEY (`id`);

--
-- A kiírt táblák AUTO_INCREMENT értéke
--

--
-- AUTO_INCREMENT a táblához `enemylootpool`
--
ALTER TABLE `enemylootpool`
  MODIFY `id` bigint(20) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- Megkötések a kiírt táblákhoz
--

--
-- Megkötések a táblához `characteritems`
--
ALTER TABLE `characteritems`
  ADD CONSTRAINT `characteritems_ibfk_1` FOREIGN KEY (`character_id`) REFERENCES `characters` (`id`) ON DELETE CASCADE;

--
-- Megkötések a táblához `enemylootpool`
--
ALTER TABLE `enemylootpool`
  ADD CONSTRAINT `enemylootpool_ibfk_1` FOREIGN KEY (`enemy_id`) REFERENCES `enemies` (`id`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
