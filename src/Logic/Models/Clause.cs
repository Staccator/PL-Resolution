using System;
using System.Collections.Generic;
using System.Linq;

namespace PL_Resolution.Logic.Models
{
    public struct Clause : IEquatable<Clause>
    {
        public bool Equals(Clause other)
        {
            return _literals.SetEquals(other._literals);
        }

        public override bool Equals(object obj)
        {
            return obj is Clause other && Equals(other);
        }

        public static bool operator ==(Clause left, Clause right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Clause left, Clause right)
        {
            return !left.Equals(right);
        }

        public int Index { get; set; }
        public (int, int)? Ancestors { get; private set; }
        private readonly HashSet<Literal> _literals;
        public HashSet<Literal> Literals => _literals;

        public Clause(IEnumerable<Literal> literals)
        {
            _literals = new HashSet<Literal>(literals);
            Ancestors = null;
            Index = -1;
        }

        public bool Empty => !_literals.Any();

        public Clause ResolveWith(Clause otherClause, Literal literal)
        {
            var result = _literals.Where(l => l != literal).ToHashSet();
            foreach (var other in otherClause.Literals)
            {
                if (other.Id == literal.Id)
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