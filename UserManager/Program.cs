using Common.Interfaces;
using Common.Extensions;
using Common.Models.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using UserManager.Data;

using UserManager.Services;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "User Manager API", Version = "v1" });

    #region Bearer
    var bearerSecurityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Enter your Bearer Token in this format: Bearer {your_token}",
    };

    c.AddSecurityDefinition("Bearer", bearerSecurityScheme);
    var bearerSecurityRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer",
                        },
                    },
                    new string[] { }
                },
            };
    c.AddSecurityRequirement(bearerSecurityRequirement);
    #endregion


    #region ApiKey
    var apiKeySecurityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Scheme = "ApiKey",
        BearerFormat = "JWT",
        Description = "Enter your ApiKey in this format: ApiKey {your_token}",
    };

    c.AddSecurityDefinition("ApiKey", apiKeySecurityScheme);
    var apiKeySecurityRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "ApiKey",
                        },
                    },
                    new string[] { }
                },
            };
    c.AddSecurityRequirement(apiKeySecurityRequirement);
    #endregion
});

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(builder =>
        builder.WithOrigins("https://localhost:7090")
            .AllowAnyHeader()
            .WithMethods(HttpMethods.Get, HttpMethods.Post)
            )
    );

builder.Services.ConfigureAuthentication(builder.Configuration);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.SettingPath));
builder.Services.AddScoped<IUserService, UserService>();


var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API Name v1");
    c.OAuthUsePkce();
});

app.UseHttpsRedirection();

app.UseRouting();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
