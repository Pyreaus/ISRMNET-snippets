namespace ISRM.isrmnet.DAL.Factories
{   /// <summary><para>
    /// This DbContextfactory contains logic to initalize a <see cref="TRACRContext"/> instance when the <c>Init()</c> method is invoked
    /// e.g. inject this into <see cref="IDbFactory{ISRMNETContext}"/> in <c>RepositoryBase</c> or <see cref="TRACRWorkUnit"/>.
    /// </para><para>
    /// EF Core expects a DbContext class in the same assembly (DAL.dll) when adding/updating migrations.
    /// <see cref="IDesignTimeDbContextFactory"/> must be implemented somewhere to facillitate EF Core's 
    /// design-time creation of <see cref="ISRMNETContext"/> instance when adding/updating migrations. 
    /// </para></summary>

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
            var optionsBuilder = new DbContextOptionsBuilder<ISRMNETContext>();
            var connectionString = _configuration.GetConnectionString("ISRMNETConnection");
            
            optionsBuilder.UseSqlServer(connectionString, x => x.MigrationsAssembly("ISRM.isrmnet.DAL"));

            return new ISRMNETContext(optionsBuilder.Options);   
        }
    }
}

