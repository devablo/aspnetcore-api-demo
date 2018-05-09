using System.ComponentModel.DataAnnotations;

namespace Devable.Demo.AspNetCore.API.Models.Domain
{
    public class Station
    {
        public int Id { get; set; }

        [Required]
        public string CallSign { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string State { get; set; }

        public string City { get; set; }
    }
}
