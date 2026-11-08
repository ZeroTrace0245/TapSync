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

### Application Structure
```
┌─────────────────────────────────────────────────────────────────┐
│                      TapSync Application                         │
├─────────────────────────────────────────────────────────────────┤
│                                                                   │
│  ┌──────────────────────────────────────────────────────────┐   │
│  │ Entry Point: App.xaml (Application Lifecycle)            │   │
│  └──────────────────────────────────────────────────────────┘   │
│                          ↓                                        │
│  ┌──────────────────────────────────────────────────────────┐   │
│  │ MainWindow.xaml (Navigation Shell)                        │   │
│  │  ├─ Navigation Sidebar (Icon-based page switching)       │   │
│  │  ├─ Content Frame (Dynamic page loading)                 │   │
│  │  └─ Status Bar (Footer info)                             │   │
│  └──────────────────────────────────────────────────────────┘   │
│          ↓                                ↓              ↓        │
│  ┌────────────────┐  ┌──────────────┐  ┌─────────────┐          │
│  │ DashboardPage  │  │ AttendeesPage│  │ AccessPage  │          │
│  │  (Default)     │  │              │  │             │          │
│  │                │  │  • Search    │  │  • Activity │          │
│  │ • Metrics      │  │  • Details   │  │  • Timeline │          │
│  │ • Charts       │  │  • Check-in  │  │  • Status   │          │
│  │ • Activity Log │  │              │  │             │          │
│  └────────────────┘  └──────────────┘  └─────────────┘          │
│                          ↓ (All pages consume)                   │
│  ┌──────────────────────────────────────────────────────────┐   │
│  │ Services Layer                                            │   │
│  │  └─ MockDataService (Data provider, business logic)      │   │
│  └──────────────────────────────────────────────────────────┘   │
│                          ↓ (All services use)                    │
│  ┌──────────────────────────────────────────────────────────┐   │
│  │ Models Layer (Data Classes)                              │   │
│  │  ├─ Event (Conference details)                           │   │
│  │  ├─ Attendee (Participant info)                          │   │
│  │  ├─ Credential (NFC access credentials)                  │   │
│  │  ├─ AccessRule (Gate access policies)                    │   │
│  │  └─ AccessLog (Activity tracking)                        │   │
│  └──────────────────────────────────────────────────────────┘   │
│                                                                   │
└─────────────────────────────────────────────────────────────────┘
```

### Data Flow Diagram
```
┌─────────────────────────────────────┐
│   MockDataService.Initialize()      │
│  (Generates mock event & attendees) │
└─────────────┬───────────────────────┘
			  │
			  ├─────────────────────────┬──────────────────────┐
			  ↓                         ↓                      ↓
		┌──────────────┐    ┌──────────────────┐    ┌─────────────────┐
		│ Event Model  │    │ Attendee List    │    │ AccessLog List  │
		│              │    │                  │    │                 │
		│ • Name       │    │ • ID, Name       │    │ • Timestamp     │
		│ • Venue      │    │ • Email          │    │ • User          │
		│ • Dates      │    │ • Category       │    │ • Gate          │
		│ • Capacity   │    │ • Credential     │    │ • Status        │
		└──────────────┘    │ • CheckIn Status │    └─────────────────┘
							└──────────────────┘
			  ↓                         ↓                      ↓
		┌─────────────────────────────────────────────────────────┐
		│        Pages Bind to Models via Properties              │
		│  (ItemsSource, SelectedItem, ObservableCollection)      │
		└─────────────────────────────────────────────────────────┘
			  ↓                         ↓                      ↓
		┌──────────────┐    ┌──────────────────┐    ┌─────────────────┐
		│ DashboardPage│    │ AttendeesPage    │    │ AccessPage      │
		│              │    │                  │    │                 │
		│ Displays:    │    │ Displays:        │    │ Displays:       │
		│ • Metrics    │    │ • Attendee list  │    │ • Activity log  │
		│ • Charts     │    │ • Search results │    │ • Timeline      │
		│ • Activity   │    │ • Detail view    │    │ • Status info   │
		└──────────────┘    └──────────────────┘    └─────────────────┘
```

### Component Dependencies
```
UI Layer (Views)
│
├─→ Services Layer (MockDataService)
│   │
│   └─→ Models Layer (Data Objects)
│       │
│       ├─ Event
│       ├─ Attendee
│       ├─ Credential
│       ├─ AccessRule
│       └─ AccessLog
│
└─→ WinUI 3 Framework
	├─ XAML Controls
	├─ Data Binding
	├─ Navigation
	└─ Visual Styling
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
- ✅ **Live Location Heatmap** (In Development) - Real-time attendee tracking with density visualization, zone-based analytics, and category filtering
- Gate validation logic
- Credential management UI
- Event setup wizard

### Phase 2: Real Data
- Replace MockDataService with database
- Add data persistence
- Implement authentication

### Phase 3: Advanced Features
- Full NFC reader integration
- Offline mode with sync
- Real-time WebSocket synchronization
- Emergency roll-call workflow with automated notifications

## 📝 File Locations & Structure

```
Project Root: D:\GitHub repos\TapSync WinUI 3\TapSync\

📦 TapSync.slnx (Solution File - Open this)
│
└─ 📁 TapSync/ (Main Application Project)
   │
   ├─ 🎯 Entry Point
   │  ├─ App.xaml              ← Application lifecycle, resource definitions
   │  ├─ App.xaml.cs           ← App initialization, event handlers
   │  ├─ MainWindow.xaml       ← Main UI shell with navigation
   │  └─ MainWindow.xaml.cs    ← Navigation logic, page loading
   │
   ├─ 📂 Models/ (Data Classes)
   │  ├─ Event.cs              ← Conference/event details
   │  ├─ Attendee.cs           ← Participant information
   │  ├─ Credential.cs         ← NFC credential data
   │  ├─ AccessRule.cs         ← Gate access policies
   │  └─ AccessLog.cs          ← Activity & access tracking
   │
   ├─ 🔧 Services/ (Business Logic)
   │  ├─ MockDataService.cs    ← Generates realistic mock data
   │  │                           (Event, Attendees, AccessLogs)
   │  └─ IDataService.cs       ← Service interface (for future real DB)
   │
   ├─ 🎨 Views/ (UI Pages)
   │  ├─ DashboardPage/
   │  │  ├─ DashboardPage.xaml          ← Dashboard layout & controls
   │  │  └─ DashboardPage.xaml.cs       ← Metrics, charts, logic
   │  ├─ AttendeesPage/
   │  │  ├─ AttendeesPage.xaml          ← Attendee list layout
   │  │  └─ AttendeesPage.xaml.cs       ← Search, filter, check-in
   │  └─ AccessPage/
   │     ├─ AccessPage.xaml             ← Activity log layout
   │     └─ AccessPage.xaml.cs          ← Log display, filtering
   │
   ├─ 📚 Assets/ (Resources)
   │  ├─ Fonts/                ← Custom typography
   │  ├─ Icons/                ← Navigation & UI icons
   │  └─ Styles/               ← Theme & control styles
   │
   ├─ 📋 Configuration Files
   │  ├─ TapSync.csproj        ← Project configuration
   │  ├─ app.manifest          ← Application manifest
   │  └─ package.appxmanifest  ← Windows app manifest
   │
   └─ 📖 Documentation
      ├─ README.md             ← Full project documentation
      ├─ QUICKSTART.md         ← This file (Getting started guide)
      └─ TapSyn.md             ← Project proposal & vision
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
