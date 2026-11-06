# TapSync - Event Access & Safety Management System

## ✅ Working Application with Mock Data

A working WinUI 3 desktop application for event management with mock data. The application demonstrates the core features of TapSync with sample attendee data and simulated access logs.

## 🎯 Current Features

### 1. **Dashboard** 📊
   - Event overview with key metrics
   - Total attendees count
   - Check-in statistics
   - Check-in rate percentage
   - Active credentials count
   - Check-in breakdown by category (General, VIP, Staff, Press)
   - Recent access activity log

### 2. **Attendees Management** 👥
   - Complete list of registered attendees
   - Search by name or email
   - View attendee details:
	 - Full name and contact email
	 - Attendee category
	 - Check-in status
	 - Last check-in time
   - Quick check-in action button

### 3. **Access Activity** 🚪
   - Real-time access logs
   - View who accessed which gate and when
   - Access decision status (Allowed, Denied, OverrideAllowed)
   - Attendee information with access details
   - Time-stamped access records

## 📁 Project Structure

```
TapSync/
├── Models/
│   ├── Event.cs              - Event data model
│   ├── Attendee.cs          - Attendee information
│   ├── Credential.cs        - NFC credential tracking
│   ├── AccessRule.cs        - Access control rules
│   └── AccessLog.cs         - Access activity logs
├── Services/
│   └── MockDataService.cs   - Mock data provider with 10 sample attendees
├── Views/
│   ├── DashboardPage.xaml   - Main dashboard view
│   ├── DashboardPage.xaml.cs
│   ├── AttendeesPage.xaml   - Attendees management
│   ├── AttendeesPage.xaml.cs
│   ├── AccessPage.xaml      - Access activity logs
│   └── AccessPage.xaml.cs
├── MainWindow.xaml          - Main application window with navigation
├── MainWindow.xaml.cs
└── App.xaml/xaml.cs         - Application entry point
```

## 🚀 Getting Started

### Prerequisites
- Visual Studio 2022 or later
- Windows 10 SDK (19041) or later
- .NET 10.0 SDK
- Windows OS (Windows 10 or later)

### Build & Run
1. Open `TapSync.slnx` in Visual Studio
2. Set `TapSync` as the startup project
3. Build the solution (Ctrl+Shift+B)
4. Run the application (F5)

### Mock Data
The application comes pre-loaded with:
- **1 Event**: Tech Conference 2026 at Convention Center Hall A
- **10 Attendees**: Mix of categories (General, VIP, Staff, Press)
- **Active Credentials**: NFC credential codes for each attendee
- **Access Logs**: Simulated gate access records
- **Randomized Check-in Status**: Some checked in, some not, realistic data

## 🎨 User Interface

- **Clean Modern Design**: Uses WinUI 3 with Mica backdrop
- **Navigation Sidebar**: Easy access to all features
- **Responsive Cards**: Key metrics displayed in organized cards
- **Data Tables**: List views for attendees and access logs
- **Search Functionality**: Quick attendee lookup
- **Real-time Updates**: Check-in and data reflection

## 📊 Mock Data Details

### Sample Attendees
- John Smith (General)
- Sarah Johnson (VIP)
- Mike Williams (Staff)
- Emily Brown (General)
- David Davis (Press)
- Lisa Miller (VIP)
- James Wilson (General)
- Rachel Moore (Staff)
- Chris Taylor (General)
- Amanda Anderson (General)

### Randomized States
- ✅ Check-in status varies (Registered, CheckedIn, Departed)
- 📱 Each attendee has an NFC credential code
- 🎫 Categories properly distributed
- 📝 Access logs generated for checked-in attendees

## 🔧 Technology Stack

- **Framework**: WinUI 3 / Windows App SDK
- **Language**: C#
- **Target**: .NET 10.0-windows
- **Minimum OS**: Windows 10 Build 19041

## ✨ Key Classes

### MockDataService
Provides mock data for development and testing:
```csharp
var service = new MockDataService();
var currentEvent = service.GetCurrentEvent();
var attendees = service.GetAttendees(eventId);
var accessLogs = service.GetAccessLogs(eventId);
```

### Models
- `Event`: Event information and metadata
- `Attendee`: Individual attendee records
- `Credential`: NFC credential tracking
- `AccessLog`: Access decision history

## 🎯 Next Steps (Future Development)

1. **Real NFC Integration**: Connect actual NFC readers
2. **Database Backend**: Replace mock data with persistent storage
3. **Authentication**: User login and role-based access
4. **Real-time Sync**: Server synchronization capabilities
5. **Emergency Workflows**: Emergency roll-call feature
6. **Reports**: Advanced reporting and analytics
7. **Offline Mode**: Full offline-first functionality

## 📝 Notes

- This is a working prototype with mock data
- All data is randomly generated on app startup
- No data persistence between sessions
- Perfect for UI/UX testing and demonstration

## 📄 License

As per TapSync Project Proposal by R. M. S. D. Rathnayaka (Index 3374)

---

**Status**: ✅ Build Successful | Ready to Run
