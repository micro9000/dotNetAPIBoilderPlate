CREATE DATABASE APINETCORESample;
USE APINETCORESample;

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