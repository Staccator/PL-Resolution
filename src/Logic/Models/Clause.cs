using System.Collections.Generic;
using System.Linq;

namespace PL_Resolution.Logic.Models
{
    public class Clause
    {
        public int Index { get; set; }
        public (int, int)? Ancestors { get; private set; }
        private HashSet<Literal> _literals;
        public HashSet<Literal> Literals => _literals;

        public Clause(IEnumerable<Literal> literals)
        {
            _literals = new HashSet<Literal>(literals);
        }

        public bool Empty => !_literals.Any();

        public Clause ResolveWith(Clause otherClause, Literal literal)
        {
            var result = _literals.Where(l => l != literal).ToHashSet();
            foreach (var other in otherClause.Literals)
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
            
            return new Clause(result)
            {
                Ancestors = (Index, otherClause.Index)
            };
        }
    }
}