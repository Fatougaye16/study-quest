# Study Quest 📚

A fun and motivating React Native mobile app designed to help students manage their studies effectively.

## Features

✨ **Timetable Management**
- Create and manage your class schedule
- View daily classes at a glance
- Add location information for each class

📖 **Course Management**
- Add courses with details (name, code, credits, instructor)
- Upload and organize course materials
- Color-coded courses for easy identification

📝 **Study Plan Generator**
- Create customized study plans for each course
- Break down topics into manageable chunks
- Track completion of each topic
- Set study durations and deadlines

📊 **Progress Tracking**
- View overall study progress
- Track hours studied per course
- Monitor completion rates
- Earn achievements for milestones

🎯 **Motivation Features**
- Daily motivational messages
- Study streak tracking
- Achievement system
- Visual progress indicators

## Tech Stack

- **Framework**: React Native with Expo
- **Language**: TypeScript
- **Navigation**: React Navigation
- **UI Components**: React Native Paper
- **Storage**: AsyncStorage
- **File Handling**: Expo Document Picker

## Getting Started

### Prerequisites

- Node.js (v14 or higher)
- npm or yarn
- Expo CLI (optional, for development)

### Installation

1. Clone the repository or navigate to the project directory:
```bash
cd study-quest
```

2. Install dependencies:
```bash
npm install
```

3. Start the development server:
```bash
npm start
```

This will start the Expo development server. You can then:
- Press `a` to open on Android emulator
- Press `i` to open on iOS simulator
- Install Expo Go app on your phone and scan the QR code

## Project Structure

```
study-quest/
├── src/
│   ├── screens/         # All screen components
│   │   ├── HomeScreen.tsx
│   │   ├── TimetableScreen.tsx
│   │   ├── CoursesScreen.tsx
│   │   ├── StudyPlanScreen.tsx
│   │   └── ProgressScreen.tsx
│   ├── types/           # TypeScript type definitions
│   │   └── index.ts
│   └── utils/           # Utility functions
│       └── storage.ts   # AsyncStorage helpers
├── App.tsx              # Main app component with navigation
├── package.json
├── tsconfig.json
├── app.json
└── README.md
```

## Usage Guide

### 1. Add Courses
- Navigate to the "Courses" tab
- Tap "Add Course" button
- Fill in course details (name, code, credits, instructor)
- Upload course materials by expanding a course and tapping "Upload"

### 2. Create Timetable
- Go to the "Timetable" tab
- Tap "Add Class" button
- Select a course, day, and time
- Optionally add a location
- View your weekly schedule organized by day

### 3. Generate Study Plans
- Navigate to "Study Plan" tab
- Tap "Create Study Plan"
- Select a course
- Enter topics (one per line)
- Set study duration per topic
- Define start and end dates
- Track your progress by checking off completed topics

### 4. Monitor Progress
- Visit the "Progress" tab to see:
  - Overall study progress percentage
  - Total hours studied
  - Course-wise completion rates
  - Achievements earned

## Features in Detail

### Home Screen
- Daily overview with motivational messages
- Today's class schedule
- Study streak counter
- Quick stats and recent sessions

### Timetable Screen
- Weekly view organized by days
- Color-coded by course
- Long-press to delete a class
- Easy-to-read time slots

### Courses Screen
- Course list with expandable details
- File management for course materials
- Credit tracking
- Instructor information

### Study Plan Screen
- Topic-based study organization
- Progress bars for each plan
- Checkbox interface for completion
- Date range tracking

### Progress Screen
- Visual progress indicators
- Hour tracking per course
- Achievement badges
- Motivational feedback

## Customization

### Colors
Courses are automatically assigned colors from a predefined palette. You can modify the color scheme in [CoursesScreen.tsx](src/screens/CoursesScreen.tsx):

```typescript
const COURSE_COLORS = ['#6366f1', '#8b5cf6', '#ec4899', '#f59e0b', '#10b981', '#06b6d4'];
```

### Theme
The app uses a consistent color theme based on indigo (`#6366f1`). You can customize this throughout the app by changing the color values in the style objects.

## Data Persistence

All data is stored locally on the device using AsyncStorage:
- Courses
- Timetable slots
- Study sessions
- Study plans

Data persists across app restarts.

## Future Enhancements

Potential features for future versions:
- Push notifications for upcoming classes
- Study reminders
- Calendar integration
- Export study plans as PDF
- Cloud sync across devices
- Study timer with Pomodoro technique
- Analytics and insights
- Social features (study groups)

## Contributing

This is an MVP (Minimum Viable Product). Contributions are welcome!

## License

This project is created for educational purposes.

## Support

For questions or issues, please create an issue in the repository.

---

Made with ❤️ for students everywhere 🎓
