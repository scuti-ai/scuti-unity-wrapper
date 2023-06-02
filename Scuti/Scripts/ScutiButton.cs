using UnityEngine;
using Scuti;
using System;
using UnityEngine.EventSystems;

public class ScutiButton : MonoBehaviour
{
    public GameObject NotificationIcon;
    public GameObject NewItems;

    private bool _started;

    private void Awake()
    {
        NotificationIcon.SetActive(false);
        NewItems?.SetActive(false);
    }

    public void Start()
    {
        ScutiSDK.Instance.OnNewProduct += OnNewProducts;
        ScutiSDK.Instance.OnNewReward += OnNewRewards;
        _started = true;
        ScutiSDK.Instance.RequestNew();
        //EventSystem.current.SetSelectedGameObject(null);
        //EventSystem.current.SetSelectedGameObject(Button);
    }

    public void OnEnable()
    {
        if(_started)
            ScutiSDK.Instance.RequestNew();
    }
     

    public void OnClick()
    { 
        ScutiSDK.Instance.ShowStore();
        NotificationIcon.SetActive(false);
    }

    private void OnNewProducts(bool value)
    {
        NewItems?.SetActive(value);
    }


    private void OnNewRewards(bool value)
    {
        NotificationIcon.SetActive(value);
    }


    private void OnDestroy()
    {
        if (ScutiSDK.Instance != null)
        {
            ScutiSDK.Instance.OnNewProduct -= OnNewProducts;
            ScutiSDK.Instance.OnNewReward -= OnNewRewards;
        }
    }


}