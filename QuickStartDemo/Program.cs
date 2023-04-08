using QuickStart.Infra.DI;
using QuickStart.Infra.Logging;
using QuickStart.Infra.Rabbitmq;
using QuickStart.Infra.Redis;
using QuickStartDemo;
using Service;

var builder = WebApplication.CreateBuilder(args);
builder.Host
    //�ӹ�Ĭ������
    .UseServiceProviderFactory(new AfServiceProviderFactory());
//Add Nlog
builder.Services.AddNlog(builder.Configuration);
//Add Redis
builder.Services.AddRedis<MyRedisPwdDecryptor>(builder.Configuration);
//Add RabbitMq
builder.Services.AddRabbitMq(builder.Configuration);
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
