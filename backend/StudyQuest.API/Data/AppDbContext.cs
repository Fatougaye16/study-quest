using Microsoft.EntityFrameworkCore;
using StudyQuest.API.Models;

namespace StudyQuest.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Student> Students => Set<Student>();
    public DbSet<Subject> Subjects => Set<Subject>();
    public DbSet<Topic> Topics => Set<Topic>();
    public DbSet<Note> Notes => Set<Note>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<TimetableEntry> TimetableEntries => Set<TimetableEntry>();
    public DbSet<StudyPlan> StudyPlans => Set<StudyPlan>();
    public DbSet<StudyPlanItem> StudyPlanItems => Set<StudyPlanItem>();
    public DbSet<StudySession> StudySessions => Set<StudySession>();
    public DbSet<Flashcard> Flashcards => Set<Flashcard>();
    public DbSet<Quiz> Quizzes => Set<Quiz>();
    public DbSet<QuizQuestion> QuizQuestions => Set<QuizQuestion>();
    public DbSet<Achievement> Achievements => Set<Achievement>();
    public DbSet<StudentProgress> StudentProgress => Set<StudentProgress>();
    public DbSet<DeviceToken> DeviceTokens => Set<DeviceToken>();
    public DbSet<Reminder> Reminders => Set<Reminder>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Student
        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.PhoneNumber).IsUnique();
            entity.Property(e => e.PhoneNumber).HasMaxLength(20).IsRequired();
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Grade).IsRequired();
            entity.Property(e => e.IsOtpEnabled).HasDefaultValue(false);
        });

        // Subject
        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Color).HasMaxLength(10);
            entity.HasIndex(e => new { e.Name, e.Grade }).IsUnique();
        });

        // Topic
        modelBuilder.Entity<Topic>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.HasOne(e => e.Subject).WithMany(s => s.Topics)
                  .HasForeignKey(e => e.SubjectId).OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(e => new { e.SubjectId, e.Order });
        });

        // Note
        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).HasMaxLength(300).IsRequired();
            entity.Property(e => e.SourceType).HasDefaultValue(0);
            entity.Property(e => e.OriginalFileName).HasMaxLength(500);
            entity.Property(e => e.IsOfficial).HasDefaultValue(false);
            entity.HasOne(e => e.Topic).WithMany(t => t.Notes)
                  .HasForeignKey(e => e.TopicId).OnDelete(DeleteBehavior.Cascade);
        });

        // Question
        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Topic).WithMany(t => t.Questions)
                  .HasForeignKey(e => e.TopicId).OnDelete(DeleteBehavior.Cascade);
        });

        // Enrollment
        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.StudentId, e.SubjectId }).IsUnique();
            entity.HasOne(e => e.Student).WithMany(s => s.Enrollments)
                  .HasForeignKey(e => e.StudentId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Subject).WithMany(s => s.Enrollments)
                  .HasForeignKey(e => e.SubjectId).OnDelete(DeleteBehavior.Cascade);
        });

        // TimetableEntry
        modelBuilder.Entity<TimetableEntry>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Location).HasMaxLength(200);
            entity.HasOne(e => e.Student).WithMany(s => s.TimetableEntries)
                  .HasForeignKey(e => e.StudentId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Subject).WithMany()
                  .HasForeignKey(e => e.SubjectId).OnDelete(DeleteBehavior.Restrict);
        });

        // StudyPlan
        modelBuilder.Entity<StudyPlan>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).HasMaxLength(200);
            entity.HasOne(e => e.Student).WithMany(s => s.StudyPlans)
                  .HasForeignKey(e => e.StudentId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Subject).WithMany()
                  .HasForeignKey(e => e.SubjectId).OnDelete(DeleteBehavior.Restrict);
        });

        // StudyPlanItem
        modelBuilder.Entity<StudyPlanItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.StudyPlan).WithMany(p => p.Items)
                  .HasForeignKey(e => e.StudyPlanId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Topic).WithMany()
                  .HasForeignKey(e => e.TopicId).OnDelete(DeleteBehavior.Restrict);
        });

        // StudySession
        modelBuilder.Entity<StudySession>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Student).WithMany(s => s.StudySessions)
                  .HasForeignKey(e => e.StudentId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Subject).WithMany()
                  .HasForeignKey(e => e.SubjectId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Topic).WithMany()
                  .HasForeignKey(e => e.TopicId).OnDelete(DeleteBehavior.SetNull);
            entity.HasIndex(e => new { e.StudentId, e.StartedAt });
        });

        // Flashcard
        modelBuilder.Entity<Flashcard>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Front).HasMaxLength(1000).IsRequired();
            entity.Property(e => e.Back).HasMaxLength(2000).IsRequired();
            entity.HasOne(e => e.Topic).WithMany(t => t.Flashcards)
                  .HasForeignKey(e => e.TopicId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Student).WithMany(s => s.Flashcards)
                  .HasForeignKey(e => e.StudentId).OnDelete(DeleteBehavior.SetNull);
        });

        // Quiz
        modelBuilder.Entity<Quiz>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Topic).WithMany()
                  .HasForeignKey(e => e.TopicId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Student).WithMany(s => s.Quizzes)
                  .HasForeignKey(e => e.StudentId).OnDelete(DeleteBehavior.Cascade);
        });

        // QuizQuestion
        modelBuilder.Entity<QuizQuestion>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Quiz).WithMany(q => q.Questions)
                  .HasForeignKey(e => e.QuizId).OnDelete(DeleteBehavior.Cascade);
        });

        // Achievement
        modelBuilder.Entity<Achievement>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Type).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Title).HasMaxLength(100).IsRequired();
            entity.HasIndex(e => new { e.StudentId, e.Type }).IsUnique();
            entity.HasOne(e => e.Student).WithMany(s => s.Achievements)
                  .HasForeignKey(e => e.StudentId).OnDelete(DeleteBehavior.Cascade);
        });

        // StudentProgress
        modelBuilder.Entity<StudentProgress>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.StudentId, e.SubjectId }).IsUnique();
            entity.HasOne(e => e.Student).WithMany(s => s.ProgressRecords)
                  .HasForeignKey(e => e.StudentId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Subject).WithMany()
                  .HasForeignKey(e => e.SubjectId).OnDelete(DeleteBehavior.Restrict);
        });

        // DeviceToken
        modelBuilder.Entity<DeviceToken>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Token).HasMaxLength(500).IsRequired();
            entity.Property(e => e.Platform).HasMaxLength(20).IsRequired();
            entity.HasIndex(e => e.Token).IsUnique();
            entity.HasOne(e => e.Student).WithMany(s => s.DeviceTokens)
                  .HasForeignKey(e => e.StudentId).OnDelete(DeleteBehavior.Cascade);
        });

        // Reminder
        modelBuilder.Entity<Reminder>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Message).HasMaxLength(500);
            entity.HasOne(e => e.Student).WithMany(s => s.Reminders)
                  .HasForeignKey(e => e.StudentId).OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(e => new { e.ScheduledAt, e.SentAt });
        });

        // Seed data
        SeedData.Seed(modelBuilder);
    }
}
