namespace PL_Resolution.Logic.Models
{
    public class Literal
    {
        public Literal(int id, bool negated)
        {
            Id = id;
            Negated = negated;
        }

        public int Id { get; }
        public bool Negated { get; set; }
    }
}