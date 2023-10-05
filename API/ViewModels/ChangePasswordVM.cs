namespace API.ViewModels
{
    public class ChangePasswordVM
    {
        public string Email { get; set; }
        public string OTP { get; set; }
        public string NewPassword { get; set; }
        public string CheckPassword { get; set; }
    }
}
