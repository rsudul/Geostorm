using System;

namespace Geostorm.Core.CameraSystem
{
    public readonly struct CameraModeId : IEquatable<CameraModeId>
    {
        public static readonly CameraModeId None = new(string.Empty);
        public static readonly CameraModeId Tpp = new("Tpp");
        public static readonly CameraModeId Fpp = new("Fpp");
        
        public string Value { get; }
        public bool IsValid => !string.IsNullOrEmpty(Value);

        public CameraModeId(string value)
        {
            Value = value ?? string.Empty;
        }

        public bool Equals(CameraModeId other)
        {
            return string.Equals(Value, other.Value, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is CameraModeId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value != null ? StringComparer.Ordinal.GetHashCode(Value) : 0;
        }

        public override string ToString()
        {
            return Value;
        }

        public static bool operator ==(CameraModeId left, CameraModeId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CameraModeId left, CameraModeId right)
        {
            return !left.Equals(right);
        }
    }
}