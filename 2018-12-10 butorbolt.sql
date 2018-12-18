--
-- Create database "`14KE_Butorbolt`"
--
CREATE DATABASE `14KE_Butorbolt`
	CHARACTER SET utf8
	COLLATE utf8_hungarian_ci;

USE `14KE_Butorbolt`;

CREATE TABLE Alapanyagok (
  id int NOT NULL AUTO_INCREMENT PRIMARY KEY,
  megnevezes varchar(30) NOT NULL UNIQUE
);

INSERT INTO Alapanyagok(megnevezes)
  VALUES ('Fa'),
         ('Műanyag'),
         ('Fém'),
         ('Papír');

CREATE PROCEDURE Alapanyagok_Select()
BEGIN
  SELECT id, megnevezes
  FROM Alapanyagok;
END;

CREATE TABLE Butorok (
  id int NOT NULL AUTO_INCREMENT PRIMARY KEY,
  megnevezes varchar(100) NOT NULL ,
  alapanyagid int NOT NULL,
  meret varchar(30) NULL,
  ar numeric(14, 2) NULL,
  CONSTRAINT FK_Butorok_alapanyagid FOREIGN KEY (alapanyagid)
    REFERENCES Alapanyagok (id)
);

CREATE UNIQUE INDEX UX_Butorok on Butorok (megnevezes, alapanyagid);

CREATE PROCEDURE Butorok_Select (
  _id int,
  _megnevezes varchar(100),
  _alapanyagid int)
BEGIN
  SELECT Butorok.id, 
         Butorok.megnevezes, 
         Butorok.alapanyagid, 
         Butorok.meret, 
         Butorok.ar,
         Alapanyagok.megnevezes as alapanyagnev
  FROM Butorok JOIN
       Alapanyagok ON Butorok.alapanyagid = Alapanyagok.id
  WHERE (Butorok.id = _id OR _id is NULL)
  AND (Butorok.alapanyagid = _alapanyagid OR _alapanyagid IS NULL)
  AND (Butorok.megnevezes LIKE CONCAT('%', _megnevezes, '%') OR _megnevezes IS NULL);
END;

CREATE PROCEDURE Butorok_Insert (
  _megnevezes varchar(100),
  _alapanyagid int,
  _meret varchar(30),
  _ar numeric(14, 2))
BEGIN
  INSERT INTO Butorok(megnevezes, alapanyagid, meret, ar)
  VALUES (_megnevezes, _alapanyagid, _meret, _ar);

  CALL Butorok_Select(LAST_INSERT_ID(), NULL, NULL);
END;

CREATE PROCEDURE Butorok_Update (
  _id int,
  _megnevezes varchar(100),
  _alapanyagid int,
  _meret varchar(30),
  _ar numeric(14, 2))
BEGIN
  UPDATE Butorok
  SET megnevezes = _megnevezes,
      alapanyagid = _alapanyagid,
      meret = _meret,
      ar = _ar
  WHERE id = _id;

  CALL Butorok_Select(_id, NULL, NULL);
END;

CREATE PROCEDURE Butorok_Delete (
  _id int)
BEGIN
  DELETE FROM Butorok
  WHERE id = _id;
END;

CALL Alapanyagok_Select()
CALL Butorok_Insert("Szék", 1, "60*80*70", 25000);
SELECT * FROM Butorok b;
Call Butorok_Update(1, "Sámli",1, "60*80*50", 24999);
SELECT * FROM Butorok b;
CALL Butorok_Delete(1);
SELECT * FROM Butorok b;