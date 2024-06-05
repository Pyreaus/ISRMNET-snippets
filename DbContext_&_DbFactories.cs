using Microsoft.EntityFrameworkCore;
using ISRM.isrmnet.Model.POCOs.Entities;
/// <summary><para>
///  EF Core expects this class to always reside in DAL.dll, circumvent this by:
///  Explicitly specifying the assembly containing this class when performing migrations: (dotnet ef migrations add <MigrationName> --startup-project <RelativePathToAssembly>)
///  --OR--  Implementing the <see cref="IDesignTimeDbContextFactory{ISRMNETContext}"/> interface in <c>ISRMNETDbFactory</c> or in a seperate class.  
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
            modelBuilder.Entity<StaffFinderUser>().HasData(staffFinderUsers, GenerateSFUsers());      
            modelBuilder.Entity<AdminUser>().HasData(adminUsers, GenerateAdminUsers());
            modelBuilder.Entity<HRUser>().HasData(hrUsers);
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
                }).ToArray();
        }
        private readonly AdminUser[] adminUsers = [
            new() { ACTIVE_USER = true, ADMIN_SFID = 501, EMAIL = "john.smith@isrm.tech", FULL_NAME = "John Smith", WINUSER = "ISRM\\JSmith" } ]
        private readonly StaffFinderUser[] staffFinderUsers = [
            new() { ActiveUser = true, Email = "john.smith@isrm.tech", FirstName = "John", LastName = "Smith", SFID = 400, WinUser = "ISRM\\JSmith" }
        ];

        static readonly string[,] names = new string[,]
        {
            {"John", "Doe"},
            {"Jane", "Smith"},
            {"Michael", "Johnson"},
            {"Emily", "Brown"},
            //[...]
        }
    }
}
    namespace ISRM.isrmnet.DAL.Factories;
    public sealed class ISRMNETDbFactory : Disposable, IDesignTimeDbContextFactory<ISRMNETContext>, IDbFactory<ISRMNETContext>
    {
        ISRMNETContext? ctx;
        private readonly ILogger<ISRMNETDbFactory> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration; 
        public ISRMNETDbFactory() {}
        public ISRMNETDbFactory(ILogger<ISRMNETDbFactory> logger, IConfiguration configuration, IWebHostEnvironment environment)
        {
            (_configuration, _environment, _logger) = ( configuration, environment, logger);
        }
        protected override void DisposeCore() => ctx?.Dispose(); //dispose if null
        public ISRMNETContext Init() //initialize DbContext 
        {
            var optionsBuilder = new DbContextOptionsBuilder<ISRMNETContext>();
            var connectionString = _configuration.GetConnectionString("ISRMNETConnection");
            
            optionsBuilder.UseSqlServer(connectionString);

            if (_environment.IsDevelopment())
            {
                _logger.LogWarning("Using DEV connection");
            }
            else _logger.LogWarning("Using PROD connection");

            return ctx ??= new ISRMNETContext(optionsBuilder.Options);
        }
        /// <summary>
        /// Implementation of <see cref="IDesignTimeDbContextFactory"/>
        /// </summary>
        public ISRMNETContext CreateDbContext(string[] args)  
        {   
            var optionsBuilder = new DbContextOptionsBuilder<ISRMNETContext>();
            var connectionString = _configuration.GetConnectionString("ISRMNETConnection");
            
            optionsBuilder.UseSqlServer(connectionString);

            return new ISRMNETContext(optionsBuilder.Options);   
        }
    }
