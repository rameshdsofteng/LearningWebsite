# 🔍 VERIFY TABLES EXIST - QUICK CHECK

## ⚡ QUICK VERIFICATION QUERIES

### Check All 3 Tables Exist

**Run this in SQL Server:**
```sql
SELECT TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_NAME IN ('Users', 'Learnings', 'LearningAssignments')
ORDER BY TABLE_NAME;
```

**Expected Output:**
```
LearningAssignments
Learnings
Users
```

---

### Count Tables
```sql
SELECT COUNT(*) AS TablesCount
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_SCHEMA = 'dbo' 
AND TABLE_NAME IN ('Users', 'Learnings', 'LearningAssignments');
```

**Expected**: 3

---

### View Table Structure

#### Users Table
```sql
SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Users'
ORDER BY ORDINAL_POSITION;
```

#### Learnings Table
```sql
SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Learnings'
ORDER BY ORDINAL_POSITION;
```

#### LearningAssignments Table
```sql
SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'LearningAssignments'
ORDER BY ORDINAL_POSITION;
```

---

### Check Foreign Keys
```sql
SELECT 
    CONSTRAINT_NAME,
    TABLE_NAME,
    REFERENCED_TABLE_NAME
FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS
WHERE CONSTRAINT_SCHEMA = 'dbo';
```

**Expected:**
- FK_LearningAssignments_Users
- FK_LearningAssignments_Learnings

---

### Check Indexes
```sql
SELECT 
    TABLE_NAME,
    INDEX_NAME,
    COLUMN_NAME
FROM INFORMATION_SCHEMA.STATISTICS
WHERE TABLE_SCHEMA = 'dbo'
AND TABLE_NAME IN ('Users', 'Learnings', 'LearningAssignments');
```

---

## 📊 EXPECTED TABLE STRUCTURE

### ✅ Users Table
- Id (int, PK, Identity)
- UserName (nvarchar(256), NOT NULL)
- PasswordHash (nvarchar(max), NOT NULL)
- Role (nvarchar(100), Nullable)

### ✅ Learnings Table
- Id (int, PK, Identity)
- Title (nvarchar(255), NOT NULL)
- Description (nvarchar(max), NOT NULL)
- Category (nvarchar(100), NOT NULL)
- DurationInHours (int, NOT NULL)

### ✅ LearningAssignments Table
- Id (int, PK, Identity)
- UserId (int, FK → Users, NOT NULL)
- LearningId (int, FK → Learnings, NOT NULL)
- AssignedDate (datetime2, NOT NULL)
- DueDate (datetime2, NOT NULL)
- Status (nvarchar(50), Default 'NotStarted')
- ProgressPercentage (int, Nullable)
- CompletedDate (datetime2, Nullable)

---

## ✅ IF TABLES DON'T EXIST

See: **CREATE_ALL_TABLES.md** for instructions to create them

Quick command:
```powershell
Add-Migration CreateLearningTables
Update-Database
```

---

## 📍 WHERE TO RUN QUERIES

### In Visual Studio
1. View → SQL Server Object Explorer
2. Expand database
3. Right-click → New Query
4. Paste and run SQL

### In SQL Server Management Studio
1. New Query
2. Select database
3. Paste and run SQL

### In Azure Data Studio
1. New Query
2. Select database
3. Paste and run SQL

---

## 🎯 COMPLETE VERIFICATION SCRIPT

Run all at once:
```sql
-- Check if tables exist
SELECT 'Tables Check:' AS Result,
       COUNT(*) AS Count
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_SCHEMA = 'dbo' 
AND TABLE_NAME IN ('Users', 'Learnings', 'LearningAssignments');

-- List tables
SELECT 'Existing Tables:' AS Result,
       TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_NAME IN ('Users', 'Learnings', 'LearningAssignments');

-- Check columns
SELECT TABLE_NAME, COUNT(*) AS ColumnCount
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME IN ('Users', 'Learnings', 'LearningAssignments')
GROUP BY TABLE_NAME;

-- Check foreign keys
SELECT CONSTRAINT_NAME
FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS;
```

---

## ✨ SUCCESS CHECKLIST

- [ ] All 3 tables exist
- [ ] All columns present
- [ ] Data types correct
- [ ] Primary keys configured
- [ ] Foreign keys configured
- [ ] Indexes created
- [ ] No errors in verification

---

**All tables verified!** ✅
