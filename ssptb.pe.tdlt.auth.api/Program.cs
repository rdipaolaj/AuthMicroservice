using ssptb.pe.tdlt.auth.api;
using ssptb.pe.tdlt.auth.data;
using ssptb.pe.tdlt.auth.api.Configuration;
using ssptb.pe.tdlt.auth.infraestructure.Modules;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddConfigurationSettings(builder);

builder.Services.AddCustomMvc(builder);
builder.Services.AddAntiForgeryToken();
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatRAssemblyConfiguration();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddCustomServicesConfiguration();
builder.Services.AddSecretsConfiguration(builder);
builder.Services.AddDatabaseHealthCheck();
builder.Services.AddMapsterConfiguration();
builder.Services.AddApiVersioningConfiguration();

builder.Services.AddDatabaseConfiguration(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.ConfigurationSwagger();
}

app.AddSecurityHeaders();
app.UseCors("CorsPolicy");
//app.UseHttpsRedirection();

app.UseAuthorization();
app.UseExceptionHandler();

app.MapControllers();

app.UseAntiforgery();

app.MapHealthChecks("/health");

app.Run();