using System;
using System.Collections.Generic;

namespace EXE202_Project_Sport_Booking.Models
{
    public partial class SubCourt
    {
        public SubCourt()
        {
            Bookings = new HashSet<Booking>();
            Reviews = new HashSet<Review>();
        }

        public int SubCourtId { get; set; }
        public int? CourtId { get; set; }
        public string? SubCourtName { get; set; }
        public string? Description { get; set; }
        public double? PricePerHour { get; set; }

        public virtual Court? Court { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
