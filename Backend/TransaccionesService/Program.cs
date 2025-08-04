using Microsoft.EntityFrameworkCore;
using TransaccionesService.Data;
using TransaccionesService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TransaccionDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient("ProductosService", client =>
{
    client.BaseAddress = new Uri("https://localhost:7111");
});

// Add services to the container.
builder.Services.AddScoped<ITransaccionService, TransaccionService>();

builder.Services.AddControllers();

// Define política CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

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

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();
