using ClientReportManager.Services;
using ClientReportManager.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClientReportManager.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsuarioService _usuarioService;

        public AccountController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            var modelo = new LoginViewModel
            {
                ReturnUrl = returnUrl
            };

            return View(modelo);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var usuario = await _usuarioService.ValidarCredencialesAsync(
                modelo.NombreUsuario,
                modelo.Clave
            );

            if (usuario == null)
            {
                ModelState.AddModelError(string.Empty, "El usuario o la contraseña no son correctos.");
                return View(modelo);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                new Claim("NombreCompleto", usuario.NombreCompleto),
                new Claim(ClaimTypes.Email, usuario.Correo)
            };

            var identidad = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            var propiedades = new AuthenticationProperties
            {
                IsPersistent = modelo.Recordarme,
                ExpiresUtc = modelo.Recordarme
                    ? DateTimeOffset.UtcNow.AddDays(7)
                    : DateTimeOffset.UtcNow.AddHours(8)
            };

            // Se crea la cookie de autenticación con los datos mínimos necesarios del usuario.
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identidad),
                propiedades
            );

            if (!string.IsNullOrWhiteSpace(modelo.ReturnUrl) && Url.IsLocalUrl(modelo.ReturnUrl))
            {
                return Redirect(modelo.ReturnUrl);
            }

            return RedirectToAction("Index", "Dashboard");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            // Se elimina la cookie de autenticación y se cierra la sesión del usuario.
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Account");
        }
    }
}