var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<WorkersHealthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WorkersHealthManagment")));

// Repository and Service registration
builder.Services.AddScoped<IWorkerTaskRepository, WorkerTaskRepository>();
builder.Services.AddScoped<IWorkerTaskService, WorkerTaskService>();

// Load Balancer
builder.Services.AddSingleton<LoadBalancerService>();

// Background Services (3 Workers)
builder.Services.AddHostedService<HealthMonitoringWorker>();
builder.Services.AddHostedService<TaskProcessingWorker>();
builder.Services.AddHostedService<WorkloadBalancingWorker>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

// Auto-migration
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<WorkersHealthDbContext>();
    context.Database.EnsureCreated();
}

app.Run();