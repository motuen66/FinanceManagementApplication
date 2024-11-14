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
>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
>>>>>> END: RESET DATABASE >>>>>>>>>>
>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>



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
	[month] INT,
	[year] INT,

	CONSTRAINT PK_FinanceRecord PRIMARY KEY ([userId], [month], [year]),
	CONSTRAINT FK_FinanceRecord FOREIGN KEY ([userId]) REFERENCES [User]([id]) ON DELETE CASCADE
);



CREATE TABLE [IncomeTransaction] (
	[id] INT IDENTITY(1, 1),
	[userId] INT,
	[amount] INT,
	[description] NVARCHAR(100),
	[date] DATETIME DEFAULT GETDATE(), 

	CONSTRAINT PK_IncomeTransaction PRIMARY KEY ([id]),
	CONSTRAINT FK_IncomeTransaction_User FOREIGN KEY ([userId]) REFERENCES [User]([id]) ON DELETE CASCADE,
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
	[date] DATETIME DEFAULT GETDATE(), 

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
	[date] DATETIME DEFAULT GETDATE(), 

	CONSTRAINT FK_SavingTransaction_SavingGoal FOREIGN KEY ([savingGoalId]) REFERENCES [SavingGoal]([id]),
	CONSTRAINT FK_SavingTransaction_User FOREIGN KEY ([userId]) REFERENCES [User]([id]) ON DELETE CASCADE,
);
/*
>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
>>>>>> END: TABLES >>>>>>>>>>
>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>



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
>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
>>>>>> END: TRIGGERS >>>>>>>>>>
>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>



*/
GO
/*
<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
<<<<<<<<<< BEGIN: EXAMPLE DATA <<<<<<<<<<
<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
*/
-- Insert vào bảng User
INSERT INTO [User] (username, password, balance, avatarPath)
VALUES
    ('abc', '123', 1000, '/images/abc_avatar.png'),
    ('john_doe', 'password123', 2000, '/images/john_avatar.png'),
    ('jane_doe', 'password456', 1500, '/images/jane_avatar.png');

-- Insert vào bảng IncomeSource

-- Insert vào bảng FinanceRecord
INSERT INTO [FinanceRecord] (userId, totalIncome, totalExpense, month, year)
VALUES 
    (1, 5200, 2100, 9, 2024),
    (1, 4800, 1900, 10, 2024),
    (1, 5000, 2000, 11, 2024),
    (1, 5500, 2500, 12, 2024),
    (2, 3200, 1100, 9, 2024),
    (2, 3100, 1200, 10, 2024),
    (2, 3000, 1200, 11, 2024),
    (2, 3300, 1300, 12, 2024),
    (3, 7200, 2900, 9, 2024),
    (3, 6800, 3100, 10, 2024),
    (3, 7000, 3000, 11, 2024),
    (3, 7200, 2800, 12, 2024);

-- Insert vào bảng IncomeTransaction
INSERT INTO [IncomeTransaction] (userId, description, amount, date)
VALUES 
    (1, 'hihi', 3200, '2024-09-01'),
    (1, 'hihi', 1000, '2024-09-10'),
    (1, 'hihi', 1200, '2024-09-20'),

    (2, 'hihi', 700, '2024-09-12'),
    (2, 'hihi', 2400, '2024-10-01'),
  
    (3,'hihi', 4000, '2024-09-03'),
    (3, 'hihi', 2500, '2024-09-25');
 
-- Insert vào bảng BudgetItem
INSERT INTO [BudgetItem] (userId, budgetName, limitAmount, isOverBudget, isDelete)
VALUES 
    (1, 'Food', 500, 0, 0),
    (1, 'Entertainment', 300, 0, 0),
    (2, 'Rent', 700, 0, 0),
    (2, 'Utilities', 100, 0, 0),
    (3, 'Travel', 1000, 0, 0),
    (3, 'Healthcare', 200, 0, 0);

-- Insert vào bảng ExpenseTransaction
INSERT INTO [ExpenseTransaction] (userId, budgetId, note, amount, date)
VALUES 
    (1, 1, 'Groceries', 200, '2024-09-05'),
    (1, 2, 'Concert ticket', 150, '2024-09-10'),
    (1, 1, 'Dining out', 120, '2024-10-05'),
    (1, 2, 'Movie night', 50, '2024-10-15'),
    (2, 3, 'Water bill', 100, '2024-09-08'),
    (2, 4, 'Flight ticket', 600, '2024-09-20'),
    (2, 3, 'Internet bill', 80, '2024-10-08'),
    (3, 5, 'Clothing', 250, '2024-09-18'),
    (3, 6, 'Dental checkup', 200, '2024-09-25'),
    (3, 5, 'Gift shopping', 150, '2024-10-10'),
    (3, 6, 'Health supplements', 100, '2024-10-30');

-- Insert vào bảng SavingGoal
INSERT INTO [SavingGoal] (userId, title, description, currentAmount, goalAmount, goalDate, isCompleted, isDelete)
VALUES 
    (1, 'Car Fund', 'Saving for a new car', 500, 5000, '2025-09-01', 0, 0),
    (2, 'Emergency Fund', 'Building emergency savings', 300, 3000, '2025-06-01', 0, 0),
    (3, 'House Fund', 'Saving for house down payment', 1000, 20000, '2026-12-01', 0, 0);

-- Insert vào bảng SavingTransaction
INSERT INTO [SavingTransaction] (userId, savingGoalId, note, amount, date)
VALUES 
    (1, 1, 'Monthly car savings', 500, '2024-09-05'),
    (1, 1, 'Extra car savings', 250, '2024-10-15'),
    (1, 2, 'Vacation savings', 200, '2024-09-10'),
    (2, 3, 'Emergency fund', 300, '2024-09-08'),
    (2, 3, 'Added to emergency fund', 150, '2024-10-10'),
    (3, 3, 'House savings installment', 1000, '2024-09-15'),
    (3, 3, 'Extra house savings', 500, '2024-10-20');

/*
>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
>>>>>> END: EXAMPLE DATA >>>>>>>>>>
>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>



*/