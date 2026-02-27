using System.ComponentModel.DataAnnotations;

namespace ChandiTechnologyWebApi.Model
{
    public class AuditLog
    {
        [Key]
        public int LogID { get; set; } 
        public string EventType { get; set; }
        public string Details { get; set; }
        public int? AgentID { get; set; } 
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;



    }
}
