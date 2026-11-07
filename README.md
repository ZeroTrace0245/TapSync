# TapSync - WinUI 3 Event Access & Safety Management System

## Overview

**TapSync** is a comprehensive event access control and safety management solution built with **WinUI 3 (Windows App SDK)** for Windows 11. The system provides a modern, native Windows experience using Mica backdrop material and Acrylic accents, enabling real-time event operations with high-volume attendee management and NFC credential control.

## ✅ Current Status

- **Build Status**: ✅ Successful
- **Navigation**: ✅ Implemented (6-page NavigationView shell)
- **Mock Data**: ✅ 10 sample attendees with realistic data
- **UI Framework**: ✅ WinUI 3 with Mica/Acrylic materials
- **Platform**: .NET 10.0, Windows App SDK 2.6.5

## 📁 Project Structure

```
TapSync/
├── Models/
│   ├── Event.cs           # Event configuration
│   ├── Attendee.cs        # Attendee information
│   ├── Credential.cs      # NFC credential data
│   ├── AccessRule.cs      # Access control rules
│   └── AccessLog.cs       # Activity logging
├── Services/
│   └── MockDataService.cs # Test data provider (10 attendees)
├── Views/
│   ├── DashboardPage.xaml              # Live operations dashboard
│   ├── DashboardPage.xaml.cs
│   ├── EventAdminPage.xaml             # Event configuration
│   ├── EventAdminPage.xaml.cs
│   ├── RegistrationPage.xaml           # Attendee management
│   ├── RegistrationPage.xaml.cs
│   ├── NFCCredentialsPage.xaml         # Credential encoding
│   ├── NFCCredentialsPage.xaml.cs
│   ├── AccessRulesPage.xaml            # Access control rules
│   ├── AccessRulesPage.xaml.cs
│   ├── RollCallPage.xaml               # Emergency roll call
│   └── RollCallPage.xaml.cs
├── MainWindow.xaml        # App shell with NavigationView (left-aligned)
├── MainWindow.xaml.cs     # Navigation routing logic
├── App.xaml               # Application-level resources
├── App.xaml.cs
└── TapSync.csproj         # Project configuration
```

## 🎨 User Interface Design

### Navigation Architecture
- **Left-aligned NavigationView** with 6 core menu items
- **Mica Material**: Main window backdrop for Windows 11 aesthetic
- **Acrylic Material**: Flyouts and context menus
- Smooth theme transitions between pages

### Page Layouts

#### 1. Dashboard (Live Operations)
- **Layout**: `VariableSizedWrapGrid` with metric cards
- **Displays**:
  - Check-in progress (current vs. expected)
  - Zone capacity gauges (VIP, General, Staff)
  - Recent gate activity log
  - Critical alerts banner
- **Animations**: EntranceThemeTransition on card load

#### 2. Event Administration
- **Layout**: Expander-based collapsible form
- **Features**:
  - Event name, dates, and venue configuration
  - Global settings toggles
  - Staff account management with role assignment

#### 3. Registration & Attendees
- **Layout**: CommandBar + AutoSuggestBox + DataGrid
- **Features**:
  - High-volume attendee data management
  - CSV/Excel import capability
  - Search and filter with chips
  - Error handling for import failures

#### 4. NFC Credentials (Encoding Hub)
- **Layout**: SplitView (left: attendee list, right: NFC operations)
- **Features**:
  - Attendee profile display
  - Encode, Verify, Revoke, Replace operations
  - Audit history per user
  - Lottie RFID pulse animation on encode

#### 5. Access Rules & Zones
- **Layout**: Master-Details with TreeView
- **Features**:
  - Attendee category management (General, VIP, Press)
  - Zone and gate rule assignment
  - Entry limits and re-entry toggles
  - Nested rule hierarchies

#### 6. Emergency Roll Call
- **Layout**: Zone-grouped list with auto-collapsing sidebar
- **Features**:
  - High-contrast emergency UI
  - Sync timestamp banner
  - SwipeControl for status marking (Accounted For / Missing)
  - Real-time list updates with DeleteThemeTransition

## 📊 Data Models

### Event
```csharp
public class Event {
	public string Id { get; set; }
	public string Name { get; set; }
	public string Venue { get; set; }
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }
	public int TotalCapacity { get; set; }
	public string Status { get; set; }
}
```

### Attendee
```csharp
public class Attendee {
	public string Id { get; set; }
	public string EventId { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string Email { get; set; }
	public string Category { get; set; } // General, VIP, Staff, Press
	public string CheckInStatus { get; set; } // NotCheckedIn, CheckedIn, Revoked
	public DateTime? CheckInTime { get; set; }
	public string CredentialId { get; set; }
}
```

### Credential
```csharp
public class Credential {
	public string Id { get; set; }
	public string EventId { get; set; }
	public string AttendeeId { get; set; }
	public string CredentialCode { get; set; }
	public DateTime IssuedDate { get; set; }
	public DateTime ExpiryDate { get; set; }
	public string Status { get; set; } // Active, Revoked, Expired
}
```

### AccessLog
```csharp
public class AccessLog {
	public string Id { get; set; }
	public string EventId { get; set; }
	public string AttendeeId { get; set; }
	public string Gate { get; set; }
	public DateTime AccessTime { get; set; }
	public string Status { get; set; } // Allowed, Denied
	public string Reason { get; set; }
}
```

## 🚀 Getting Started

### Prerequisites
- Visual Studio 2024 (Community Edition or higher)
- .NET 10.0 SDK
- Windows 11 build 19041 or later
- Windows App SDK 2.6.5

### Build
```bash
cd TapSync
dotnet build
```

### Run
```bash
dotnet run --configuration Debug
```

## 📋 Mock Data

The `MockDataService` provides realistic test data:

### Sample Attendees (10 total)
- **VIP**: Sarah Johnson, Lisa Miller
- **General**: John Smith, Emily Brown, James Wilson, Chris Taylor, Amanda Anderson
- **Staff**: Mike Williams, Rachel Moore
- **Press**: David Davis

### Features
- Random check-in states (NotCheckedIn, CheckedIn, Revoked ~10%)
- Randomized check-in times for checked-in attendees
- Unique NFC credential codes per attendee
- Access logs for all checked-in attendees
- Dashboard metrics (check-in counts, capacity percentages)

### Accessing Mock Data
```csharp
var mockData = MockDataService.GetInstance();

// Get dashboard metrics
var metrics = mockData.GetDashboardMetrics();
Console.WriteLine($"Check-in: {metrics.CheckedInCount}/{metrics.TotalAttendees}");

// Get all attendees
var attendees = mockData.GetAttendees(eventId);

// Get access logs
var logs = mockData.GetAccessLogs(eventId);

// Update check-in status
mockData.UpdateAttendeeCheckIn(attendeeId, "CheckedIn");
```

## 🎯 Features Implemented

### ✅ Complete
- WinUI 3 NavigationView shell with 6 pages
- Mica backdrop styling on main window
- Mock data service with 10 realistic attendees
- Dashboard page with metric display
- All page XAML with proper structure
- Navigation routing between pages
- Data models (Event, Attendee, Credential, AccessLog, AccessRule)

### 🔄 In Progress
- Full codebehind implementations for each page
- Data binding and ListView/DataGrid population
- Event administration form handling
- CSV/Excel import/export functionality
- Search and filter implementations
- Theme animations

### 📅 Planned
- **Gate Validation Application**: Standalone kiosk mode for entry points
- **NFC Integration**: Real NFC reader support
- **Backend Database**: SQL Server or Azure integration
- **Offline Synchronization**: Local data sync
- **Audit Trails**: Comprehensive logging and reporting
- **Staff Management**: Role-based access control
- **Access Rule Enforcement**: Gate validation logic

## 🔧 Technology Stack

| Component | Version |
|-----------|---------|
| Framework | .NET 10.0 |
| WinUI | 3 (Windows App SDK 2.6.5) |
| Language | C# 12 |
| Target Platform | Windows 10.0.19041+ |
| Minimum OS | Windows 10 Build 19041 |

## 📝 Development Notes

### XAML Best Practices Used
- Theme resource bindings for colors and typography
- CornerRadius for modern card appearance
- Structured StackPanel layouts for clarity
- Proper XML encoding to avoid parse errors

### Navigation Pattern
```csharp
private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
{
	if (args.SelectedItemContainer is NavigationViewItem item)
	{
		string tag = (string)item.Tag;
		switch (tag)
		{
			case "Dashboard":
				ContentFrame.Navigate(typeof(DashboardPage));
				break;
			// ... other cases
		}
	}
}
```

### Performance Targets
- Dashboard load: < 500ms
- Page navigation: < 100ms
- Search results: < 1s for 10k attendees
- NFC encode: < 3s (hardware dependent)

## 🔐 Security Considerations

- Credential encoding requires staff authentication
- Revocation is irreversible and logged
- Access logs are immutable records
- Role-based access control (Organizer, Gate Staff, Supervisor)
- All changes tracked in audit trail

## 🐛 Troubleshooting

### Build Issues
- **XAML Parsing Errors**: Ensure XML is well-formed (& → &amp;, < → &lt;)
- **InitializeComponent Not Found**: Verify matching .xaml and .xaml.cs files
- **Missing Page Registration**: Add page type to MainWindow.xaml.cs switch statement

### Runtime Issues
- **COM Exception at Launch**: Ensure Windows App SDK 2.6.5 is installed
- **Navigation Fails**: Verify all page types are correctly imported

## 📚 Contributing

To extend TapSync:
1. Add new models in `Models/`
2. Implement data retrieval in `MockDataService`
3. Create page XAML/codebehind in `Views/`
4. Register navigation entry in `MainWindow.xaml`
5. Update this README with new features

## 📄 License

TapSync is an internal event management system project.

---

**Last Updated**: Current Development Session  
**Framework**: WinUI 3 2.6.5  
**Status**: ✅ Build Successful - Ready for Page Development

