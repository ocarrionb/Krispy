using System.ComponentModel.DataAnnotations;

namespace Krispy.Models
{
    public class Sales
    {
        [Key]
        public int Id { get; set; }

        public required string UserName { get; set; }

        public required string Direction { get; set; }

        public int Type { get; set; }

        public required int Amount { get; set; }
    }
}
