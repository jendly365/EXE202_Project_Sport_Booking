using System;
using System.Collections.Generic;

namespace EXE202_Project_Sport_Booking.Models
{
    public partial class Category
    {
        public Category()
        {
            Courts = new HashSet<Court>();
        }

        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<Court> Courts { get; set; }
    }
}
