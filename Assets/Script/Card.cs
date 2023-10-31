using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public static class Coroutine_Caching
{
    public static readonly WaitForFixedUpdate WaitForFixedUpdate 
        = new WaitForFixedUpdate();
    public static readonly WaitForSeconds m_waitForSeconds = 
        new WaitForSeconds(5.0f);  
}

public class Card : MonoBehaviour
{ 
    public Animator anim;

    public Text CountDownText;
 
    //private void Start()
    //{
    //    //GameManager.CardDestroy += DestroyCard;
    //    //GameManager.CardClose += CloseCard;
    //}
    private float CountDown = 5.0f; 

    private IEnumerator CountDown_Coroutine;
    private void Start()
    {
        CountDown_Coroutine = CountDownRoutine();        
    }

    public void OpenCard()
    {
        anim.SetBool("IsOpen", true);
        transform.Find("front").gameObject.SetActive(true);
        transform.Find("back").gameObject.SetActive(false);                  

        if (GameManager.instance.FirstCard == null)
        {
            GameManager.instance.FirstCard = gameObject;
            CountDown = 5.0f;
            CountDown_Coroutine = CountDownRoutine();
            StartCoroutine(CountDown_Coroutine); 
        }
        else
        {
            GameManager.instance.SecondCard = gameObject;
            GameManager.instance.IsMatched();
        }
    }

    IEnumerator CountDownRoutine()
    {        
        while(CountDown > 0.0f)
        {
            transform.Find("Canvas(CountDown)").gameObject.SetActive(true);
            CountDown -= Time.deltaTime;
            CountDownText.text = CountDown.ToString("N2");
            yield return null;
        }        
        CountDown = 5.0f;
        GameManager.instance.FirstCard = null;
        CloseCard();               
    }

    public void DestroyCard()
    {
        transform.Find("Canvas(CountDown)").gameObject.SetActive(false);       
        StopCoroutine(CountDown_Coroutine);
        Invoke("DestroyCardInvoke", 0.7f);
    }

    void DestroyCardInvoke()
    {
        Destroy(gameObject);
    }

    public void CloseCard()
    {
        transform.Find("Canvas(CountDown)").gameObject.SetActive(false);
        StopCoroutine(CountDown_Coroutine);
        Invoke("CloseCardInvoke", 0.7f);
    }

    void CloseCardInvoke()
    {
        anim.SetBool("IsOpen", false);
        transform.Find("back").gameObject.SetActive(true);
        transform.Find("front").gameObject.SetActive(false);
    }
}
