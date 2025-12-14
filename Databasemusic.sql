-- ========================================
-- MUSIC INSTRUMENT EXPLORER DATABASE
-- Complete SQL Script for MySQL Workbench
-- ========================================

-- Step 1: Create Database
CREATE DATABASE IF NOT EXISTS musicdb;
USE musicdb;

-- Step 2: Create Tables
-- ========================================

-- Instruments Table
CREATE TABLE instruments (
    instrument_id INT PRIMARY KEY AUTO_INCREMENT,
    instrument_name VARCHAR(100) NOT NULL,
    instrument_type VARCHAR(50) NOT NULL,
    UNIQUE KEY (instrument_name)
);

-- Albums Table
CREATE TABLE albums (
    album_id INT PRIMARY KEY AUTO_INCREMENT,
    album_name VARCHAR(200) NOT NULL,
    artist VARCHAR(200) NOT NULL,
    release_year VARCHAR(4),
    UNIQUE KEY (album_name, artist)
);

-- Songs Table
CREATE TABLE songs (
    song_id INT PRIMARY KEY AUTO_INCREMENT,
    song_name VARCHAR(200) NOT NULL,
    album_id INT NOT NULL,
    duration VARCHAR(10),
    genre VARCHAR(50),
    audio_path VARCHAR(500),
    FOREIGN KEY (album_id) REFERENCES albums(album_id) ON DELETE CASCADE
);

-- Song-Instruments Junction Table (Many-to-Many)
CREATE TABLE song_instruments (
    song_instrument_id INT PRIMARY KEY AUTO_INCREMENT,
    song_id INT NOT NULL,
    instrument_id INT NOT NULL,
    FOREIGN KEY (song_id) REFERENCES songs(song_id) ON DELETE CASCADE,
    FOREIGN KEY (instrument_id) REFERENCES instruments(instrument_id) ON DELETE CASCADE,
    UNIQUE KEY (song_id, instrument_id)
);

-- Step 3: Insert Sample Data
-- ========================================

-- Insert Sample Instruments
INSERT INTO instruments (instrument_name, instrument_type) VALUES
('Acoustic Guitar', 'String'),
('Electric Guitar', 'String'),
('Bass Guitar', 'String'),
('Violin', 'String'),
('Cello', 'String'),
('Piano', 'Keyboard'),
('Synthesizer', 'Keyboard'),
('Drums', 'Percussion'),
('Trumpet', 'Brass'),
('Saxophone', 'Woodwind'),
('Flute', 'Woodwind'),
('Clarinet', 'Woodwind');

-- Insert Sample Albums
INSERT INTO albums (album_name, artist, release_year) VALUES
('Acoustic Dreams', 'The Melody Makers', '2020'),
('Electric Nights', 'Thunder Band', '2021'),
('Classical Collection', 'Symphony Orchestra', '2019'),
('Jazz Essentials', 'Jazz Masters', '2022'),
('Rock Legends', 'Stone Rockers', '2018'),
('Piano Sonatas', 'Classical Virtuoso', '2023');

UPDATE albums SET album_name = 'Debussy: Syrinx & Other Works' WHERE album_name = 'Jazz Essentials';
UPDATE albums SET artist = 'Claude Debussy' WHERE album_name = 'Debussy: Syrinx & Other Works';

-- Insert Sample Songs for Album 1 (Acoustic Dreams)
INSERT INTO songs (song_name, album_id, duration, genre, audio_path) VALUES
('Morning Sunrise', 1, '3:45', 'Folk', 'C:\\Music\\morning_sunrise.wav'),
('Gentle Breeze', 1, '4:12', 'Acoustic', 'C:\\Music\\gentle_breeze.wav'),
('Mountain Echo', 1, '5:30', 'Folk', 'C:\\Music\\mountain_echo.wav');

UPDATE songs SET song_name = 'Multo' WHERE song_name = 'Morning Sunrise';
UPDATE songs SET song_name = 'Tingin' WHERE song_name = 'Gentle Breeze';
UPDATE songs SET song_name = 'Misteryoso' WHERE song_name = 'Mountain Echo';
UPDATE songs SET duration = '3:42' WHERE song_name = 'Tingin';
UPDATE songs SET duration = '3:41' WHERE song_name = 'Misteryoso';
UPDATE songs SET duration = '3:57' WHERE song_name = 'Multo';
UPDATE songs SET genre = 'OPM' WHERE song_name = 'Multo';
UPDATE songs SET genre = 'OPM' WHERE song_name = 'Misteryoso';
UPDATE songs SET genre = 'OPM' WHERE song_name = 'Tingin';
UPDATE songs SET audio_path = 'C:\\Music\\Misteryoso.wav' WHERE song_name = 'Misteryoso';
UPDATE songs SET audio_path = 'C:\\Music\\Tingin.wav' WHERE song_name = 'Tingin';

-- Insert Sample Songs for Album 2 (Electric Nights)
INSERT INTO songs (song_name, album_id, duration, genre, audio_path) VALUES
('Lightning Strike', 2, '3:20', 'Rock', 'C:\\Music\\lightning_strike.wav'),
('Neon Dreams', 2, '4:05', 'Electronic Rock', 'C:\\Music\\neon_dreams.wav'),
('Thunder Road', 2, '3:55', 'Rock', 'C:\\Music\\thunder_road.wav');

UPDATE songs SET song_name = 'Thunder Lightning' WHERE song_name = 'Thunder Road'; 
UPDATE songs SET audio_path = 'C:\\Music\\imagine_dragons.wav' WHERE song_name = 'Thunder Lightning';

-- Insert Sample Songs for Album 3 (Classical Collection)
INSERT INTO songs (song_name, album_id, duration, genre, audio_path) VALUES
('String Quartet No. 1', 3, '6:30', 'Classical', 'C:\\Music\\string_quartet.wav'),
('Violin Concerto', 3, '8:15', 'Classical', 'C:\\Music\\violin_concerto.wav'),
('Cello Suite', 3, '7:00', 'Classical', 'C:\\Music\\cello_suite.wav');

-- Insert Sample Songs for Album 4 (Jazz Essentials)
INSERT INTO songs (song_name, album_id, duration, genre, audio_path) VALUES
('Blue Note', 4, '5:20', 'Jazz', 'C:\\Music\\blue_note.wav'),
('Smooth Saxophone', 4, '4:45', 'Jazz', 'C:\\Music\\smooth_sax.wav'),
('Midnight Jazz', 4, '6:10', 'Jazz', 'C:\\Music\\midnight_jazz.wav');

DELETE from songs WHERE song_name = 'Blue Note';
DELETE from songs WHERE song_name = 'Smooth Saxophone';
UPDATE songs SET song_name = 'Syrinx' WHERE song_name = 'Midnight Jazz';
UPDATE songs SET audio_path = 'C:\\Music\\Syrinx.wav' WHERE song_name = 'Syrinx';

-- Insert Sample Songs for Album 5 (Rock Legends)
INSERT INTO songs (song_name, album_id, duration, genre, audio_path) VALUES
('Guitar Hero', 5, '4:30', 'Hard Rock', 'C:\\Music\\guitar_hero.wav'),
('Bass Thunder', 5, '3:50', 'Rock', 'C:\\Music\\bass_thunder.wav'),
('Drum Solo', 5, '2:45', 'Rock', 'C:\\Music\\drum_solo.wav');

UPDATE songs SET song_name = 'Eye of the Tiger' WHERE song_name = 'Guitar Hero';
UPDATE songs SET song_name = 'Thunder' WHERE song_name = 'Imagine Dragons';
UPDATE songs SET audio_path = 'C:\\Music\\imagine_dragons.wav' WHERE song_name = 'Thunder';

-- Insert Sample Songs for Album 6 (Piano Sonatas)
INSERT INTO songs (song_name, album_id, duration, genre, audio_path) VALUES
('Moonlight Sonata', 6, '5:30', 'Classical', 'C:\\Music\\moonlight.wav'),
('Piano Dreams', 6, '4:15', 'Contemporary', 'C:\\Music\\piano_dreams.wav'),
('Nocturne in E', 6, '6:00', 'Classical', 'C:\\Music\\nocturne.wav');

-- Step 4: Link Songs to Instruments
-- ========================================

-- Link Songs to Instruments (Album 1 - Acoustic Dreams)
INSERT INTO song_instruments (song_id, instrument_id) VALUES
(1, 1), (1, 4), (1, 6),  -- Morning Sunrise: Acoustic Guitar, Violin, Piano
(2, 1), (2, 11),         -- Gentle Breeze: Acoustic Guitar, Flute
(3, 1), (3, 5);          -- Mountain Echo: Acoustic Guitar, Cello

-- Link Songs to Instruments (Album 2 - Electric Nights)
INSERT INTO song_instruments (song_id, instrument_id) VALUES
(4, 2), (4, 3), (4, 8),  -- Lightning Strike: Electric Guitar, Bass, Drums
(5, 2), (5, 7),          -- Neon Dreams: Electric Guitar, Synthesizer
(6, 2), (6, 3), (6, 8);  -- Thunder Road: Electric Guitar, Bass, Drums

-- Link Songs to Instruments (Album 3 - Classical Collection)
INSERT INTO song_instruments (song_id, instrument_id) VALUES
(7, 4), (7, 5),          -- String Quartet: Violin, Cello
(8, 4), (8, 6),          -- Violin Concerto: Violin, Piano
(9, 5);                  -- Cello Suite: Cello

-- Link Songs to Instruments (Album 4 - Jazz Essentials)
INSERT INTO song_instruments (song_id, instrument_id) VALUES
(10, 6), (10, 9), (10, 8),  -- Blue Note: Piano, Trumpet, Drums
(11, 10), (11, 6),           -- Smooth Saxophone: Saxophone, Piano
(12, 10), (12, 3), (12, 8);  -- Midnight Jazz: Saxophone, Bass, Drums

UPDATE song_instruments SET instrument_id = 11 WHERE song_id = 12 AND instrument_id = 10;
DELETE FROM song_instruments WHERE song_id = 12 and instrument_id = 3;
DELETE FROM song_instruments WHERE song_id = 12 and instrument_id = 8;

-- Link Songs to Instruments (Album 5 - Rock Legends)
INSERT INTO song_instruments (song_id, instrument_id) VALUES
(13, 2), (13, 8),        -- Guitar Hero: Electric Guitar, Drums
(14, 3), (14, 8),        -- Bass Thunder: Bass Guitar, Drums
(15, 8);                 -- Drum Solo: Drums

-- Link Songs to Instruments (Album 6 - Piano Sonatas)
INSERT INTO song_instruments (song_id, instrument_id) VALUES
(16, 6),                 -- Moonlight Sonata: Piano
(17, 6),                 -- Piano Dreams: Piano
(18, 6);                 -- Nocturne in E: Piano

UPDATE songs SET song_name = 'Multo' WHERE song_name = 'Morning Sunrise';
UPDATE albums SET album_name = 'Multo' WHERE album_name = 'Acoustic Dreams';
UPDATE songs SET audio_path = 'C:\\Music\\multo.wav' WHERE song_name = 'multo';
UPDATE albums SET artist = 'Cup of Joe' WHERE album_name = 'Multo';

-- Step 5: Verification Queries (Optional - Run to verify data)
-- ========================================

-- View all instruments with their types
SELECT * FROM instruments ORDER BY instrument_type, instrument_name;

-- View all albums with song count
SELECT a.album_name, a.artist, COUNT(s.song_id) as song_count
FROM albums a
LEFT JOIN songs s ON a.album_id = s.album_id
GROUP BY a.album_id
ORDER BY a.album_name;

-- View songs with their instruments (example for String instruments)
SELECT DISTINCT a.album_name, a.artist, s.song_name, i.instrument_name, i.instrument_type
FROM albums a
INNER JOIN songs s ON a.album_id = s.album_id
INNER JOIN song_instruments si ON s.song_id = si.song_id
INNER JOIN instruments i ON si.instrument_id = i.instrument_id
WHERE i.instrument_type = 'String'
ORDER BY a.album_name, s.song_name;

-- ========================================
-- DATABASE SETUP COMPLETE!
-- ========================================
-- Your database is now ready to use with the C# application
-- Remember to update the connection string in MainForm.cs