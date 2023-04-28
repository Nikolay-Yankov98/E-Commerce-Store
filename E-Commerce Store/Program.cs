using E_Commerce_Store.Extenstions;
using E_Commerce_Store.Middleware;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

app.UseStatusCodePagesWithReExecute("/errors/{0}");

 app.UseSwagger();
app.UseSwaggerUI();


app.UseStaticFiles();
//app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

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
