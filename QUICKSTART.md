# Quick Start Guide - Study Quest

## 🚀 How to Run the App

### Option 1: Start the Development Server

Run this command in the terminal:

```bash
npm start
```

This will:
- Start the Metro bundler
- Open Expo DevTools in your browser
- Show a QR code you can scan

### Option 2: Run on Android

```bash
npm run android
```

*Requires: Android Studio and Android emulator set up*

### Option 3: Run on iOS (Mac only)

```bash
npm run ios
```

*Requires: Xcode and iOS simulator*

## 📱 Testing on Your Phone

1. Install **Expo Go** app from:
   - Google Play Store (Android)
   - App Store (iOS)

2. Run `npm start` in your terminal

3. Scan the QR code with:
   - Camera app (iOS)
   - Expo Go app (Android)

## 🎯 What to Do First

1. **Add Your First Course**
   - Open the app
   - Go to "Courses" tab
   - Tap "Add Course"
   - Fill in: Course Name, Code, Credits
   - Tap "Add"

2. **Create Your Timetable**
   - Go to "Timetable" tab
   - Tap "Add Class"
   - Select your course
   - Choose day and time
   - Tap "Add"

3. **Make a Study Plan**
   - Go to "Study Plan" tab
   - Tap "Create Study Plan"
   - Select your course
   - Enter topics (one per line)
   - Set duration per topic
   - Add start and end dates
   - Tap "Create"

4. **Track Your Progress**
   - Complete topics in your study plan
   - Check the "Progress" tab to see your stats
   - Watch your study hours grow!

## 🎨 Customization

### Change App Icon and Splash Screen

Replace these placeholder files in the `assets/` folder:
- `icon.png` (1024x1024 px)
- `splash.png` (1284x2778 px recommended)
- `adaptive-icon.png` (1024x1024 px)
- `favicon.png` (48x48 px or larger)

### Modify Colors

Edit the color values in your screen files:
- Primary color: `#6366f1` (indigo)
- Success color: `#10b981` (green)
- Warning color: `#f59e0b` (orange)
- Error color: `#ef4444` (red)

## 🐛 Troubleshooting

### "Module not found" errors
```bash
npm install
```

### Metro bundler issues
```bash
npm start -- --reset-cache
```

### Port already in use
```bash
npm start -- --port 8082
```

### Clear everything and restart
```bash
rm -rf node_modules
npm install
npm start
```

## 📚 Features Overview

✅ **Implemented (MVP)**
- Timetable creation and management
- Course management with file uploads
- Study plan generation
- Progress tracking
- Motivational messages
- Achievement badges
- Data persistence (local storage)

## 🔧 Tech Stack

- **React Native** - Mobile framework
- **Expo** - Development platform
- **TypeScript** - Type safety
- **React Navigation** - Navigation
- **React Native Paper** - UI components
- **AsyncStorage** - Local data storage

## 💡 Tips

1. **Data is Local**: All your data is stored on your device
2. **No Login Required**: Just start using it!
3. **Free to Customize**: Modify any part of the code
4. **Study Daily**: Build your streak!

## 🎓 Made for Students

This app is designed to help you:
- Stay organized with your coursework
- Plan your study sessions effectively
- Track your progress visually
- Stay motivated with achievements

Happy studying! 📚✨
