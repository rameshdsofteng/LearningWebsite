# 🔧 FIX: "Invalid object name 'Learnings'" Error

## 🎯 Problem
**Error**: `Microsoft.Data.SqlClient.SqlException: 'Invalid object name 'Learnings'.'`

**Cause**: Database tables for Learnings and LearningAssignments don't exist in the database yet.

---

## ✅ SOLUTION (3 steps, 2 minutes)

### Step 1: Delete Database (30 seconds)
The existing database is out of sync. Delete it so it can be recreated:

**Option A: Via SQL Server Object Explorer**
1. Right-click on `LearningWebsiteDb` database
2. Select "Delete"
3. Click "Close existing connections" if prompted
4. Click "OK"

**Option B: Via appsettings.json**
Change the database name temporarily to force creation of a new one (optional).

---

### Step 2: Stop the Application (10 seconds)
```
Press Shift+F5 to stop debugging
OR click the red Stop button
```

---

### Step 3: Run Database Migration (60 seconds)

Open **Package Manager Console**:
```
Tools → NuGet Package Manager → Package Manager Console
```

Run these commands:
```powershell
# Remove old migrations if any exist
Remove-Migration

# Create new migration with all tables
Add-Migration InitialCreate

# Apply migration to create tables
Update-Database
```

**Expected Output**:
```
Build succeeded.
To undo this action, use Remove-Migration.
Target database is: 'LearningWebsiteDb'...
Successfully applied migration...
```

---

### Step 4: Start Application (30 seconds)
```
Press F5 to start debugging
```

**Expected Result**: 
- ✅ Application starts without errors
- ✅ Database tables created automatically
- ✅ Sample data seeded
- ✅ No "Invalid object name" error

---

## ✅ VERIFICATION

After the fix, verify:

### Check 1: Database Tables Exist
1. Open **SQL Server Object Explorer**
2. Expand database → **Tables**
3. Verify you see:
   - ✅ `Learnings` table
   - ✅ `LearningAssignments` table
   - ✅ `Users` table

### Check 2: Application Runs
1. Press F5
2. Home page loads
3. No errors in Output window

### Check 3: Sample Data Exists
1. Navigate to `/Employee`
2. Login: `employee1` / `Password123!`
3. Dashboard shows assignments
4. ✅ No "Failed to load" error

---

## 🚨 IF STILL HAVING ISSUES

### Issue: Still getting "Invalid object name"

**Solution**:
```powershell
# In Package Manager Console:
# Drop the entire database and recreate
Update-Database -Migration 0
Update-Database
```

---

### Issue: Migration fails

**Solution**:
```powershell
# Clear all migrations
Remove-Migration -Force

# Recreate migration
Add-Migration InitialCreate

# Apply
Update-Database
```

---

## 📝 WHAT WAS WRONG

The issue was:
1. ❌ `DbInitializer` was calling `LearningDataInitializer.InitializeLearnings()`
2. ❌ But the database tables didn't exist
3. ❌ EF Core's `EnsureCreated()` only creates tables that were already in the old schema

**The Fix**: 
- ✅ Run migrations to create new tables
- ✅ EnsureCreated() now includes Learning tables
- ✅ Data initialization works

---

## 🎓 WHY THIS HAPPENED

When code changes add new DbSets (Learnings, LearningAssignments), you need to:
1. Create a migration: `Add-Migration [name]`
2. Apply the migration: `Update-Database`

This tells EF Core to create the new tables in the database.

Without this step, the code tries to access tables that don't exist → Error!

---

## ✅ YOU'RE FIXED!

Once you run the 3 steps above, your database will have all tables and sample data will seed automatically.

**Next**: Test the dashboard!
```
Login: employee1 / Password123!
Navigate: /Employee
Expected: Dashboard loads with assignments
```

---

## 📚 REFERENCE

- **DbInitializer.cs** - Automatically creates tables and seeds users
- **LearningDataInitializer.cs** - Automatically seeds learnings and assignments
- **AppDbContext.cs** - Defines the schema

---

**The fix is simple: Run the migration!** 🚀
