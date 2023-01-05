using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using PopularMovieCatalogBackend.APIBehavior;
using PopularMovieCatalogBackend.Filter;
using PopularMovieCatalogBackend.Filters;
using PopularMovieCatalogBackend.Helpers;
using PopularMovieCatalogBackend.Helpers.ImageInAzureStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace PopularMovieCatalogBackend
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            // Clear the ClaimType.email long string
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // database connection Initialised
            services.AddDbContext<ApplicationDbContext>( options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), 
            // For location and map
            sqlOptions => sqlOptions.UseNetTopologySuite()));


            //Custom Excepcion Filters 
            // Bad request behaviour 
            services.AddControllers(option =>
            {
                option.Filters.Add(typeof(MyExceptionFilter));
                option.Filters.Add(typeof(ParseBadRequest));

            }).ConfigureApiBehaviorOptions(BadRequestBehavior.Parse);

            // Action filter is added
            services.AddTransient<MyExceptionFilter>();

            // Add services to the container.
            services.AddControllers();
           
          
            // Adding Cors policy to the system
            services.AddCors(options =>
            {
                var frontendURL = Configuration.GetValue<string>("frontEnd_url");
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins(frontendURL).AllowAnyMethod().AllowAnyHeader()
                    .WithExposedHeaders(new string[] { "totalAmountOfRecords" });
                });
            });

            // Automapper for the database entities mapping with DTOS
            services.AddAutoMapper(typeof(Startup));          
            services.AddSingleton(provider => new MapperConfiguration(config =>
            {
            var geometryFactory = provider.GetRequiredService<GeometryFactory>();
            config.AddProfile(new AutoMapperProfiles(geometryFactory));
            }).CreateMapper());
            services.AddSingleton<GeometryFactory>(NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326));


            // Dependencis for AzureStorage Services
            services.AddScoped<IFileStorageServices, AzureStorageServices>();

            // For authentication and authoraization
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            //Jwt Authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer( options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"])),                      
                        ClockSkew = TimeSpan.Zero
                    };
                });

            //Claim based Authorization Admin or user
            services.AddAuthorization(options =>
            {
                options.AddPolicy("IsAdmin", policy => policy.RequireClaim("role", "admin"));
            });
           


            /*
            // Dependencis for LocalStorage Services
            services.AddScoped<IFileStorageServices, ImageStorageServices>();
            services.AddHttpContextAccessor();

            */
            // Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = " PopularMovieCatalogBackend", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app,  IWebHostEnvironment env, ILogger<Startup> logger)
        {
            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI( c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PopularMovieCatalogBackend v1"));
            }

            app.UseHttpsRedirection();
            // Dependencis for LocalStorage Services
            app.UseStaticFiles();
            app.UseRouting();            
            app.UseCors();
            app.UseAuthorization();
            app.UseAuthentication();    
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }

    }
}
