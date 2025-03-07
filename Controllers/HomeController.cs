using AvatarDinamicoPersonalizado.Data;
using AvatarDinamicoPersonalizado.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Imaging;
using System.Drawing;

namespace AvatarDinamicoPersonalizado.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        private void GuardarSesion(Registro usuario)
        {
            // Se pasa por parámetro el modelo del usuario y se establece
            // en Session las variables necesarias para guardar la sesión
            HttpContext.Session.SetString("NombreUsuario", usuario.Nombre);
            HttpContext.Session.SetString("AvatarUrl", usuario.AvatarUrl);
            HttpContext.Session.SetInt32("IdUsuario", usuario.Id);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {
            // Paso 1
            if (ModelState.IsValid)
            {
                // Paso 2
                var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == model.Email);

                // Paso 3
                if (usuario != null)
                {
                    // Paso 4
                    GuardarSesion(usuario);

                    // Paso 5
                    return RedirectToAction("Perfil", new { id = usuario.Id });
                }
                else
                {
                    ViewBag.Error = "El correo ingresado no está registrado.";
                } 
            }
            return View(model);
        }

        private string GetIniciales(string nombre)
        {
            // Paso 1
            string[] palabras = nombre.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string iniciales = "";

            // Este bucle recorre cada palabra del array
            foreach (var palabra in palabras)
            {
                // Paso 2
                iniciales += char.ToUpper(palabra[0]);

                // Paso 3
                if (iniciales.Length == 2) break;
            }

            return iniciales;
        }

        private byte[] GenerarAvatar(string iniciales, string colorHex)
        {
            // Dimensiones de la imagen
            int ancho = 150, alto = 150;

            // Paso 1
            Bitmap bitmap = new Bitmap(ancho, alto);

            // Paso 2
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                // Paso 3
                Color color = ColorTranslator.FromHtml(colorHex);

                // Paso 4
                g.Clear(color);

                // Paso 5
                Font fuente = new Font("Arial", 50, FontStyle.Bold);

                // Paso 6
                Brush textoBlanco = Brushes.White;

                // Paso 7
                SizeF tamano = g.MeasureString(iniciales, fuente);

                // Paso 8
                float x = (ancho - tamano.Width) / 2;
                float y = (alto - tamano.Height) / 2;

                // Paso 9
                g.DrawString(iniciales, fuente, textoBlanco, x, y);
            }

            // Paso 10
            using MemoryStream ms = new MemoryStream();

            // Paso 11
            bitmap.Save(ms, ImageFormat.Png);

            // Paso 12
            return ms.ToArray();
        }

        public IActionResult CrearCuenta()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CrearCuenta(Registro usuario)
        {
            // Paso 1
            HttpContext.Session.Remove("NombreUsuario");
            HttpContext.Session.Remove("AvatarUrl");
            HttpContext.Session.Remove("IdUsuario");

            // Paso 2
            if (ModelState.IsValid)
            {
                // Paso 3
                string iniciales = GetIniciales(usuario.Nombre);

                // Paso 4
                byte[] imagenAvatar = GenerarAvatar(iniciales, usuario.ColorAvatar);

                // Paso 5
                string carpetaAvatar = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/avatars");

                // Paso 6
                if (!Directory.Exists(carpetaAvatar))
                {
                    Directory.CreateDirectory(carpetaAvatar);
                }

                // Paso 7
                string nombreAvatar = $"{Guid.NewGuid()}.png";

                // Paso 8
                string nombreArchivo = Path.Combine(carpetaAvatar, nombreAvatar);

                // Paso 9
                System.IO.File.WriteAllBytes(nombreArchivo, imagenAvatar);

                // Paso 10
                usuario.AvatarUrl = nombreAvatar;

                // Paso 11
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                // Paso 12
                HttpContext.Session.SetString("NombreUsuario", usuario.Nombre);
                HttpContext.Session.SetString("AvatarUrl", usuario.AvatarUrl);
                HttpContext.Session.SetInt32("IdUsuario", usuario.Id);

                // Paso 13
                return RedirectToAction("Perfil", new { id = usuario.Id });
            }
            return View(usuario);
        }

        public async Task<IActionResult> Perfil(int id)
        {
            // Se busca el usuario en la base de datos
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

            // Verificar si el usuario no existe
            if (usuario == null)
            {
                return NotFound();
            }

            // Se recuperan los datos de la sesión si están disponibles
            var nombreUsuario = HttpContext.Session.GetString("NombreUsuario");
            var avatarUrl = HttpContext.Session.GetString("AvatarUrl");

            // Si los datos de la sesión no son nulos,
            // se actualizan los datos del usuario
            if (!string.IsNullOrEmpty(nombreUsuario))
            {
                usuario.Nombre = nombreUsuario;
                usuario.AvatarUrl = avatarUrl;
            }

            // Devolver la vista con los datos del usuario
            return View(usuario);
        }

        public IActionResult Logout()
        {
            // Se eliminan las variables guardadas en Session
            // para limpiar la sesión y redirige a la vista
            // principal de la aplicación
            HttpContext.Session.Remove("NombreUsuario");
            HttpContext.Session.Remove("AvatarUrl");
            HttpContext.Session.Remove("IdUsuario");

            return RedirectToAction("Index", "Home");
        }
    }
}
