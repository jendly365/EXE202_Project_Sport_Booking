using System;
using System.Collections.Generic;

namespace EXE202_Project_Sport_Booking.Models
{
    public partial class User
    {
        public User()
        {
            Bookings = new HashSet<Booking>();
            Courts = new HashSet<Court>();
            Reviews = new HashSet<Review>();
        }

        public int UserId { get; set; }
        public int? RoleId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
        public string? Address { get; set; }
        public string? AvatarUrl { get; set; }
        public bool IsVerified { get; set; }
        public DateTime? PasswordResetSentAt { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public virtual Role? Role { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<Court> Courts { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
