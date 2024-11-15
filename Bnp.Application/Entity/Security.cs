
using System.ComponentModel.DataAnnotations;

namespace Bnp.Application.Entity
{
    public class Security
    {
        [Key]
        public int Id { get; set; }
        public string Isin { get; set; }
        public decimal Price { get; set; }
    }
}
