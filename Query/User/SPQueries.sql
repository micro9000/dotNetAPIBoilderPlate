DROP PROCEDURE IF EXISTS spGetUserRoles;
DELIMITER //
CREATE PROCEDURE spGetUserRoles(UserId BIGINT)
BEGIN
	SELECT R.*
    FROM UserRoles AS UR
    JOIN Users AS U ON UR.userId=U.id
    JOIN Roles AS R ON UR.roleId=R.id
    WHERE UR.isDeleted=false AND R.isDeleted=false AND UR.userID=UserId;
END //
DELIMITER ;


DROP PROCEDURE IF EXISTS spGetUserRole;
DELIMITER //
CREATE PROCEDURE spGetUserRole(UserId BIGINT, UserRoleId BIGINT)
BEGIN
	SELECT R.*
    FROM UserRoles AS UR
    JOIN Users AS U ON UR.userId=U.id
    JOIN Roles AS R ON UR.roleId=R.id
    WHERE UR.isDeleted=false AND R.isDeleted=false AND UR.userID=UserId AND UR.id=UserRoleId;
END //
DELIMITER ;