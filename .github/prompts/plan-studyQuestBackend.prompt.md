## Plan: Study Quest .NET Backend

**TL;DR** — Build a .NET 9 Web API backend with PostgreSQL, Twilio phone-number OTP authentication, grade 10-12 curriculum content (admin-seeded + AI-generated), OpenAI-powered AI features (flashcards, quizzes, topic explanations, summarization), progress tracking, and FCM push notification reminders. Single-project monolith structure, Docker-ready for deployment.

Subjects: Maths, English, Science, Physics, Biology, Chemistry, Agriculture, Geography — grades 10-12.

---

### **Steps**

#### 1. Scaffold the .NET project
- Create `backend/` folder at repo root
- `dotnet new webapi` with .NET 9 (latest stable as of Feb 2026)
- Folder structure inside the single project:
  ```
  backend/StudyQuest.API/
  ├── Controllers/
  ├── Models/          (EF entities)
  ├── DTOs/
  ├── Services/
  │   ├── Interfaces/
  │   └── Implementations/
  ├── Data/            (DbContext, migrations, seed data)
  ├── Auth/            (OTP logic, JWT)
  ├── Middleware/
  ├── Configuration/
  ├── appsettings.json
  ├── Dockerfile
  └── Program.cs
  ```

#### 2. Install NuGet packages
- `Microsoft.EntityFrameworkCore` + `Npgsql.EntityFrameworkCore.PostgreSQL` — PostgreSQL via EF Core
- `Microsoft.AspNetCore.Authentication.JwtBearer` — JWT auth
- `Twilio` — SMS OTP delivery
- `FirebaseAdmin` — FCM push notifications
- `OpenAI` (official .NET SDK) — AI features
- `Swashbuckle.AspNetCore` — Swagger docs
- `FluentValidation.AspNetCore` — request validation

#### 3. Define domain entities in `Models/`
| Entity | Key Fields | Relationships |
|---|---|---|
| **Student** | Id (Guid), PhoneNumber, FirstName, LastName, Grade (10/11/12), CreatedAt | Has many Enrollments, StudySessions, StudyPlans, ProgressRecords |
| **Subject** | Id, Name, Grade, Description, Color | Has many Topics, seed-loaded with 8 subjects x 3 grades = 24 rows |
| **Topic** | Id, SubjectId, Name, Order, Description | Has many Notes, Questions |
| **Note** | Id, TopicId, Title, Content (rich text/markdown), IsAIGenerated, CreatedAt | Belongs to Topic |
| **Question** | Id, TopicId, QuestionText, AnswerText, Difficulty (1-3), IsAIGenerated | Belongs to Topic |
| **Enrollment** | Id, StudentId, SubjectId, EnrolledAt | Student picks which subjects they study |
| **TimetableEntry** | Id, StudentId, SubjectId, DayOfWeek, StartTime, EndTime, Location | Student's weekly schedule |
| **StudyPlan** | Id, StudentId, SubjectId, StartDate, EndDate, CreatedAt | Has many StudyPlanItems |
| **StudyPlanItem** | Id, StudyPlanId, TopicId, ScheduledDate, Duration, IsCompleted, CompletedAt | Individual task in a plan |
| **StudySession** | Id, StudentId, SubjectId, TopicId, StartedAt, EndedAt, DurationMinutes, Notes | Actual study time logged |
| **Flashcard** | Id, TopicId, StudentId?, Front, Back, IsAIGenerated | Can be per-topic or per-student |
| **Quiz** | Id, TopicId, StudentId, Score, TotalQuestions, CompletedAt | Quiz attempt record |
| **QuizQuestion** | Id, QuizId, QuestionText, Options (JSON), CorrectAnswer, StudentAnswer | Individual quiz question |
| **Achievement** | Id, StudentId, Type, Title, Description, XPReward, UnlockedAt | Gamification |
| **StudentProgress** | Id, StudentId, SubjectId, XP, Level, Streak, TotalStudyMinutes, LastStudyDate | Per-subject progress |
| **DeviceToken** | Id, StudentId, Token, Platform, CreatedAt | FCM tokens for push |
| **Reminder** | Id, StudentId, Title, Message, ScheduledAt, SentAt, Type | Scheduled reminders |

#### 4. Create `Data/AppDbContext.cs` and configuration
- Configure all entities with Fluent API (indexes on PhoneNumber, composite keys where needed)
- Unique index on `Student.PhoneNumber`
- Seed data: 8 subjects x 3 grades, starter topics per subject, sample notes and questions
- Create initial EF migration

#### 5. Build Authentication system (`Auth/`)
- **`POST /api/auth/request-otp`** — Takes phone number, generates 6-digit OTP, stores hashed OTP with 5-min expiry in DB/cache, sends via Twilio SMS
- **`POST /api/auth/verify-otp`** — Validates OTP, creates Student if new, returns JWT access token + refresh token
- **`POST /api/auth/refresh`** — Refresh expired access tokens
- **`POST /api/auth/logout`** — Invalidate refresh token
- JWT tokens with `StudentId`, `PhoneNumber`, `Grade` claims
- OTP rate limiting: max 3 requests per phone per 10 minutes
- Use `IMemoryCache` or Redis for OTP storage (in-memory for dev)

#### 6. Build API Controllers & Services

**SubjectsController** — `GET /api/subjects?grade=10` (list by grade)
**TopicsController** — `GET /api/subjects/{id}/topics` (list topics for a subject)
**NotesController** — `GET /api/topics/{id}/notes`, `POST` (admin)
**QuestionsController** — `GET /api/topics/{id}/questions`, `POST` (admin)
**EnrollmentsController** — `POST /api/enrollments` (student picks subjects), `GET` (list student's subjects)

**TimetableController**
- `GET /api/timetable` — Student's full schedule
- `POST /api/timetable` — Add entry
- `PUT /api/timetable/{id}` — Update
- `DELETE /api/timetable/{id}` — Remove

**StudyPlanController**
- `GET /api/study-plans` — Student's plans
- `POST /api/study-plans` — Create plan (optionally AI-generated)
- `PUT /api/study-plans/{id}/items/{itemId}` — Toggle completion
- `DELETE /api/study-plans/{id}`

**StudySessionController**
- `POST /api/study-sessions` — Log a session
- `GET /api/study-sessions?subjectId=&from=&to=` — History with filters

**ProgressController**
- `GET /api/progress` — Overall progress (XP, level, streak, per-subject stats)
- `GET /api/progress/achievements` — All achievements with unlock status

**AIController** — the core AI-powered features:
- `POST /api/ai/summarize` — Summarize a note into key points
- `POST /api/ai/flashcards` — Generate flashcards from a topic/note content
- `POST /api/ai/quiz` — Generate a quiz on a topic (MCQ + short answer)
- `POST /api/ai/explain` — Explain a topic at the student's grade level
- `POST /api/ai/study-plan` — AI-generate a study plan for a subject

**ReminderController**
- `POST /api/reminders` — Create a reminder
- `GET /api/reminders` — List upcoming reminders
- `DELETE /api/reminders/{id}`

**ProfileController**
- `GET /api/profile` — Student profile
- `PUT /api/profile` — Update name, grade, daily goal

#### 7. Implement AI Service (`Services/AIService.cs`)
- Wrap OpenAI .NET SDK (`GPT-4o-mini` for cost efficiency, `GPT-4o` for complex explanations)
- System prompts tailored per feature:
  - **Summarize**: "You are an educational assistant. Summarize the following note for a grade {X} student into clear bullet points."
  - **Flashcards**: "Generate {N} flashcards as JSON array [{front, back}] from this content, appropriate for grade {X}."
  - **Quiz**: "Generate {N} multiple-choice questions as JSON. Include question, 4 options, correct answer, and explanation. Difficulty: grade {X} level."
  - **Explain**: "Explain this topic to a grade {X} student. Use simple language, examples, and analogies."
  - **Study Plan**: "Create a {N}-day study plan for {subject}, covering these topics: {list}. Return as JSON with dates, topics, tasks."
- Response parsing with structured outputs / JSON mode
- Caching: cache AI responses per topic+type to avoid repeated API calls
- Rate limiting per student (e.g., 50 AI requests/day)

#### 8. Implement Notification Service (`Services/NotificationService.cs`)
- Firebase Admin SDK for sending push notifications
- Background service (`IHostedService`) that checks for due reminders every minute
- Sends FCM push to student's registered device tokens
- Automatic reminders: "Time to study {subject}!" based on timetable schedule
- Streak reminders: "Don't break your {N}-day streak!" if no session logged today by 6 PM

#### 9. Implement Progress & Gamification (`Services/ProgressService.cs`)
- XP awarded for: completing study sessions, finishing plan items, quiz scores, streaks
- Level calculation: `Level = floor(totalXP / 500) + 1`
- Streak: consecutive days with at least one study session
- Achievement checks triggered after each action (same 12 achievements from frontend, plus new ones)
- Leaderboard potential (future): per-grade ranking by XP

#### 10. Seed curriculum data (`Data/SeedData.cs`)
- For each of the 8 subjects x 3 grades, define topics:
  - **Maths 10**: Algebraic Expressions, Equations & Inequalities, Number Patterns, Functions & Graphs, etc.
  - **Maths 11**: Exponents & Surds, Equations, Financial Maths, Probability, etc.
  - **Maths 12**: Sequences & Series, Differential Calculus, Statistics, etc.
  - Similar breakdowns for Physics, Chemistry, Biology, English, Science, Agriculture, Geography
- Seed starter notes and questions for each topic (~5-10 notes, ~10-20 questions per topic)
- Mark seeded content as `IsAIGenerated = false`

#### 11. Configure `Program.cs`
- Register all services with DI
- Configure EF Core with PostgreSQL connection string
- Configure JWT authentication
- Configure CORS for React Native
- Add Swagger in Development
- Add global exception handling middleware
- Add request logging middleware
- Configure rate limiting

#### 12. Docker & deployment config
- `Dockerfile` — multi-stage build (SDK for build, ASP.NET runtime for final)
- `docker-compose.yml` at repo root — API + PostgreSQL + Redis (optional for caching)
- `.env.example` with required environment variables:
  - `ConnectionStrings__DefaultConnection`
  - `Jwt__Secret`, `Jwt__Issuer`, `Jwt__Audience`
  - `Twilio__AccountSid`, `Twilio__AuthToken`, `Twilio__PhoneNumber`
  - `OpenAI__ApiKey`
  - `Firebase__CredentialsPath`
- Health check endpoint: `GET /health`

#### 13. Update frontend types and add API client
- Update `src/types/index.ts` to align with backend DTOs (add `Student`, `Subject`, `Topic`, `Note`, `Question`, `Flashcard`, `Quiz` types)
- Create `src/services/api.ts` — Axios/fetch wrapper with JWT token management
- Create `src/services/auth.ts` — OTP request, verification, token storage
- Gradually migrate screens from AsyncStorage to API calls

---

### **Verification**
- Run `dotnet build` — project compiles with zero errors
- Run `dotnet ef migrations add Initial` — migration generates successfully
- `docker-compose up` — API + PostgreSQL start, Swagger accessible at `http://localhost:5000/swagger`
- Test auth flow: request OTP → verify → receive JWT → access protected endpoints
- Test AI endpoints: send a topic → receive flashcards/quiz/summary as structured JSON
- Test CRUD on all entities via Swagger

### **Decisions**
- **PostgreSQL** over SQL Server — open-source, better for containerized deployment
- **Twilio** for SMS OTP — most reliable, well-documented
- **OpenAI GPT-4o-mini** as default AI model — best cost/quality balance; GPT-4o for complex explanations
- **Monolith** over Clean Architecture — faster to build, sufficient for current scope, can refactor later
- **FCM** for push notifications — free, works on both iOS and Android
- **Hybrid curriculum** — admin-seeded core content + AI supplements for dynamic flashcards/quizzes/explanations
- **Docker + cloud-ready** from the start — docker-compose for local dev, deployable to any cloud
