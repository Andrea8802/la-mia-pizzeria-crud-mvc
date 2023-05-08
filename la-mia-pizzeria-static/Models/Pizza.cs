using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace la_mia_pizzeria_static.Models
{
    [Table("pizza")]
    public class Pizza
    {
        [Key]
        public long Id { get; set; }

        [StringLength(32, ErrorMessage = "Il nome può avere massimo 32 caratteri")]
        [Required(ErrorMessage = "Il nome è richiesto")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "La descrizione è richiesta")]
        [MinFiveWords]
        public string? Descrizione { get; set; }

        [Required(ErrorMessage = "Il prezzo è richiesto")]
        [Range(0.01, 100.00, ErrorMessage = "La pizza deve avere un valore fra 0,01 e 100")]

        public decimal Prezzo { get; set; }

        [Required(ErrorMessage = "L'Immagine è richiesta")]
        [Url]
        public string Img { get; set; }

    }
}
