/* user */
CREATE TABLE [user](
	[id] uniqueidentifier NOT NULL PRIMARY KEY,
	[name] varchar(20) NOT NULL
)

/* event */
CREATE TABLE [event](
	[id] uniqueidentifier NOT NULL PRIMARY KEY,
	[user_id] uniqueidentifier NOT NULL,
	[description] varchar(100) NOT NULL,
	CONSTRAINT [FK_event_user] FOREIGN KEY([user_id]) REFERENCES [user]([id])
)
