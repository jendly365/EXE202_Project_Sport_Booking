using EXE202_Project_Sport_Booking.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EXE202_Project_Sport_Booking.Pages.Customers
{
	public class ViewCourtListModel : PageModel
	{
		private readonly EXE201_Rental_Sport_FieldContext _context;

		public ViewCourtListModel(EXE201_Rental_Sport_FieldContext context)
		{
			_context = context;
		}

		public List<Category> Categories { get; set; } = new List<Category>();
		public List<Court> Courts { get; set; } = new List<Court>();
		public string CategoryName { get; set; }
		public int PageSize { get; set; } = 6; // 6 sân mỗi trang
		public int CurrentPage { get; set; }
		public int TotalPages { get; set; }
		public List<int> SelectedCategoryIds { get; set; } = new List<int>();
		public string SearchQuery { get; set; } = "";


		// Thêm tham số categoryId để lọc các sân theo danh mục
		public async Task OnGetAsync(int pageNumber = 1, List<int> selectedCategoryIds = null, string searchQuery = "", string addressQuery = "")
		{
			// Lưu lại giá trị searchQuery để hiển thị lại trên giao diện
			SearchQuery = searchQuery;

			// Nếu có danh sách selectedCategoryIds từ URL, gán vào SelectedCategoryIds
			if (selectedCategoryIds != null && selectedCategoryIds.Any())
			{
				SelectedCategoryIds = selectedCategoryIds;
			}

			// Lấy danh sách các Category để hiển thị trong form lọc
			Categories = await _context.Categories.ToListAsync();

			var query = _context.Courts.AsQueryable();

			// Lọc theo SelectedCategoryIds nếu có
			if (SelectedCategoryIds.Any())
			{
				query = query.Where(c => SelectedCategoryIds.Contains(c.CategoryId));
				if (SelectedCategoryIds.Count == 1)
				{
					var selectedCategory = await _context.Categories
						.Where(c => SelectedCategoryIds.Contains(c.CategoryId))
						.Select(c => c.CategoryName)
						.FirstOrDefaultAsync();
					CategoryName = selectedCategory ?? "Tất cả sân";
				}
				else
				{
					CategoryName = "Nhiều danh mục";
				}
			}
			else
			{
				CategoryName = "Tất cả sân";
			}

			// Tìm kiếm theo tên sân
			if (!string.IsNullOrWhiteSpace(SearchQuery))
			{
				query = query.Where(c => c.CourtName.Contains(SearchQuery));
			}

			// Tìm kiếm theo địa điểm
			if (!string.IsNullOrWhiteSpace(addressQuery))
			{
				query = query.Where(c => c.Address.Contains(addressQuery));
			}

			// Tính tổng số sân và số trang
			int totalCourts = await query.CountAsync();
			TotalPages = (int)Math.Ceiling(totalCourts / (double)PageSize);
			CurrentPage = pageNumber;

			// Lấy danh sách sân theo trang
			Courts = await query.Skip((pageNumber - 1) * PageSize)
								.Take(PageSize)
								.ToListAsync();
		}




	}
}
