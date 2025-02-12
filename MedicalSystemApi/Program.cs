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
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// To Work Patch Action
builder.Services.AddControllers().AddNewtonsoftJson();

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
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

//builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("EmailConfiguration"));

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
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>(); //  ›⁄Ì· ExceptionMiddleware

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
