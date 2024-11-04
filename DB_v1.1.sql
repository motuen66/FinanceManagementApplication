GO
/*
<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
<<<<<<<<<< BEGIN: CREATE DATABASE <<<<<<<<<<
<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
*/
	IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'FinanceManagementApplication')
		CREATE DATABASE [FinanceManagementApplication];
/*
<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
<<<<<<<<<< END: CREATE DATABASE <<<<<<<<<<
<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
*/
GO
USE [FinanceManagementApplication];
GO
/*
<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
<<<<<<<<<< BEGIN: RESET DATABASE <<<<<<<<<<
<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
*/
	DECLARE @Sql NVARCHAR(500) DECLARE @Cursor CURSOR

	SET @Cursor = CURSOR FAST_FORWARD FOR
	SELECT DISTINCT sql = 'ALTER TABLE [' + tc2.TABLE_SCHEMA + '].[' +  tc2.TABLE_NAME + '] DROP [' + rc1.CONSTRAINT_NAME + '];'
	FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc1
	LEFT JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc2 ON tc2.CONSTRAINT_NAME =rc1.CONSTRAINT_NAME

	OPEN @Cursor FETCH NEXT FROM @Cursor INTO @Sql

	WHILE (@@FETCH_STATUS = 0)
	BEGIN
	Exec sp_executesql @Sql
	FETCH NEXT FROM @Cursor INTO @Sql
	END

	CLOSE @Cursor DEALLOCATE @Cursor
	GO
	/* */
	DECLARE @tableName NVARCHAR(MAX)
	DECLARE tableCursor CURSOR FOR
	SELECT name
	FROM sys.tables

	OPEN tableCursor
	FETCH NEXT FROM tableCursor INTO @tableName

	WHILE @@FETCH_STATUS = 0
	BEGIN
		DECLARE @sql NVARCHAR(MAX)
		SET @sql = N'DROP TABLE ' + QUOTENAME(@tableName)
		EXEC sp_executesql @sql
		FETCH NEXT FROM tableCursor INTO @tableName
	END

	CLOSE tableCursor
	DEALLOCATE tableCursor
/*
>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
>>>>>>>> END: RESET DATABASE >>>>>>>>>>
>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

*/
GO
/*
<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
<<<<<<<<<< BEGIN: TABLES <<<<<<<<<<
<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
*/
CREATE TABLE [User] (
	[id] INT IDENTITY(1, 1),
	[username] NVARCHAR(50),
	[password] VARCHAR(32) NOT NULL,
	[balance] INT DEFAULT(0),
	[avatarPath] NVARCHAR(MAX),

	CONSTRAINT PK_User PRIMARY KEY ([id])
);

CREATE TABLE [FinanceRecord] (
	[userId] INT,
	[totalIncome] INT,
	[totalExpense] INT,
	[from] DATE,
	[to] DATE,

	CONSTRAINT FK_FinanceRecord FOREIGN KEY ([userId]) REFERENCES [User]([id]) ON DELETE CASCADE
);

CREATE TABLE [IncomeSource] (
	[id] INT IDENTITY(1, 1),
	[sourceName] NVARCHAR(50),
	[isDelete] BIT DEFAULT(0),

	CONSTRAINT PK_IncomeSource PRIMARY KEY ([id])
);

CREATE TABLE [IncomeTransaction] (
	[userId] INT,
	[sourceId] INT,
	[amount] INT,
	[date] DATE, 

	CONSTRAINT FK_IncomeTransaction_User FOREIGN KEY ([userId]) REFERENCES [User]([id]) ON DELETE CASCADE,
	CONSTRAINT FK_IncomeTransaction_IncomeSource FOREIGN KEY ([sourceId]) REFERENCES [IncomeSource]([id])
);

CREATE TABLE [BudgetItem] (
	[id] INT IDENTITY(1, 1),
	[userId] INT,
	[budgetName] NVARCHAR(50),
	[limitAmount] INT,
	[isOverBudget] BIT DEFAULT(0),
	[isDelete] BIT DEFAULT(0),

	CONSTRAINT PK_BudgetItem PRIMARY KEY ([id]),
	CONSTRAINT FK_BudgetItem_User FOREIGN KEY ([userId]) REFERENCES [User]([id]) ON DELETE CASCADE
);

CREATE TABLE [ExpenseTransaction] (
	[userId] INT,
	[budgetId] INT,
	[note] NVARCHAR(100),
	[amount] MONEY NOT NULL,
	[date] DATE DEFAULT CAST(GETDATE() AS DATE), 

	CONSTRAINT FK_ExpenseTransaction_User FOREIGN KEY ([userId]) REFERENCES [User]([id]) ON DELETE CASCADE,
	CONSTRAINT FK_ExpenseTransaction_BudgetItem FOREIGN KEY ([budgetId]) REFERENCES BudgetItem([id])
);

CREATE TABLE [SavingGoal] (
	[id] INT IDENTITY(1, 1),
	[userId] INT,
	[title] NVARCHAR(100),
	[description] NVARCHAR(500),
	[currentAmount] INT DEFAULT(0),
	[goalAmount] INT,
	[goalDate] DATE,
	[isCompleted] BIT DEFAULT(0),
	[isDelete] BIT DEFAULT(0),

	CONSTRAINT PK_SavingGoal PRIMARY KEY ([id]),
	CONSTRAINT FK_SavingGoal_User FOREIGN KEY ([userId]) REFERENCES [User]([id]) ON DELETE CASCADE,
);

CREATE TABLE [SavingTransaction] (
	[userId] INT,
	[savingGoalId] INT,
	[note] NVARCHAR(100),
	[amount] INT NOT NULL,
	[date] DATE DEFAULT CAST(GETDATE() AS DATE), 

	CONSTRAINT FK_SavingTransaction_SavingGoal FOREIGN KEY ([savingGoalId]) REFERENCES [SavingGoal]([id]),
	CONSTRAINT FK_SavingTransaction_User FOREIGN KEY ([userId]) REFERENCES [User]([id]) ON DELETE CASCADE,
);
/*
>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
>>>>>>>> END: TABLES >>>>>>>>>>
>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

*/
GO
/*
<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
<<<<<<<<<< BEGIN:TRIGGER <<<<<<<<<<
<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
*/
-- Trigger to update balance after inserting income transaction
CREATE TRIGGER trg_UpdateBalanceAfterIncomeInsert
ON [IncomeTransaction]
AFTER INSERT
AS
BEGIN
    UPDATE [User]
    SET balance = balance + i.amount
    FROM [User] u
    JOIN inserted i ON u.id = i.userId;
END;
GO

-- Trigger to update balance after deleting income transaction
CREATE TRIGGER trg_UpdateBalanceAfterIncomeDelete
ON [IncomeTransaction]
AFTER DELETE
AS
BEGIN
    UPDATE [User]
    SET balance = balance - d.amount
    FROM [User] u
    JOIN deleted d ON u.id = d.userId;
END;
GO

-- Trigger to update balance and check over-budget after inserting expense transaction
CREATE TRIGGER trg_UpdateBalanceAfterExpenseInsert
ON [ExpenseTransaction]
AFTER INSERT
AS
BEGIN
    -- Update user balance
    UPDATE [User]
    SET balance = balance - i.amount
    FROM [User] u
    JOIN inserted i ON u.id = i.userId;

    -- Update isOverBudget flag for each BudgetItem based on the sum of expenses
   UPDATE [BudgetItem]
	SET isOverBudget = CASE 
						  WHEN expenseTotal > limitAmount THEN 1 
						  ELSE 0 
					   END
	FROM [BudgetItem] b
	INNER JOIN (
		SELECT e.budgetId, SUM(e.amount) AS expenseTotal
		FROM [ExpenseTransaction] e
		WHERE e.budgetId IN (SELECT budgetId FROM inserted)
		GROUP BY e.budgetId
	) AS totalExpense ON b.id = totalExpense.budgetId;
END;
GO


-- Trigger to update balance and budget after deleting expense transaction
CREATE TRIGGER trg_UpdateBalanceAfterExpenseDelete
ON [ExpenseTransaction]
AFTER DELETE
AS
BEGIN
    UPDATE [User]
    SET balance = balance + d.amount
    FROM [User] u
    JOIN deleted d ON u.id = d.userId;
END;
GO

-- Trigger to update saving goal amount after inserting saving transaction
CREATE TRIGGER trg_UpdateSavingGoalAfterInsert
ON [SavingTransaction]
AFTER INSERT
AS
BEGIN
    -- Update current amount in SavingGoal
    -- After Insert
	UPDATE [SavingGoal]
	SET currentAmount = currentAmount + i.amountSum,
		isCompleted = CASE WHEN currentAmount + i.amountSum >= goalAmount THEN 1 ELSE 0 END
	FROM [SavingGoal] s
	JOIN (SELECT savingGoalId, SUM(amount) AS amountSum FROM inserted GROUP BY savingGoalId) i
	ON s.id = i.savingGoalId;
END;
GO

-- Trigger to update saving goal amount after deleting saving transaction
CREATE TRIGGER trg_UpdateSavingGoalAfterDelete
ON [SavingTransaction]
AFTER DELETE
AS
BEGIN
    -- Update current amount in SavingGoal
	-- After Delete
	UPDATE [SavingGoal]
	SET currentAmount = currentAmount - d.amountSum,
		isCompleted = CASE WHEN currentAmount - d.amountSum >= goalAmount THEN 1 ELSE 0 END
	FROM [SavingGoal] s
	JOIN (SELECT savingGoalId, SUM(amount) AS amountSum FROM deleted GROUP BY savingGoalId) d
	ON s.id = d.savingGoalId;
END;
GO

-- Trigger to calculate total income and expense after each update


-- Trigger to prevent user deletion with associated records
CREATE TRIGGER trg_PreventUserDelete
ON [User]
INSTEAD OF DELETE
AS
BEGIN
    IF EXISTS (SELECT 1 FROM [FinanceRecord] WHERE userId IN (SELECT id FROM deleted))
    BEGIN
        RAISERROR ('Cannot delete user with active finance records.', 16, 1);
    END
    ELSE
    BEGIN
        DELETE FROM [User] WHERE id IN (SELECT id FROM deleted);
    END
END;
GO
/*
>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
>>>>>>>> END: TRIGGERS >>>>>>>>>>
>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

*/
GO
/*
<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
<<<<<<<<<< BEGIN: EXAMPLE DATA <<<<<<<<<<
<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
*/
-- Insert sample data into [User]
INSERT INTO [User] (username, password, balance, avatarPath) 
VALUES 
('Hung', '123', 500, 'resources/test.jpg'),
('Anh', '123', 1000, 'resources/test.jpg'),
('Tu', '123', 250, NULL);

-- Insert 50 rows into [FinanceRecord]
DECLARE @i INT = 1;
WHILE @i <= 50
BEGIN
    DECLARE @to DATE = DATEADD(MONTH, -@i, EOMONTH(GETDATE(), 1)); -- Start of the month
    DECLARE @from DATE = EOMONTH(DATEADD(MONTH, -@i, GETDATE())); -- End of the month

    INSERT INTO [FinanceRecord] (userId, totalIncome, totalExpense, [from], [to])
    VALUES 
        ((@i % 3) + 1, FLOOR(RAND() * 10000), FLOOR(RAND() * 10000), @from, @to);

    SET @i = @i + 1;
END;

-- Insert 50 rows into [IncomeSource]
SET @i = 1;
WHILE @i <= 50
BEGIN
    INSERT INTO [IncomeSource] (sourceName, isDelete)
    VALUES 
        ('Source_' + CAST(@i AS NVARCHAR), @i % 2);
    SET @i = @i + 1;
END

-- Insert 50 rows into [IncomeTransaction]
SET @i = 1;
WHILE @i <= 50
BEGIN
    INSERT INTO [IncomeTransaction] (userId, sourceId, amount, date)
    VALUES 
        ((@i % 3) + 1, (@i % 10) + 1, (RAND() * 1000), DATEADD(DAY, -@i, GETDATE()));
    SET @i = @i + 1;
END

-- Insert 50 rows into [BudgetItem]
SET @i = 1;
WHILE @i <= 50
BEGIN
    INSERT INTO [BudgetItem] (budgetName, limitAmount, isOverBudget, isDelete)
    VALUES 
        ('Budget_' + CAST(@i AS NVARCHAR), (RAND() * 1000), @i % 2, (@i + 1) % 2);
    SET @i = @i + 1;
END

-- Insert 50 rows into [ExpenseTransaction]
SET @i = 1;
WHILE @i <= 50
BEGIN
    INSERT INTO [ExpenseTransaction] (userId, budgetId, note, amount, date)
    VALUES 
        ((@i % 3) + 1, (@i % 10) + 1, 'Expense_' + CAST(@i AS NVARCHAR), (RAND() * 500), DATEADD(DAY, -@i, GETDATE()));
    SET @i = @i + 1;
END

-- Insert 50 rows into [SavingGoal]
SET @i = 1;
WHILE @i <= 50
BEGIN
    INSERT INTO [SavingGoal] (userId, title, description, currentAmount, goalAmount, goalDate, isCompleted, isDelete)
    VALUES 
        ((@i % 3) + 1, 'Goal_' + CAST(@i AS NVARCHAR), 'Description for goal ' + CAST(@i AS NVARCHAR), 
         (RAND() * 1000), (RAND() * 10000), DATEADD(YEAR, 1, GETDATE()), @i % 2, (@i + 1) % 2);
    SET @i = @i + 1;
END

-- Insert 50 rows into [SavingTransaction]
SET @i = 1;
WHILE @i <= 50
BEGIN
    INSERT INTO [SavingTransaction] (savingGoalId, note, amount, date)
    VALUES 
        ((@i % 10) + 1, 'Saving note ' + CAST(@i AS NVARCHAR), (RAND() * 500), DATEADD(DAY, -@i, GETDATE()));
    SET @i = @i + 1;
END
/*
>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
>>>>>>>> END: EXAMPLE DATA >>>>>>>>>>
>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

*/
