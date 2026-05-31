using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoHipodromoGrupoF.Logica;
using System.Security.Claims;

namespace ProyectoHipodromoGrupoF.UI.Controllers
{
    public class AuthController : Controller
    {
        private readonly UsuarioService _usuarioService;

        public AuthController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Por favor ingrese usuario y contraseña.";
                return View();
            }

            var usuario = _usuarioService.ValidarUsuario(username, password);

            if (usuario != null)
            {
                // Si es propietario (rol 1), resolver su código de propietario a partir de la cédula
                var codigoPropietario = string.Empty;
                if (usuario.IdCatRol == 1)
                    codigoPropietario = _usuarioService.ObtenerCodigoPropietarioPorCedula(usuario.Cedula) ?? string.Empty;

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,       usuario.Nombre),
                    new Claim(ClaimTypes.Role,        usuario.IdCatRol.ToString()),
                    new Claim("UsuarioId",            usuario.Codigo),
                    new Claim("Cedula",               usuario.Cedula),
                    new Claim("CodigoPropietario",    codigoPropietario)  // "pro0001" para propietarios, vacío para otros
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Usuario o contraseña incorrectos.";
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
