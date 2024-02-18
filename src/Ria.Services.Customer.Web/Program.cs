using FluentValidation;
using Ria.Services.Customer.Web;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddScoped<IValidator<Customer>, CustomerValidator>();
services.AddSingleton<CustomerRepository>();
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();