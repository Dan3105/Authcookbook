namespace AuthServer.Model.UserModel
{
    public class User
    {
        public int id { get; set; }
        public required string username { get; set; }
        public required string displayName { get; set; }
        public string? email { get; set; }
        public string? hashPassword { get; set; }
    }

    public class UserRegistrationDTO
    {
        public string? username { get; set; } 
        public string? password { get; set; }
        public string? email { get; set; }
    }
}
