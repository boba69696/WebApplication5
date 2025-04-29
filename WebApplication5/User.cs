using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace WebApplication5
{
    [Table("User")]
    public class User : BaseModel
    {
        [PrimaryKey("id")] // Явно указываем имя столбца в БД
        public int Id { get; set; }

        [Column("age")]
        public string Age { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("login")]
        public string Login { get; set; }
    }
}