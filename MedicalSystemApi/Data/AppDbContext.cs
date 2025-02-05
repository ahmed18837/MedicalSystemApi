using MedicalSystemApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedicalSystemApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<BillItem> BillItems { get; set; }
        public DbSet<BillMedicalTest> BillMedicalTests { get; set; }
        public DbSet<MedicalTest> MedicalTests { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Medication> Medications { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // تخصيص Sequence واحدة لجميع الـ IDs
            modelBuilder.HasSequence<int>("CommonSequence", schema: "dbo")
                .StartsAt(1000)
                .IncrementsBy(5); // 5 الزيادة بمقدار 

            // Id start with 1000 and increase with 5 for all Ids
            modelBuilder.Entity<Patient>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEXT VALUE FOR dbo.CommonSequence");

            modelBuilder.Entity<Doctor>()
                .Property(d => d.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEXT VALUE FOR dbo.CommonSequence");

            modelBuilder.Entity<Appointment>()
                .Property(a => a.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEXT VALUE FOR dbo.CommonSequence");

            modelBuilder.Entity<Department>()
                .Property(a => a.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEXT VALUE FOR dbo.CommonSequence");

            modelBuilder.Entity<Staff>()
                .Property(a => a.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEXT VALUE FOR dbo.CommonSequence");

            modelBuilder.Entity<Bill>()
                .Property(a => a.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEXT VALUE FOR dbo.CommonSequence");

            modelBuilder.Entity<BillItem>()
                .Property(a => a.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEXT VALUE FOR dbo.CommonSequence");

            modelBuilder.Entity<BillMedicalTest>()
                .Property(a => a.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEXT VALUE FOR dbo.CommonSequence");

            modelBuilder.Entity<MedicalTest>()
                .Property(a => a.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEXT VALUE FOR dbo.CommonSequence");

            modelBuilder.Entity<MedicalRecord>()
                .Property(a => a.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEXT VALUE FOR dbo.CommonSequence");

            modelBuilder.Entity<Medication>()
                .Property(a => a.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEXT VALUE FOR dbo.CommonSequence");


            // Relationships

            // Patient ➡ Appointments (Optional) (One-Many)
            modelBuilder.Entity<Patient>()
                .HasMany(a => a.Appointments)
                .WithOne(p => p.Patient)
                .HasForeignKey(p => p.PatientId)
                .OnDelete(DeleteBehavior.SetNull);

            // Patient → MedicalRecords (Required)
            modelBuilder.Entity<Patient>()
                .HasMany(p => p.MedicalRecords)
                .WithOne(m => m.Patient)
                .HasForeignKey(m => m.PatientId)
                .IsRequired();

            // Doctor ➡ Appointments (Optional) (One-Many)
            modelBuilder.Entity<Doctor>()
                .HasMany(a => a.Appointments)
                .WithOne(d => d.Doctor)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.SetNull);

            // Doctor → MedicalRecords (Required)
            modelBuilder.Entity<Doctor>()
                .HasMany(d => d.MedicalRecords)
                .WithOne(m => m.Doctor)
                .HasForeignKey(m => m.DoctorId)
                .IsRequired();

            // Doctor ➡ Department (Required) (One-Many)
            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.Department)
                .WithMany(dept => dept.Doctors)
                .HasForeignKey(d => d.DepartmentId)
                .IsRequired();

            // Staff -> Appointments (Optional)
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Staff)
                .WithMany(s => s.Appointments)
                .HasForeignKey(a => a.StaffId)
                .OnDelete(DeleteBehavior.SetNull);

            // Staff -> Bills (Optional)
            //modelBuilder.Entity<Bill>()
            //    .HasOne(b => b.Staff)
            //    .WithMany(s => s.Bills)
            //    .HasForeignKey(b => b.StaffId)
            //    .OnDelete(DeleteBehavior.Restrict);

            // Patient -> Bills (Required)
            modelBuilder.Entity<Bill>()
                .HasOne(b => b.Patient)
                .WithMany(p => p.Bills)
                .HasForeignKey(b => b.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            // Bill -> BillItems (Required)
            modelBuilder.Entity<BillItem>()
                .HasOne(bi => bi.Bill)
                .WithMany(b => b.BillItems)
                .HasForeignKey(bi => bi.BillId)
                .OnDelete(DeleteBehavior.Cascade);

            // Bill ➡ MedicalTest (Many-Many)
            modelBuilder.Entity<BillMedicalTest>()
                .HasKey(bmt => bmt.Id);

            modelBuilder.Entity<BillMedicalTest>()
                .HasOne(bmt => bmt.Bill)
                .WithMany(b => b.BillMedicalTests)
                .HasForeignKey(bmt => bmt.BillId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BillMedicalTest>()
                .HasOne(bmt => bmt.MedicalTest)
                .WithMany(mt => mt.BillMedicalTests)
                .HasForeignKey(bmt => bmt.MedicalTestId)
                .OnDelete(DeleteBehavior.Cascade);

            // تحديد العلاقة بين MedicalRecord و Medication باستخدام Fluent API
            modelBuilder.Entity<Medication>()
                .HasOne(m => m.MedicalRecord)       // Medication يحتوي على MedicalRecord
                .WithMany(mr => mr.Medications)    // MedicalRecord يحتوي على العديد من Medications
                .HasForeignKey(m => m.MedicalRecordId)  // تحديد الـ Foreign Key في جدول Medication
                .OnDelete(DeleteBehavior.Cascade);  // تحديد أن حذف السجل الطبي سيؤدي إلى حذف الأدوية المرتبطة به

            modelBuilder.Entity<Department>()
                .HasMany(d => d.Staffs) // العلاقة One-to-Many
                .WithOne(s => s.Department) // كل موظف ينتمي إلى قسم
                .HasForeignKey(s => s.DepartmentId) // المفتاح الخارجي
                .OnDelete(DeleteBehavior.Restrict); // التحكم في حذف البيانات (عدم حذف الموظفين عند حذف القسم)
        }
    }
}

