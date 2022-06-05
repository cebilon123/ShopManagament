using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseSqlServer(builder.Configuration["connection"]);
});

// rejestruje tutaj service które potem "wstrzykiwany" jest w konstruktorze kontrollera (wszystkim tym zajmuje sie asp.net framework)
builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<ProductService>();
builder.Services.AddTransient<OrderService>();
builder.Services.AddTransient<InvoiceService>();
builder.Services.AddTransient<UploadService>();
builder.Services.AddTransient<QueryService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     
// }

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
