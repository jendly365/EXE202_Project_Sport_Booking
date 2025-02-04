using System;
using System.Collections.Generic;

namespace EXE202_Project_Sport_Booking.Models
{
    public partial class Booking
    {
        public Booking()
        {
            Payments = new HashSet<Payment>();
        }

        public int BookingId { get; set; }
        public int? UserId { get; set; }
        public int? SubCourtId { get; set; }
        public string? Status { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public virtual SubCourt? SubCourt { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
