using System.Collections.Generic;
using System.Linq;
using System.Text;
using PL_Resolution.Logic.Models;

namespace PL_Resolution.Logic.Services
{
    public class Solver
    {
        private readonly Dictionary<int, string> _indexToName;
        private readonly bool _shouldDiscardTautologies;

        public Solver(Dictionary<int, string> indexToName, bool shouldDiscardTautologies)
        {
            _indexToName = indexToName;
            _shouldDiscardTautologies = shouldDiscardTautologies;
        }

        public (bool result, string log) FindResolution(List<Clause> inputClauses)
        {
            var log = new StringBuilder();
            log.AppendLine("Wejście Programu");
            foreach (var clause in inputClauses)
            {
                log.AppendLine(ClauseToString(clause));
            }
            int courseIndex = 0;
            int nextClauseIndex = inputClauses.Max(c => c.Index) + 1;
            var allClauses = inputClauses.ToList();
            do
            {
                log.AppendLine($"Przebieg {++courseIndex}:");
                var newClauses = new List<Clause>();
                
                for (var i = 0; i < allClauses.Count - 1; i++)
                {
                    var ci = allClauses[i];
                    for (var j = i + 1; j < allClauses.Count; j++)
                    {
                        var cj = allClauses[j];
                        var resolvents = ResolveTwoClauses(ci, cj);
                        
                        // found solution
                        if (resolvents.Any(r => r.Empty))
                        {
                            log.AppendLine(ClauseToString(resolvents.First(r => r.Empty)));
                            return (true, log.ToString());
                        }

                        newClauses.AddRange(resolvents);
                    }
                }

                // remove tautologies
                var distinct = newClauses.Distinct().ToList();
                var unique = distinct.Where(d => allClauses.All(a => a != d)).ToList();
                if (!unique.Any())
                {
                    log.AppendLine("Nie znaleziono żadnych nowych klauzul");
                    return (false, log.ToString());
                }
                
                for (int i = 0; i < unique.Count; i++)
                {
                    var clause = unique[i];
                    clause.Index = nextClauseIndex++;
                    allClauses.Add(clause);
                    log.AppendLine(ClauseToString(clause));
                }
            } while (true);
        }

        private List<Clause> ResolveTwoClauses(Clause c1, Clause c2)
        {
            var resolvents = new List<Clause>();
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
            var ancestors = !(clause.Ancestors is null) ? $"z {clause.Ancestors}" : "";
            var literals = clause.Literals.ToList();
            var clauseString = literals.Any() ? literals.First().ToString() + " " : "";
            for (int i = 1; i < literals.Count; i++)
            {
                clauseString += $" v {literals[i]}";
            }

            return $"{clause.Index}. {ancestors} : {clauseString}";
        }
    }
}