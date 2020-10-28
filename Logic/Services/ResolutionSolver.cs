using System.Collections.Generic;
using System.Linq;
using PL_Resolution.Logic.Models;

namespace PL_Resolution.Logic.Services
{
    public class ResolutionSolver
    {
        private bool shouldDiscardTautologies = true;
        public bool FindResolution(List<Clause> clauses)
        {
            var newClauses = new HashSet<Clause>();
            do
            {
                for (int i = 0; i < clauses.Count - 1; i++)
                {
                    Clause ci = clauses[i];
                    for (int j = i + 1; j < clauses.Count; j++)
                    {
                        Clause cj = clauses[j];
                        HashSet<Clause> resolvents = PlResolve(ci, cj);
                        if (resolvents.Contains(Clause.Empty))
                        {
                            return true;
                        }

                        newClauses.UnionWith(resolvents);
                    }
                }

                // if new is subset of clauses then return false
                if (newClauses.All(clauses.Contains))
                {
                    return false;
                }

                // clauses <- clauses U new
                clauses.AddRange(newClauses);
            } while (true);
        }

        public HashSet<Clause> PlResolve(Clause ci, Clause cj)
        {
            HashSet<Clause> resolvents = new HashSet<Clause>();

            // The complementary positive literals from C_i
            ResolvePositiveWithNegative(ci, cj, resolvents);
            // The complementary negative literals from C_i
            ResolvePositiveWithNegative(cj, ci, resolvents);

            return resolvents;
        }


        protected void ResolvePositiveWithNegative(Clause c1, Clause c2,
            HashSet<Clause> resolvents)
        {
            // // Calculate the complementary positive literals from c1 with
            // // the negative literals from c2
            // HashSet<PropositionSymbol> complementary = HashSetOps.intersection(
            //     c1.getPositiveSymbols(), c2.getNegativeSymbols());
            // // Construct a resolvent clause for each complement found
            // for (PropositionSymbol complement :
            // complementary) {
            //     List<Literal> resolventLiterals = new ArrayList<Literal>();
            //     // Retrieve the literals from c1 that are not the complement
            //     foreach (Literal c1l in c1.getLiterals()) {
            //         if (c1l.isNegativeLiteral()
            //             || !c1l.getAtomicSentence().equals(complement))
            //         {
            //             resolventLiterals.add(c1l);
            //         }
            //     }
            //     // Retrieve the literals from c2 that are not the complement
            //     for (Literal c2l :
            //     c2.getLiterals()) {
            //         if (c2l.isPositiveLiteral()
            //             || !c2l.getAtomicSentence().equals(complement))
            //         {
            //             resolventLiterals.add(c2l);
            //         }
            //     }
            //     // Construct the resolvent clause
            //     Clause resolvent = new Clause(resolventLiterals);
            //     // Discard tautological clauses if this optimization is turned on.
            //     if (!(isDiscardTautologies() && resolvent.isTautology()))
            //     {
            //         resolvents.add(resolvent);
            //     }
            // }
        }

        protected void DiscardTautologies(HashSet<Clause> clauses)
        {
            if (shouldDiscardTautologies)
            {
                clauses.RemoveWhere(c => c.IsTautology);
            }
        }
    }
}