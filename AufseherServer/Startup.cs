using AufseherServer.Services;
using AufseherServer.Services.MongoDB;
using MongoDB.Bson.Serialization;

namespace AufseherServer;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<MongoDbService>(sp =>
        {
            var connectionString = Configuration.GetSection("MongoDB:ConnectionString");
            
            return new MongoDbService(connectionString.Value);
        });

        var assembly = typeof(Program).Assembly;
        var collectionServices = assembly.GetTypes()
            .Where(type => type.IsClass && !type.IsAbstract && typeof(IService).IsAssignableFrom(type));

        foreach (var service in collectionServices)
        {
            var interfaceType = service.GetInterfaces().FirstOrDefault(i => i.Name == $"I{service.Name}");

            if (interfaceType != null)
            {
                services.AddScoped(interfaceType, service);
            }
        }
        
        services.AddScoped<LevelingDBService>();
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
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
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
    
}