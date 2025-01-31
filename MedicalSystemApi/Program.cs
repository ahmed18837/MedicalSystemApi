using MedicalSystemApi.Data;
using MedicalSystemApi.Exceptions;
using MedicalSystemApi.Mapping;
using MedicalSystemApi.Repository;
using MedicalSystemApi.Repository.Implement;
using MedicalSystemApi.Repository.Interfaces;
using MedicalSystemApi.Services.Implements;
using MedicalSystemApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// To Work Patch Action
builder.Services.AddControllers().AddNewtonsoftJson();

// Register DI for Connection String
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
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
// Register AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));





builder.Services.AddMemoryCache();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>(); // ÊÝÚíá ExceptionMiddleware

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
