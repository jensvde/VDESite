using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class OnderdeelBestelnummer
    {
        [Key]
        public int BestelnummerId { get; set; }
        public string Nr { get; set; }
        public string Url { get; set; }
        public virtual Onderdeel Onderdeel { get; set; }
    }
}
