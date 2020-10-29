using System.Collections.Generic;
using System.Linq;

namespace PL_Resolution.Logic.Models
{
    public class Clause
    {
        private HashSet<Literal> _literals;
        public HashSet<Literal> Literals => _literals;

        public Clause(IEnumerable<Literal> literals)
        {
            _literals = new HashSet<Literal>(literals);
        }

        public static Clause Empty => new Clause(new List<Literal>());

        public bool IsTautology => throw new System.NotImplementedException();

        public Clause ResolveWith(Clause c2, Literal literal)
        {
            var result = _literals.Where(l => l != literal).ToHashSet();
            foreach (var other in c2.Literals)
            {
                if (result.Contains(other.Negation))
                {
                    result.Remove(other.Negation);
                }
                else
                {
                    result.Add(other);
                }
            }

            return new Clause(result);
        }
    }
}