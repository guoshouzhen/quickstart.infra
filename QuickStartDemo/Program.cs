using QuickStart.Infra.DI;
using QuickStart.Infra.Logging;
using QuickStart.Infra.Redis;
using QuickStartDemo;
using Service;

var builder = WebApplication.CreateBuilder(args);
builder.Host
    //½Ó¹ÜÄ¬ÈÏÈÝÆ÷
    .UseServiceProviderFactory(new AfServiceProviderFactory());
//Add Nlog
builder.Services.AddNlog(builder.Configuration);
//Add Redis
builder.Services.AddRedis(builder.Configuration, new MyRedisPwdDecryptor());
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddService();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
