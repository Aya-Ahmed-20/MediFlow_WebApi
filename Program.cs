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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace MediFlowApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Database & Identity
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            // 2. Authentication & JWT Bearer
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,

                    ValidIssuer = builder.Configuration["JWT:Issuer"] ?? "MediFlowApi",
                    ValidAudience = builder.Configuration["JWT:Audience"] ?? "MediFlowUsers",
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"] ?? "TemporarySuperLongSecretKeyForDevelopmentOnly2026!"))
                };
            });

            // 3. Authorization
            builder.Services.AddAuthorization();

            // 4. Application Services
            builder.Services.AddScoped<IMedicineServices, MedicineServices>();
            builder.Services.AddScoped<IConsultationsService, ConsultationService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<PrescriptionMapper>();
            builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();

            builder.Services.AddControllers();

            // 5. FluentValidation
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<MedicineCreateDtoValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<RegisterDtoValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<LoginValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<CreatePrescriptionItemDtoValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<CreatePrescriptionDtoValidator>();

            // 6. AutoMapper
            builder.Services.AddAutoMapper(options =>
            {
                options.AddProfile<MedicineProfile>();
            });

            // 7. API Versioning
            builder.Services.AddApiVersioning(option =>
            {
                option.DefaultApiVersion = new ApiVersion(1, 0);
                option.AssumeDefaultVersionWhenUnspecified = true;
            }).AddApiExplorer(option =>
            {
                option.GroupNameFormat = "'v'VVV";
                option.SubstituteApiVersionInUrl = true;
            });

            // 8. Swagger Configuration with JWT Support
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "MediFlow API",
                    Version = "v1",
                    Description = "API for MediFlow Healthcare Management System"
                });

                // JWT Bearer Token  Swagger UI
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "أدخلي الـ Token بالطريقة التالية: Bearer {your_token}"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            var app = builder.Build();

            // Middlewares Pipeline
            app.UseMiddleware<ExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MediFlow API v1");
                });
            }

            app.UseHttpsRedirection();

            // Authentication أولاً ثم Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}