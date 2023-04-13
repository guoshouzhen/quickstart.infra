using System.Diagnostics.CodeAnalysis;

namespace QuickStart.Infra.RabbitMq.Models
{
    /// <summary>
    /// Deadletter exchange model.
    /// </summary>
    internal sealed class DeadLetterExchange
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }

    /// <summary>
    /// Equality comparer implementation for IEqualityComparer.
    /// </summary>
    internal sealed class DeadLetterExchangeEqualityComparer : IEqualityComparer<DeadLetterExchange>
    {
        public bool Equals(DeadLetterExchange? x, DeadLetterExchange? y)
        {
            if (ReferenceEquals(x, y)) 
            {
                return true;
            }
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) 
            {
                return false;
            }

            return string.Equals(x.Name, y.Name, StringComparison.OrdinalIgnoreCase) && string.Equals(x.Type, y.Type, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode([DisallowNull] DeadLetterExchange obj)
        {
            return HashCode.Combine(obj.Name, obj.Type);
        }
    }
}
