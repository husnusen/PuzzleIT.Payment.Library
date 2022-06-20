using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddTransient<IDataStoreFactory, DataStoreFactory>()
                .AddTransient<IPaymentService, PaymentService>()
                .AddScoped<AccountDataStore>()
                .AddScoped<IDataStore, AccountDataStore>(s => s.GetService<AccountDataStore>())
                .AddScoped<BackupAccountDataStore>()
                .AddScoped<IDataStore, BackupAccountDataStore>(s => s.GetService<BackupAccountDataStore>());

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
