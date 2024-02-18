using System.Threading.Channels;
using FluentValidation;
using Ria.Services.Customer.Web;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddScoped<IValidator<Customer>, CustomerValidator>();
services.AddSingleton<CustomerRepository>();
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddSingleton(Channel.CreateUnbounded<Customer[]>(new UnboundedChannelOptions { SingleReader = true }));
services.AddHostedService<CustomersSync>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();