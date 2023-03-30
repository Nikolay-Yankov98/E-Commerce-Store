using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//service for the DB
builder.Services.AddDbContext<StoreContext>(opt =>
{
    //the Connection string for the given DB
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConneciton"));
});
builder.Services.AddScoped<IProductRepository,ProductRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
//Creating/Migration the DB
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var context = services.GetRequiredService<StoreContext>();
var logger = services.GetRequiredService<ILogger<Program>>();

try
{
    await context.Database.MigrateAsync();
   
    await StoreContextSeed.SeedAsync(context);

}
catch(Exception ex)
{
    logger.LogError(ex,"An Error occured durring migration.");
}

app.Run();
