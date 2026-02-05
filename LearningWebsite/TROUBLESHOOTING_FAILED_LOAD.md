# 🔧 Troubleshooting: "Failed to load dashboard data" Error

## 🎯 Issue
You're seeing **"Failed to load dashboard data"** in the Employee dashboard's "My Learning Assignments" section.

---

## ✅ QUICK FIX (Do This First)

### Step 1: Run Database Migration
This is the most likely cause. Open **Package Manager Console** and run:

```powershell
Add-Migration AddLearningModels
Update-Database
```

**What this does**: Creates the necessary database tables (Learnings, LearningAssignments)

**How to know it worked**: No errors in the console, and you should see new tables in SQL Server Object Explorer

---

### Step 2: Seed Sample Data
The database needs sample assignments. In **Package Manager Console**, run:

```powershell
# First, open Package Manager Console
# Then paste this and run:
$context = Get-DbContext
$hasher = [Microsoft.AspNetCore.Identity.PasswordHasher[LearningWebsite.Models.ApplicationUser]]::new()
[LearningWebsite.Data.LearningDataInitializer]::InitializeLearnings($context, $hasher)
```

**Alternative**: Modify `DbInitializer.cs` to include:
```csharp
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<AppDbContext>();
    var hasher = services.GetRequiredService<IPasswordHasher<ApplicationUser>>();
    DbInitializer.Initialize(db, hasher);
    LearningDataInitializer.InitializeLearnings(db, hasher);  // ADD THIS LINE
}
```

---

### Step 3: Restart Application
Press **F5** to restart the application.

---

## 🔍 DETAILED DIAGNOSIS

If the quick fix doesn't work, follow these steps:

### Check 1: Verify API Endpoint is Working

1. Open Browser DevTools (**F12**)
2. Go to **Network** tab
3. Refresh the page
4. Look for a request to `/api/dashboard/employee`

**Expected results**:
- ✅ Status: `200` (green)
- ✅ Response type: `json`
- ✅ Size: > 0

**If you see**:
- ❌ Status: `401` → User not logged in, need to login first
- ❌ Status: `403` → User lacks permission, check role
- ❌ Status: `404` → API endpoint not found
- ❌ Status: `500` → Server error, check error log

### Check 2: Look at API Response

1. In DevTools Network tab
2. Click on the `/api/dashboard/employee` request
3. Go to **Response** tab
4. Check what data is returned

**Good response** looks like:
```json
{
  "totalAssignments": 5,
  "completed": 2,
  "inProgress": 2,
  "notStarted": 1,
  "assignments": [...]
}
```

**Bad response** (error):
```json
{
  "error": "...",
  "message": "..."
}
```

### Check 3: Check Browser Console for Errors

1. Open DevTools (**F12**)
2. Go to **Console** tab
3. Look for red error messages

**Common errors**:
- `TypeError: Cannot read property 'assignments' of undefined` → API not returning proper data
- `Fetch error` → Network/CORS issue
- `JSON parse error` → Invalid JSON from API

---

## 🔧 SOLUTION BY ERROR TYPE

### Error: "Cannot find property"
**Cause**: Database tables don't exist yet
**Solution**: Run migrations (see Quick Fix Step 1)

### Error: 404 Not Found
**Cause**: API endpoint doesn't exist
**Solution**: Ensure `DashboardController.cs` exists in `Controllers/Api/`

### Error: 401 Unauthorized
**Cause**: Not logged in
**Solution**: Login using `/Account/Login` first with credentials:
- Username: `employee1`
- Password: `Password123!`

### Error: 403 Forbidden
**Cause**: User doesn't have Employee role
**Solution**: Login with a user that has the Employee role

### Error: 500 Internal Server Error
**Cause**: Server-side error
**Solution**: 
1. Check Application Output window for error details
2. Check Visual Studio Debug output
3. Look for exception messages

---

## ✅ VERIFICATION CHECKLIST

Complete these steps to verify everything is set up:

### Database Setup
- [ ] Migration `AddLearningModels` exists
- [ ] Migration has been applied (`Update-Database` ran successfully)
- [ ] SQL Server Object Explorer shows:
  - [ ] `Learnings` table
  - [ ] `LearningAssignments` table
  - [ ] New columns in `Users` table

### Application Setup
- [ ] `DashboardController.cs` exists in `Controllers/Api/`
- [ ] `Learning.cs` exists in `Models/`
- [ ] `LearningAssignment.cs` exists in `Models/`
- [ ] `Program.cs` has `app.MapControllers();`

### Data Setup
- [ ] At least 1 Learning record in database
- [ ] At least 1 LearningAssignment record for logged-in user
- [ ] User ID matches in Users table

### Authentication
- [ ] Logged in as `employee1` (Employee role)
- [ ] User.Identity?.Name shows in dashboard
- [ ] No 401/403 errors

---

## 🗄️ MANUAL DATABASE CHECK

If you want to verify the database manually:

### In SQL Server Management Studio or VS Database Explorer

**Check tables exist**:
```sql
SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME IN ('Learnings', 'LearningAssignments')
```

**Check Learnings have data**:
```sql
SELECT * FROM Learnings
```

**Check Assignments have data**:
```sql
SELECT * FROM LearningAssignments
```

**Check for current user's assignments**:
```sql
SELECT la.* FROM LearningAssignments la
WHERE la.UserId = 1  -- Replace 1 with employee1's user ID
```

---

## 📝 STEP-BY-STEP RECOVERY

If you're still stuck, follow this exact sequence:

### Phase 1: Database Setup (5 minutes)
```powershell
# 1. Open Package Manager Console
# 2. Run this:
Add-Migration AddLearningModels

# 3. Run this:
Update-Database

# 4. Verify in SQL Server Object Explorer:
# - Right-click on database
# - Refresh
# - Expand Tables
# - Look for "Learnings" and "LearningAssignments"
```

### Phase 2: Sample Data (2 minutes)
```powershell
# In Package Manager Console:
# Option A: If using DbInitializer
# Edit Data/DbInitializer.cs and add the LearningDataInitializer call

# Option B: Direct PowerShell
# First, ensure LearningDataInitializer.cs exists
# Then restart app with F5
```

### Phase 3: Restart and Test (2 minutes)
```
1. Press F5 to start app
2. Navigate to /Account/Login
3. Login as employee1 / Password123!
4. Navigate to /Employee
5. Check if data appears
```

### Phase 4: Debug if Still Failed
```
1. Press F12 to open DevTools
2. Go to Network tab
3. Look for /api/dashboard/employee request
4. Check status code and response
5. Go to Console tab
6. Look for error messages
7. Take screenshot and reference DASHBOARD_QUICK_REFERENCE.md
```

---

## 🆘 IF STILL NOT WORKING

### Get More Information

In **Package Manager Console**, run:
```powershell
# Check if tables exist
Get-DbContext

# This will show if database is connected
```

In **Visual Studio Output Window** (Debug section), look for:
- Database connection messages
- Migration messages
- Any error messages

### Enable Detailed Logging

Add this to `appsettings.json`:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  }
}
```

This will show database queries in Output window.

---

## 📞 QUICK REFERENCE

| Issue | Check | Fix |
|-------|-------|-----|
| No data shows | Browser DevTools → Network | Run migration + seed data |
| 401 Error | Login page | Login with employee1/Password123! |
| 403 Error | User role | Verify user has Employee role |
| 404 Error | API endpoint exists | Check Controllers/Api/ folder |
| 500 Error | Output window | Look for exception details |
| No assignments | Database | Seed sample data |

---

## ✅ SUCCESS INDICATORS

You'll know it's fixed when:
- ✅ Page shows 4 metric cards (Total, Completed, In Progress, Not Started)
- ✅ Table shows assignments (or "No learning assignments" message if none exist)
- ✅ No red error messages
- ✅ Numbers update correctly
- ✅ Update button works

---

## 📚 RELATED DOCUMENTATION

- **DASHBOARD_IMPLEMENTATION.md** - Technical details
- **API_TESTING_GUIDE.md** - Test the API directly
- **TESTING_GUIDE.md** - Full testing procedures
- **QUICK_REFERENCE_CARD.md** - Quick lookup

---

**Still stuck?** Follow the "STEP-BY-STEP RECOVERY" section exactly as written, and the dashboard should start working!
