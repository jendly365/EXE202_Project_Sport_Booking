using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using EXE202_Project_Sport_Booking.Models;
using Microsoft.EntityFrameworkCore;

namespace EXE202_Project_Sport_Booking.Pages.FormLogin
{
    public class LoginModel : PageModel
    {
        private readonly EXE201_Rental_Sport_FieldContext _context;

        public LoginModel(EXE201_Rental_Sport_FieldContext context)
        {
            _context = context;
        }
        [BindProperty]
        [Required]
        public string Email { get; set; }

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _context.Users.Include(u => u.Role)
                                           .FirstOrDefaultAsync(u => u.Email == Email && u.Password == Password);

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, user.Role?.RoleName ?? "Guest")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties { IsPersistent = true };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                // Redirect based on role
                return user.RoleId switch
                {
                    1 => RedirectToPage("/Admin/Dashboard"),          // Admin Page
                    2 => RedirectToPage("/Manager/Home"),             // Manager Page
                    3 => RedirectToPage("/Index"),            // Customer Page
                    4 => RedirectToPage("/Staff/Home"),               // Staff Page
                    5 => RedirectToPage("/Owner/Home"),               // Owner Page
                    6 => RedirectToPage("/BookingManager/Home"),      // Booking Manager Page
                    7 => RedirectToPage("/Guest/Home"),               // Guest Page
                    8 => RedirectToPage("/FieldTechnician/Home"),     // Field Technician Page
                    9 => RedirectToPage("/EventOrganizer/Home"),      // Event Organizer Page
                    10 => RedirectToPage("/MarketingSpecialist/Home"), // Marketing Specialist Page
                    _ => RedirectToPage("/Index")                     // Default Page
                };
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }

        public void OnGet()
        {
        }
    }
}
