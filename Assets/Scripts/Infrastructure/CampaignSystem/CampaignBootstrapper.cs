using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;
using Geostorm.Core.CampaignSystem;

namespace Geostorm.Infrastructure.CampaignSystem
{
    public sealed class CampaignBootstrapper : IStartable, IDisposable
    {
        private readonly CampaignManifest _campaignManifest;
        private readonly ICampaignSession _campaignSession;

        private readonly CancellationTokenSource _cancellationTokenSource = new();

        public CampaignBootstrapper(CampaignManifest campaignManifest, ICampaignSession campaignSession)
        {
            _campaignManifest = campaignManifest;
            _campaignSession = campaignSession;
        }

        public void Start()
        {
            _ = StartCampaignAsync();
        }

        private async Task StartCampaignAsync()
        {
            try
            {
                await _campaignSession.StartCampaignAsync(_campaignManifest, _cancellationTokenSource.Token);
            }
            catch (OperationCanceledException) { }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }
    }
}
