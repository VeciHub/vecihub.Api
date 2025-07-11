using System;
using System.Security.Cryptography;
using System.Text;
using VeciHub.IAM.Domain.Commands;
using VeciHub.IAM.Domain.Model;
using VeciHub.IAM.Domain.Repositories;
using VeciHub.IAM.Infrastructure.Tokens.JWT.Services;

namespace VeciHub.IAM.Application.Internal.CommandServices
{
    public class UserCommandService
    {
        private readonly IUserRepository _userRepository;
        private readonly TokenService _tokenService;

        public UserCommandService(IUserRepository userRepository, TokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<string> RegisterAsync(SignUpCommand command)
        {
            var existing = await _userRepository.FindByEmailAsync(command.Email);
            if (existing != null) throw new Exception("Usuario ya registrado");

            var hash = HashPassword(command.Password);
            var user = new User(command.Username, command.Email, hash);
            await _userRepository.AddAsync(user);

            return _tokenService.GenerateToken(user); // Devuelve token al registrarse
        }

        public async Task<string> LoginAsync(SignInCommand command)
        {
            var user = await _userRepository.FindByEmailAsync(command.Email);
            if (user == null || !VerifyPassword(command.Password, user.PasswordHash))
                throw new Exception("Credenciales inválidas");

            return _tokenService.GenerateToken(user);
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private bool VerifyPassword(string input, string hash)
        {
            return HashPassword(input) == hash;
        }
    }
}
