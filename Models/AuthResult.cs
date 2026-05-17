namespace MediFlowApi.Models
{
    public class AuthResult
    {
        public bool IsAuthenticated { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? UserName{ get; set; } 
    }
}
