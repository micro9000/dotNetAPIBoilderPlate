CREATE DATABASE APINETCORESample;
USE APINETCORESample;

CREATE TABLE Roles (
	id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    roleKey VARCHAR(50),
    isDeleted TINYINT DEFAULT 0
)ENGINE=INNODB;

DROP TABLE Roles;

INSERT INTO Roles (roleKey) VALUES ('admin'), ('superuser');
SELECT * FROM Roles;


CREATE TABLE Users (
	id BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    userName VARCHAR(50),
    fullName VARCHAR(100),
    email VARCHAR(100),
    password VARCHAR(255),
    createdAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    updatedAt DATETIME,
    deletedAt DATETIME
)ENGINE=INNODB;
ALTER TABLE Users
ADD COLUMN isDeleted TINYINT DEFAULT 0;

SELECT * FROM Users WHERE isDeleted=0;
SELECT * FROM Users;


CREATE TABLE UserRoles (
	id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    roleId INT NOT NULL,
    userId BIGINT NOT NULL,
    createdAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    deletedAt DATETIME,
    isDeleted TINYINT DEFAULT 0
)ENGINE=INNODB;

INSERT INTO UserRoles (roleId, userId) VALUES (1,1), (2,1);

SELECT * FROM UserRoles;


SELECT DISTINCT U.*
FROM UserRoles AS UR
JOIN Users AS U ON UR.userId=U.id
JOIN Roles AS R ON UR.roleId=R.id
WHERE UR.roleId IN (1,2);

SELECT R.*
FROM UserRoles AS UR
JOIN Users AS U ON UR.userId=U.id
JOIN Roles AS R ON UR.roleId=R.id
WHERE UR.isDeleted=0 AND R.isDeleted=0 AND UR.userID=1;

SELECT R.*
FROM UserRoles AS UR
JOIN Users AS U ON UR.userId=U.id
JOIN Roles AS R ON UR.roleId=R.id
WHERE UR.isDeleted=0 AND R.isDeleted=0 AND UR.userID=1;


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
    isDeleted TINYINT DEFAULT 0,
    FOREIGN KEY (userId) REFERENCES Users (id)
)ENGINE=INNODB;

insert into UserRefreshTokens (UserId, Token, Expires, Created, CreatedByIp, Revoked,RevokedByIp, ReplacedByToken, IsDeleted) 
values (@UserId, @Token, @Expires, @Created, @CreatedByIp, @Revoked, @RevokedByIp, @ReplacedByToken, @IsDeleted);select SCOPE_IDENTITY() id;

SELECT * FROM UserRefreshTokens;