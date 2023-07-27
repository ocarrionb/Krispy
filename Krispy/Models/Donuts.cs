using System.ComponentModel.DataAnnotations;

namespace Krispy.Models
{
    public class Donuts
    {
        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }
    }
}
