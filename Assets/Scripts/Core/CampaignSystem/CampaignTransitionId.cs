using System;

namespace Geostorm.Core.CampaignSystem
{
    public readonly struct CampaignTransitionId : IEquatable<CampaignTransitionId>
    {
        public static readonly CampaignTransitionId None = new(string.Empty);

        public string Value { get; }
        public bool IsValid => !string.IsNullOrEmpty(Value);

        public CampaignTransitionId(string value)
        {
            Value = value ?? string.Empty;
        }

        public bool Equals(CampaignTransitionId other)
        {
            return string.Equals(Value, other.Value, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is CampaignTransitionId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value != null ? StringComparer.Ordinal.GetHashCode(Value) : 0;
        }

        public override string ToString()
        {
            return Value;
        }

        public static bool operator ==(CampaignTransitionId left, CampaignTransitionId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CampaignTransitionId left, CampaignTransitionId right)
        {
            return !left.Equals(right);
        }
    }
}
