# Avatar Dinámico Personalizado

## 🚀 Descripción

Aplicación web desarrollada con ASP.NET Core MVC que permite a los usuarios generar automáticamente un avatar personalizado con sus iniciales y el color que elijan. El sistema almacena el avatar y los datos del usuario en una base de datos SQL Server, facilitando la visualización y gestión de perfiles.

## 🛠️ Tecnologías Utilizadas

- **Framework:** ASP.NET Core MVC (.NET 9)
- **Lenguajes:** C#, HTML5, CSS, JavaScript
- **Frontend:** Bootstrap 5, jQuery
- **Backend:** Entity Framework Core, SQL Server
- **Gestión de Sesión:** ASP.NET Core Session
- **Generación de Imágenes:** System.Drawing.Common

## ✨ Características

- ✅ **Generación automática de avatar** con iniciales y color personalizado
- ✅ **Registro y login de usuarios** con almacenamiento seguro en base de datos
- ✅ **Visualización de perfil** con avatar y datos personales
- ✅ **Interfaz responsiva** adaptada para desktop y móvil
- ✅ **Gestión de sesión** para mantener datos del usuario logueado
- ✅ **Almacenamiento de avatares** en el servidor con nombres únicos
- ✅ **Validación de formularios** y mensajes de error amigables

## 📱 Funcionalidades Principales

### 📝 Registro de Usuario
Permite crear una cuenta introduciendo nombre, email y color favorito. El sistema genera el avatar y almacena los datos.

### 🔑 Login de Usuario
Acceso mediante email. Si el usuario existe, se recupera la sesión y se muestra el perfil.

### 🎨 Generación de Avatar
Crea una imagen PNG con las iniciales del usuario y el color seleccionado, centrando el texto y guardando el archivo en el servidor.

### 👤 Visualización de Perfil
Muestra el avatar, nombre, email y color elegido en una tarjeta de perfil elegante y responsiva.

## 🏗️ Estructura del Proyecto

```
AvatarDinamicoPersonalizado/
├── Controllers/
│   └── HomeController.cs
├── Data/
│   └── ApplicationDbContext.cs
├── Models/
│   ├── Registro.cs
│   ├── Login.cs
│   └── ErrorViewModel.cs
├── Views/
│   ├── Home/
│   │   ├── Index.cshtml
│   │   ├── CrearCuenta.cshtml
│   │   ├── Login.cshtml
│   │   └── Perfil.cshtml
│   └── Shared/
│       ├── _Layout.cshtml
│       ├── Error.cshtml
│       └── _ValidationScriptsPartial.cshtml
├── wwwroot/
│   ├── avatars/
│   ├── css/
│   ├── images/
│   ├── js/
│   └── lib/
├── Program.cs
├── appsettings.json
├── LICENSE
└── README.md
```

## 🌐 API Backend

La aplicación utiliza Entity Framework Core para interactuar con la base de datos SQL Server. El modelo principal es `Registro`, que almacena los datos del usuario y la URL del avatar generado.

## 📈 Rendimiento y Optimización

- 📱 Interfaz totalmente responsiva con Bootstrap
- ⚡ Generación eficiente de imágenes en el servidor
- 🔄 Gestión de sesión optimizada para mantener la experiencia de usuario
- 🎨 Validación y manejo de errores en formularios

## 🔄 Actualizaciones Recientes

**v1.0.0** (2025) - Lanzamiento inicial
- Generación automática de avatares personalizados
- Registro y login de usuarios
- Visualización de perfil con avatar
- Interfaz responsiva y moderna
- Almacenamiento seguro de datos y avatares

---

## 📄 Licencia

Este proyecto está disponible para visualización y evaluación profesional. Ver el archivo [LICENSE](LICENSE) para más detalles sobre términos de uso y restricciones.
