CREATE DATABASE APINETCORESample;
USE APINETCORESample;

CREATE TABLE Roles (
	id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    roleKey VARCHAR(50),
    createdAt DATETIME DEFAULT NOW(),
    updatedAt DATETIME DEFAULT NOW() ON UPDATE NOW(),
    deletedAt DATETIME
)ENGINE=INNODB;
ALTER TABLE Roles
ADD COLUMN isDeleted BOOLEAN DEFAULT False;

INSERT INTO Roles (roleKey) VALUES ('admin'), ('superuser');
SELECT * FROM Roles;


CREATE TABLE Users (
	id BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    userName VARCHAR(50),
    fullName VARCHAR(100),
    email VARCHAR(100),
    password VARCHAR(255),
    acceptTerms BOOLEAN,
    verificationToken TEXT,
    isVerified BOOLEAN,
    resetToken TEXT,
    resetTokenExpiresAt DATETIME,
    passwordResetAt DATETIME,
    createdAt DATETIME DEFAULT NOW(),
    updatedAt DATETIME DEFAULT NOW() ON UPDATE NOW(),
    deletedAt DATETIME
)ENGINE=INNODB;
ALTER TABLE Users
ADD COLUMN isDeleted BOOLEAN DEFAULT False;
ALTER TABLE Users
ADD COLUMN verifiedAt DATETIME;


SELECT * FROM Users;

SELECT MIN(DATETIME);


CREATE TABLE UserRoles (
	id BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    roleId INT NOT NULL,
    userId BIGINT NOT NULL,
    createdAt DATETIME DEFAULT NOW(),
    updatedAt DATETIME DEFAULT NOW() ON UPDATE NOW(),
    deletedAt DATETIME,
    isDeleted BOOLEAN DEFAULT False
)ENGINE=INNODB;
INSERT INTO UserRoles (roleId, userId) VALUES (1,1), (2,1);

SELECT * FROM UserRoles WHERE id=@UserRoleId AND userId=@UserId;


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
ALTER TABLE UserRefreshTokens
ADD COLUMN isDeleted BOOLEAN DEFAULT False;


SELECT * FROM UserRefreshTokens;

SELECT * FROM UserRefreshTokens AS URT
INNER JOIN Users AS U ON U.id = URT.userId
WHERE U.isDeleted=false AND URT.isDeleted=false 
AND URT.token = 'fhzvJ9qxrCogV/xCe8DaqkLzIE3XzK9bfbIIoko6cYuKjp0NqWseLN48ZsIOo30lE6+o9dbLSh/K/hDmxYRNMg==';