using EXE202_Project_Sport_Booking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace EXE202_Project_Sport_Booking.Pages.FormLogin
{
    public class SignupModel : PageModel
    {
        private readonly EXE201_Rental_Sport_FieldContext _context;
        private readonly IConfiguration _configuration;

        public SignupModel(EXE201_Rental_Sport_FieldContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [BindProperty, Required(ErrorMessage = "Họ và tên không được để trống")]
        public string FullName { get; set; }

        [BindProperty, Required(ErrorMessage = "Email không được để trống"), EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [BindProperty, Required(ErrorMessage = "Mật khẩu không được để trống"), DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        public string Password { get; set; }

        [BindProperty, Required(ErrorMessage = "Xác nhận mật khẩu không được để trống"), DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu nhập lại không khớp")]
        public string RePassword { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Kiểm tra email đã tồn tại chưa
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Email này đã được đăng ký.");
                return Page();
            }

            // Tạo mã OTP ngẫu nhiên
            var otpCode = new Random().Next(100000, 999999).ToString();

            // Gửi OTP qua email
            bool emailSent = SendVerificationEmail(Email, otpCode);
            if (!emailSent)
            {
                ModelState.AddModelError(string.Empty, "Lỗi khi gửi email. Vui lòng thử lại.");
                return Page();
            }

            // Tạo user mới với mật khẩu không mã hóa
            var newUser = new User
            {
                FullName = FullName,
                Email = Email,
                Password = Password, // ✅ Lưu trực tiếp mật khẩu
                RoleId = 3, // Mặc định là Customer
                IsVerified = false
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            // Lưu OTP, Email, và trạng thái đăng ký vào session
            HttpContext.Session.SetString("OTP", otpCode);
            HttpContext.Session.SetString("Email", Email);
            HttpContext.Session.SetString("OTP_Purpose", "Signup");

            return RedirectToPage("/FormLogin/Verify"); // ✅ Chuyển hướng đến trang nhập OTP
        }

        private bool SendVerificationEmail(string email, string code)
        {
            try
            {
                var smtpSettings = _configuration.GetSection("EmailSettings");

                using (var smtpClient = new SmtpClient(smtpSettings["SmtpServer"]))
                {
                    smtpClient.Port = int.Parse(smtpSettings["Port"]);
                    smtpClient.Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"]);
                    smtpClient.EnableSsl = true;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(smtpSettings["FromEmail"]),
                        Subject = "Mã xác thực OTP của bạn",
                        Body = $"Mã OTP của bạn là: {code}",
                        IsBodyHtml = false,
                    };

                    mailMessage.To.Add(email);
                    smtpClient.Send(mailMessage);
                }

                return true; // ✅ Gửi email thành công
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi gửi email: {ex.Message}");
                return false; // ❌ Gửi email thất bại
            }
        }
    }
}
