using System.Collections.Generic;
using System.Linq;

namespace ActiveSelection
{
    /// <summary>
    /// Base class for all value objects.
    /// </summary>
    public abstract class ValueObject
    {
        /// <summary>
        /// Returns collection of equality components.
        /// </summary>
        protected abstract IEnumerable<object> GetEqualityComponents();

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (GetType() != obj.GetType())
                return false;

            var valueObject = (ValueObject) obj;

            return GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Aggregate(1, (current, obj) =>
                {
                    unchecked
                    {
                        return current * 23 + (obj?.GetHashCode() ?? 0);
                    }
                });
        }

        /// <summary>
        /// Checks if two objects are equal.
        /// </summary>
        public static bool operator ==(ValueObject a, ValueObject b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        /// <summary>
        /// Checks if two objects are not equal.
        /// </summary>
        public static bool operator !=(ValueObject a, ValueObject b)
        {
            return !(a == b);
        }
    }}