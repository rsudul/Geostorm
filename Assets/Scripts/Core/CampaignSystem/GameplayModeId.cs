using System;

namespace Geostorm.Core.CampaignSystem
{
    public readonly struct GameplayModeId : IEquatable<GameplayModeId>
    {
        public static readonly GameplayModeId None = new(string.Empty);

        public string Value { get; }
        public bool IsValid => !string.IsNullOrEmpty(Value);

        public GameplayModeId(string value)
        {
            Value = value ?? string.Empty;
        }

        public bool Equals(GameplayModeId other)
        {
            return string.Equals(Value, other.Value, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is GameplayModeId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value != null ? StringComparer.Ordinal.GetHashCode(Value) : 0;
        }

        public override string ToString()
        {
            return Value;
        }

        public static bool operator ==(GameplayModeId left, GameplayModeId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GameplayModeId left, GameplayModeId right)
        {
            return !left.Equals(right);
        }
    }
}
