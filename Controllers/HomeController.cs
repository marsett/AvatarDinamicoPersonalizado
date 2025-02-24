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

        public IActionResult CrearCuenta()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CrearCuenta(Usuario usuario)
        {
            // Limpiar la sesión antes de guardar el nuevo usuario
            HttpContext.Session.Remove("UserName");
            HttpContext.Session.Remove("AvatarUrl");
            HttpContext.Session.Remove("Id");

            if (ModelState.IsValid)
            {
                // Generar iniciales
                string initials = GetInitials(usuario.Nombre);

                // Crear avatar con el color elegido
                byte[] avatarImage = GenerateAvatar(initials, usuario.ColorAvatar);

                // Asegurarse de que la carpeta "avatars" exista
                string avatarsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/avatars");
                if (!Directory.Exists(avatarsFolder))
                {
                    Directory.CreateDirectory(avatarsFolder);
                }

                // Guardar la imagen generada en un directorio local
                string fileName = $"{Guid.NewGuid()}.png";
                string filePath = Path.Combine(avatarsFolder, fileName);
                System.IO.File.WriteAllBytes(filePath, avatarImage);

                // Guardar URL del avatar en la base de datos
                usuario.AvatarUrl = $"/avatars/{fileName}";

                // Guardar usuario en la base de datos
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                // Guardar en la sesión el nombre y el avatar
                HttpContext.Session.SetString("UserName", usuario.Nombre);
                HttpContext.Session.SetString("AvatarUrl", usuario.AvatarUrl);
                HttpContext.Session.SetInt32("Id", usuario.Id);

                return RedirectToAction("Perfil", new { id = usuario.Id });
            }
            return View(usuario);
        }

        

        public async Task<IActionResult> Perfil(int id)
        {
            // Obtener usuario por ID desde la base de datos
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

            // Verificar si el usuario no existe
            if (usuario == null)
            {
                return NotFound(); // Si no se encuentra el usuario, devuelve un error 404
            }


            // Verificar si hay datos de sesión
            var userName = HttpContext.Session.GetString("UserName");
            var avatarUrl = HttpContext.Session.GetString("AvatarUrl");

            // Si los datos existen en la sesión, asignarlos al usuario
            if (!string.IsNullOrEmpty(userName))
            {
                usuario.Nombre = userName;
                usuario.AvatarUrl = avatarUrl;
            }

            return View(usuario);
        }

        private string GetInitials(string nombre)
        {
            string[] words = nombre.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string initials = "";

            foreach (var word in words)
            {
                initials += char.ToUpper(word[0]);
                if (initials.Length == 2) break;
            }

            return initials;
        }

        private byte[] GenerateAvatar(string initials, string colorHex)
        {
            int width = 150, height = 150;
            Bitmap bitmap = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                // Convertimos el código HEX a Color
                Color backgroundColor = ColorTranslator.FromHtml(colorHex);
                g.Clear(backgroundColor);

                // Texto de iniciales
                Font font = new Font("Arial", 50, FontStyle.Bold);
                Brush textBrush = Brushes.White;
                SizeF textSize = g.MeasureString(initials, font);
                float x = (width - textSize.Width) / 2;
                float y = (height - textSize.Height) / 2;
                g.DrawString(initials, font, textBrush, x, y);
            }

            // Convertimos la imagen en bytes
            using MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            return ms.ToArray();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Logout()
        {
            // Eliminar las claves de la sesión relacionadas con el usuario
            HttpContext.Session.Remove("UserName");
            HttpContext.Session.Remove("AvatarUrl");
            HttpContext.Session.Remove("UserId");

            // Redirigir al Index
            return RedirectToAction("Index", "Home");
        }

    }
}
