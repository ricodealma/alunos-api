using System.ComponentModel.DataAnnotations.Schema;

namespace Alunos.Api.Infra.Data.User.Entities
{
    [Table("usuario")]
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
