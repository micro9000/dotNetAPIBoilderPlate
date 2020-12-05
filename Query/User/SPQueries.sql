DROP PROCEDURE IF EXISTS spGetUserRoles;
DELIMITER //
CREATE PROCEDURE spGetUserRoles(UserId SMALLINT)
BEGIN
	SELECT R.*
    FROM UserRoles AS UR
    JOIN Users AS U ON UR.userId=U.id
    JOIN Roles AS R ON UR.roleId=R.id
    WHERE UR.isDeleted=false AND R.isDeleted=false AND UR.userID=UserId;
END //
DELIMITER ;