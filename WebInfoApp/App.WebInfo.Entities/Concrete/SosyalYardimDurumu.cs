using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Core.Entites;

namespace App.WebInfo.Entities.Concrete
{
    public class SosyalYardimDurumu : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SosyalYardimDurumuId { get; set; }

        [MaxLength(100)]
        public string SosyalYardimDurumuName { get; set; }

    }

}
