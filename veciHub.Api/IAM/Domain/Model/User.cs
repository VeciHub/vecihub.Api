namespace VeciHub.IAM.Domain.Model
{
    public class User
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Username { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public string Role { get; private set; } = "User"; // o "Admin"

        public User(string username, string email, string passwordHash, string role = "User")
        {
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
        }
        public void UpdateUsername(string username) => Username = username;
        public void UpdateEmail(string email) => Email = email;

    }

}
