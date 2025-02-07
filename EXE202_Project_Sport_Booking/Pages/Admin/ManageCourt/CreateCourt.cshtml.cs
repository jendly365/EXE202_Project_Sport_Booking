using EXE202_Project_Sport_Booking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EXE202_Project_Sport_Booking.Pages.Admin.ManageCourt
{
    public class CreateCourtModel : PageModel
    {

        private readonly EXE201_Rental_Sport_FieldContext _context;

        private readonly IWebHostEnvironment _environment;

        public CreateCourtModel(EXE201_Rental_Sport_FieldContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public List<Category> Categories { get; set; }

        public List<string> statusCourt { get; set; }
        public void OnGet()
        {
            Categories = _context.Categories.ToList();  

            statusCourt = _context.Courts.Select(st => st.Status).ToList();
        }

        [BindProperty]
        public IFormFile ImageFile { get; set; }

        [BindProperty]
        public Court Court { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if(ImageFile != null)
            {
                var uploadFolder = Path.Combine(_environment.WebRootPath, "uploads");

                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                var fileName = $"{Guid.NewGuid()}_{ImageFile.FileName}";
                var filePath = Path.Combine(_environment.WebRootPath, "uploads", fileName);

                using(var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(fileStream);
                }
                Court.ImageUrl = $"uploads/{fileName}";
            }

            //Court.CategoryId = int.Parse(Request.Form["CategoryId"]);
            _context.Courts.Add(Court);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Admin/ManageCourt/GetListCourts");
        }
    }
}
