using System.Reflection;
using Matrix.Core.Exceptions;
using Matrix.Core.Services;
using Matrix.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Matrix.Core.Extensions;

public static class ApiStartupExtensions{
    /// <summary>
    /// Add in the standard api features for the matrix api
    /// </summary>

    public static void AddStandardApiFeatures(this WebApplicationBuilder app)
    {
        app.AddSwaggerGen();
        
        //Add in the exception response so that we can throw exceptions inside services to return specific responses
        app.Services.AddControllers(options => options.Filters.Add<ExceptionResponseFilter>());

        app.Services.AddMatrixDb();
        app.Services.AddMapper();
        app.Services.AddStandardServices();
    }

    public static void AddMapper(this IServiceCollection services)
    {
        //We only want to add maps for our assemblies
        List<Assembly> wombatAssemblies =
            AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName?.Contains("Matrix") ?? false).ToList();
        services.AddAutoMapper(wombatAssemblies);
    }
    
    /// <summary>
    /// Add the standard services for the matrix api
    /// </summary>
    public static void AddStandardServices(this IServiceCollection services)
    {
        //Use reflection so we don't have to remember to add new services here, just inherit from base service
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        IEnumerable<Type> baseServiceTypes = assemblies.SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract && t.IsAssignableTo(typeof(BaseService)));
        foreach (Type serviceType in baseServiceTypes)
        {
            // Use try add to not accidentally duplicate anything added manually.
            services.TryAddScoped(serviceType);
        }
    }

    /// <summary>
    /// Add the matrix database connection
    /// </summary>
    public static void AddMatrixDb(this IServiceCollection services, string dbName = "MatrixDb")
    {
        //Use in memory database for simplicity - Could use sql server or sq lite here instead
        services.AddDbContext<MatrixDbContext>(options =>
            options.UseInMemoryDatabase(dbName));
    }

    /// <summary>
    /// Add in the default swagger for the matrix api
    /// </summary>
    public static void AddSwaggerGen(this WebApplicationBuilder builder)
    {
        Assembly apiAssembly = Assembly.GetEntryAssembly() ??
                               throw new NullReferenceException("Could not find a valid assembly.");

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.UseInlineDefinitionsForEnums();

            var xmlFilename = $"{apiAssembly.GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Bike Matrix",
                Version = "v1"
            });

        });
    }

    /// <summary>
    /// Adds the standard app features for the api
    /// </summary>
    public static void AddStandardAppFeatures(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.AddSwaggerUi();
        }

        app.UseCors(corsBuilder =>
        {
            corsBuilder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed(origin => true);
        });
        
        app.UseRouting();
        app.MapControllers();
    }
    
    /// <summary>
    /// Add the ui for swagger when running
    /// </summary>
    public static void AddSwaggerUi(this WebApplication app, string urlPrefix = "")
    {
        app.UseSwagger(options => { options.RouteTemplate = $"{urlPrefix}/swagger/{{documentname}}/swagger.json"; });
        app.UseSwaggerUI(options =>
        {
            options.EnablePersistAuthorization();
            options.DefaultModelsExpandDepth(0);
            options.DefaultModelRendering(ModelRendering.Example);
            options.EnableDeepLinking();
            options.DocExpansion(DocExpansion.None);
            options.ShowExtensions();
        });
    }

}