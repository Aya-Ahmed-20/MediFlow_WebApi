
using Asp.Versioning;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediFlowApi.Data;
using MediFlowApi.DTOs;
using MediFlowApi.Interfaces;
using MediFlowApi.Middlewares;
using MediFlowApi.Models;
using MediFlowApi.Profiles;
using MediFlowApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MediFlowApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddScoped<IMedicineServices, MedicineServices>();
            builder.Services.AddControllers();
            //Add Fluent Validators
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<MedicineCreateDtoValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<RegisterDtoValidator>();

            builder.Services.AddAutoMapper(options =>
            {
                options.AddProfile<MedicineProfile>();
            });
            //Recording Authentication service
            builder.Services.AddScoped<IAuthService, AuthService>();
            //Add UserIdentity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()

             .AddEntityFrameworkStores<AppDbContext>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            //Adding Versioning
            builder.Services.AddApiVersioning(option =>
            {
                option.DefaultApiVersion = new ApiVersion(1, 0);
                option.AssumeDefaultVersionWhenUnspecified = true;
            }).AddApiExplorer(option =>
            {
                option.GroupNameFormat = "'v'VVV";
                option.SubstituteApiVersionInUrl=true;
            }
             );
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMiddleware<ResponseTimeMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
