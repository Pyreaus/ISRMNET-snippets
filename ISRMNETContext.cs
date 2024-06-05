using Microsoft.EntityFrameworkCore;
using ISRM.isrmnet.Model.POCOs.Entities;
/// <summary><para>
///  EF Core expects this class to always reside in DAL.dll, circumvent this by:
///  Explicitly specifying the assembly containing this class when performing migrations: (dotnet ef migrations add <MigrationName> --startup-project <RelativePathToAssembly>)
///  --OR--  Implementing the IDesignTimeDbContextFactory<ISRMNETContext> interface in ISRMNETDbFactory or in a seperate class.  
/// <summary><para>

namespace ISRM.isrmnet.Model.Contexts
{
    public sealed partial class ISRMNETContext(DbContextOptions<ISRMNETContext> opt) : DbContext(opt) 
    {
        public DbSet<HRUser> HRUsers { get; set; }
        public DbSet<AdminUser> AdmUsers { get; set; }
        public DbSet<StaffFinderUser> SFUsers { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {                                 
                optionsBuilder.UseSqlServer("<connnection-string>"); //fallback string
            }
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HRUser>().HasData(hrUsers, GenerateHRUsers());
            modelBuilder.Entity<AdminUser>().HasData(adminUsers, GenerateAdminUsers());
            modelBuilder.Entity<StaffFinderUser>().HasData(staffFinderUsers, GenerateSFUsers());      
        }
        private static HRUser?[] GenerateHRUsers() => [];
        private static AdminUser?[] GenerateAdminUsers() => [];
        private static StaffFinderUser?[] GenerateSFUsers()
        {
            return Enumerable.Range(0, names.GetLength(0))
                .Select(index => new StaffFinderUser()
                {
                    SFID = 100 + index,
                    WinUser = $"ISRM\\{names[index, 0][0]}{names[index, 1]}",
                    Email = $"{names[index, 0].ToLower()}.{names[index, 1].ToLower()}@isrm.tech",
                    FirstName = names[index, 0],
                    LastName = names[index, 1],
                    Shareholder = false,
                    ActiveUser = true,
                    Human = false,
                })
                .ToArray();
        }
        private readonly HRUser[] hrUsers = [];
        private readonly AdminUser[] adminUsers = [ 
            new() { ACTIVE_USER = true, ADMIN_SFID = 501, EMAIL = "john.smith@isrm.tech", FULL_NAME = "John Smith", WINUSER = "ISRM\\JSmith" }
            new() { ACTIVE_USER = true, ADMIN_SFID = 501, EMAIL = "john.smith@isrm.tech", FULL_NAME = "John Smith", WINUSER = "ISRM\\JSmith" }
        ]
        private readonly StaffFinderUser[] staffFinderUsers = [ new() { ActiveUser = true, Email = "john.smith@isrm.tech", FirstName = "John", LastName = "Smith", SFID = 400, WinUser = "ISRM\\JSmith" }];

        static readonly string[,] names = new string[,]
        {
            {"John", "Doe"},
            {"Jane", "Smith"},
            {"Michael", "Johnson"},
            {"Emily", "Brown"},
            {"David", "Jones"},
            {"Sarah", "Williams"},
            {"Robert", "Miller"},
            {"Jessica", "Davis"},
            {"William", "Garcia"},
            {"Linda", "Martinez"},
            {"Richard", "Rodriguez"},
            {"Susan", "Martinez"},
            {"Joseph", "Hernandez"},
            {"Karen", "Lopez"},
            {"Thomas", "Gonzalez"},
            {"Lisa", "Wilson"},
            {"Charles", "Anderson"},
            {"Betty", "Thomas"},
            {"Christopher", "Taylor"},
            {"Dorothy", "Moore"},
            {"Daniel", "Jackson"},
            {"Margaret", "Martin"},
            {"Matthew", "Lee"},
            {"Barbara", "Perez"},
            {"Anthony", "Thompson"},
            {"Susan", "White"},
            {"Mark", "Harris"},
            {"Elizabeth", "Sanchez"},
            {"Steven", "Clark"},
            {"Mary", "Ramirez"},
            {"Paul", "Lewis"},
            {"Patricia", "Robinson"},
            {"Andrew", "Walker"},
            {"Jennifer", "Young"},
            {"Joshua", "Allen"},
            {"Linda", "King"},
            {"George", "Wright"},
            {"Sandra", "Scott"},
            {"Kevin", "Torres"},
            {"Ashley", "Nguyen"},
            {"Brian", "Hill"},
            {"Amanda", "Flores"},
            {"Edward", "Green"},
            {"Melissa", "Adams"},
            {"Ronald", "Nelson"},
            {"Kimberly", "Baker"},
            {"Timothy", "Hall"},
            {"Michelle", "Rivera"},
            {"Jason", "Campbell"},
            {"Emily", "Mitchell"},
            {"Jeffrey", "Carter"},
            {"Angela", "Roberts"},
            {"Ryan", "Gomez"},
            {"Helen", "Phillips"},
            {"Jacob", "Evans"},
            {"Deborah", "Turner"},
            {"Gary", "Torres"},
            {"Stephanie", "Parker"},
            {"Nicholas", "Collins"},
            {"Rebecca", "Edwards"},
            {"Eric", "Stewart"},
            {"Laura", "Flores"},
            {"Stephen", "Morris"},
            {"Megan", "Nguyen"},
            {"Jonathan", "Murphy"},
            {"Cheryl", "Rivera"},
            {"Larry", "Cook"},
            {"Amy", "Rogers"},
            {"Scott", "Morgan"},
            {"Anna", "Peterson"},
            {"Frank", "Cooper"}
        };
            //--- uncomment to apply the cutom configurations for each entity (or as needed).         ref: https://www.learnentityframeworkcore.com/migrations
            // modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());      
            // modelBuilder.Entity<HRUser>().HasData(new HRUser());
            // modelBuilder.Entity<AdminUser>().HasData(new AdminUser());
            // modelBuilder.Entity<StaffFinderUser>().HasData(new StaffFinderUser());      
            // modelBuilder.Entity<StaffFinderUser>().ToView("StaffFinderUser").HasKey(x=>x.PFID);
    }
}
