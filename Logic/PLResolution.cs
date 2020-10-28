using System.Collections.Generic;

namespace PL_Resolution.Logic
{
    public class PLResolution
    {
        public boolean plResolution(KnowledgeBase kb, Sentence alpha)
        {
            // clauses <- the set of clauses in the CNF representation
            // of KB & ~alpha
            Set<Clause> clauses = setOfClausesInTheCNFRepresentationOfKBAndNotAlpha(
                kb, alpha);
            // new <- {}
            Set<Clause> newClauses = new LinkedHashSet<Clause>();
            // loop do
            do
            {
                // for each pair of clauses C_i, C_j in clauses do
                List<Clause> clausesAsList = new ArrayList<Clause>(clauses);
                for (int i = 0; i < clausesAsList.size() - 1; i++)
                {
                    Clause ci = clausesAsList.get(i);
                    for (int j = i + 1; j < clausesAsList.size(); j++)
                    {
                        Clause cj = clausesAsList.get(j);
                        // resolvents <- PL-RESOLVE(C_i, C_j)
                        Set<Clause> resolvents = plResolve(ci, cj);
                        // if resolvents contains the empty clause then return true
                        if (resolvents.contains(Clause.EMPTY))
                        {
                            return true;
                        }

                        // new <- new U resolvents
                        newClauses.addAll(resolvents);
                    }
                }

                // if new is subset of clauses then return false
                if (clauses.containsAll(newClauses))
                {
                    return false;
                }

                // clauses <- clauses U new
                clauses.addAll(newClauses);
            } while (true);
        }

        public Set<Clause> plResolve(Clause ci, Clause cj)
        {
            Set<Clause> resolvents = new LinkedHashSet<Clause>();

            // The complementary positive literals from C_i
            resolvePositiveWithNegative(ci, cj, resolvents);
            // The complementary negative literals from C_i
            resolvePositiveWithNegative(cj, ci, resolvents);

            return resolvents;
        }

        private boolean discardTautologies = true;

        public PLResolution()
        {
            this(true);
        }

        protected Set<Clause> setOfClausesInTheCNFRepresentationOfKBAndNotAlpha(
            KnowledgeBase kb, Sentence alpha)
        {
            // KB & ~alpha;
            Sentence isContradiction = new ComplexSentence(Connective.AND,
                kb.asSentence(), new ComplexSentence(Connective.NOT, alpha));
            // the set of clauses in the CNF representation
            Set<Clause> clauses = new LinkedHashSet<Clause>(
                ConvertToConjunctionOfClauses.convert(isContradiction)
                    .getClauses());

            discardTautologies(clauses);

            return clauses;
        }

        protected void resolvePositiveWithNegative(Clause c1, Clause c2,
            Set<Clause> resolvents)
        {
            // Calculate the complementary positive literals from c1 with
            // the negative literals from c2
            Set<PropositionSymbol> complementary = SetOps.intersection(
                c1.getPositiveSymbols(), c2.getNegativeSymbols());
            // Construct a resolvent clause for each complement found
            for (PropositionSymbol complement :
            complementary) {
                List<Literal> resolventLiterals = new ArrayList<Literal>();
                // Retrieve the literals from c1 that are not the complement
                for (Literal c1l :
                c1.getLiterals()) {
                    if (c1l.isNegativeLiteral()
                        || !c1l.getAtomicSentence().equals(complement))
                    {
                        resolventLiterals.add(c1l);
                    }
                }
                // Retrieve the literals from c2 that are not the complement
                for (Literal c2l :
                c2.getLiterals()) {
                    if (c2l.isPositiveLiteral()
                        || !c2l.getAtomicSentence().equals(complement))
                    {
                        resolventLiterals.add(c2l);
                    }
                }
                // Construct the resolvent clause
                Clause resolvent = new Clause(resolventLiterals);
                // Discard tautological clauses if this optimization is turned on.
                if (!(isDiscardTautologies() && resolvent.isTautology()))
                {
                    resolvents.add(resolvent);
                }
            }
        }

        protected void discardTautologies(Set<Clause> clauses)
        {
            if (isDiscardTautologies())
            {
                Set<Clause> toDiscard = new HashSet<Clause>();
                for (Clause c :
                clauses) {
                    if (c.isTautology())
                    {
                        toDiscard.add(c);
                    }
                }
                clauses.removeAll(toDiscard);
            }
        }
    }
}