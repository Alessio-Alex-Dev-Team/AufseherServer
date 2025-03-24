using System.Reflection;
using AufseherServer.Data.v1;
using AufseherServer.Infrastructure;
using AufseherServer.Infrastructure.v1;
using AufseherServer.Services;

namespace AufseherServer
{
	public class Startup(IConfiguration configuration)
	{

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<MongoDbService>(_ => new MongoDbService(configuration["MongoDB:ConnectionString"]));
			

			Assembly assembly = typeof(Program).Assembly;
			IEnumerable<Type> collectionServices = assembly.GetTypes()
				.Where(type => type is { IsClass: true, IsAbstract: false } && typeof(IService).IsAssignableFrom(type));

			foreach (Type service in collectionServices)
			{
				Type? interfaceType = service.GetInterfaces().FirstOrDefault(i => i.Name == $"I{service.Name}");

				if (interfaceType != null)
				{
					services.AddScoped(interfaceType, service);
				}
				else if (service.Name.EndsWith("Service"))
				{
					services.AddScoped(service);
				}
			}

			services.AddSingleton<Settings>();
			
			
			services.AddCors(options =>
			{
				options.AddPolicy("AllowAll",
					builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
			});

			services.AddControllersWithViews();
			services.AddSwaggerGen();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			//app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();
			app.UseSwaggerAuthorization();
			app.UseSwagger();
			app.UseSwaggerUI(options =>
			{
				options.RoutePrefix = string.Empty;
				options.SwaggerEndpoint("/swagger/v1/swagger.json", "AufseherServer");
			});
			app.UseAuthorization();


			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					"default",
					"{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}