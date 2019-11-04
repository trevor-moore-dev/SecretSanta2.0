using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using SecretSanta2._0.Models.DB;
using SecretSanta2._0.Models.DTO;
using SecretSanta2._0.Services.Business;
using SecretSanta2._0.Services.Business.Interfaces;
using SecretSanta2._0.Services.Data;
using SecretSanta2._0.Services.Data.Interfaces;
using SecretSanta2._0.Services.Hubs;
using System.Text;

namespace SecretSanta2._0
{
	public class Startup
	{
		public Startup(IWebHostEnvironment env)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
				.AddEnvironmentVariables();
			Configuration = builder.Build();
			CurrentEnvironment = env;
		}

		public IConfiguration Configuration { get; }
		private IWebHostEnvironment CurrentEnvironment { get; set; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddCors(options =>
			{
				options.AddDefaultPolicy(
					policy =>
					{
						policy.AllowAnyHeader();
						policy.AllowAnyMethod();
						policy.SetIsOriginAllowed((host) => true);
						policy.AllowCredentials();
					});
			});

			services.AddControllersWithViews();

			services.AddAuthentication()
				.AddJwtBearer(cfg =>
				{
					cfg.RequireHttpsMetadata = false;
					cfg.SaveToken = true;

					cfg.TokenValidationParameters = new TokenValidationParameters()
					{
						ValidIssuer = Configuration["AppSettings:AppDomain"],
						ValidAudiences = new[] { Configuration["AppSettings:AppAudience"] },
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["AppSettings:JwtSecret"])),
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateIssuerSigningKey = true
					};
				});

			services.Configure<CookiePolicyOptions>(options =>
			{
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});

            services.AddSignalR();

            if (CurrentEnvironment.IsDevelopment())
			{
				services.AddSingleton<IDAO<Participant, ParticipantDTO>>(service => new SantaDAO(
					Configuration["ConnectionStrings:LocalMongoDBConnection"],
					Configuration["ConnectionStrings:LocalMongoDBDatabase"],
					Configuration["ConnectionStrings:LocalMongoDBCollection"]));
			}
			else
			{
				services.AddSingleton<IDAO<Participant, ParticipantDTO>>(service => new SantaDAO(
					Configuration["ConnectionStrings:HerokuMongoDBConnection"],
					Configuration["ConnectionStrings:HerokuMongoDBDatabase"],
					Configuration["ConnectionStrings:HerokuMongoDBCollection"]));
			}

			services.AddSingleton<ISantaService, SantaService>();
			services.AddSingleton<IAuthenticationService, AuthenticationService>();
			
			services.AddSpaStaticFiles(configuration =>
			{
				configuration.RootPath = "ClientApp/build";
			});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseSpaStaticFiles();
			app.UseCors();
			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();
			app.UseCookiePolicy();

            app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapHub<SantaHub>("/SantaHub");
            });

			app.UseSpa(spa =>
			{
				spa.Options.SourcePath = "ClientApp";

				if (env.IsDevelopment())
				{
					spa.UseReactDevelopmentServer(npmScript: "start");
				}
			});
		}
	}
}
