using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;

namespace WebApplication5
{
    [Table("City")]
    public class City : BaseModel
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("population")]
        public long Population { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}