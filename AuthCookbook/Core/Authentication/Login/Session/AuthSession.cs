using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthCookbook.Core.Authentication.Login.Session
{
    [Index(nameof(CreatedAt), IsDescending = [true])]
    [PrimaryKey(nameof(SessionId))]
    public class AuthSession
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid SessionId { get; set; }

        public Guid UserId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ExpiresAt { get; set; }
    }
}
