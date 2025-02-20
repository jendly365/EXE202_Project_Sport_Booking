using EXE202_Project_Sport_Booking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EXE202_Project_Sport_Booking.Components
{
	public class HeaderViewComponent : ViewComponent
	{
		private readonly EXE201_Rental_Sport_FieldContext _context;

		public HeaderViewComponent(EXE201_Rental_Sport_FieldContext context)
		{
			_context = context;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var categories = await _context.Categories.ToListAsync();
			return View(categories);
		}
	}
}
