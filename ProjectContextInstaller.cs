using System;
using UnityEngine;
using Zenject;
using IngameDebugConsole;
using System.Collections.Generic;
using Unity.Services.Core;
using Unity.Services.Core.Environments;

namespace Project.DI
{
    [CreateAssetMenu(menuName = "Project Context Installer", fileName = "ProjectContextInstaller", order = 0)]
    public class ProjectContextInstaller : ScriptableObjectInstaller<ProjectContextInstaller>
    {
        //Add your service scripts here, to be injected
        [Header("Services Settings")]
        [SerializeField]
        private AdsManager _adsManager;
        [SerializeField]
        private RewardedManager _rewardsManager;
        [SerializeField]
        private SocailManager _socialManager;
        [SerializeField]
        private LocalizationManager _localizationManager;
        [SerializeField]
        private ServicesManager _servicesManager;
        [SerializeField]
        private GameManager _gameManager;
        [SerializeField]
        private LevelManager _levelManager;
        [SerializeField]
        private AudioManager _audioManager;
        [SerializeField]
        private AnalyticsManager _analyticsManager;

        //add you settings options here
        [Header("Game Settings")]
        [SerializeField]
        private bool _allowCheats;
        [SerializeField]
        private bool _testMode;
        [SerializeField]
        private bool _allowGameConsole;

        //add your game configs here
        [Header("Configs")]
        [SerializeField]
        private ProjectConfig _devConfig;
        [SerializeField]
        private ProjectConfig _prodConfig;

        //add you settings here
        [SerializeField]
        private ProjectSettings _devSettings;
        [SerializeField]
        private ProjectSettings _prodSettings;
        

        public override void InstallBindings()
        {
            Application.targetFrameRate = 60;

            try {
                var options = new InitializationOptions()
                    .SetEnvironmentName(_devSettings?"development":"production");
 
               UnityServices.InitializeAsync(options);
            }
            catch (Exception exception) {
                // An error occurred during initialization.
            }
            CheatsManager.EnableCheatSystem(allowCheats);

            GameSettings.IsABTestModeEnabled = _testMode;
            GameSettings.IsGameConsoleEnabled = _allowGameConsole;

            //Ads
            Container.BindInterfacesAndSelfTo<AdsSettings>().FromInstance(_adsSettings).AsSingle();
            Container.BindInterfacesAndSelfTo<AdsManager>().FromNewComponentOnNewGameObject().AsSingle();
            Container.BindInterfacesAndSelfTo<RewardedManager>().FromScriptableObject(_rewardsManager).AsSingle();

            //Localization
            Container.BindInterfacesAndSelfTo<LocalizationManager>().AsSingle();

            Container.BindInterfacesAndSelfTo<GameManager>().FromComponentInNewPrefab(_gameManager).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<LevelManager>().FromComponentInNewPrefab(_levelManager).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<AudioManager>().FromComponentInNewPrefab(_audioManager).AsSingle();
            Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<ServicesManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<AnalyticsManager>().AsSingle();
        }
    }
}