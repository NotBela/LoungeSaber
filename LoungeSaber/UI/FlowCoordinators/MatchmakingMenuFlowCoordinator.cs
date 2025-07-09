using System;
using System.Collections;
using System.Threading.Tasks;
using BeatSaberMarkupLanguage.FloatingScreen;
using HarmonyLib;
using HMUI;
using IPA.Utilities;
using JetBrains.Annotations;
using LoungeSaber.Models.Packets.ServerPackets;
using LoungeSaber.Models.Server;
using LoungeSaber.Server;
using LoungeSaber.UI.BSML.Leaderboard;
using LoungeSaber.UI.BSML.Match;
using LoungeSaber.UI.BSML.Menu;
using SiraUtil.Logging;
using SongCore;
using Zenject;
using UnityEngine;

namespace LoungeSaber.UI.FlowCoordinators
{
    public class MatchmakingMenuFlowCoordinator : FlowCoordinator, IInitializable, IDisposable
    {
        [Inject] private readonly LoungeSaberApi _loungeSaberApi = null;
        
        [Inject] private readonly MainFlowCoordinator _mainFlowCoordinator = null;
        [Inject] private readonly MatchFlowCoordinator _matchFlowCoordinator = null;
        [Inject] private readonly VotingScreenViewController _votingScreenViewController = null;
        [Inject] private readonly CantConnectToServerViewController _cantConnectToServerViewController = null;
        [Inject] private readonly MissingMapsViewController _missingMapsViewController = null;
        
        [Inject] private readonly ServerListener _serverListener = null;
        [Inject] private readonly MatchmakingMenuViewController _matchmakingMenuViewController = null;
        
        [Inject] private readonly MatchResultsViewController _matchResultsViewController = null;
        
        [Inject] private readonly LoungeSaberLeaderboardViewController _leaderboardViewController = null;
        
        [Inject] private readonly CheckingServerStatusViewController _checkingServerStatusViewController = null;
        
        [Inject] private readonly SiraLog _siraLog = null;
        
        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            showBackButton = true;
            SetTitle("LoungeSaber");

            if (firstActivation)
            {
                ProvideInitialViewControllers(_checkingServerStatusViewController);
                _checkingServerStatusViewController.SetControllerState(CheckingServerStatusViewController.ControllerState.CheckingServer);

                Task.Run(async Task () =>
                {
                    await StartServerCheckingExecutionFlow();
                });
                
                return;
            }
            
            ProvideInitialViewControllers(_matchmakingMenuViewController, leftScreenViewController: _leaderboardViewController);
        }

        private async Task StartServerCheckingExecutionFlow()
        {
            var serverResponse = await _loungeSaberApi.GetServerStatus();

            if (serverResponse == null)
            {
                PresentViewControllerSynchronously(_cantConnectToServerViewController);
                _cantConnectToServerViewController.SetReasonText("InvalidServerResponse");
                return;
            }
            
            serverResponse.AllowedModVersions.Do(i => _siraLog.Info(i));

            if (!serverResponse.AllowedModVersions.Contains(IPA.Loader.PluginManager.GetPluginFromId("LoungeSaber").HVersion.ToString()))
            {
                PresentViewControllerSynchronously(_cantConnectToServerViewController);
                _cantConnectToServerViewController.SetReasonText("OutdatedPluginVersion");
                return;
            }
            
            _siraLog.Info(IPA.Loader.PluginManager.GetPluginFromId("LoungeSaber").HVersion.ToString());

            if (!serverResponse.AllowedGameVersions.Contains(UnityGame.GameVersion.ToString()))
            {
                PresentViewControllerSynchronously(_cantConnectToServerViewController);
                _cantConnectToServerViewController.SetReasonText("OutdatedGameVersion");
                return;
            }

            if (serverResponse.State != ServerStatus.ServerState.Online)
            {
                PresentViewControllerSynchronously(_cantConnectToServerViewController);
                _cantConnectToServerViewController.SetReasonText("ServerInMaintenance");
                return;
            }
            
            _checkingServerStatusViewController.SetControllerState(CheckingServerStatusViewController.ControllerState.CheckingMaps);

            var maps = await _loungeSaberApi.GetMapHashes();
            var missingMapHashes = maps.Where(i => Loader.GetLevelByHash(i) == null).ToArray();
            
            if (missingMapHashes.Length > 0)
            {
                PresentViewControllerSynchronously(_missingMapsViewController);
                _missingMapsViewController.SetMissingMapCount(missingMapHashes.Length);
                _missingMapsViewController.UserChoseToDownloadMaps += OnUserChoseToDownloadMaps;
                
                
                void OnUserChoseToDownloadMaps(bool choice)
                {
                    _missingMapsViewController.UserChoseToDownloadMaps -= OnUserChoseToDownloadMaps;

                    if (choice)
                    {
                        PresentViewControllerSynchronously(_checkingServerStatusViewController);
                        _checkingServerStatusViewController.SetControllerState(CheckingServerStatusViewController.ControllerState.CheckingMaps);
                        // TODO: download maps

                        while (Loader.AreSongsLoading);
                        
                        PresentViewControllerSynchronously(_matchmakingMenuViewController, _leaderboardViewController);
                    }
                    else
                        BackButtonWasPressed(null);
                }
            }
        }

        private void OnContinueButtonPressed() => DismissFlowCoordinator(_matchFlowCoordinator);

        private void OnMatchCreated(MatchCreatedPacket packet)
        {
            PresentFlowCoordinatorSynchronously(_matchFlowCoordinator);
            _votingScreenViewController.PopulateData(packet);
        }

        private void PresentFlowCoordinatorSynchronously(FlowCoordinator flowCoordinator)
        {
            StartCoroutine(PresentFlowCoordinatorSynchronouslyCoroutine());
            return;
            
            IEnumerator PresentFlowCoordinatorSynchronouslyCoroutine()
            {
                yield return new WaitForEndOfFrame();
                
                PresentFlowCoordinator(flowCoordinator);
            }
        }

        private void PresentViewControllerSynchronously(ViewController viewController, [CanBeNull] ViewController leftViewController = null, [CanBeNull] ViewController rightViewController = null)
        {
            while (isInTransition);
            
            StartCoroutine(PresentViewControllerSynchronouslyCoroutine());
            return;
            
            IEnumerator PresentViewControllerSynchronouslyCoroutine()
            {
                yield return new WaitForEndOfFrame();
                
                ReplaceTopViewController(viewController);
                SetLeftScreenViewController(leftViewController, ViewController.AnimationType.In);
                SetRightScreenViewController(rightViewController, ViewController.AnimationType.In);
            }
        }

        public void Dispose()
        {
            _serverListener.OnMatchCreated -= OnMatchCreated;
            _matchResultsViewController.ContinueButtonPressed -= OnContinueButtonPressed;
        }
        
        public void Initialize()
        {
            _serverListener.OnMatchCreated += OnMatchCreated;
            _matchResultsViewController.ContinueButtonPressed += OnContinueButtonPressed;
        }

        protected override void BackButtonWasPressed(ViewController _)
        {
            _mainFlowCoordinator.DismissFlowCoordinator(this);
        }
    }
}  