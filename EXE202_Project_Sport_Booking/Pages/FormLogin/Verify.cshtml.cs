using EXE202_Project_Sport_Booking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EXE202_Project_Sport_Booking.Pages.FormLogin
{
    public class VerifyModel : PageModel
    {
        private readonly EXE201_Rental_Sport_FieldContext _context;

        [BindProperty] public string OTP { get; set; }
        [BindProperty] public string Email { get; set; }

        public VerifyModel(EXE201_Rental_Sport_FieldContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            Email = HttpContext.Session.GetString("Email");
            string sessionOTP = HttpContext.Session.GetString("OTP");
            string otpPurpose = HttpContext.Session.GetString("OTP_Purpose");

            Console.WriteLine($"[DEBUG] OnGet - Email: {Email}, OTP: {sessionOTP}, Purpose: {otpPurpose}");

            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(sessionOTP) || string.IsNullOrEmpty(otpPurpose))
            {
                return RedirectToPage("/FormLogin/Login");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Email = HttpContext.Session.GetString("Email");
            string sessionOTP = HttpContext.Session.GetString("OTP");
            string otpPurpose = HttpContext.Session.GetString("OTP_Purpose");

            Console.WriteLine($"[DEBUG] OnPost - Email: {Email}, OTP: {sessionOTP}, Purpose: {otpPurpose}, Input OTP: {OTP}");

            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(sessionOTP) || string.IsNullOrEmpty(otpPurpose))
            {
                ModelState.AddModelError("", "Session expired. Please request a new OTP.");
                return RedirectToPage("/FormLogin/ForgotPassword");
            }

            if (OTP != sessionOTP)
            {
                ModelState.AddModelError("", "Invalid OTP. Please try again.");
                return Page();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == Email);
            if (user == null)
            {
                ModelState.AddModelError("", "User not found.");
                return RedirectToPage("/FormLogin/Login");
            }

            // Xóa OTP khỏi Session sau khi xác nhận thành công
            HttpContext.Session.Remove("OTP");
            HttpContext.Session.Remove("OTP_Purpose");

            if (otpPurpose == "Signup")
            {
                user.IsVerified = true;
                await _context.SaveChangesAsync();
                HttpContext.Session.Remove("Email"); // Xóa email khỏi session sau khi xác thực
                return RedirectToPage("/FormLogin/Login");
            }
            else if (otpPurpose == "ForgotPassword")
            {
                return RedirectToPage("/FormLogin/ResetPassword");
            }

            return Page();
        }
    }
}
