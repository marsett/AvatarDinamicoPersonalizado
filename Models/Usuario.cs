namespace AvatarDinamicoPersonalizado.Models
{
    #region
    /*
      CREATE TABLE Usuarios (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Nombre NVARCHAR(100),
        Email NVARCHAR(100),
        ColorAvatar NVARCHAR(7),
        AvatarUrl NVARCHAR(255)
      );
     */
    #endregion
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string ColorAvatar { get; set; }
        public string AvatarUrl { get; set; } = "";
    }

}
