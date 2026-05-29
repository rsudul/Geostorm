using System;

namespace Geostorm.Core.CampaignSystem
{
    public sealed class CampaignStepContextStore : ICampaignStepContextProvider, ICampaignStepContextWriter
    {
        public CampaignStepStartContext CurrentContext { get; private set; }
        public bool HasCurrentContext => CurrentContext != null;

        public bool TryGetCurrentContext(out CampaignStepStartContext context)
        {
            context = CurrentContext;
            return context != null;
        }

        public void SetCurrentContext(CampaignStepStartContext context)
        {
            CurrentContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void ClearCurrentContext()
        {
            CurrentContext = null;
        }
    }
}
