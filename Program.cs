using BilleteraCryptoProjectAPI.Data;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using System;
using BilleteraCryptoProjectAPI.Logic;
using BilleteraCryptoProjectAPI.Services;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<ICriptoyaService, CriptoyaService>();


var connectionString = CryptoWalletApiDBContext.GetConnectionString();

builder.Services.AddDbContext<CryptoWalletApiDBContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    )
);

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<IAccionService, AccionLogic>();
builder.Services.AddScoped<IClienteService, ClienteLogic>();
builder.Services.AddScoped<ICriptoService, CriptoLogic>();
builder.Services.AddScoped<ICuentaService, CuentaLogic>();
builder.Services.AddScoped<IEstadoService, EstadoLogic>();
builder.Services.AddScoped<IOperacionService, OperacionLogic>();
builder.Services.AddScoped<IHistorialPrecioService, HistorialPrecioLogic>();
builder.Services.AddScoped<IMovimientoService, MovimientoLogic>();
builder.Services.AddScoped<IDashboardService, DashboardLogic>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});




var app = builder.Build();
app.UseCors();
if (app.Environment.IsDevelopment()) {
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseExceptionHandler(errorApp => {
    errorApp.Run(async context => {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "text/plain";

        var error = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
        if (error != null) {
            await context.Response.WriteAsync(error.Error.Message);
        }
    });
});

app.Run();


