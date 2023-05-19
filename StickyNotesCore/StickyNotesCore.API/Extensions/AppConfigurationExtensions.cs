using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StickyNotesCore.API.Controllers.Configuration;
using StickyNotesCore.API.Domain.Data.Contexts;
using System.Reflection;
using System.Text.Json.Serialization;

namespace StickyNotesCore.API.Extensions
{
	public static class AppConfigurationExtensions
	{
		public const string CORS_POLICY = "ApiCorsPolicy";

		public static void AddApiDependencies(this IServiceCollection services, IConfiguration configuration)
		{
			// Configures the standar error response in case of failures.
			services.AddControllersWithViews().ConfigureApiBehaviorOptions(option =>
			{
				option.InvalidModelStateResponseFactory = InvalidModelStateResponseFactory.ProduceErrorResponse;
			}).AddJsonOptions(options =>
			{
				options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
			});

			// Swagger Documentation.
			services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "Sticky Notes API",
					Version = "v1",
					Description = "RESTful API for the Sticky Notes App",
					Contact = new OpenApiContact
					{
						Name = "Evandro Gomes",
						Url = new Uri("https://evgomes.github.io/"),
						Email = "evandro.ggomes@outlook.com"
					},
				});

				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				options.IncludeXmlComments(xmlPath);
			});

			// CORS options
			var uiBaseAddress = configuration.GetValue<string>("CORS:ClientBaseAddress") ?? throw new ArgumentNullException("cors");
			services.AddCors(options =>
			{
				options.AddPolicy(CORS_POLICY, policy =>
				{
					policy.WithOrigins(uiBaseAddress).AllowAnyMethod().AllowAnyHeader();
				});
			});
		}

		public static void AddDatabaseDependencies(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<StickyNotesContext>(options =>
			{
				options.UseSqlServer(configuration.GetConnectionString("StickyNotes"));
			});
		}

		public static void AddInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
		{
			// MediatR
			services.AddMediatR(options => options.RegisterServicesFromAssemblyContaining<Program>());

			// AutoMapper
			services.AddAutoMapper(options => options.AllowNullCollections = true, typeof(Program).Assembly);
		}

		public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
		{
		}

		public static void ApplyPendingMigrations(this WebApplication? app)
		{
			if (app == null)
			{
				throw new ArgumentNullException(nameof(app));
			}

			// NOTE: do not use this approach in applications that runs in more than one instance.
			using (var scope = app.Services.CreateScope())
			{
				var context = scope.ServiceProvider.GetRequiredService<StickyNotesContext>();
				if (context.Database.GetPendingMigrations().Any())
				{
					context.Database.Migrate();
				}
			}
		}

		public static void UseMiddlewares(this WebApplication? app)
		{
			if (app == null)
			{
				throw new ArgumentNullException(nameof(app));
			}

			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Error");
				app.UseHsts();
			}

			app.UseSwagger().UseSwaggerUI(options =>
			{
				options.SwaggerEndpoint("/swagger/v1/swagger.json", "Sticky Notes API");
				options.DocumentTitle = "Sticky Notes API";
			});

			app.UseCors(CORS_POLICY);

			if (!app.Environment.IsDevelopment())
			{
				app.UseHttpsRedirection();
			}

			app.UseStaticFiles();

			app.UseRouting();

			app.MapControllers();
		}
	}
}
