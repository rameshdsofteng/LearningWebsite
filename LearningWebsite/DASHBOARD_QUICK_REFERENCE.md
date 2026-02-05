# Dashboard Features - Quick Reference

## Role-Based Access

### Employee Dashboard (/Employee)
**Who Can Access**: Users with "Employee" role

**What Employees See**:
- Summary cards: Total, Completed, In Progress, Not Started assignments
- Detailed table of personal learning assignments
- Assignment details: Title, Category, Status, Progress %, Dates
- Days until due (with color coding: red if overdue, yellow if within 7 days)
- "Update" button to modify progress and status

**What Employees Can Do**:
- View all assigned learnings
- Track personal progress
- Update learning status (Not Started → In Progress → Completed)
- Update progress percentage
- View completion dates

### Manager Dashboard (/Manager)
**Who Can Access**: Users with "Manager" role

**What Managers See**:
- Team overview with 6 metric cards
- Team member cards showing individual progress
- All team assignments in table format

**Metrics Displayed**:
- Team Members count
- Total Assignments
- Completed count
- In Progress count
- Not Started count
- Completion Rate percentage

**Team Overview Tab**:
- Cards for each team member
- Progress bar with completion percentage
- Breakdown: Total, Completed, In Progress, Not Started
- Responsive grid layout

**All Assignments Tab**:
- Table view of all team member assignments
- Employee name, Learning title, Status, Progress
- Assigned and Due dates
- Sortable columns

### HR Dashboard (/HR)
**Who Can Access**: Users with "HR" role

**What HR Sees**:
- Organization-wide learning metrics
- Interactive charts and analytics
- Employee progress tracking
- Learning category analysis

**Key Metrics**:
- Total Employees
- Total Managers
- Total Assignments
- Completion statistics
- Overall Completion Rate

**Charts**:
1. **Status Distribution Pie Chart**
   - Visual representation of all assignments by status
   - Color coded: Green (Completed), Yellow (In Progress), Red (Not Started)

2. **Category Completion Bar Chart**
   - Horizontal bar chart showing completion rate by learning category
   - Helps identify which categories have higher/lower completion rates

**Employee Progress Tab**:
- Table of all employees
- Total assignments assigned
- Completed count
- Individual completion rate
- Color-coded progress bar
- Color coding: Green (≥80%), Yellow (50-80%), Red (<50%)

**Category Analysis Tab**:
- Learning categories with metrics
- Total assignments per category
- Completed per category
- Category completion rate
- Visual progress bars

## API Endpoints Reference

### Dashboard Data Endpoints

| Method | Endpoint | Auth Required | Role Required | Purpose |
|--------|----------|---------------|---------------|---------|
| GET | `/api/dashboard/employee` | Yes | Employee | Get employee's dashboard data |
| GET | `/api/dashboard/manager` | Yes | Manager | Get manager's team dashboard data |
| GET | `/api/dashboard/hr` | Yes | HR | Get organization-wide analytics |

### Learning Management Endpoints

| Method | Endpoint | Auth Required | Role Required | Purpose |
|--------|----------|---------------|---------------|---------|
| GET | `/api/learnings` | No | None | List all available learnings |
| GET | `/api/learnings/{id}` | No | None | Get specific learning details |
| POST | `/api/learnings` | Yes | HR | Create new learning |
| PUT | `/api/learnings/{id}` | Yes | HR | Update learning |
| DELETE | `/api/learnings/{id}` | Yes | HR | Delete learning |

### Assignment Management Endpoints

| Method | Endpoint | Auth Required | Role Required | Purpose |
|--------|----------|---------------|---------------|---------|
| POST | `/api/assignments` | Yes | Manager/HR | Create new assignment |
| GET | `/api/assignments/{id}` | Yes | Any | Get assignment details |
| PUT | `/api/assignments/{id}` | Yes | Any* | Update assignment status/progress |
| DELETE | `/api/assignments/{id}` | Yes | Manager/HR | Delete assignment |

*Employees can only update their own assignments; Managers/HR can update any

## Common Workflows

### Assigning a Learning to an Employee

1. Navigate to `/api/assignments` (POST)
2. Send JSON:
   ```json
   {
     "userId": 5,
     "learningId": 2,
     "dueDate": "2024-02-28"
   }
   ```
3. Assignment created with "NotStarted" status

### Employee Updating Progress

1. Employee clicks "Update" on their dashboard
2. Modal opens showing current status and progress
3. Change status dropdown and/or adjust progress slider
4. Click "Save Changes"
5. Update saved via `/api/assignments/{id}` (PUT)
6. Dashboard refreshes with new data

### Manager Monitoring Team

1. Manager navigates to Manager Dashboard
2. Views summary metrics at top
3. Clicks "Team Overview" tab to see individual team members
4. Clicks "All Assignments" tab to see detailed assignment list
5. Identifies team members needing support based on progress

### HR Generating Reports

1. HR navigates to HR Dashboard
2. Views key metrics and charts
3. Identifies trends by category
4. Reviews employee progress table
5. Can use "Category Analysis" tab to drill down by learning type

## Data Visualization Elements

### Progress Bars
- **Height**: 20-25px for visibility
- **Color**: Blue by default, color-coded in HR dashboard
- **Display**: Shows percentage inside or adjacent to bar

### Status Badges
- **Completed**: Green background
- **In Progress**: Yellow/Orange background
- **Not Started**: Gray background

### Metric Cards
- **Design**: Bootstrap cards with colored headers
- **Layout**: Responsive grid (2-6 columns depending on screen)
- **Content**: Title, large number, optional percentage

### Tables
- **Sorting**: Bootstrap table with hover effects
- **Responsive**: Adapts to mobile with horizontal scroll
- **Actions**: Buttons inline or in action columns

## Troubleshooting Common Issues

### "No assignments at this time"
- HR needs to create learnings first
- Manager needs to assign learnings to employees
- Check that LearningAssignments exist for current user

### Charts not displaying
- Ensure Chart.js library loaded
- Check browser console for errors
- Verify data returned from API contains completionByCategory

### Data not refreshing
- Click F5 to refresh dashboard
- Check browser network tab for API errors
- Verify user has correct role

### Cannot create/update assignments
- Verify you're logged in as Manager or HR
- Check that user and learning IDs are valid
- Ensure due date is in the future

## Performance Notes

- Dashboards load data via API on page load
- Loading spinners show while data fetches
- Tables are client-side sortable with Bootstrap
- Charts update when tab is clicked
- No pagination implemented (for small datasets)

## Security Features

- Role-based authorization on all endpoints
- Users can only see their own data (except Manager/HR)
- CSRF tokens on form submissions
- SQL injection protected via Entity Framework
- All sensitive operations require authentication

## Customization Tips

### Change Colors
Edit the Bootstrap color classes in views:
- `bg-primary`, `bg-success`, `bg-warning`, `bg-danger`, `bg-secondary`, `bg-info`
- Modify chart colors in JavaScript `backgroundColor` properties

### Change Date Format
In JavaScript, replace:
```javascript
new Date(assignment.assignedDate).toLocaleDateString()
```
With custom format as needed

### Add More Metrics
Extend API responses in controllers and display in views
Add new chart types using Chart.js documentation

### Customize Table Columns
Remove/add `<th>` and `<td>` elements in table HTML
Adjust column width with Bootstrap column classes
