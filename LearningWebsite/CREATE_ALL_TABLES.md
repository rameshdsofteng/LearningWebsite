# 🗄️ CREATE ALL TABLES - COMPLETE GUIDE

## 📊 TABLES REQUIRED

Based on your models, you need these 3 tables:

1. **Users** - For user authentication and roles
2. **Learnings** - For learning courses
3. **LearningAssignments** - For assignment tracking

---

## ✅ OPTION 1: AUTOMATIC (RECOMMENDED)

### Using Entity Framework Migrations

**Step 1: Open Package Manager Console**
```
Tools → NuGet Package Manager → Package Manager Console
```

**Step 2: Create Migration**
```powershell
Add-Migration CreateLearningTables
```

**Step 3: Apply Migration**
```powershell
Update-Database
```

**Result**: All 3 tables created automatically! ✅

---

## ✅ OPTION 2: MANUAL SQL SCRIPT

If migrations don't work, use this SQL script directly.

**Open SQL Server Management Studio or Query Window, then run:**

### Create Users Table
```sql
CREATE TABLE [Users] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [UserName] NVARCHAR(256) NOT NULL,
    [PasswordHash] NVARCHAR(MAX) NOT NULL,
    [Role] NVARCHAR(100) NULL
);

CREATE UNIQUE INDEX UX_Users_UserName ON [Users]([UserName]);
```

### Create Learnings Table
```sql
CREATE TABLE [Learnings] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Title] NVARCHAR(255) NOT NULL,
    [Description] NVARCHAR(MAX) NOT NULL,
    [Category] NVARCHAR(100) NOT NULL,
    [DurationInHours] INT NOT NULL
);

CREATE INDEX IX_Learnings_Category ON [Learnings]([Category]);
```

### Create LearningAssignments Table
```sql
CREATE TABLE [LearningAssignments] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [UserId] INT NOT NULL,
    [LearningId] INT NOT NULL,
    [AssignedDate] DATETIME2 NOT NULL,
    [DueDate] DATETIME2 NOT NULL,
    [Status] NVARCHAR(50) NOT NULL DEFAULT 'NotStarted',
    [ProgressPercentage] INT NULL,
    [CompletedDate] DATETIME2 NULL,
    CONSTRAINT [FK_LearningAssignments_Users] FOREIGN KEY ([UserId]) 
        REFERENCES [Users]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_LearningAssignments_Learnings] FOREIGN KEY ([LearningId]) 
        REFERENCES [Learnings]([Id]) ON DELETE CASCADE
);

CREATE INDEX IX_LearningAssignments_UserId ON [LearningAssignments]([UserId]);
CREATE INDEX IX_LearningAssignments_LearningId ON [LearningAssignments]([LearningId]);
CREATE INDEX IX_LearningAssignments_Status ON [LearningAssignments]([Status]);
```

### Create Migration History Table (if needed)
```sql
CREATE TABLE [__EFMigrationsHistory] (
    [MigrationId] NVARCHAR(150) NOT NULL,
    [ProductVersion] NVARCHAR(32) NOT NULL,
    PRIMARY KEY ([MigrationId])
);
```

---

## ✅ OPTION 3: VERIFY TABLES EXIST

### Check if Tables Already Exist

**Run this SQL query:**
```sql
SELECT TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_NAME IN ('Users', 'Learnings', 'LearningAssignments');
```

**Expected Result:**
```
Users
Learnings
LearningAssignments
```

If you see all 3, tables exist! ✅

---

## 📋 TABLE STRUCTURE DETAILS

### Users Table
```
Column              | Type           | Constraint
─────────────────────────────────────────────
Id                 | INT            | PRIMARY KEY, Identity
UserName           | NVARCHAR(256)  | NOT NULL, UNIQUE
PasswordHash       | NVARCHAR(MAX)  | NOT NULL
Role               | NVARCHAR(100)  | Nullable
```

### Learnings Table
```
Column              | Type           | Constraint
─────────────────────────────────────────────
Id                 | INT            | PRIMARY KEY, Identity
Title              | NVARCHAR(255)  | NOT NULL
Description        | NVARCHAR(MAX)  | NOT NULL
Category           | NVARCHAR(100)  | NOT NULL
DurationInHours    | INT            | NOT NULL
```

### LearningAssignments Table
```
Column              | Type           | Constraint
─────────────────────────────────────────────
Id                 | INT            | PRIMARY KEY, Identity
UserId             | INT            | FOREIGN KEY → Users
LearningId         | INT            | FOREIGN KEY → Learnings
AssignedDate       | DATETIME2      | NOT NULL
DueDate            | DATETIME2      | NOT NULL
Status             | NVARCHAR(50)   | NOT NULL, Default 'NotStarted'
ProgressPercentage | INT            | Nullable
CompletedDate      | DATETIME2      | Nullable
```

---

## ✅ VERIFICATION

After creating tables, verify they exist:

### In Visual Studio (SQL Server Object Explorer)
1. View → SQL Server Object Explorer
2. Expand: (localdb)\mssqllocaldb
3. Expand: Your database
4. Expand: Tables
5. Verify: Users, Learnings, LearningAssignments exist

### In SQL Server Management Studio
```sql
SELECT COUNT(*) AS TableCount
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_SCHEMA = 'dbo' 
AND TABLE_NAME IN ('Users', 'Learnings', 'LearningAssignments');
```

Expected: 3 (all tables)

---

## 🎯 RECOMMENDED: OPTION 1 (Auto Migration)

**Step 1**: Open Package Manager Console
```
Tools → NuGet Package Manager → Package Manager Console
```

**Step 2**: Run command
```powershell
Add-Migration CreateLearningTables
```

**Step 3**: Apply
```powershell
Update-Database
```

**Step 4**: Verify
Open SQL Server Object Explorer → See 3 tables ✅

---

## 🚀 NEXT STEPS

1. **Create Tables** (using Option 1, 2, or 3)
2. **Verify Tables Exist** (use verification queries above)
3. **Run Application** (F5 in Visual Studio)
4. **Seed Sample Data** (DbInitializer runs automatically)
5. **Test Dashboards** (login and view)

---

## ⚠️ TROUBLESHOOTING

### "Migration already exists"
→ Use: `Remove-Migration` first, then `Add-Migration CreateLearningTables`

### "Table already exists"
→ Run verification query to confirm
→ Or drop and recreate using SQL

### "Cannot create foreign key"
→ Ensure parent table (Users, Learnings) exists first
→ Use Option 2 SQL script (tables created in correct order)

### "Database doesn't exist"
→ Create database first in SQL Server
→ Or connection string will auto-create if allowed

---

## ✨ SUCCESS INDICATORS

✅ All 3 tables exist
✅ Foreign keys configured
✅ Indexes created
✅ Constraints in place
✅ App runs without SQL errors
✅ DbInitializer can seed data

---

**All tables are now created!** ✅
