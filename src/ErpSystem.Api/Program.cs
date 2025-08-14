using ErpSystem.Application.Interfaces;
using ErpSystem.Infrastructure.Persistence;
using ErpSystem.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Default")
        ?? "Data Source=./erp.db";
    options.UseSqlite(connectionString);
});

builder.Services.AddScoped<IAccountingService, AccountingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/accounts", async (IAccountingService svc) =>
{
    var list = await svc.GetAccountsAsync();
    return Results.Ok(list);
}).WithOpenApi();

app.MapPost("/api/accounts", async (IAccountingService svc, ErpSystem.Domain.Entities.Account account) =>
{
    var created = await svc.CreateAccountAsync(account);
    return Results.Created($"/api/accounts/{created.Id}", created);
}).WithOpenApi();

app.MapGet("/api/journal-entries", async (IAccountingService svc, DateOnly? from, DateOnly? to) =>
{
    var entries = await svc.GetJournalEntriesAsync(from, to);
    return Results.Ok(entries);
}).WithOpenApi();

app.MapPost("/api/journal-entries", async (IAccountingService svc, ErpSystem.Domain.Entities.JournalEntry entry) =>
{
    var posted = await svc.PostJournalEntryAsync(entry);
    return Results.Created($"/api/journal-entries/{posted.Id}", posted);
}).WithOpenApi();

app.Run();
