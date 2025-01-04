using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Pr_Concesionaria.Models;
using System.Security.Claims;

namespace Pr_Concesionaria.Controllers
{
    public class AutenticacionController : Controller
    {
        public readonly IConfiguration configuration;

        public AutenticacionController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            Usuario usuario = new Usuario();
            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Login(Usuario usuario) 
        {
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(configuration["ConnectionStrings:cn"]))
            {
                if (string.IsNullOrEmpty(usuario.NombreUsuario) || string.IsNullOrEmpty(usuario.Password))
                {
                    ModelState.AddModelError("", "Ingresar los datos solicitados");
                }
                else 
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("sp_seguridad_usuario", cn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@usuario", usuario.NombreUsuario);
                    cmd.Parameters.AddWithValue("@password", usuario.Password);

                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                            new Claim(ClaimTypes.Role, usuario.Password)
                        };
                        ViewBag.mensaje = "usuario.NombreUsuario";
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Datos no validos!");
                    }
                }
                
            }
            ViewBag.mensaje = mensaje;
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Autenticacion");
        }
    }
}
