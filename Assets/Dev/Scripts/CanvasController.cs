using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Dev.Scripts;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public static CanvasController Instance { get; private set; }
    
    [SerializeField] private TextMeshProUGUI totalGoldText = null;
    [SerializeField] private GameObject totalGemCanvas=null;
    [SerializeField] private GameObject floatingGoldText = null;
    [SerializeField] private Image floatingGoldRenderer = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        SignUpEvents();
    }

    private void SignUpEvents()
    {
        GameEvents.GemSoldEvent += SetText;
    }

    #region Button Methods

    public void OpenTotalGemCanvas()
    {
        totalGemCanvas.SetActive(true);
    }

    public void CloseTotalGemCanvas()
    {
        totalGemCanvas.SetActive(false);
    }

    #endregion

    private void SetText(int value,Gem gem)
    {
        totalGoldText.text = PlayerPrefs.GetInt("TotalGem").ToString();

        totalGoldText.rectTransform.DOScale(1.5f, 0.3f).OnComplete(() =>
        {
            totalGoldText.rectTransform.DOScale(1, 0.3f);
        });


        var characterPos = Character.Instance.transform.position;

        var floatingGemObject =
            Instantiate(floatingGoldRenderer, characterPos, Quaternion.identity);
        floatingGemObject.sprite = gem.gemType.icon;

        var floatingTextObject = Instantiate(floatingGoldText, characterPos , Quaternion.identity);
        var textComponent = floatingTextObject.GetComponent<Text>();
        textComponent.text = "+" + value;
        floatingTextObject.transform.SetParent(transform, false);
        floatingGemObject.transform.SetParent(transform,false);
        
        PlayFloatingTextAnimation(floatingTextObject,floatingGemObject);

    }
    
    
    private void PlayFloatingTextAnimation(GameObject floatingTextObject , Image floatingGemObject)
    {
        
        floatingTextObject.transform.DOMoveY(totalGoldText.transform.position.y, 1f).SetEase(Ease.InSine).OnUpdate(() =>
        {
            floatingTextObject.transform.DOScale(0f, .5f).SetDelay(.5f);
        }).OnComplete(() => Destroy(floatingTextObject));
        
        floatingGemObject.rectTransform.DOMove(totalGoldText.transform.position, 1f).SetEase(Ease.InSine).OnUpdate(() =>
        {
            floatingGemObject.transform.DOScale(0f, 1f).SetDelay(1);
        }).OnComplete(() => Destroy(floatingGemObject.gameObject));
    }

}
