using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TwoHandGrabbable : XRGrabInteractable
{
    public List<IXRSelectInteractor> selectingInteractors = new List<IXRSelectInteractor>();

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (!selectingInteractors.Contains(args.interactorObject))
            selectingInteractors.Add(args.interactorObject);

        // Не вызывать base, чтобы не выбрасывать предыдущую руку
        if (selectingInteractors.Count == 1)
            base.OnSelectEntered(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        selectingInteractors.Remove(args.interactorObject);

        if (selectingInteractors.Count == 0)
            base.OnSelectExited(args);
    }

    public bool IsGrabbedBy(IXRSelectInteractor interactor)
    {
        return selectingInteractors.Contains(interactor);
    }
}
