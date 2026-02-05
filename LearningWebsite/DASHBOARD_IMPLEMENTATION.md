# Dashboard Implementation Guide

## Overview
This implementation provides role-based dashboards with API endpoints and UI components for managing learning assignments across three user roles: Employee, Manager, and HR.

## Database Models

### Learning.cs
Represents available learning courses/modules:
- `Id`: Primary key
- `Title`: Learning title
- `Description`: Learning description
- `Category`: Learning category (e.g., "Technical", "Soft Skills")
- `DurationInHours`: Training duration
- `Assignments`: Navigation property to LearningAssignment

### LearningAssignment.cs
Tracks user learning assignments:
- `Id`: Primary key
- `UserId`: Foreign key to ApplicationUser
- `LearningId`: Foreign key to Learning
- `AssignedDate`: When the learning was assigned
- `DueDate`: Assignment due date
- `Status`: Current status (NotStarted, InProgress, Completed)
- `ProgressPercentage`: Completion progress (0-100%)
- `CompletedDate`: Date when completed
- `User`: Navigation property
- `Learning`: Navigation property

## API Endpoints

### Dashboard Controller (`/api/dashboard/`)

#### 1. Employee Dashboard
**Endpoint**: `GET /api/dashboard/employee`
**Authorization**: EmployeeOnly policy
**Response**:
```json
{
  "totalAssignments": 5,
  "completed": 2,
  "inProgress": 2,
  "notStarted": 1,
  "assignments": [
    {
      "id": 1,
      "title": "C# Fundamentals",
      "category": "Technical",
      "status": "InProgress",
      "assignedDate": "2024-01-15",
      "dueDate": "2024-02-15",
      "progressPercentage": 50,
      "completedDate": null,
      "daysUntilDue": 10
    }
  ]
}
```

**Features**:
- View personal learning assignments
- Track progress on each assignment
- See due dates and days remaining
- Update status and progress percentage

#### 2. Manager Dashboard
**Endpoint**: `GET /api/dashboard/manager`
**Authorization**: ManagerOnly policy
**Response**:
```json
{
  "totalTeamMembers": 5,
  "totalAssignments": 20,
  "completedAssignments": 8,
  "inProgressAssignments": 7,
  "notStartedAssignments": 5,
  "completionRate": 40.0,
  "teamAssignments": [
    {
      "userName": "john.doe",
      "totalAssigned": 4,
      "completed": 2,
      "inProgress": 1,
      "notStarted": 1,
      "assignments": [...]
    }
  ]
}
```

**Features**:
- Overview of team learning metrics
- Individual team member progress cards
- Team-wide completion statistics
- Visibility into all team assignments

#### 3. HR Dashboard
**Endpoint**: `GET /api/dashboard/hr`
**Authorization**: HROnly policy
**Response**:
```json
{
  "totalEmployees": 10,
  "totalManagers": 2,
  "totalAssignments": 50,
  "completedAssignments": 25,
  "inProgressAssignments": 15,
  "notStartedAssignments": 10,
  "overallCompletionRate": 50.0,
  "completionByCategory": [
    {
      "category": "Technical",
      "total": 20,
      "completed": 12,
      "completionRate": 60.0
    }
  ],
  "employeeProgress": [...]
}
```

**Features**:
- Organization-wide learning analytics
- Completion rates by learning category
- Employee progress tracking
- Visual charts and graphs

### Learnings Controller (`/api/learnings/`)

#### GET /api/learnings
Get all available learnings (public endpoint)

#### GET /api/learnings/{id}
Get specific learning by ID

#### POST /api/learnings
Create new learning (HR only)
```json
{
  "title": "Advanced C#",
  "description": "Advanced C# concepts",
  "category": "Technical",
  "durationInHours": 40
}
```

#### PUT /api/learnings/{id}
Update learning (HR only)

#### DELETE /api/learnings/{id}
Delete learning (HR only)

### Assignments Controller (`/api/assignments/`)

#### POST /api/assignments
Create new assignment (Manager/HR only)
```json
{
  "userId": 5,
  "learningId": 2,
  "dueDate": "2024-02-28"
}
```

#### GET /api/assignments/{id}
Get assignment details

#### PUT /api/assignments/{id}
Update assignment status and progress (User/Manager/HR)
```json
{
  "status": "InProgress",
  "progressPercentage": 75
}
```

#### DELETE /api/assignments/{id}
Delete assignment (Manager/HR only)

## UI Dashboards

### Employee Dashboard (`/Employee/Index`)
**Features**:
- Summary cards showing: Total, Completed, In Progress, Not Started
- Detailed assignment table with:
  - Learning title and category
  - Current status (visual badge)
  - Progress bar
  - Assigned and due dates
  - Days remaining indicator
  - Update button
- Modal for updating progress and status
- Real-time data loading via API

### Manager Dashboard (`/Manager/Index`)
**Features**:
- Team metrics summary cards
- Two tab views:
  1. **Team Overview**: Card view of each team member with completion percentage
  2. **All Assignments**: Table view of all team assignments
- Progress bars for visual completion tracking
- Quick team performance snapshot

### HR Dashboard (`/HR/Index`)
**Features**:
- Organization-wide metrics
- Two interactive charts:
  1. **Status Distribution**: Doughnut chart (Completed, In Progress, Not Started)
  2. **Category Analysis**: Bar chart of completion rates by category
- Two tab views:
  1. **Employee Progress**: Table of all employees with completion rates
  2. **Category Analysis**: Learning categories with completion metrics
- Color-coded progress bars (green ≥80%, yellow 50-80%, red <50%)
- Utilizes Chart.js for visualizations

## Integration Steps

### 1. Database Migration
After deploying this code, you'll need to create a migration:
```powershell
Add-Migration AddLearningModels
Update-Database
```

### 2. Seed Sample Data (Optional)
Add to `DbInitializer.cs`:
```csharp
if (!db.Learnings.Any())
{
    var learnings = new[]
    {
        new Learning { Title = "C# Fundamentals", Category = "Technical", DurationInHours = 30 },
        new Learning { Title = "Leadership Skills", Category = "Soft Skills", DurationInHours = 20 }
    };
    db.Learnings.AddRange(learnings);
    db.SaveChanges();
}
```

### 3. Authentication Claims
Ensure the authentication system sets the user ID in claims:
```csharp
var claims = new List<Claim>
{
    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
    new Claim(ClaimTypes.Name, user.UserName),
    new Claim(ClaimTypes.Role, user.Role)
};
```

## Security Considerations

1. **Authorization Policies**: All endpoints use role-based authorization
2. **User Isolation**: Employees can only see their own assignments
3. **CSRF Protection**: Forms include CSRF tokens
4. **SQL Injection**: Uses Entity Framework parameterized queries

## Client-Side Technologies

- **Bootstrap 5**: UI components and layout
- **Vanilla JavaScript**: API calls and DOM manipulation
- **Chart.js**: Data visualization for HR dashboard
- **Fetch API**: Asynchronous data loading

## Customization

### Change Team Assignment Logic
In `DashboardController.GetManagerDashboard()`, modify the team member query:
```csharp
var teamMembers = await _context.Users
    .Where(u => u.Role == "Employee" && u.DepartmentId == managerId) // Add department logic
    .Select(u => u.Id)
    .ToListAsync();
```

### Add Custom Metrics
Extend the dashboard response objects to include additional metrics like:
- Average completion time
- Most popular courses
- Overdue assignments
- Compliance requirements

### Styling
Customize colors and styling in the view files by modifying:
- Card background colors (e.g., `bg-primary`, `bg-success`)
- Chart colors in JavaScript
- Progress bar colors
- Badge styles

## Testing the API

Use tools like Postman or cURL to test endpoints:

```bash
# Get Employee Dashboard
curl -H "Authorization: Bearer {token}" https://localhost:7000/api/dashboard/employee

# Create Assignment
curl -X POST https://localhost:7000/api/assignments \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer {token}" \
  -d '{"userId":5,"learningId":2,"dueDate":"2024-02-28"}'

# Update Assignment Progress
curl -X PUT https://localhost:7000/api/assignments/1 \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer {token}" \
  -d '{"status":"InProgress","progressPercentage":50}'
```

## Troubleshooting

### Dashboards show no data
1. Verify user is authenticated and has correct role
2. Check browser console for API errors
3. Ensure learning assignments exist in database
4. Verify database migrations were applied

### API returns 401 Unauthorized
1. Check authentication cookie is present
2. Verify user session is valid
3. Ensure user has assigned role

### Charts don't render
1. Verify Chart.js library loaded successfully
2. Check browser console for JavaScript errors
3. Ensure data returned from API is valid

## Future Enhancements

1. Export dashboards to PDF/Excel
2. Email notifications for due assignments
3. Manager assignment of learnings via UI
4. Learning completion certificates
5. Integration with external LMS systems
6. Advanced filtering and search
7. Mobile responsive improvements
8. Real-time updates via SignalR
