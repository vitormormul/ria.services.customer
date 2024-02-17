using Ria.Services.Customer.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<CustomerRepository>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();