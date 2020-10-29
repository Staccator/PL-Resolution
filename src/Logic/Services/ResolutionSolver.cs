using System.Collections.Generic;
using System.Linq;
using PL_Resolution.Logic.Models;

namespace PL_Resolution.Logic.Services
{
    public static class ResolutionSolver
    {
        private static readonly bool shouldDiscardTautologies = true;

        public static bool FindResolution(ParseResult parseResult)
        {
            var clauses = parseResult.Clauses.ToHashSet();
            var newClauses = new HashSet<Clause>();
            do
            {
                var clausesList = clauses.ToList();
                for (var i = 0; i < clausesList.Count - 1; i++)
                {
                    var ci = clausesList[i];
                    for (var j = i + 1; j < clausesList.Count; j++)
                    {
                        var cj = clausesList[j];
                        var resolvents = ResolveTwoClauses(ci, cj);
                        if (resolvents.Contains(Clause.Empty)) return true;

                        newClauses.UnionWith(resolvents);
                    }
                }

                // if new is subset of clauses then return false
                if (newClauses.IsSubsetOf(clauses)) return false;

                // clauses <- clauses U new
                clauses.UnionWith(newClauses);
            } while (true);
        }

        private static HashSet<Clause> ResolveTwoClauses(Clause c1, Clause c2)
        {
            var resolvents = new HashSet<Clause>();
            foreach (var literal in c1.Literals)
                if (c2.Literals.Contains(literal.Negation))
                {
                    var resolvent = c1.ResolveWith(c2, literal);
                    resolvents.Add(resolvent);
                }

            return resolvents;
        }

        private static void DiscardTautologies(HashSet<Clause> clauses)
        {
            if (shouldDiscardTautologies) clauses.RemoveWhere(c => c.IsTautology);
        }
    }
}