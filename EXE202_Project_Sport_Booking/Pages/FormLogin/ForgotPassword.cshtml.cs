using EXE202_Project_Sport_Booking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;

namespace EXE202_Project_Sport_Booking.Pages.FormLogin
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly EXE201_Rental_Sport_FieldContext _context;
        private readonly IConfiguration _configuration; // Inject cấu hình

        [BindProperty] public string Email { get; set; }

        public ForgotPasswordModel(EXE201_Rental_Sport_FieldContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration; // Lưu cấu hình
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Email))
            {
                ModelState.AddModelError("", "Please enter your email.");
                return Page();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Email not found in our system.");
                return Page();
            }

            // Generate OTP
            Random random = new Random();
            string otp = random.Next(100000, 999999).ToString();

            // Store OTP in session
            HttpContext.Session.SetString("OTP", otp);
            HttpContext.Session.SetString("Email", Email);
            HttpContext.Session.SetString("OTP_Purpose", "ForgotPassword");

            // Send OTP via Email
            bool emailSent = SendOTPByEmail(Email, otp);
            if (!emailSent)
            {
                ModelState.AddModelError("", "Failed to send OTP. Please try again later.");
                return Page();
            }

            return RedirectToPage("/FormLogin/Verify");
        }

        private bool SendOTPByEmail(string toEmail, string otp)
        {
            try
            {
                // Lấy thông tin email từ appsettings.json
                string fromEmail = _configuration["EmailSettings:FromEmail"];
                string smtpServer = _configuration["EmailSettings:SmtpServer"];
                int port = int.Parse(_configuration["EmailSettings:Port"]);
                string username = _configuration["EmailSettings:Username"];
                string password = _configuration["EmailSettings:Password"];

                var fromAddress = new MailAddress(fromEmail, "Sport Booking");
                var toAddress = new MailAddress(toEmail);
                const string subject = "Password Reset OTP";
                string body = $"Your OTP for password reset is: {otp}";

                var smtp = new SmtpClient
                {
                    Host = smtpServer,
                    Port = port,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(username, password)
                };

                using var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = false
                };

                smtp.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return false;
            }
        }
    }
}
