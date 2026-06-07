using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Services;
using Infrastructure.Data;
using Infrastructure.Persistense.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();



builder.Services.AddDbContext<AppDbContext>(opt 
    => opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBorrowRepository, BorrowRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();

builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddScoped<IBorrowService, BorrowService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()  
    .WriteTo.File("Logs/log-.txt", 
        rollingInterval: RollingInterval.Day) 
    .CreateBootstrapLogger();  


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

