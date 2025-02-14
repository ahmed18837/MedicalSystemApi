using MedicalSystemApi.Data;
using MedicalSystemApi.Exceptions;
using MedicalSystemApi.Helpers;
using MedicalSystemApi.Mapping;
using MedicalSystemApi.Models.Entities;
using MedicalSystemApi.Repository;
using MedicalSystemApi.Repository.Implement;
using MedicalSystemApi.Repository.Interfaces;
using MedicalSystemApi.Services.Implements;
using MedicalSystemApi.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// To Work Patch Action

// To 2FACode
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// Register DI for Connection String
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// To Mapping appSetting To JWT
builder.Services.Configure<JWT>(builder.Configuration.GetSection(("Jwt")));

// Add Identity services
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true; //  Ì ÿ·» —ﬁ„
    options.Password.RequireLowercase = false; // ? ·« Ì ÿ·» Õ—›« ’€Ì—«
    options.Password.RequireUppercase = false; // ? ·« Ì ÿ·» Õ—›« ﬂ»Ì—«
    options.Password.RequireNonAlphanumeric = false; // ? ·« Ì ÿ·» —„“« Œ«’« (@, #, !)
    options.Password.RequiredLength = 8; // «·Õœ «·√œ‰Ï ·ÿÊ· ﬂ·„… «·„—Ê— (Ì„ﬂ‰ﬂ  €ÌÌ—Â)
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();



builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false;
        o.SaveToken = false;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddControllers();

// Add Swagger configuration for versioning and JWT support
builder.Services.AddSwaggerGen(options =>
{
    // Swagger setup for multiple API versions
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Medical System API",
        Version = "v1",
        Description = "API documentation for Version 1"
    });
    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "Medical System API",
        Version = "v2",
        Description = "API documentation for Version 2"
    });

    options.SwaggerDoc("v3", new OpenApiInfo
    {
        Title = "Medical System API",
        Version = "v3",
        Description = "API documentation for Version 3"
    });
    // Adding Bearer Token to Swagger 
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Description = "Enter the JWT token without 'Bearer'"
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
                },
                Scheme = "Oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});


builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddScoped<IStaffRepository, StaffRepository>();
builder.Services.AddScoped<IStaffService, StaffService>();

builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IPatientService, PatientService>();

builder.Services.AddScoped<IMedicationRepository, MedicationRepository>();
builder.Services.AddScoped<IMedicationService, MedicationService>();

builder.Services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();
builder.Services.AddScoped<IMedicalRecordService, MedicalRecordService>();

builder.Services.AddScoped<IMedicalTestRepository, MedicalTestRepository>();
builder.Services.AddScoped<IMedicalTestService, MedicalTestService>();

builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IDoctorService, DoctorService>();

builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();

builder.Services.AddScoped<IBillRepository, BillRepository>();
builder.Services.AddScoped<IBillService, BillService>();

builder.Services.AddScoped<IBillMedicalTestRepository, BillMedicalTestRepository>();
builder.Services.AddScoped<IBillMedicalTestService, BillMedicalTestService>();

builder.Services.AddScoped<IBillItemRepository, BillItemRepository>();
builder.Services.AddScoped<IBillItemService, BillItemService>();

builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();


builder.Services.AddScoped<IEmailService, EmailService>();


// Register AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddTransient<IFileService, FileService>();

builder.Services.AddSingleton<IWebHostEnvironment>(builder.Environment);



builder.Services.AddMemoryCache();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddAuthorization();


var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>(); //  ›⁄Ì· ExceptionMiddleware

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1");
    c.SwaggerEndpoint("/swagger/v2/swagger.json", "V2");
    c.SwaggerEndpoint("/swagger/v3/swagger.json", "V3");

    c.EnableDeepLinking();
});


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
