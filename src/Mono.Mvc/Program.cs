using Microsoft.EntityFrameworkCore;
using Mono.Data;

var builder = WebApplication.CreateBuilder(args);

NinjectProvider.Initialize();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


AppDbContext _db = NinjectProvider.Get<AppDbContext>();
bool deleted = await _db.Database.EnsureDeletedAsync();
bool created = await _db.Database.EnsureCreatedAsync();


app.Run();
