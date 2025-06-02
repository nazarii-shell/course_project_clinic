using BussinessLogic.Profiles;
using DataAccess.UoW;
using DomainData;
using DomainData.services;
using PresentationLayer.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// pevent json looping
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
    );
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ClinicContext>();
builder.Services.AddAutoMapper(
    typeof(UserProfile).Assembly,
    typeof(PatientProfile).Assembly,
    typeof(AppointmentProfile).Assembly,
    typeof(DoctorProfile).Assembly,
    typeof(ServiceProfile).Assembly
);


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PatientService>();
builder.Services.AddScoped<DoctorService>();
builder.Services.AddScoped<AppoinmentService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ServiceService>();

var app = builder.Build();

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