using System.ComponentModel.DataAnnotations;

namespace ChandiTechnologyWebApi.Model
{
    public class HotelBooking
    {

        [Key]
        public int BookingID { get; set; } 
        public int AgentID { get; set; } 
        public string HotelID { get; set; }
        public string HotelName { get; set; }
        public string City { get; set; }
        public DateTime Checkin { get; set; }
        public DateTime CheckOut { get; set; }
        public int Guests { get; set; }
        public decimal TotalPrice { get; set; }
        public string BookingStatus { get; set; }
        public string APIResponse { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

       

        public Agent Agent { get; set; }
    }
}
