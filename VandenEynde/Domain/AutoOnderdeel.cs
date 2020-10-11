namespace Domain
{
    public class AutoOnderdeel
    {
        public int AutoId { get; set; }
        public Auto Auto { get; set; }
        public int OnderdeelId { get; set; }
        public Onderdeel Onderdeel { get; set; }
    }
}
