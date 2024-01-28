using facebook_wall_project.Data;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IUserService,UserService>();
builder.Services.AddSingleton<IPostService,PostService>();

builder.Services.AddSignalR();
builder.Services.AddDbContext<AppDbContext>( options=>{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
}, ServiceLifetime.Singleton);

builder.Services.AddCors(options => {

    options.AddPolicy("AllowOrigin", builder => {

         // Allow requests from two specific URLs during SignalR testing
        builder.WithOrigins("http://localhost:3000","http://localhost:3001")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .WithExposedHeaders("X-Total-Count")
        .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Face_Book_App/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowOrigin");

// ... other middleware ...

app.UseAuthorization();

app.UsePathBase("/Face_Book_App");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<LikeHub>("/Like-Hub")
.RequireCors("AllowOrigin");
        app.MapControllers();
app.Run();
