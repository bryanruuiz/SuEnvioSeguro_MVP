using SuEnvioSeguro.API.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using SuEnvioSeguro.API.Services;
using SuEnvioSeguro.API.Exceptions;
using SuEnvioSeguro.API.Services.Strategies;
using SuEnvioSeguro.API.Services.Factories;
using SuEnvioSeguro.API.Services.Facades;
using Microsoft.AspNetCore.Http;
using SuEnvioSeguro.API.Services.Singletons;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();

// Agregar Swagger UI
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando Bearer. Ejemplo: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(_ => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer", null, null),
            new List<string>()
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("WebApp", policy => policy
        .WithOrigins("http://localhost:5173", "http://127.0.0.1:5173")
        .AllowAnyHeader()
        .AllowAnyMethod());
});

// Database Configuration
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar servicios de negocio
builder.Services.AddScoped<ServicioCriptografia>();
builder.Services.AddScoped<JwtTokenService>();
builder.Services.AddScoped<CalculadoraTarifas>();
builder.Services.AddScoped<EnvioFactory>();
builder.Services.AddScoped<FacturacionFacade>();
builder.Services.AddSingleton(_ => GeneradorCodigoFactura.ObtenerInstancia());

// Registrar estrategias de tarifa (Strategy Pattern)
builder.Services.AddScoped<ITarifaMunicipioStrategy, EstrategiaTarifaMedellin>();
builder.Services.AddScoped<ITarifaMunicipioStrategy, EstrategiaTarifaEnvigado>();
builder.Services.AddScoped<ITarifaMunicipioStrategy, EstrategiaTarifaItagui>();
builder.Services.AddScoped<ITarifaMunicipioStrategy, EstrategiaTarifaSabaneta>();
builder.Services.AddScoped<ITarifaMunicipioStrategy, EstrategiaTarifaBello>();
builder.Services.AddScoped<ITarifaMunicipioStrategy, EstrategiaTarifaCaldas>();
builder.Services.AddScoped<ITarifaMunicipioStrategy, EstrategiaTarifaLaEstrella>();
builder.Services.AddScoped<ITarifaMunicipioStrategy, EstrategiaTarifaCopacabana>();
builder.Services.AddScoped<ITarifaMunicipioStrategy, EstrategiaTarifaGirardota>();
builder.Services.AddScoped<ITarifaMunicipioStrategy, EstrategiaTarifaBarbosa>();

// Agregar controladores
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Title = "La solicitud contiene datos inválidos.",
                Status = StatusCodes.Status400BadRequest,
                Type = "https://httpstatuses.com/400",
                Instance = context.HttpContext.Request.Path
            };

            return new BadRequestObjectResult(problemDetails);
        };
    });

var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("La clave Jwt:Key no está configurada.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.FromMinutes(2)
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseExceptionHandler(exceptionApp =>
{
    exceptionApp.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        var problemDetails = exception switch
        {
            ApiException apiException => new ProblemDetails
            {
                Title = apiException.Title,
                Detail = apiException.Message,
                Status = apiException.StatusCode,
                Type = $"https://httpstatuses.com/{apiException.StatusCode}",
                Instance = context.Request.Path
            },
            _ => new ProblemDetails
            {
                Title = "Error interno del servidor",
                Detail = "Ocurrió un error inesperado al procesar la solicitud.",
                Status = StatusCodes.Status500InternalServerError,
                Type = "https://httpstatuses.com/500",
                Instance = context.Request.Path
            }
        };

        problemDetails.Extensions["traceId"] = context.TraceIdentifier;
        context.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsJsonAsync(problemDetails);
    });
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SuEnvioSeguro API V1");
    });

    app.MapGet("/", () => Results.Redirect("/swagger"));
}
else
{
    app.MapGet("/", () => Results.Ok(new { servicio = "SuEnvioSeguro.API", estado = "Activo" }));
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors("WebApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Aplicar migraciones automáticamente al iniciar
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

app.Run();
