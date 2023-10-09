using Auth.Models.Settings;
using Auth.Services;
using Auth.Services.Interfaces;
using Common.Http;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.SettingPath));
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAppHttpClient, AppHttpClient>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpClient(AppHttpClient.UsersHttpClientName, httpClient =>
{
    httpClient.BaseAddress = new Uri(builder.Configuration.GetValue<string>("Urls:UserManager"));
    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
        "ApiKey", builder.Configuration["UserManagerApiKey"]);
});
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth Manager");
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization(); 
app.MapControllers();
app.Run();
