CREATE DATABASE APINETCORESample;
USE APINETCORESample;

CREATE TABLE Roles (
	id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    roleKey VARCHAR(50),
    createdAt DATETIME DEFAULT NOW(),
    updatedAt DATETIME DEFAULT NOW() ON UPDATE NOW(),
    deletedAt DATETIME
)ENGINE=INNODB;


INSERT INTO Roles (roleKey) VALUES ('admin'), ('superuser');
SELECT * FROM Roles;


CREATE TABLE Users (
	id BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    userName VARCHAR(50),
    fullName VARCHAR(100),
    email VARCHAR(100),
    password VARCHAR(255),
    createdAt DATETIME DEFAULT NOW(),
    updatedAt DATETIME DEFAULT NOW() ON UPDATE NOW(),
    deletedAt DATETIME
)ENGINE=INNODB;


SELECT * FROM Users WHERE isDeleted=0;
SELECT * FROM Users;


CREATE TABLE UserRoles (
	id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    roleId INT NOT NULL,
    userId BIGINT NOT NULL,
    createdAt DATETIME DEFAULT NOW(),
    updatedAt DATETIME DEFAULT NOW() ON UPDATE NOW(),
    deletedAt DATETIME
)ENGINE=INNODB;
INSERT INTO UserRoles (roleId, userId) VALUES (1,1), (2,1);


CREATE TABLE UserRefreshTokens (
	id BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    userId BIGINT NOT NULL,
    token TEXT,
    expires DATETIME,
    created DATETIME,
    createdByIp VARCHAR(50),
    revoked DATETIME,
    revokedByIp VARCHAR(50),
    replacedByToken TEXT,
    createdAt DATETIME DEFAULT NOW(),
    updatedAt DATETIME DEFAULT NOW() ON UPDATE NOW(),
    deletedAt DATETIME,
    FOREIGN KEY (userId) REFERENCES Users (id)
)ENGINE=INNODB;


SELECT * FROM UserRefreshTokens;