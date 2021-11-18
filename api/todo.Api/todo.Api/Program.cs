using FreeSql;
using todo.Api.Hubs;
using todo.Api.Util.Helper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy("cors",
        builder => builder
        .SetIsOriginAllowed(_=>true)
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());
});

IFreeSql freeSql = new FreeSqlBuilder()
    .UseConnectionString(DataType.MySql, "Server=1.15.248.70;User ID=root;Password=123456;port=3306;Database=todo_db;CharSet=utf8;pooling=true;SslMode=None;")
    .UseAutoSyncStructure(true)
    .Build();
builder.Services.AddSingleton<IFreeSql>(freeSql);


var app = builder.Build();

// Configure the HTTP request pipeline.
// app.Environment.IsDevelopment()
if (true)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.UseRouting();
app.UseCors("cors");

app.UseEndpoints(endpoints => {
    endpoints.MapHub<ChatHub>("/ChatHub");
});

RedisHelper.Initialization(new CSRedis.CSRedisClient("127.0.0.1"));

app.Run();
