using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Policy.API.Data;
using Policy.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Inject DB context
builder.Services.AddDbContext<PolicyDBContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("PolicyDBConnectionString"))
    );

var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>() ;
builder.Services.AddSingleton<EmailConfiguration>(emailConfig);

builder.Services.AddCors((setup) => {
    setup.AddPolicy("default", (options) =>
    {
        options.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
    });
});

builder.Services.Configure<FormOptions>(opts =>
{
    opts.ValueLengthLimit = int.MaxValue;
    opts.MultipartBodyLengthLimit = int.MaxValue;
    opts.MemoryBufferThreshold = int.MaxValue;
});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
    RequestPath = new PathString("/Resources")
});

app.UseCors("default");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
