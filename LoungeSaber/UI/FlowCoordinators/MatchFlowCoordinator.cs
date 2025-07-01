using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeatSaberMarkupLanguage.GameplaySetup;
using HarmonyLib;
using HMUI;
using LoungeSaber.Game;
using LoungeSaber.Models.Map;
using LoungeSaber.Models.Packets.ServerPackets;
using LoungeSaber.Models.Packets.UserPackets;
using LoungeSaber.Server;
using LoungeSaber.UI.BSML;
using SiraUtil.Logging;
using UnityEngine;
using Zenject;

namespace LoungeSaber.UI.FlowCoordinators
{
    public class MatchFlowCoordinator : FlowCoordinator
    {
        [Inject] private readonly GameplaySetupViewController _gameplaySetupViewController = null;
        [Inject] private readonly VotingScreenViewController _votingScreenViewController = null;
        [Inject] private readonly AwaitingMapDecisionViewController _awaitingMapDecisionViewController = null;
        [Inject] private readonly WaitingForMatchToStartViewController _waitingForMatchToStartViewController = null;
        [Inject] private readonly AwaitMatchEndViewController _awaitMatchEndViewController = null;
        [Inject] private readonly MatchResultsViewController _matchResultsViewController = null;
        
        [Inject] private readonly ServerListener _serverListener = null;
        [Inject] private readonly MatchManager _matchManager = null;
        
        [Inject] private readonly SiraLog _siraLog = null;
         
        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            SetupGameplaySetupViewController();
            
            SetTitle("Match Room");
            showBackButton = false;
            ProvideInitialViewControllers(_votingScreenViewController, _gameplaySetupViewController);
            
            _votingScreenViewController.MapSelected += OnMapSelected;
            _serverListener.OnMatchStarting += OnMatchStarting;
            _matchManager.OnLevelCompleted += OnLevelCompleted;
            _serverListener.OnMatchResults += OnMatchResultsReceived;
        }

        private void OnMatchResultsReceived(MatchResults results)
        {
            PresentViewControllerSynchronously(_matchResultsViewController);
            _matchResultsViewController.PopulateData(results);
        }

        private void OnLevelCompleted(LevelCompletionResults levelCompletionResults, StandardLevelScenesTransitionSetupDataSO standardLevelScenesTransitionSetupData)
        {
            Task.Run(async () =>
            {
                await _serverListener.SendPacket(new ScoreSubmissionPacket(levelCompletionResults.multipliedScore, ScoreModel.ComputeMaxMultipliedScoreForBeatmap(standardLevelScenesTransitionSetupData.transformedBeatmapData),
                    levelCompletionResults.gameplayModifiers.proMode, levelCompletionResults.notGoodCount, levelCompletionResults.fullCombo));
            });
            PresentViewControllerSynchronously(_awaitMatchEndViewController, true);
        }

        private async void OnMatchStarting(MatchStarted packet)
        {
            try
            {
                PresentViewControllerSynchronously(_waitingForMatchToStartViewController);
                _waitingForMatchToStartViewController.PopulateData(packet.MapSelected);

                await Task.Delay(packet.TransitionToGameTime - DateTime.UtcNow);
                _matchManager.StartMatch(packet.MapSelected, packet.StartingTime, _gameplaySetupViewController.gameplayModifiers.proMode);
            }
            catch (Exception e)
            {
                _siraLog.Error(e);
            }
        }
        
        private void PresentViewControllerSynchronously(ViewController viewController, bool immediately = false) => StartCoroutine(PresentViewControllerSynchronouslyCoroutine(viewController, immediately));

        private IEnumerator PresentViewControllerSynchronouslyCoroutine(ViewController viewController, bool immediately)
        {
            yield return new WaitForEndOfFrame();
            
            PresentViewController(viewController, immediately: immediately);
        }

        private void OnMapSelected(List<VotingMap> votingMaps, VotingMap selected)
        {
            PresentViewController(_awaitingMapDecisionViewController);
            _awaitingMapDecisionViewController.PopulateData(votingMaps, selected);
        }

        private void SetupGameplaySetupViewController()
        {
            _gameplaySetupViewController.didActivateEvent += OnGameplaySetupViewActivated;
             
            _gameplaySetupViewController.Setup(true, true,true, false, PlayerSettingsPanelController.PlayerSettingsPanelLayout.Singleplayer);
        }
        
        protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
        {
            ResetGameplaySetupView();
            
            _gameplaySetupViewController.didActivateEvent -= OnGameplaySetupViewActivated;
            _votingScreenViewController.MapSelected -= OnMapSelected;
            _serverListener.OnMatchStarting -= OnMatchStarting;
            _matchManager.OnLevelCompleted -= OnLevelCompleted;
            _serverListener.OnMatchResults -= OnMatchResultsReceived;
        }
        
        private void ResetGameplaySetupView() => _gameplaySetupViewController._gameplayModifiersPanelController._gameplayModifierToggles.Do(i => i.gameObject.SetActive(true));

        private void OnGameplaySetupViewActivated(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling) => 
            _gameplaySetupViewController._gameplayModifiersPanelController._gameplayModifierToggles
            .Where(i => i.name != "ProMode")
            .Do(i => i.gameObject.SetActive(false));
    }
}