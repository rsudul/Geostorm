using System;

namespace Geostorm.Core.CampaignSystem
{
    public readonly struct CampaignStepType : IEquatable<CampaignStepType>
    {
        public static readonly CampaignStepType None = new(string.Empty);
        public static readonly CampaignStepType Scene = new("Scene");

        public string Value { get; }
        public bool IsValid => !string.IsNullOrEmpty(Value);

        public CampaignStepType(string value)
        {
            Value = value ?? string.Empty;
        }

        public bool Equals(CampaignStepType other)
        {
            return string.Equals(Value, other.Value, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is CampaignStepType other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value != null ? StringComparer.Ordinal.GetHashCode(Value) : 0;
        }

        public override string ToString()
        {
            return Value;
        }

        public static bool operator ==(CampaignStepType left, CampaignStepType right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CampaignStepType left, CampaignStepType right)
        {
            return !left.Equals(right);
        }
    }
}
