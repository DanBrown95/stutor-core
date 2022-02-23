using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Stripe;
using stutor_core.Services;
using stutor_core.Services.Interfaces;
using stutor_core.Configurations;
using stutor_core.Database;
using Microsoft.EntityFrameworkCore;
using Twilio;
using stutor_core.Repositories.Interfaces;
using stutor_core.Repositories;

namespace stutor_core
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:8080").WithHeaders("Authorization","Content-Type");
                    });
            });

            string domain = $"https://{Configuration["Auth0:domain"]}/"; // For authorizing with auth0 access tokens
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = domain;
                options.Audience = Configuration["Auth0:audience"];
            });

            StutorCoreM2MSettings stutorCoreM2mSettings = new StutorCoreM2MSettings();
            Configuration.GetSection("StutorCoreM2m").Bind(stutorCoreM2mSettings);
            services.AddSingleton<StutorCoreM2MSettings>(stutorCoreM2mSettings);

            TwilioClient.Init(Configuration["SMSSettings:accountSid"], Configuration["SMSSettings:authToken"]);
            services.Configure<SMSSettings>(Configuration.GetSection("SMSSettings"));

            AWSS3Settings awsS3Settings = new AWSS3Settings();
            Configuration.GetSection("AWSS3").Bind(awsS3Settings);
            services.AddSingleton<AWSS3Settings>(awsS3Settings);

            // ... the rest of ConfigureServices
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddMvc().AddJsonOptions(
                options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            services.AddMvc();
            
            //Dependency injection code
            services.AddTransient<IEmailService, EmailService>();

            var connection = Configuration["Database:amazonRDS"];

            // Adding entity framework and sql connection
            services.AddEntityFrameworkSqlServer()
                .AddDbContext<ApplicationDbContext>
            (option => option.UseSqlServer(connection));

            services.AddScoped<ICareerService, CareerService>();
            services.AddScoped<ICareerRepository, CareerRepository>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IDictionaryService, DictionaryService>();
            services.AddScoped<IDictionaryRepository, DictionaryRepository>();
            services.AddScoped<IExpertService, ExpertService>();
            services.AddScoped<IExpertRepository, ExpertRepository>();
            services.AddScoped<IOrderService, Services.OrderService>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<ITopicService, TopicService>();
            services.AddScoped<ITopicRepository, TopicRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // This is your real test secret API key.
            StripeConfiguration.ApiKey = Configuration["Stripe:testApiKey"];

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(MyAllowSpecificOrigins);
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
