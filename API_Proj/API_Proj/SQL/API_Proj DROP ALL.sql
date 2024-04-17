-- Disable referencing foreign keys, Might have to run multpile times
EXEC sp_MSforeachtable "ALTER TABLE ? NOCHECK CONSTRAINT all"
-- Drop all tables
EXEC sp_MSforeachtable "DROP TABLE IF EXISTS ?"

-- or
--DROP TABLE IF EXISTS EmployeeOffice;
--DROP TABLE IF EXISTS Employee;
--DROP TABLE IF EXISTS Office;
--DROP TABLE IF EXISTS Laptop;
--DROP TABLE IF EXISTS Region;
--DROP TABLE IF EXISTS __EFMigrationsHistory;


-- Enable foreign keys back
EXEC sp_MSforeachtable "ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all"
