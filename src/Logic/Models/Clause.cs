using System;
using System.Collections.Generic;
using System.Linq;

namespace PL_Resolution.Logic.Models
{
    public struct Clause : IEquatable<Clause>
    {
        public int Index { get; set; }
        public (int, int)? Ancestors { get; private set; }
        public List<Literal> Literals { get; }
        public HashSet<Literal> LiteralsSet { get; }

        public Clause(IEnumerable<Literal> literals)
        {
            Literals = literals.ToList();
            LiteralsSet = new HashSet<Literal>(literals);
            Ancestors = null;
            Index = -1;
        }

        public bool Equals(Clause other)
        {
            return LiteralsSet.SetEquals(other.LiteralsSet);
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


        public bool Empty => !Literals.Any();

        public Clause ResolveWith(Clause otherClause, Literal literal)
        {
            var result = Literals.Where(l => l != literal).ToList();
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

        public override string ToString()
        {
            var ancestors = Ancestors != null ? $"<- {Ancestors}" : "";

            var clauseString = String.Join($" {Constants.ALT} ", Literals);

            return $"{Index}. {ancestors} : {clauseString}";
        }
    }
}