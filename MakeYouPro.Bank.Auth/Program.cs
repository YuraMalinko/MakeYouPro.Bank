using MakeYouPro.Bank.Api.Auth;
using MakeYouPro.Bank.Dal.Auth.Context;
using MakeYouPro.Bank.Dal.Auth.Repository;
using MakeYouPro.Bank.Service.Auth;
using MakeYouPro.Bank.Service.Auth.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddAutoMapper(typeof(MapperApiUserProfile), typeof(MapperBllUserProfile));
builder.Services.AddSingleton<AuthContext>();

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
