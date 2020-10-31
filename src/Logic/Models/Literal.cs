using System;
using System.Reflection.Metadata;

namespace PL_Resolution.Logic.Models
{
    public struct Literal
    {
        public string Id { get; }

        public bool IsNegated { get; }

        public string Name { get; }

        public Literal Negation => new Literal(Id, Name, !IsNegated);

        public Literal(string id, string name, bool isNegated)
        {
            Id = id;
            IsNegated = isNegated;
            Name = name;
        }

        public override string ToString()
        {
            string result = "";
            if (IsNegated)
            {
                result += Constants.NEG;
            }

            result += Options.UseFullNames ? Name : Id;

            return result;
        }

        public bool Equals(Literal other)
        {
            return Id == other.Id && IsNegated == other.IsNegated;
        }

        public override bool Equals(object obj)
        {
            return obj is Literal other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, IsNegated);
        }

        public static bool operator ==(Literal left, Literal right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Literal left, Literal right)
        {
            return !left.Equals(right);
        }
    }
}