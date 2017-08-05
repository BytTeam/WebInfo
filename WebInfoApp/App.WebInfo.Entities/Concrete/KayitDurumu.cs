using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Entites;

namespace App.WebInfo.Entities.Concrete
{
    public class KayitDurumu : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int KayitDurumuId { get; set; }
      
        [MaxLength(100)]
        public string KayitDurumuName { get; set; }
    }

}
