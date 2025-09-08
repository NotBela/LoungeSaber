using System.Collections;
using System.Reflection;
using HMUI;
using SiraUtil.Logging;
using UnityEngine;
using Zenject;

namespace LoungeSaber.UI;

// exists to stop the game from crashing when presenting a new screen while doing async stuff
public abstract class SynchronousFlowCoordinator : FlowCoordinator
{   
    protected void ReplaceViewControllerSynchronously(ViewController viewController, bool immediately = false)
    {
        if (!isActivated) 
            return;

        if (topViewController == viewController)
            return;
        
        while (isInTransition);
            
        StartCoroutine(PresentViewControllerSynchronouslyCoroutine(viewController, immediately: immediately));
    }
    
    private IEnumerator PresentViewControllerSynchronouslyCoroutine(ViewController viewController, bool immediately)
    {
        yield return new WaitForEndOfFrame();
                
        ReplaceTopViewController(viewController, animationType: immediately ? ViewController.AnimationType.None : ViewController.AnimationType.In);
    }

    public void PresentFlowCoordinatorSynchronously(FlowCoordinator flowCoordinator, bool immediately = false)
    {
        while (isInTransition);
        
        StartCoroutine(PresentFlowCoordinatorCoroutine(flowCoordinator, immediately));
    }

    private IEnumerator PresentFlowCoordinatorCoroutine(FlowCoordinator flowCoordinator, bool immediately)
    {
        yield return new WaitForEndOfFrame();
        
        PresentFlowCoordinator(flowCoordinator, immediately: immediately);
    }
}