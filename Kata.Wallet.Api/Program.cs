using Kata.Wallet.Database;
using System.Text.Json.Serialization;
using Kata.Wallet.Services;
using Microsoft.EntityFrameworkCore;
using Kata.Wallet.Domain;

var builder = WebApplication.CreateBuilder(args);

// Configura los servicios para la inyección de dependencias
builder.Services.AddDbContext<DataContext>(options =>
    options.UseInMemoryDatabase("WalletDb")); // Usa la base de datos en memoria
IServiceCollection walletServiceRegistration = builder.Services.AddScoped<IWalletService, WalletService>();  // Registra WalletService
builder.Services.AddScoped<ITransactionService, TransactionService>(); // Registra TransactionService

builder.Services.AddControllers().AddJsonOptions(x =>
{
    // serialize enums as strings in api responses 
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
}); ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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
