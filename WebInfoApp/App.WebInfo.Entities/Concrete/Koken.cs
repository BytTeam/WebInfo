using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Entites;

namespace App.WebInfo.Entities.Concrete
{
    public class Koken : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int KokenId { get; set; }

        [MaxLength(100)]
        public string KokenName { get; set; }
    }

}
