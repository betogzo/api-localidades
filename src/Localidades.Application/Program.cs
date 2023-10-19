using Localidades.Application.Configurations;
using Localidades.Application.Endpoints;
using Localidades.Application.Services;
using Localidades.Application.ViewModels.Validators.User;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

Settings.ConfigureEPPlusLicense();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddValidatorsFromAssemblyContaining(typeof(UserEndpointRequestValidator));

var secretClient = builder.AzureSecretClient();
secretClient.SetAppSecrets();

builder.AddSqlServerDbContext();

builder.Services.ConfigureAuthenticationAuthorization();
builder.Services.ConfigureCustomSwaggerGen();

builder.ConfigureApplicationServices();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.ConfigureCustomSwaggerUI();

app.ConfigureCustomGlobalErrorHandling();

app.ConfigureUserEndpoints();
app.ConfigureStateEndpoints();
app.ConfigureCityEndpoints();
app.ConfigureUploadEndpoint();

app.UseHttpsRedirection();

app.Run();