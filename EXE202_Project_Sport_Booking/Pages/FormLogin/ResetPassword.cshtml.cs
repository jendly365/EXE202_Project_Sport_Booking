using EXE202_Project_Sport_Booking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace EXE202_Project_Sport_Booking.Pages.FormLogin
{
    public class ResetPasswordModel : PageModel
    {
        private readonly EXE201_Rental_Sport_FieldContext _context;

        [BindProperty] public string NewPassword { get; set; }
        [BindProperty] public string ConfirmPassword { get; set; }

        public ResetPasswordModel(EXE201_Rental_Sport_FieldContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("Email") == null)
            {
                return RedirectToPage("/FormLogin/Login");
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            string email = HttpContext.Session.GetString("Email");

            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("", "Session expired. Please request a new OTP.");
                return RedirectToPage("/FormLogin/ForgotPassword");
            }

            if (NewPassword != ConfirmPassword)
            {
                ModelState.AddModelError("", "Passwords do not match.");
                return Page();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                ModelState.AddModelError("", "User not found.");
                return Page();
            }

            // ❌ LƯU MẬT KHẨU KHÔNG HASH (KHÔNG AN TOÀN)
            user.Password = NewPassword;
            await _context.SaveChangesAsync();

            // Xóa session
            HttpContext.Session.Remove("Email");
            HttpContext.Session.Remove("OTP_Purpose");

            return RedirectToPage("/FormLogin/Login");
        }

    }
}
