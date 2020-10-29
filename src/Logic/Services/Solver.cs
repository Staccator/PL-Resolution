using System.Collections.Generic;
using System.Linq;
using System.Text;
using PL_Resolution.Logic.Models;

namespace PL_Resolution.Logic.Services
{
    public class Solver
    {
        private readonly Dictionary<int, string> _indexToName;
        private readonly bool _showFullNames = true;

        public Solver(Dictionary<int, string> indexToName)
        {
            _indexToName = indexToName;
        }

        public (bool result, string log) FindResolution(List<Clause> inputClauses)
        {
            var log = new StringBuilder();
            log.AppendLine("Wejście Programu");
            foreach (var clause in inputClauses) log.AppendLine(ClauseToString(clause));
            var courseIndex = 0;
            var nextClauseIndex = inputClauses.Max(c => c.Index) + 1;
            var allClauses = inputClauses.ToList();
            do
            {
                log.AppendLine($"\nPrzebieg {++courseIndex}:");
                var newClauses = new List<Clause>();

                for (var i = 0; i < allClauses.Count - 1; i++)
                {
                    var ci = allClauses[i];
                    for (var j = i + 1; j < allClauses.Count; j++)
                    {
                        var cj = allClauses[j];
                        var resolvents = ResolveTwoClauses(ci, cj);

                        if (resolvents.Any(r => r.Empty))
                        {
                            var emptyClause = resolvents.First(r => r.Empty);
                            log.AppendLine($"Pusta Klauzula: <- {emptyClause.Ancestors}");
                            return (true, log.ToString());
                        }

                        newClauses.AddRange(resolvents);
                    }
                }

                var distinct = newClauses.Distinct().ToList();
                var unique = distinct.Where(d => allClauses.All(a => a != d)).ToList();
                if (!unique.Any())
                {
                    log.AppendLine("Nie znaleziono żadnych nowych klauzul");
                    return (false, log.ToString());
                }

                for (var i = 0; i < unique.Count; i++)
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
            var ancestors = !(clause.Ancestors is null) ? $"<- {clause.Ancestors}" : "";
            var literals = clause.Literals.ToList();
            var first = literals.First();
            var clauseString = (first.IsNegated ? "-" : "") + (!_showFullNames ? first.ToString() : _indexToName[first.Id]) + " ";
            for (var i = 1; i < literals.Count; i++)
            {
                var literal = _showFullNames ? _indexToName[literals[i].Id] : literals[i].ToString();
                var optionalDash = literals[i].IsNegated ? "-" : "";
                clauseString += $" v {optionalDash}{literal}";
            }

            return $"{clause.Index}. {ancestors} : {clauseString}";
        }
    }
}