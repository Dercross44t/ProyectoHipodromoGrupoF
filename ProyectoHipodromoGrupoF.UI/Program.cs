using ProyectoHipodromoGrupoF.AccesoADatos;
using ProyectoHipodromoGrupoF.Logica;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options =>
{
    var policy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    options.Filters.Add(new Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter(policy));
});

// Sesión
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<CatalogoRepository>();
builder.Services.AddScoped<CatalogoService>();
builder.Services.AddScoped<ContactosRepository>();
builder.Services.AddScoped<ContactosService>();

// Capa AccesoADatos
builder.Services.AddSingleton<ProyectoHipodromoGrupoF.AccesoADatos.ConexionDB>();
builder.Services.AddScoped<ProyectoHipodromoGrupoF.AccesoADatos.CatalogosRepository>();
builder.Services.AddScoped<ProyectoHipodromoGrupoF.AccesoADatos.UsuarioRepository>();
builder.Services.AddScoped<ProyectoHipodromoGrupoF.AccesoADatos.PersonasRepository>();
builder.Services.AddScoped<ProyectoHipodromoGrupoF.AccesoADatos.EquinosRepository>();
builder.Services.AddScoped<ProyectoHipodromoGrupoF.AccesoADatos.EventosRepository>();
builder.Services.AddScoped<ProyectoHipodromoGrupoF.AccesoADatos.InventarioRepository>();
builder.Services.AddScoped<ProyectoHipodromoGrupoF.AccesoADatos.FacturacionRepository>();
builder.Services.AddScoped<ProyectoHipodromoGrupoF.AccesoADatos.InscripcionesRepository>();
builder.Services.AddScoped<ProyectoHipodromoGrupoF.AccesoADatos.VeterinarioRepository>();
builder.Services.AddScoped<ProyectoHipodromoGrupoF.AccesoADatos.ResultadosRepository>();
builder.Services.AddScoped<ProyectoHipodromoGrupoF.AccesoADatos.ProveedoresRepository>();

// Capa Lógica
builder.Services.AddScoped<ProyectoHipodromoGrupoF.Logica.CatalogosService>();
builder.Services.AddScoped<ProyectoHipodromoGrupoF.Logica.UsuarioService>();
builder.Services.AddScoped<ProyectoHipodromoGrupoF.Logica.PersonasService>();
builder.Services.AddScoped<ProyectoHipodromoGrupoF.Logica.EquinosService>();
builder.Services.AddScoped<ProyectoHipodromoGrupoF.Logica.EventosService>();
builder.Services.AddScoped<ProyectoHipodromoGrupoF.Logica.InventarioService>();
builder.Services.AddScoped<ProyectoHipodromoGrupoF.Logica.FacturacionService>();
builder.Services.AddScoped<ProyectoHipodromoGrupoF.Logica.InscripcionesService>();
builder.Services.AddScoped<ProyectoHipodromoGrupoF.Logica.VeterinarioService>();
builder.Services.AddScoped<ProyectoHipodromoGrupoF.Logica.ResultadosService>();
builder.Services.AddScoped<ProyectoHipodromoGrupoF.Logica.ProveedoresService>();

// Autenticación por cookies
builder.Services.AddAuthentication(
    Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();