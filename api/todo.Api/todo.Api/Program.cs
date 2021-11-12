using todo.Api.Hubs;

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

//var webSocketOptions = new WebSocketOptions()
//{
//    KeepAliveInterval = TimeSpan.FromSeconds(120),
//};
//webSocketOptions.AllowedOrigins.Add("http://localhost:8080/");

//app.UseWebSockets(webSocketOptions);

app.UseRouting();
app.UseCors("cors");

app.UseEndpoints(endpoints => {
    endpoints.MapHub<ChatHub>("/ChatHub");
});

RedisHelper.Initialization(new CSRedis.CSRedisClient("127.0.0.1"));

app.Run();
