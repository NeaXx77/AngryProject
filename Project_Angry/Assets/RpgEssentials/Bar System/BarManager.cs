using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RpgEssentials;

namespace RpgEssentials
{
    [RequireComponent(typeof(Bar))]
    public class BarManager : MonoBehaviour
    {
        [SerializeField]private Bar bar;
        [Header("Parameters")]
        [SerializeField] private float maxAmount = 1;
        [SerializeField] private float currentAmount = 1;
        [Tooltip("The more smooth is, the faster the bar will be modified")]
        [SerializeField]private float smooth = 10;
        [Header("UI objects")]
        [SerializeField] private Image imageBar;
        [Tooltip("Optional")]
        [SerializeField] private TextMeshProUGUI displayAmountText = null;
        public enum DisplayMethod
        {
            none,
            pourcent,
            amount
        }
        public DisplayMethod displayMethod = DisplayMethod.amount;
        public Bar.BarType barType;
        public enum TextBarType{
            direct,
            fromFillAmount
        }
        public TextBarType textBarType;
        void Awake(){
            bar = GetComponent<Bar>();
            bar.barType = barType;
            if(displayAmountText != null && displayMethod == DisplayMethod.none){
                SetActiveText(false);
            }
            imageBar = GetComponent<Image>();
            imageBar.fillAmount = currentAmount/maxAmount;
            bar.SetCurrentAmount(currentAmount);
            bar.SetmaxAmount(maxAmount);
            bar.SetSmooth(smooth);
        }
        void Update()
        {
            UpdateAmount();
            UpdateParameters();
            SetText();
        }
        public void SetSmooth(float smooth){
            this.smooth = smooth;
        }
        public void SetCurrentAmount(float currentAmount){
            this.currentAmount = currentAmount;
        }
        public void SetMaxAmount(float maxAmount){
            this.maxAmount = maxAmount;
        }
        public void ChangeBarAmount(float amount){
            if(amount < 0){
                bar.Consume(-amount,imageBar);
            }else
            {
                bar.FillBar(amount,imageBar);
            }
        }
        //Test consume and fillup on the GUI inspector
        public void ConsumeBar()
        {
            bar.Consume(10, imageBar);
        }
        public void FillUp(){
            bar.FillBar(10,imageBar);
        }
        void UpdateParameters(){
            bar.barType = barType;
            bar.SetCurrentAmount(currentAmount);
            bar.SetmaxAmount(maxAmount);
            bar.SetSmooth(smooth);
        }
        void UpdateAmount(){
            currentAmount = bar.GetCurrentAmount();
            maxAmount = bar.GetMaxAmount();
        }
        void SetText(){
            if(displayMethod != DisplayMethod.none){
                switch (textBarType)
                {
                    case TextBarType.direct:
                        if(displayMethod == DisplayMethod.amount){
                            displayAmountText.text = currentAmount.ToString();
                        }else{
                            float purcent = Mathf.Round((currentAmount/maxAmount)*100);
                            displayAmountText.text = purcent.ToString()+"%";
                        }
                    break;
                    case TextBarType.fromFillAmount:
                        if(displayMethod == DisplayMethod.amount){
                            float filledAmount = Mathf.Round(maxAmount*imageBar.fillAmount);
                            displayAmountText.text = filledAmount.ToString();
                        }else{
                            float purcent = Mathf.Round((imageBar.fillAmount)*100);
                            displayAmountText.text = purcent.ToString()+"%";
                        }
                    break;
                    default:
                    break;
                }
            }
        }
        public void SetActiveText(bool state){
            displayAmountText.gameObject.SetActive(state);
        }
    }
}
