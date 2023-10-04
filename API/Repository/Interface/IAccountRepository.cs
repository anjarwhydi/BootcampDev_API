namespace API.Repository.Interface
{
    public interface IAccountRepository
    {
        bool login(string email, string password);
        bool SendEmail(string email);
        int ChangePasswordUsingOTP(string email, string otp, string newPassword, string checkPassword);
    }
}
