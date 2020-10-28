using System.Collections.Generic;

namespace PL_Resolution.Logic.Models
{
    public class Clause
    {
        private List<Literal> literals;

        public Clause(List<Literal> literals)
        {
            this.literals = literals;
        }

        public static Clause Empty => new Clause(new List<Literal>());

        public bool IsTautology => throw new System.NotImplementedException();
    }
}