CREATE TABLE ChatHistory
(
    ID UNIQUEIDENTIFIER NOT NULL,
    Author NVARCHAR(255) NOT NULL,
    Text NVARCHAR(MAX) NOT NULL,
    TimeStamp DATETIME NOT NULL DEFAULT GETDATE()
)
GO

INSERT INTO ChatHistory (ID, Author, Text)
VALUES (NEWID(), 'System', 'Chat history initialized'),
        (NEWID(), 'System', 'Welcome to the chat room'),
        (NEWID(), 'System', 'Current location is Convendum')
