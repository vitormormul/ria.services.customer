using System.Threading.Channels;
using FluentValidation;
using Ria.Services.Customer.Web;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddScoped<IValidator<Customer>, CustomerValidator>();
services.AddSingleton<ICustomerRepository, CustomerRepository>();
services.AddSingleton<ICustomerPublisher, CustomerPublisher>();
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddSingleton(Channel.CreateUnbounded<Customer[]>(new UnboundedChannelOptions { SingleReader = true }));
services.AddHostedService<CustomerConsumer>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();