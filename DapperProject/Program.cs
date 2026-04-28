using DapperProject.Context;
using DapperProject.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<DapperContext>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();