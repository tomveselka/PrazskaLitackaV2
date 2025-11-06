using System.ComponentModel.DataAnnotations;

namespace PrazskaLitacka.Domain.Entities
{
    public class User
    {
        [Key] 
        public int Id { get; set; }
        [Required] 
        public string Name { get; set; } = null!;
        [Required] 
        public string Login { get; set; } = null!;
        [Required] 
        public string Password { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;
        public string Role { get; set; } = "";

        public ICollection<RaceEntry> RaceEntries { get; set; } = new List<RaceEntry>();
    }
}
