using System;

namespace Geostorm.Core.CampaignSystem
{
    public readonly struct CampaignId : IEquatable<CampaignId>
    {
        public static readonly CampaignId None = new(string.Empty);

        public string Value { get; }
        public bool IsValid => !string.IsNullOrEmpty(Value);

        public CampaignId(string value)
        {
            Value = value ?? string.Empty;
        }

        public bool Equals(CampaignId other)
        {
            return string.Equals(Value, other.Value, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is CampaignId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value != null ? StringComparer.Ordinal.GetHashCode(Value) : 0;
        }

        public override string ToString()
        {
            return Value;
        }

        public static bool operator ==(CampaignId left, CampaignId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CampaignId left, CampaignId right)
        {
            return !left.Equals(right);
        }
    }
}
