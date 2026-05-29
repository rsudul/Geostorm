using System.Collections.Generic;

namespace Geostorm.Core.CampaignSystem
{
    public sealed class CampaignManifestValidationResult
    {
        private readonly List<string> _errors = new();
        private readonly List<string> _warnings = new();

        public IReadOnlyList<string> Errors => _errors;
        public IReadOnlyList<string> Warnings => _warnings;

        public bool IsValid => _errors.Count == 0;
        public bool HasWarnings => _warnings.Count > 0;

        public void AddError(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                _errors.Add(message);
            }
        }

        public void AddWarning(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                _warnings.Add(message);
            }
        }
    }
}
