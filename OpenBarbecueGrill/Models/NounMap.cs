namespace OpenBarbecueGrill.Models
{
    public record class NounMap
    {
        public NounMap(string origin, string dest)
        {
            Origin = origin;
            Dest = dest;
        }

        public string Origin { get; set; }
        public string Dest { get; set; }
    }
}
