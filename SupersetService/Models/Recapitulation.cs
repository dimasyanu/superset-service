using IDX_DPS.Utility.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupersetService.Models
{
    [Table("SS_Recapitulation")]
    public class Recapitulation
    {
        [Key]
        public int IDRecap { get; set; }
        
        [TransactionKey]
        public DateTime? Date { get; set; }
        
        [TransactionKey]
        public string? IDInstrument { get; set; }
        
        [TransactionKey]
        public string? IDBoard { get; set; }

        public decimal? Volume { get; set; }
        public decimal? Value { get; set; }
        public decimal? Frequency { get; set; }
    }
}
