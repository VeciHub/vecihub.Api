global using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using VeciHub.IAM.Application.Internal.CommandServices;
using VeciHub.IAM.Application.Internal.QueryServices;
using VeciHub.IAM.Domain.Repositories;
using VeciHub.IAM.Infrastructure.Persistence.EFC;
using VeciHub.IAM.Infrastructure.Tokens.JWT.Services;
using VeciHub.Shared.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Configuración de la conexión a MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (connectionString == null) throw new InvalidOperationException("Connection string not found.");

// Usa Pomelo y define versión MySQL (ajusta si tu versión es distinta)
var serverVersion = new MySqlServerVersion(new Version(8, 0, 42)); // <-- Cambia si tu MySQL es otra versión

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    if (builder.Environment.IsDevelopment())
        options.UseMySql(connectionString, serverVersion)
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
    else if (builder.Environment.IsProduction())
        options.UseMySql(connectionString, serverVersion)
            .LogTo(Console.WriteLine, LogLevel.Error);
});

// Configuración JWT
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// CORS (habilitar frontend desde Azure Static Web Apps)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UserCommandService>();
builder.Services.AddSingleton<TokenService>();
builder.Services.AddScoped<IUserQueryRepository, UserQueryRepository>();
builder.Services.AddScoped<UserQueryService>();

// Swagger con soporte para JWT
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "VeciHub API", Version = "v1" });

    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Description = "Introduce tu token JWT aquí",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

Console.WriteLine("CONNECTION: " + connectionString);

var app = builder.Build();

// 🚀 Asegura creación de base de datos si no existe (opcional pero útil en dev)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
}

// Middleware
app.UseCors("AllowAll");
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
