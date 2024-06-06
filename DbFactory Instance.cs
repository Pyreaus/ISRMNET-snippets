namespace ISRM.isrmnet.DAL.Factories
{   /// <summary><para>
    /// DbContextfactory containing logic for initalize <see cref="TRACRContext"/> instances when the <c>Init()</c> method is invoked
    /// e.g. inject using <see cref="IDbFactory{ISRMNETContext}"/> in <c>RepositoryBase</c> or <see cref="TRACRWorkUnit"/>.</para>

    /// <para>EF Core expects a DbContext class in the same assembly (DAL.dll) when adding/updating migrations.
    /// <see cref="IDesignTimeDbContextFactory"/> must be implemented somewhere to facillitate EF Core's 
    /// design-time creation of <see cref="ISRMNETContext"/> instance when adding/updating migrations.
    /// </para></summary>
    public sealed class ISRMNETDbFactory : Disposable,
    IDbFactory<ISRMNETContext>, IDesignTimeDbContextFactory<ISRMNETContext>
    {
        private readonly IConfiguration _configuration; 
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<ISRMNETDbFactory> _logger;
        ISRMNETContext? ctx;
        public ISRMNETDbFactory(ILogger<ISRMNETDbFactory> logger, IConfiguration configuration, IWebHostEnvironment environment)
        {
            (_configuration, _environment, _logger) = ( configuration, environment, logger);
        }
        public ISRMNETDbFactory() {}
        protected override void DisposeCore() => ctx?.Dispose(); 
        public ISRMNETContext Init() //initialize DbContext 
        {
            var optionsBuilder = new DbContextOptionsBuilder<ISRMNETContext>();
            var connectionString = _configuration.GetConnectionString("ISRMNETConnection");
            
            optionsBuilder.UseSqlServer(connectionString, x => x.MigrationsAssembly("ISRM.isrmnet.DAL"));

            if (_environment.IsDevelopment())
            {
                _logger.LogWarning("Using DEV connection");
            }
            else _logger.LogWarning("Using PROD connection");

            return ctx ??= new ISRMNETContext(optionsBuilder.Options, _environment.IsDevelopment());
        }
        public ISRMNETContext CreateDbContext(string[] args)  
        {   
            var configPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "ISRM.isrmnet.API", "appsettings.Development.json");
            
            IConfigurationRoot localBuild = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(configPath, optional: false, reloadOnChange: true)
                .AddJsonFile(
                    configPath.Replace(
                     "appsettings.Development.json", 
                     $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json"), optional: true) 
                .AddUserSecrets("937bd283-7c13-46cd-b6ac-a9d01c9d6d75")
                .AddEnvironmentVariables()
                .Build();
            
            var optionsBuilder = new DbContextOptionsBuilder<ISRMNETContext>();
            var connectionString = localBuild.GetSection("ConnectionStrings")["ISRMNETConnection"];

            optionsBuilder.UseSqlServer(connectionString, x => x.MigrationsAssembly("ISRM.isrmnet.DAL"));
            if (string.IsNullOrWhiteSpace(connectionString) || connectionString == "secret")
            {
                throw new Exception("Connection string couldn't be found or is still set to 'secret'");
            }
            return new ISRMNETContext(optionsBuilder.Options);   
        }
    }
}

