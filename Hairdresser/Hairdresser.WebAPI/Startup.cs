using Hairdresser.Core.Application.Implementation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Hairdresser.Core.Application.Interfaces;
using Hairdresser.Core.Domain.Interfaces;
using Hairdresser.Core.Entities;
using Hairdresser.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Hairdresser.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Hairdresser.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Create a byte array with random values. This byte array is used
            // to generate a key for signing JWT tokens.
            byte[] secretBytes = new byte[40];
            var rand = new Random();
            rand.NextBytes(secretBytes);

            // Add JWT based authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretBytes),
                    ValidateLifetime = true, // validate the expiration and not before values in the token
                    ClockSkew = TimeSpan.FromMinutes(5) // 5 minute tolerance for the expiration date
                };
            });

            string mySqlConnectionStr = Configuration.GetConnectionString("DefaultConnection");
            //services.AddDbContextPool<NanoDBContext>(options => options.UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr)).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
            services.AddDbContext<HairdresserDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddScoped<ICRUDService<Treatment>, TreatmentService>();
            services.AddScoped<ICrudRepository<Treatment>, TreatmentRepository>();

            services.AddScoped<ICRUDService<Appointment>, AppointmentService>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<AppointmentService, AppointmentService>();

            services.AddDistributedMemoryCache();

            services.AddSingleton<ILoginService>(new LoginService(secretBytes));

            // obs: Change after deployment to allow frontend URL
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                    .WithOrigins("http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .Build();
                });
            });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
