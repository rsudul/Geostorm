using System;

namespace Geostorm.Core.CampaignSystem
{
    public readonly struct CampaignStepId : IEquatable<CampaignStepId>
    {
        public static readonly CampaignStepId None = new(string.Empty);

        public string Value { get; }
        public bool IsValid => !string.IsNullOrEmpty(Value);

        public CampaignStepId(string value)
        {
            Value = value ?? string.Empty;
        }

        public bool Equals(CampaignStepId other)
        {
            return string.Equals(Value, other.Value, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is CampaignStepId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value != null ? StringComparer.Ordinal.GetHashCode(Value) : 0;
        }

        public override string ToString()
        {
            return Value;
        }

        public static bool operator ==(CampaignStepId left, CampaignStepId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CampaignStepId left, CampaignStepId right)
        {
            return !left.Equals(right);
        }
    }
}
