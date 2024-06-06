/// <summary><para>
///  EF Core expects this class to always reside in DAL.dll, circumvent this by:
///  Explicitly specifying the assembly containing this class when performing migrations: (dotnet ef migrations add <MigrationName> --startup-project <RelativePathToAssembly>)
///  --OR--  Implementing the <see cref="IDesignTimeDbContextFactory{ISRMNETContext}"/> interface in <c>ISRMNETDbFactory</c> or in a seperate class.  
/// <summary><para>
namespace ISRM.isrmnet.Model.Contexts
{
    public sealed partial class ISRMNETContext(DbContextOptions<ISRMNETContext> opt, bool? isDevelopment=null) : DbContext(opt) 
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
            if (isDevelopment == true) 
            {
                optionsBuilder.EnableDetailedErrors();
                optionsBuilder.EnableSensitiveDataLogging();
            }
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdminUser>(entity =>
            {
                entity.HasKey(x => x.ADMIN_ID); 
                entity.Property(x => x.ADMIN_ID).ValueGeneratedNever();  
            });
            modelBuilder.Entity<HRUser>(entity =>
            {
                entity.HasKey(x => x.HR_ID); 
                entity.Property(x => x.HR_ID).ValueGeneratedNever();  
            });

            modelBuilder.Entity<HRUser>().HasData([.. hrUsers, .. GenerateHRUsers()]);
            modelBuilder.Entity<AdminUser>().HasData([.. adminUsers, .. GenerateAdminUsers()]);
            modelBuilder.Entity<StaffFinderUser>().HasData([.. staffFinderUsers, .. GenerateSFUsers()]);
        }
        private static StaffFinderUser?[] GenerateSFUsers()
        {
            return Enumerable.Range(0, names.GetLength(0))
                .Select(index => new StaffFinderUser()
                {
                    SFID = 100 + index,
                    WinUser = $"ISRM\\{names[index, 0][0]}{names[index, 1]}",
                    FirstName = names[index, 0],
                    Email = $"{names[index, 0].ToLower()}.{names[index, 1].ToLower()}@isrm.tech",
                    LastName = names[index, 1],
                    Shareholder = false,
                    ActiveUser = true,
                    Human = false,
                }).ToArray();
        }
        private static AdminUser?[] GenerateAdminUsers() => [];
        
        private static HRUser?[] GenerateHRUsers() => [];
        
        private readonly AdminUser[] adminUsers = [
            new() { ADMIN_ID = 10, ACTIVE_USER = true, ADMIN_SFID = 501, EMAIL = "habeeb@isrm.tech", FULL_NAME = "Habeeb Surooprajally", WINUSER = "ISRM\\HSurooprajally" },
            new() { ADMIN_ID = 11, ACTIVE_USER = true, ADMIN_SFID = 502, EMAIL = "andras@isrm.tech", FULL_NAME = "Andras CSaki", WINUSER = "ISRM\\ACsaki" },
            new() { ADMIN_ID = 12, ACTIVE_USER = true, ADMIN_SFID = 500, EMAIL = "noel@isrm.tech", FULL_NAME = "Noel Rhima", WINUSER = "ISRM\\NRhima" }
        ];
        private readonly StaffFinderUser[] staffFinderUsers = [
            new() { ActiveUser = true, Email = "noel@isrm.tech", FirstName = "Noel", LastName = "Rhima", SFID = 500, WinUser = "ISRM\\NRhima", Shareholder = true },
            new() { ActiveUser = true, Email = "joseph@hseven.io", FirstName = "Joseph", LastName = null, SFID = 601, WinUser = "ISRM\\Joseph", Shareholder = true},
            new() { ActiveUser = true, Email = "andras@isrm.tech", FirstName = "Andras", LastName = "Csaki", SFID = 502, WinUser = "ISRM\\ACsaki", Shareholder = true},
            new() { ActiveUser = true, Email = "ceo@peterjonas.co.uk", FirstName = "Peter", LastName = "Jonas", SFID = 601, WinUser = "ISRM\\PJonas", Shareholder = true },
            new() { ActiveUser = true, Email = "habeeb@isrm.tech", FirstName = "Habeeb", LastName = "Surooprajally", SFID = 501, WinUser = "ISRM\\HSurooprajally", Shareholder = true}                
        ];
        private readonly HRUser[] hrUsers = [
            new() { ACTIVE_USER = true, EMAIL = "jordan.barrett@isrm.tech", FULL_NAME = "Jordan Barrett", HR_SFID = 401, WINUSER = "ISRM\\JBarrett" },
            new() { ACTIVE_USER = true, EMAIL = "kylie.jenner@isrm.tech", FULL_NAME = "Kylie Jenner", HR_SFID = 402, WINUSER = "ISRM\\KJenner" },
            new() { ACTIVE_USER = true, EMAIL = "john.smith@isrm.tech", FULL_NAME = "John Smith", HR_SFID = 400, WINUSER = "ISRM\\JSmith" }
        ];
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

            //[...]
        };
    }
}
