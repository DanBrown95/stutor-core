using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Okta.AspNetCore;
using Stripe;
using stutor_core.Services;
using stutor_core.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using stutor_core.Configurations;
using stutor_core.Database;
using Microsoft.EntityFrameworkCore;
using GraphiQl;

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
                        builder.WithOrigins("http://localhost:8080").WithHeaders("Authorization","content-type");
                    });
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = OktaDefaults.ApiAuthenticationScheme;
                options.DefaultChallengeScheme = OktaDefaults.ApiAuthenticationScheme;
                options.DefaultSignInScheme = OktaDefaults.ApiAuthenticationScheme;
            })
            .AddOktaWebApi(new OktaWebApiOptions()
            {
                OktaDomain = "https://dev-870310.okta.com"
            });

            SMSSettings smsSettings = new SMSSettings();
            Configuration.GetSection("SMSSettings").Bind(smsSettings);
            services.AddSingleton<SMSSettings>(smsSettings);

            // ... the rest of ConfigureServices
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddMvc().AddJsonOptions(
                options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            services.AddMvc();
            
            //Dependency injection code
            services.AddTransient<IEmailService, EmailService>();

            // Adding entity framework and sql connection
            services.AddEntityFrameworkSqlServer()
                .AddDbContext<ApplicationDbContext>
            (option => option.UseSqlServer(Configuration["Database:connection"]));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // This is your real test secret API key.
            StripeConfiguration.ApiKey = "sk_test_622paREJpij1vZUp46UCQQ280043PjyrRF";

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
            app.UseGraphiQl("/graphql");
            app.UseMvc();
        }
    }
}
