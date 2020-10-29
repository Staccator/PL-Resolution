using System.Collections.Generic;

namespace PL_Resolution.Logic.Models
{
    public class ParseResult
    {
        public ParseResult(List<Clause> clauses, Dictionary<int, string> indexToName)
        {
            Clauses = clauses;
            IndexToName = indexToName;
        }

        public List<Clause> Clauses { get; }
        public Dictionary<int, string> IndexToName { get; }
    }
}