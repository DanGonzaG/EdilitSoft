using EdilitSoft.app.Data;
using EdilitSoft.app.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var listaConexiones = new List<string> {
    builder.Configuration.GetConnectionString("Daniel"),
    builder.Configuration.GetConnectionString("JuanP"),
    builder.Configuration.GetConnectionString("Andy"),
    builder.Configuration.GetConnectionString("Mija"),
};

string CadenaValida = null;

foreach (var recorer in listaConexiones)
{
    try
    {
        var builderOpcional = new DbContextOptionsBuilder<Contexto>();
        builderOpcional.UseSqlServer(recorer);

        using var temContext = new Contexto(builderOpcional.Options);
        if (temContext.Database.CanConnect())
        {
            CadenaValida = recorer;
            Console.WriteLine($"Cadena de conexcion valiada: {CadenaValida}");
            break;
        }
    }
    catch
    {
        Console.WriteLine($"Cadena de conexion invalida: {CadenaValida}");
    }
}

if (CadenaValida == null)
{
    Console.WriteLine("No se encontro cadena de conexion");
}

// Add services to the container.
builder.Services.AddDbContext<Contexto>(options => options.UseSqlServer(CadenaValida));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(CadenaValida));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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
app.MapRazorPages();

app.Run();
