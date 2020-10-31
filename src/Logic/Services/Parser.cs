using System;
using System.Collections.Generic;
using System.Linq;
using PL_Resolution.Logic.Models;

namespace PL_Resolution.Logic.Services
{
    public static class Parser
    {
        public static List<Clause> Parse(string[] fileLines)
        {
            var symbolsPhase = true;
            var symbolToName = new Dictionary<string, string>();
            var clauseIndex = 0;

            var resultClauses = new List<Clause>();

            for (var i = 0; i < fileLines.Length; i++)
            {
                var line = fileLines[i];
                if (string.IsNullOrWhiteSpace(line))
                {
                    symbolsPhase = false;
                    continue;
                }

                var split = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (symbolsPhase)
                {
                    if (split.Length < 2)
                        throw new ParseException(i + 1, "There must be at least 2 elements in line - symbol and name");

                    var symbol = split[0];
                    var name = split[1];

                    symbolToName[symbol] = name;
                }
                else
                {
                    var literals = new List<Literal>();
                    for (var j = 0; j < split.Length; j++)
                    {
                        var symbol = split[j];
                        var negation = false;
                        if (symbol.First() == Constants.NEG_INPUT)
                        {
                            negation = true;
                            symbol = symbol.Substring(1);
                        }

                        if (!symbolToName.ContainsKey(symbol))
                            throw new ParseException(i + 1, "Unrecognized symbol");

                        var name = symbolToName[symbol];
                        var literal = new Literal(symbol, name, negation);
                        literals.Add(literal);
                    }

                    var clause = new Clause(literals)
                    {
                        Index = ++clauseIndex
                    };
                    resultClauses.Add(clause);
                }
            }

            return resultClauses;
        }
    }
}