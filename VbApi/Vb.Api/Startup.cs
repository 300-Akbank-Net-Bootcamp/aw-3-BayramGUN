
using System.Reflection;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Vb.Data;
using Vb.Business.Cqrs;
using Vb.Business.Mapper;
using Vb.Business.Validator;
using FluentValidation;
using Microsoft.Data.SqlClient;

namespace VbApi;

public class Startup
{
    public IConfiguration Configuration;

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        var sqlConnectionStringBuilder = new SqlConnectionStringBuilder(
            Configuration.GetConnectionString("MsSqlConnection")!)
        {
            // Db password comes from user-secrets 
            Password = Configuration["MsSQLDbPassword"]!
        };
        string connection = sqlConnectionStringBuilder.ConnectionString;
        services.AddDbContext<VbDbContext>(options => options.UseSqlServer(connection));
        //services.AddDbContext<VbDbContext>(options => options.UseNpgsql(connection));
        
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateCustomerCommand).GetTypeInfo().Assembly));

        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MapperConfig()));
        services.AddSingleton(mapperConfig.CreateMapper());

        // services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddControllers().AddFluentValidation(x =>
        {
            x.RegisterValidatorsFromAssemblyContaining<CreateCustomerValidator>();
        });
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
    
    public void Configure(IApplicationBuilder app,IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(x => { x.MapControllers(); });
    }
}
