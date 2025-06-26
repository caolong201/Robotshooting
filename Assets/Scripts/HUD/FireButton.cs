
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class FireButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private MachineGun gun;

    private void OnEnable()
    {
        gun.CanFire = false;
        transform.DOKill();
        transform.localScale = Vector3.one;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        gun.CanFire = true;
        transform.DOPunchScale(Vector3.one * 0.1f, 0.167f).SetLoops(-1);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        gun.CanFire = false;
        transform.DOKill();
        transform.localScale = Vector3.one;
    }
}