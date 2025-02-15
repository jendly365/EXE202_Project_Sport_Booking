using EXE202_Project_Sport_Booking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EXE202_Project_Sport_Booking.Pages.Owner
{
    public class GetListCourtModel : PageModel
    {

        private readonly EXE201_Rental_Sport_FieldContext _context;

        public GetListCourtModel(EXE201_Rental_Sport_FieldContext context)
        {
            _context = context;
        }
        
        public IList<Court> Courts { get; set; }

        public async Task OnGetAsync()
        {
            Courts = await _context.Courts
                       .Include(c => c.Category)
                       .ToListAsync();
        }
    }
}
