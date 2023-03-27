using Scuti;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitSDK : MonoBehaviour
{
    public void Start()
    {
        //ScutiSDK.Instance.OnNewProduct += OnNewProducts;
        //ScutiSDK.Instance.OnNewReward += OnNewRewards;
        //_started = true;
        ScutiSDK.Instance.RequestNew();
        //EventSystem.current.SetSelectedGameObject(null);
        //EventSystem.current.SetSelectedGameObject(Button);
    }
}
