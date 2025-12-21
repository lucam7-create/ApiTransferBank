using ApiTransferBank.Data;
using ApiTransferBank.Repository;
using ApiTransferBank.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// -------------------------------------------------------------------------
// 1. CONFIGURACIÓN DE LA BASE DE DATOS (PostgreSQL)
// -------------------------------------------------------------------------
builder.Services.AddDbContext<ContextoBanco>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ConexionPostgres")));

// -------------------------------------------------------------------------
// 2. INYECCIÓN DE DEPENDENCIAS (Repositorios y Servicios)
// -------------------------------------------------------------------------
builder.Services.AddScoped<IRepositorioBancario, RepositorioBancario>();
builder.Services.AddScoped<IServicioTransferencia, ServicioTransferencia>();

// -------------------------------------------------------------------------
// 3. CONFIGURACIÓN DE AUTENTICACIÓN JWT
// -------------------------------------------------------------------------
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"] ?? "Clave_Super_Secreta_De_Seguridad_2025_Bank");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// -------------------------------------------------------------------------
// 4. CONFIGURACIÓN DE SWAGGER (Para soportar el botón de Autorizar con JWT)
// -------------------------------------------------------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiTransferBank", Version = "v1" });

    // Configura el botón "Authorize" en la interfaz de Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingresa el token JWT así: Bearer {tu_token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// -------------------------------------------------------------------------
// 5. MIDDLEWARES (Orden de ejecución)
// -------------------------------------------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();