using API.Repository.Interface;
using MailKit.Net.Smtp;
using MimeKit;

namespace API.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly MyContext context;
        public AccountRepository(MyContext context)
        {
            this.context = context;
        }
        public bool login(string email, string password)
        {
            var data = context.Employees.FirstOrDefault(e => e.Email == email);
            var account = context.Accounts.Single(a => a.NIK == data.NIK);
            bool validate = BCrypt.Net.BCrypt.EnhancedVerify(password, account.Password);
            return validate;
        }

        public bool SendEmail(string email)
        {
            var data = context.Employees.FirstOrDefault(e => e.Email == email);
            if (data == null)
            {
                throw new ArgumentException("Email not found");
            }
            var name = data.FirstName + " " + data.LastName;
            var mail = new MimeMessage();
            string SendOTP = GenerateAndSaveOTP(data.NIK);
            mail.From.Add(new MailboxAddress("Anjar Wahyudi", "anjarwhydi@gmail.com"));
            mail.To.Add(new MailboxAddress(name, email));

            mail.Subject = "Reset Password Notification";
            mail.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = $"Hi {name}\r\nEnter this OTP code to verify your account\r\n{SendOTP}"
            };
            using (var smtp = new SmtpClient())
            {
                smtp.Connect("smtp.gmail.com", 587, false);
                smtp.Authenticate("anjarwhydi@gmail.com", "ifip ahfh avzn zteo");
                smtp.Send(mail);
                smtp.Disconnect(true);
            }
            return true;
        }

        private string GenerateOTP()
        {
            const string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            char[] otpArray = new char[6];

            for (int i = 0; i < 6; i++)
            {
                otpArray[i] = allowedChars[random.Next(0, allowedChars.Length)];
            }

            return new string(otpArray);
        }

        public string GenerateAndSaveOTP(string NIK)
        {
            var account = context.Accounts.FirstOrDefault(a => a.NIK == NIK);
            string otp = GenerateOTP();
            account.CreatedAt = DateTime.Now;
            account.IsUsed = false;
            account.OTP = otp;
            context.SaveChanges();
            return otp;
        }

        public int ChangePasswordUsingOTP(string email, string otp, string newPassword, string checkPassword)
        {
            var data = context.Employees.FirstOrDefault(e => e.Email == email);
            var account = context.Accounts.FirstOrDefault(a => a.NIK == data.NIK);

            if (account == null)
            {
                throw new ArgumentException("Account not found");
            }

            if (account.OTP == otp)
            {
                if(newPassword != checkPassword){
                    throw new ArgumentException ("Check the password again!");
                }
                DateTime dateTime = DateTime.Now;
                DateTime timeElapsed = account.CreatedAt.AddMinutes(5);
                if(dateTime >= timeElapsed){
                    throw new ArgumentException("Re-send OTP");
                }

                if (account.IsUsed == true)
                {
                    throw new ArgumentException("OTP already in use");
                }
                account.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(newPassword);
                account.IsUsed = true;
                return context.SaveChanges();
            }
            else
            {
                throw new ArgumentException("Incorrect OTP");
            }
        }

        //public bool ValidateOTP(string otp)
        //{
        //    var account = context.Accounts.FirstOrDefault(a => a.OTP == otp);
        //    if (account == null)
        //    {
        //        throw new ArgumentException("Incorrect OTP");
        //    }

        //    // OTP valid, hapus OTP dari kolom 'otp' di tabel 'Account'
        //    account.OTP = null;

        //    // Simpan perubahan ke database
        //    context.SaveChanges();

        //    return true;
        //}

        //public Task ResetPassword(string email, string password)
        //{
        //    var data = context.Employees.FirstOrDefault(e => e.Email == email);
        //    if (data == null)
        //    {
        //        throw new ArgumentException("Email not exist");
        //    }
        //    var sendEmail = SendEmail(email);

        //    account.Password;
        //}
    }
}
