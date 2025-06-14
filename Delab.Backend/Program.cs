
using Delab.AccessData.Data;
using Delab.Backend.Class;
using Delab.Backend.Data;
using Delab.Common.Helper;
using Delab.Shared.Class;
using Delab.Shared.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

namespace Delab.Backend;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers()
            .AddJsonOptions( x => x.JsonSerializerOptions.ReferenceHandler =ReferenceHandler.IgnoreCycles);
        // Add services to the container.

        //builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        //builder.Services.AddSwaggerGen();
        
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Orders Backend", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer scheme. <br /> <br />
                      Enter 'Bearer' [space] and then your token in the text input below.<br /> <br />
                      Example: 'Bearer 12345abcdef'<br /> <br />",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
          {
            {
              new OpenApiSecurityScheme
              {
                Reference = new OpenApiReference
                  {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                  },
                  Scheme = "oauth2",
                  Name = "Bearer",
                  In = ParameterLocation.Header,
                },
                new List<string>()
              }
            });
        });


        builder.Services.AddDbContext<DataContext>( x=> 
                                x.UseSqlServer("name=DefaultConnection" 
                                ,option => option.MigrationsAssembly("Delab.Backend")));

        //Para realizar logueo de los usuarios
        builder.Services.AddIdentity<User, IdentityRole>(cfg =>
        {
            //Agregamos Validar Correo para dar de alta al Usuario
            cfg.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
            cfg.SignIn.RequireConfirmedEmail = true;

            cfg.User.RequireUniqueEmail = true;
            cfg.Password.RequireDigit = false;
            cfg.Password.RequiredUniqueChars = 0;
            cfg.Password.RequireLowercase = false;
            cfg.Password.RequireNonAlphanumeric = false;
            cfg.Password.RequireUppercase = false;
            //Sistema per bloccare login dopo n tentativi
            cfg.Lockout.MaxFailedAccessAttempts = 3;
            cfg.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);  //TODO: Tempo di blocco
            cfg.Lockout.AllowedForNewUsers = true;
        }).AddDefaultTokenProviders()  
          .AddEntityFrameworkStores<DataContext>();

        //si occupa di validare i token JWT ricevuti
        //La configurazione può integrare OAuth2 se collegata a un Authorization Server che emette i token. e questo non lo è
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddCookie()
        .AddJwtBearer(x => x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwtKey"]!)),
            ClockSkew = TimeSpan.Zero //Tolleranda tra differenti orari client server
        });

        builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
        builder.Services.Configure<SendGridSettings>(builder.Configuration.GetSection("SendGrid"));
        builder.Services.AddTransient<SeedDb>();
        builder.Services.AddScoped<IUserHelper, UserHelper>();
        builder.Services.AddScoped<IUtilityTools, UtilityTools>();
        builder.Services.AddScoped<IFileStorage, FileStorage>();

        //Inicio de Area de los Serviciios
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin", builder =>
            {
                builder.WithOrigins("https://localhost:7023") // dominio de tu aplicación Blazor
                     .AllowAnyHeader()
                     .AllowAnyMethod()
                     .WithExposedHeaders(new string[] { "Totalpages", "Counting" });
            });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            
                
            //string url = "https://localhost:7023/swagger/index.html";
            //Task.Run(() => OpenBrowser(url));
            
        }

        //Llamar el Servicio de CORS
        app.UseCors("AllowSpecificOrigin");

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
    // Método para abrir el navegador
    static void OpenBrowser(string url)
    {
        try
        {
            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            };
            System.Diagnostics.Process.Start(psi);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al abrir el navegador: {ex.Message}");
        }
    }
}

