using MoodMe;
using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private EmotionsManager _emotionsManager;
    public override void InstallBindings()
    {
        Container.Bind<EmotionsManager>().FromInstance(_emotionsManager).AsSingle().NonLazy();
    }
}
