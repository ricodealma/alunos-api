using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Alunos.Api.App.Extensions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Configure CORS with allowed origins
var allowedOrigins = builder.Configuration["AllowedOrigins"]?.Split(',') ?? new[] { "*" };
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        if (allowedOrigins.Contains("*"))
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        }
        else
        {
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        }
    });
});

// Add JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomServices(builder.Configuration);
builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Alunos Microservice API",
        Description = @"API RESTful para gerenciamento de alunos.

**Recursos:**
- CRUD completo de alunos
- Busca com filtros (nome, email, série)
- Paginação de resultados
- Validação de dados

**Tecnologias:**
- .NET 9.0
- Entity Framework Core
- PostgreSQL
- Swagger/OpenAPI",
        Contact = new OpenApiContact
        {
            Name = "API Support",
            Email = "support@alunos-api.com"
        }
    });

    // Incluir comentários XML
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }

    // Incluir comentários XML do Domain
    var domainXmlFile = "Alunos.Api.Domain.xml";
    var domainXmlPath = Path.Combine(AppContext.BaseDirectory, domainXmlFile);
    if (File.Exists(domainXmlPath))
    {
        options.IncludeXmlComments(domainXmlPath);
    }
});

var app = builder.Build();

app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI(config =>
{
    config.SwaggerEndpoint("/swagger/v1/swagger.json", "Alunos Microservice");
    config.RoutePrefix = string.Empty;
});
app.UseHttpsRedirection();

app.FillEnvironmentVariables(app.Configuration);
app.AddEndpoints();
app.Run();
