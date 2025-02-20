using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EXE202_Project_Sport_Booking.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EXE202_Project_Sport_Booking.Pages.Customers
{
    public class HomePageModel : PageModel
    {
        public readonly EXE201_Rental_Sport_FieldContext _context;

        public HomePageModel(EXE201_Rental_Sport_FieldContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string? SearchName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchLocation { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? SelectedCategory { get; set; }

        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Court> Courts { get; set; } = new List<Court>(); // Danh sách sân tìm được

        public async Task OnGetAsync()
        {
            ViewData["Categories"] = await _context.Categories.ToListAsync();
            // Lấy danh sách các loại sân
            Categories = await _context.Categories.ToListAsync();

            // Query cơ sở dữ liệu với điều kiện lọc
            var query = _context.Courts.Include(c => c.Category).AsQueryable();

            if (!string.IsNullOrEmpty(SearchName))
            {
                query = query.Where(c => c.CourtName.Contains(SearchName));
            }

            if (!string.IsNullOrEmpty(SearchLocation))
            {
                query = query.Where(c => c.Address.Contains(SearchLocation));
            }

            if (SelectedCategory.HasValue && SelectedCategory.Value > 0)
            {
                query = query.Where(c => c.CategoryId == SelectedCategory.Value);
            }

            // Lấy danh sách sân sau khi lọc
            Courts = await query.ToListAsync();
        }
    }
}
