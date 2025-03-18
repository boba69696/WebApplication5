using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;





namespace WebApplication5
{
    [Table("User")]
    public class User : BaseModel
    {
        [PrimaryKey]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("age")]
        public string Age { get; set; }
        [Column("password")]
        public string Password { get; set; }
        [Column("login")]
        public string Email { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set;}
    }
}
