BEGIN TRANSACTION;

use SyncUp;
go

ALTER TABLE Applications DROP CONSTRAINT appidPK; 
ALTER TABLE Applications DROP CONSTRAINT useridFK; 
drop table Applications;

ALTER TABLE Messages DROP CONSTRAINT msgidPK; 
ALTER TABLE Messages DROP CONSTRAINT fromFK; 
ALTER TABLE Messages DROP CONSTRAINT toFK; 
drop table Messages;

ALTER TABLE Friends DROP CONSTRAINT relationshipPK; 
ALTER TABLE Friends DROP CONSTRAINT userFK; 
ALTER TABLE Friends DROP CONSTRAINT friendFK; 
drop table Friends;

ALTER TABLE Users DROP CONSTRAINT useridPK; 
drop table Users;
go


Create Table Users (
userid		uniqueidentifier	Not NULL DEFAULT NEWSEQUENTIALID(),
username	varchar(MAX)		Not NULL,
password	varchar(MAX)		Not NULL,
salt		char(16)			Not NULL,
creation	DateTime			NULL DEFAULT GETDATE(),
lastlogin	DateTime			NULL,
email		varchar(MAX)		NULL,
token		varchar(255)		NULL,
Constraint useridPK Primary Key (userid));

Create Table Applications (
appid		uniqueidentifier	Not NULL DEFAULT NEWSEQUENTIALID(),
token		varchar(255)		NULL,
name		varchar(MAX)		NULL,
userid		uniqueidentifier	Not NULL,
localip		varchar(15)			NULL,
localport	int					NULL,
remoteip	varchar(15)			NULL,
remoteport	int					NULL,
lastconnection	DateTime		Not NULL DEFAULT GETDATE(),
updatedate	DateTime			Not NULL DEFAULT GETDATE(),
Constraint appidPK Primary Key (appid),
CONSTRAINT useridFK FOREIGN KEY(userid) REFERENCES Users(userid));

Create Table Friends (
userid		uniqueidentifier	Not NULL,
friendid	uniqueidentifier	Not NULL,
creation	DateTime			Not NULL DEFAULT GETDATE(),
Constraint relationshipPK Primary Key (userid, friendid),
CONSTRAINT userFK FOREIGN KEY(userid) REFERENCES Users(userid),
CONSTRAINT friendFK FOREIGN KEY(friendid) REFERENCES Users(userid));

Create Table Messages (
msgid		uniqueidentifier	Not NULL DEFAULT NEWSEQUENTIALID(),
pushed		DateTime			Not NULL DEFAULT GETDATE(),			--Sent
popped		DateTime			NULL,								--Received
fromid		uniqueidentifier	Not NULL,
toid		uniqueidentifier	Not NULL,
msg			varchar(MAX)		NULL,
Constraint msgidPK Primary Key (msgid),
CONSTRAINT fromFK FOREIGN KEY(fromid) REFERENCES Users(userid),
CONSTRAINT toFK FOREIGN KEY(toid) REFERENCES Users(userid));

COMMIT TRANSACTION;

INSERT INTO Users (username, password, salt) VALUES ('Administrator', 'abc123', '1234567890123456');
INSERT INTO Users (username, password, salt, email) VALUES ('test', 'test', '1234567890123456', 'test@test.com');
INSERT INTO Users (username, password, salt) VALUES ('usera', 'usera', '1234567890123456');
INSERT INTO Users (username, password, salt) VALUES ('userb', 'userb', '1234567890123456');
go

SELECT * FROM Users;
SELECT * FROM Applications;
SELECT * FROM Friends;
SELECT * FROM Messages;