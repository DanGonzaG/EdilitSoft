using EdilitSoft.app.Data;
using EdilitSoft.app.Models;

using EdilitSoft.app.ServiciosDaniel;
using EdilitSoft.app.ServiciosJuanPa; // JuanPa: para registrar los servicios personalizados de Clientes y Proveedores
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

/*var listaConexiones = new List<string> 
{
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
        var builderOpcional = new DbContextOptionsBuilder<ApplicationDbContext>();
        builderOpcional.UseSqlServer(recorer);

        using var temContext = new ApplicationDbContext(builderOpcional.Options);
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
}*/



// Add services to the container.
//builder.Services.AddDbContext<Contexto>(options => options.UseSqlServer(CadenaValida));

var connectionString = builder.Configuration.GetConnectionString("Server") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
Console.WriteLine($"Cadena de conexión utilizada: {connectionString}");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContext<Contexto>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient();

builder.Services.AddScoped<ICotizacion, Cotizacion>();

// JuanPa: registro del servicio personalizado para Clientes
builder.Services.AddScoped<IClienteService, ClienteService>();
// JuanPa: Servicio para Proveedores
builder.Services.AddScoped<IProveedorService, ProveedorService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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
app.MapRazorPages();

app.Run();
