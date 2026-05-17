using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace MediFlowApi.DTOs
{
    public class LoginDto 
    {
        [EmailAddress]
        public string Email { set; get; }
        public string Password { set; get; }
    }
}
