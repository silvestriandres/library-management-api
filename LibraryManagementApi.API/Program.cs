using LibraryManagementApi.API.Data;
using Microsoft.EntityFrameworkCore;
using LibraryManagementApi.API.Repositories;
using LibraryManagementApi.API.Repositories.Interfaces;
using LibraryManagementApi.API.Services;
using LibraryManagementApi.API.Services.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using LibraryManagementApi.API.Validators;
using LibraryManagementApi.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        "v1",
        new()
        {
            Title = "Library Management API",
            Version = "v1",
            Description = "RESTful API for managing library books. [Provincia NET Challenge]"
        });
});

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateBookDtoValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();