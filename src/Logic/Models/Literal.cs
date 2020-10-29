using System;

namespace PL_Resolution.Logic.Models
{
    public struct Literal
    {
        private int _id;
        private bool _isNegated;
        
        public int Id => _id;
        public bool IsNegated => _isNegated;
        public Literal Negation => new Literal(_id, !_isNegated);
        public Literal(int id, bool isNegated)
        {
            _id = id;
            _isNegated = isNegated;
        }

        public override string ToString()
        {
            return (_isNegated ? "-" : "") + _id;
        }
        
        public bool Equals(Literal other)
        {
            return _id == other._id && _isNegated == other._isNegated;
        }

        public override bool Equals(object obj)
        {
            return obj is Literal other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_id, _isNegated);
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