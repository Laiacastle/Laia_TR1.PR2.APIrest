namespace Api_Laia_T1.PR2.APIrest.Models.DTOs
{
    public class RegisterDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;

        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
