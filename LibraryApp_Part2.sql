--------------------Drop tables for fresh initialization-----------------------
DROP TABLE Roles;
DROP TABLE Users;
DROP TABLE Books;
DROP TABLE Exceptions;

--------------------Drop create procedures for fresh initialization------------
DROP PROCEDURE AddRole;
DROP PROCEDURE AddUser;
DROP PROCEDURE AddBook;
DROP PROCEDURE AddException;

--------------------Drop read procedures for fresh initialization--------------
DROP PROCEDURE SelectAllRoles;
DROP PROCEDURE ReturnNumRoles;
DROP PROCEDURE SelectAllUsers;
DROP PROCEDURE SelectAllBooks;

--------------------Drop update procedures for fresh initialization------------
DROP PROCEDURE UpdateRole;
DROP PROCEDURE UpdateUserGeneral;
DROP PROCEDURE UpdateUserLoginStatus;
DROP PROCEDURE UpdateBook;
DROP PROCEDURE UpdateBookIsCheckedOut;

--------------------Drop delete procedures for fresh initialization------------
DROP PROCEDURE DeleteRole;
DROP PROCEDURE DeleteUser;
DROP PROCEDURE DeleteBook;


GO

------------------------INITIALIZE TABLES-----------------------------------
CREATE TABLE Roles (RoleID INT UNIQUE, RoleName VARCHAR(255));

CREATE TABLE Users (UserID INT UNIQUE, FirstName VARCHAR(255), LastName VARCHAR(255), DOB DateTime, 
					RoleID INT, Username VARCHAR(255) UNIQUE, Password VARCHAR(255), IsLoggedIn BIT);

INSERT INTO Users (UserID, FirstName, LastName, DOB, RoleID, Username, Password, IsLoggedIn)
VALUES (0, 'Initial', 'Admin', 2022-01-01, 1, 'InitialAdmin', 'SuperSecretAdminPassword', 0);

CREATE TABLE Books (BookID INT UNIQUE, Title VARCHAR(255), Genre VARCHAR(255), IsCheckedOut BIT, UserID INT UNIQUE);

CREATE TABLE Exceptions (Occurence DateTime, ExceptionType VARCHAR(255), FunctionName VARCHAR(255));

GO





------------------------CREATE PROCEDURES-----------------------------------
CREATE PROCEDURE AddRole (	@newRoleID INT, @newRoleName VARCHAR(255))
AS
BEGIN
	INSERT INTO Roles (RoleID, RoleName)
	VALUES (@newRoleID, @newRoleName);
END
GO

CREATE PROCEDURE AddUser (	@newUserID INT, 
							@newLastName VARCHAR(255),
							@newFirstName VARCHAR(255),
							@newDOB DateTime,
							@newRoleID INT,
							@newUsername VARCHAR(255),
							@newPassword VARCHAR(255))
AS
BEGIN
	INSERT INTO Users (UserID, FirstName, LastName, DOB, RoleID, Username, Password, IsLoggedIn)
	VALUES (@newUserID, @newFirstName, @newLastName, @newDOB, @newRoleID, @newUsername, @newPassword, 0);
END
GO

CREATE PROCEDURE AddBook (@newBookID INT, @newTitle VARCHAR(255), @newGenre VARCHAR(255))
AS
BEGIN
	INSERT INTO Books (BookID, Title, Genre, IsCheckedOut, UserID)
	VALUES (@newBookID, @newTitle, @newGenre, 0, NULL);
END
GO

CREATE PROCEDURE AddException (@newOccurence DateTime, @newExceptionType VARCHAR(255), @newFunctionName VARCHAR(255))
AS
BEGIN
	INSERT INTO Exceptions (Occurence, ExceptionType, FunctionName)
	VALUES (@newOccurence, @newExceptionType, @newFunctionName);
END
GO





------------------------------------READ PROCEDURES---------------------------------------
CREATE PROCEDURE SelectAllRoles								
AS
BEGIN
	SELECT * FROM Roles;
END
GO

CREATE PROCEDURE SelectAllUsers								
AS
BEGIN
	SELECT * FROM Users;
END
GO

CREATE PROCEDURE ReturnNumRoles								
AS
BEGIN
	SELECT COUNT(*) FROM Roles;
END
GO

CREATE PROCEDURE SelectAllBooks								
AS
BEGIN
	SELECT * FROM Books;
END
GO











------------------------------------UPDATE PROCEDURES------------------------------------
CREATE PROCEDURE UpdateRole (	@roleIDtoUpdate INT, 
							@newRoleName VARCHAR(255))
AS
BEGIN
	UPDATE Roles SET RoleName = @newRoleName
	WHERE RoleID = @roleIDtoUpdate;
END
GO

CREATE PROCEDURE UpdateUserGeneral (@userIDtoUpdate INT, 
									@newLastName VARCHAR(255),
									@newFirstName VARCHAR(255),
									@newDOB DateTime,
									@newRoleID INT,
									@newUsername VARCHAR(255),
									@newPassword VARCHAR(255),
									@newIsLoggedIn BIT)
AS
BEGIN
	UPDATE Users SET FirstName = @newFirstName, LastName = @newLastName, DOB = @newDOB, RoleID = @newRoleID, Username = @newUsername, Password = @newPassword, IsLoggedIn = @newIsLoggedIn
	WHERE UserID = @userIDtoUpdate;
END
GO

CREATE PROCEDURE UpdateUserLoginStatus(@userName VARCHAR(255), @LogInStatus BIT)
AS
BEGIN
UPDATE Users SET IsLoggedIn = @LoginStatus WHERE Username = @userName;
END
GO

CREATE PROCEDURE UpdateBook (@bookIDtoUpdate INT, @newTitle VARCHAR(255), @newGenre VARCHAR(255), @newIsCheckedOut BIT, @newUserID INT)
AS
BEGIN
	UPDATE Books SET Title=@newTitle, Genre=@newGenre, IsCheckedOut=@newIsCheckedOut, UserID=@newUserID
	WHERE BookID=@bookIDtoUpdate;
END
GO

CREATE PROCEDURE UpdateBookIsCheckedOut (@bookIDtoUpdate INT, @newIsCheckedOut BIT, @newUserID INT)
AS
BEGIN
	UPDATE Books SET IsCheckedOut=@newIsCheckedOut, UserID=@newUserID
	WHERE BookID=@bookIDtoUpdate;
END
GO








-----------------------------------------------DELETE PROCEDURES--------------------------------------------
CREATE PROCEDURE DeleteRole (@roleIDtoDelete INT)
AS
BEGIN
	DELETE FROM Roles WHERE RoleID = @roleIDtoDelete;
END
GO

CREATE PROCEDURE DeleteUser (@userIDtoDelete INT)
AS
BEGIN
	DELETE FROM Users WHERE UserID = @userIDtoDelete;
END
GO

CREATE PROCEDURE DeleteBook (@bookIDtoDelete INT)
AS
BEGIN
	DELETE FROM Books WHERE BookID = @bookIDtoDelete;
END
GO


--------------------------------------PROCEDURE TESTING-------------------------------------------------
EXEC AddRole @newRoleID = 0, @newRoleName = 'Guest';
EXEC AddRole @newRoleID = 1, @newRoleName = 'Administrator';
EXEC AddRole @newRoleID = 2, @newRoleName = 'Librarian';
EXEC AddRole @newRoleID = 3, @newRoleName = 'Patron';

EXEC AddUser @newUserID = 1, @newLastName = 'Klaus', @newFirstName = 'Christopher', 
			@newDOB = '1988-08-31', @newRoleID = 3, @newUsername = 'chrisklaus83188', 
			@newPassword = 'abc123';
EXEC AddUser @newUserID = 2, @newLastName = 'Gomez Leal', @newFirstName = 'Gabriela', 
			@newDOB = '1990-11-09', @newRoleID = 3, @newUsername = 'gabsgmz', 
			@newPassword = 'abc123';
EXEC AddUser @newUserID = 3, @newLastName = '1', @newFirstName = 'SomeOtherUser', 
			@newDOB = '2022-04-25', @newRoleID = 3, @newUsername = 'SomeOtherUser1', 
			@newPassword = 'abc123';
EXEC AddUser @newUserID = 4, @newLastName = '2', @newFirstName = 'SomeOtherUser', 
			@newDOB = '2022-04-25', @newRoleID = 3, @newUsername = 'SomeOtherUser2', 
			@newPassword = 'abc123';




EXEC UpdateRole @roleIDtoUpdate = 3, @newRoleName = 'Member';
EXEC SelectAllRoles;

EXEC UpdateRole @roleIDtoUpdate = 3, @newRoleName = 'Patron';
EXEC SelectAllRoles;

EXEC UpdateUserGeneral @userIDtoUpdate = 4, 
									@newLastName = 'Different Name',
									@newFirstName = 'Some Other Name',
									@newDOB = '1999-12-31',
									@newRoleID = 2,
									@newUsername = 'SomeOtherUser2',
									@newPassword = 'def456',
									@newIsLoggedIn = 0;
EXEC SelectAllUsers;

EXEC AddRole @newRoleID = 4, @newRoleName = 'DummyRole';
EXEC SelectAllRoles;
EXEC DeleteRole @roleIDtoDelete = 4;
EXEC SelectAllRoles;

EXEC DeleteUser @userIDtoDelete = 3;
EXEC DeleteUser @userIDtoDelete = 4;
EXEC SelectAllUsers;

EXEC AddBook @newBookID = 0, @newTitle = "C++ Programming", @newGenre = "Computer Science";
EXEC SelectAllBooks;

GO





