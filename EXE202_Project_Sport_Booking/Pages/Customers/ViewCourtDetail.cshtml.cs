using EXE202_Project_Sport_Booking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EXE202_Project_Sport_Booking.Pages.Customers
{
	public class ViewCourtDetailModel : PageModel
	{
		private readonly EXE201_Rental_Sport_FieldContext _context;

		public ViewCourtDetailModel(EXE201_Rental_Sport_FieldContext context)
		{
			_context = context;
		}

		public Court Court { get; set; }
		public List<Category> Categories { get; set; } = new List<Category>();
		public List<SubCourt> SubCourts { get; set; } = new List<SubCourt>();

		[BindProperty(SupportsGet = true)]
		public int PageNumber { get; set; } = 1;
		public int PageSize { get; set; } = 3;
		public int TotalPages { get; set; }

		public async Task<IActionResult> OnGetAsync(int id, int pageNumber = 1)
		{
			if (id == 0)
			{
				return NotFound();
			}

			// Lấy Court cùng với Category, User và SubCourts
			Court = await _context.Courts
				.Include(c => c.Category)
				.Include(c => c.User)
				.Include(c => c.SubCourts)  // Load danh sách SubCourt
				.FirstOrDefaultAsync(c => c.CourtId == id);

			if (Court == null)
			{
				return NotFound();
			}

			Categories = await _context.Categories.ToListAsync();

			// Lấy tất cả SubCourts của Court
			var allSubCourts = await _context.SubCourts
				.Where(s => s.CourtId == id)
				.Include(s => s.Reviews)  // Load Review của từng SubCourt
				.ThenInclude(r => r.User)  // Load User của Review
				.ToListAsync();

			// Gộp tất cả Review từ các SubCourt
			var allReviews = allSubCourts.SelectMany(s => s.Reviews).ToList();

			// Gán danh sách Review vào Model
			ViewData["Reviews"] = allReviews;

			// Phân trang SubCourt
			TotalPages = (int)System.Math.Ceiling(allSubCourts.Count / (double)PageSize);
			SubCourts = allSubCourts.Skip((pageNumber - 1) * PageSize).Take(PageSize).ToList();
			PageNumber = pageNumber;

			return Page();
		}

	}
}
