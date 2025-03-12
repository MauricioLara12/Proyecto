using CCTP_Manage.Helpers;
using CCTP_Manage.Models.Entities;
using CCTP_Manage.Models.ViewModel;
using CCTP_Manage.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using System.Security.Claims;

namespace CCTP_Manage.Controllers
{
    public class AccountController : Controller
    {
        Repository<Usuario> repository;
        SaloneventosdbContext context;

        public AccountController()
        {
            context = new();
            repository = new(context);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel vm)
        {
            ModelState.Clear();
            if (string.IsNullOrWhiteSpace(vm.Correo))
            {
                ModelState.AddModelError("", "Debe indicar el Correo de usuario");
            }
            if (string.IsNullOrWhiteSpace(vm.Contrasena))
            {
                ModelState.AddModelError("", "Debe indicar la contraseña");
            }
            if (ModelState.IsValid)
            {
                var usuario = repository.GetAll().FirstOrDefault(x => x.Correo == vm.Correo
                && x.Contrasena == Encriptacion.Encriptar(vm.Contrasena));

                if (usuario == null) //No existe 
                {
                    ModelState.AddModelError("", "Correo de usuario o contraseña incorrectos");
                    return View(vm);
                }

                //Si existe el usuario
                //Crear sus claims (informacion conocida del usuario: autenticarlo)
                List<Claim> claims = new();
                claims.Add(new Claim("Id", usuario.Id.ToString()));
                claims.Add(new Claim(ClaimTypes.Email, usuario.Correo));

                ClaimsIdentity claimsidentity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                ClaimsPrincipal principal = new ClaimsPrincipal(claimsidentity);


                this.HttpContext.SignInAsync(principal);

                return RedirectToAction("Index", "Home");
            }
            return View(vm);
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(SignUpViewModel vm)
        {
            if (string.IsNullOrWhiteSpace(vm.Correo))
            {
                ModelState.AddModelError("", "Debe indicar el correo del usuario");
            }
            if (string.IsNullOrWhiteSpace(vm.Nombre))
            {
                ModelState.AddModelError("", "Debe indicar el nombre del usuario");
            }
            if (string.IsNullOrWhiteSpace(vm.Contrasena))
            {
                ModelState.AddModelError("", "Debe indicar la Contraseña del usuario");
            }
            if (string.IsNullOrWhiteSpace(vm.Telefono))
            {
                ModelState.AddModelError("", "Debe indicar el Telefono del usuario");
            }
            if (vm.Contrasena != vm.RepetirContrasena)
            {
                ModelState.AddModelError("", "Las Contraseñas no Coinciden");
            }
            if (repository.GetAll().Any(x => x.Nombre == vm.Nombre))
            {
                ModelState.AddModelError("", "Ya existe un Usuario con el Mismo Nombre");
            }
            if (repository.GetAll().Any(x => x.Correo == vm.Correo))
            {
                ModelState.AddModelError("", "Ya existe un Usuario con el Mismo Correo");
            }
            if (repository.GetAll().Any(x => x.Telefono == vm.Telefono))
            {
                ModelState.AddModelError("", "Ya existe un Usuario con el Mismo Telefono");
            }
            //if (repository.GetAll().Any(x => x.Telefono.Length != 10 || !x.Telefono.All(char.IsDigit)))
            //{
            //    ModelState.AddModelError("", "El número de teléfono debe tener exactamente 10 dígitos y solo contener números.");
            //}
            if (ModelState.IsValid)
            {
                Usuario u = new();
                u.Correo = vm.Correo;
                u.Nombre = vm.Nombre;
                u.Telefono = vm.Telefono;
                u.Contrasena = Encriptacion.Encriptar(vm.Contrasena);  //Encriptar en SHA
                u.FechaRegistro = DateOnly.FromDateTime(DateTime.Now);
                u.TipoUsuario = "Cliente";

                repository.Insert(u);
                return RedirectToAction("Login");
            }
            else
            {
                return View(vm);
            }
        }

        public IActionResult SignOut()
        {
            HttpContext.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}