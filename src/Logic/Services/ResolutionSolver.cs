using System.Collections.Generic;
using System.Linq;
using System.Text;
using PL_Resolution.Logic.Models;

namespace PL_Resolution.Logic.Services
{
    public class ResolutionSolver
    {
        private readonly Dictionary<int, string> _indexToName;
        private readonly bool _shouldDiscardTautologies;

        public ResolutionSolver(Dictionary<int, string> indexToName, bool shouldDiscardTautologies)
        {
            _indexToName = indexToName;
            _shouldDiscardTautologies = shouldDiscardTautologies;
        }

        public bool FindResolution(List<Clause> inputClauses)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Wejście Programu");
            int courseIndex = 0;
            int nextClauseIndex = inputClauses.Max(c => c.Index) + 1;
            var allClauses = inputClauses.ToHashSet();
            do
            {
                sb.AppendLine($"Przebieg {++courseIndex}:");
                var newClauses = new HashSet<Clause>();
                var allClausesList = allClauses.ToList();
                for (var i = 0; i < allClausesList.Count - 1; i++)
                {
                    var ci = allClausesList[i];
                    for (var j = i + 1; j < allClausesList.Count; j++)
                    {
                        var cj = allClausesList[j];
                        var resolvents = ResolveTwoClauses(ci, cj);
                        
                        // found solution
                        if (resolvents.Any(r => r.Empty)) return true;

                        newClauses.UnionWith(resolvents);
                    }
                }

                // no new clauses found
                if (newClauses.IsSubsetOf(allClauses)) return false;

                var uniqueNewClauses = allClauses.Except(newClauses);
                foreach (var clause in uniqueNewClauses)
                {
                    allClauses.Add(clause);
                    sb.AppendLine(ClauseToString(clause));
                }
                
            } while (true);
        }

        private HashSet<Clause> ResolveTwoClauses(Clause c1, Clause c2)
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

        private string ClauseToString(Clause clause)
        {
            var ancestors = !(clause.Ancestors is null) ? $"z ${clause.Ancestors}" : "";
            var literals = clause.Literals.ToList();
            var clauseString = literals.First().ToString() + " ";
            for (int i = 1; i < literals.Count; i++)
            {
                clauseString += $" v {literals[i]}";
            }

            return $"{clause.Index}. {ancestors} : {clauseString}";
        }
    }
}