using Microsoft.EntityFrameworkCore;
using PopularMovieCatalogBackend.APIBehavior;
using PopularMovieCatalogBackend.Filter;
using PopularMovieCatalogBackend.Filters;

namespace PopularMovieCatalogBackend
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // database connection Initialised
            services.AddDbContext<ApplicationDbContext>( options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


            //Custom Excepcion Filters
            services.AddControllers(option =>
            {
                option.Filters.Add(typeof(MyExceptionFilter));
                option.Filters.Add(typeof(ParseBadRequest));

            }).ConfigureApiBehaviorOptions(BadRequestBehavior.Parse);

            //Jwt Authentication
           // services.AddAuthentication(JwtBearerDefaults.AuthenticateScheme).AddJwtBearer();

            // Action filter is added
            services.AddTransient<MyExceptionFilter>();

            // Automapper for the database entities mapping with DTOS
            services.AddAutoMapper(typeof(Startup));

            // Add services to the container.
            services.AddControllers();
            services.AddSwaggerGen( c =>
            {
                c.SwaggerDoc("v1", new() { Title = " PopularMovieCatalogBackend", Version = "v1" });
            });

            // Adding Cors policy to the system
            services.AddCors(options =>
            {
                var frontendURL = Configuration.GetValue<string>("frontEnd_url");
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins(frontendURL).AllowAnyMethod().AllowAnyHeader();
                });
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
            app.UseRouting();
            app.UseCors();
            app.UseAuthorization();
            app.UseAuthentication();    
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseAuthorization();

        }

    }
}
