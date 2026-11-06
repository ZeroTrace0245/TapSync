# TapSync Quick Start Guide

## 🎯 What's Been Built

A fully functional **WinUI 3 desktop application** with mock data for TapSync - an NFC event access and safety management system.

## 🚀 To Run the App

### Option 1: Visual Studio
1. Open **TapSync.slnx** in Visual Studio
2. Press **F5** to run
3. The app will start with the Dashboard page

### Option 2: Command Line
```bash
cd "D:\GitHub repos\TapSync WinUI 3\TapSync"
dotnet run --project TapSync\TapSync.csproj
```

## 📱 App Pages

### 📊 Dashboard (Default)
Shows live event metrics:
- Total attendees: 10
- Checked in: ~3-5 (randomized)
- Check-in rate: percentage calculated
- Active credentials: ~9-10
- Check-in breakdown by category
- Recent access activity (last 10 entries)

### 👥 Attendees
Browse and manage attendees:
- Search by name or email
- View full attendee details
- See check-in status and time
- Quick check-in button

### 🚪 Access Activity
View access logs:
- Who accessed which gate
- When they accessed it
- Access status (Allowed/Denied/OverrideAllowed)
- Time-stamped records

## 📊 Mock Data Details

### Event Created
```
Name: Tech Conference 2026
Venue: Convention Center Hall A
Dates: 5-7 days from now
Capacity: 500 attendees
```

### Sample Attendees (10 total)
Each has:
- Random first/last name
- Email address
- Category: General, VIP, Staff, or Press
- Random check-in status
- Assigned NFC credential with code

### Generated Data
- Access logs for all checked-in attendees
- Gate information: Main, VIP, Staff entrances
- Randomized check-in times

## 🎨 User Interface Features

- **Sidebar Navigation**: Easy page switching
- **Cards Dashboard**: Key metrics in visual cards
- **Search Box**: Attendee lookup
- **Data Tables**: List views with columns
- **Buttons**: Check-in actions
- **Modern Design**: WinUI 3 Mica backdrop

## 🔧 Architecture

```
App.xaml (Entry Point)
└── MainWindow.xaml (Navigation Shell)
	├── Navigation Sidebar
	└── Content Frame
		├── DashboardPage (Default)
		├── AttendeesPage
		└── AccessPage

Models (Data Classes)
├── Event
├── Attendee
├── Credential
├── AccessRule
└── AccessLog

Services (Business Logic)
└── MockDataService (Provides sample data)
```

## 💡 Key Features Working

✅ **Navigation** - Switch between pages seamlessly
✅ **Data Binding** - UI updates with data
✅ **Mock Data** - Realistic test data
✅ **Search** - Find attendees by name/email
✅ **Check-in** - Update attendee status
✅ **Metrics** - Real-time calculations
✅ **Logging** - Access activity tracking

## 🎯 Next Steps (To Extend)

### Phase 1: Add More Features
- Gate validation logic
- Credential management UI
- Event setup wizard

### Phase 2: Real Data
- Replace MockDataService with database
- Add data persistence
- Implement authentication

### Phase 3: Advanced Features
- NFC reader integration
- Offline mode
- Real-time synchronization
- Emergency roll-call workflow

## 📝 File Locations

```
Project Root: D:\GitHub repos\TapSync WinUI 3\TapSync\

TapSync/
├── TapSync.slnx              ← Open this file
├── README.md                 ← Full documentation
├── TapSync/
│   ├── App.xaml/cs           ← Entry point
│   ├── MainWindow.xaml/cs    ← Main window
│   ├── Models/               ← Data models
│   │   ├── Event.cs
│   │   ├── Attendee.cs
│   │   ├── Credential.cs
│   │   ├── AccessRule.cs
│   │   └── AccessLog.cs
│   ├── Services/             ← Business logic
│   │   └── MockDataService.cs
│   └── Views/                ← UI Pages
│       ├── DashboardPage.xaml/cs
│       ├── AttendeesPage.xaml/cs
│       └── AccessPage.xaml/cs
└── TapSync (Package)/        ← Packaging
```

## 🐛 Troubleshooting

### App won't build
- Run: `dotnet clean` then rebuild
- Check that all Views files have corresponding .xaml files
- Ensure XAML is valid XML

### Pages don't show
- Verify navigation code in MainWindow.xaml.cs
- Check Frame element exists in MainWindow.xaml
- Ensure DashboardPage loads on startup

### Data not showing
- Verify MockDataService creates attendees
- Check ItemsSource binding in XAML
- Ensure data types match in code-behind

## 📞 Support

For issues with the TapSync project, refer to:
- Project Proposal: TapSyn.md
- This guide: README.md + QUICKSTART.md

---

**Status: ✅ Ready to Run**  
**Build: ✅ Successful**  
**Features: ✅ All Working**

Enjoy exploring TapSync! 🎉
