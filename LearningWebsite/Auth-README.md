# Role-based Authentication (Employee / Manager / HR)

This project implements simple role-based authentication using Cookie Authentication and a small EF Core-backed user table.

Seeded accounts (created automatically on first run):

- Employee: user `employee1`, password `Password123!`, role `Employee`
- Manager: user `manager1`, password `Password123!`, role `Manager`
- HR: user `hr1`, password `Password123!`, role `HR`

Pages:
- Login (Employee): /Account/LoginEmployee
- Login (Manager): /Account/LoginManager
- Login (HR): /Account/LoginHR

Dashboards:
- Employee dashboard: /Employee
- Manager dashboard: /Manager
- HR dashboard: /HR

Notes:
- Logging is performed using the built-in ILogger; important events are logged with LogInformation / LogWarning / LogError.
- Database: SQL Server LocalDB by default. Change `ConnectionStrings:DefaultConnection` in `appsettings.json` to point to your SQL Server instance.
- The default seeded password can be changed by editing `Data/DbInitializer.cs`.
