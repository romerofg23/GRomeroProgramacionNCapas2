using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var ConString = builder.Configuration.GetConnectionString("GRomeroProgramacionNCapas");
builder.Services.AddDbContext<DL.GromeroProgramacionNcapasContext>(Options => Options.UseSqlServer(ConString));

builder.Services.AddScoped<BL.Usuario>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
