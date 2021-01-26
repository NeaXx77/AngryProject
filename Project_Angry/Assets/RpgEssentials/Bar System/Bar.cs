using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RpgEssentials
{
    [RequireComponent(typeof(BarManager))]
    [RequireComponent(typeof(Image))]
    public class Bar: MonoBehaviour{
        //The base of all bars with function definition and virtual function
        protected float maxAmount;
        protected float currentAmount;
        protected float smoothRatio;
        public enum BarType
        {
            direct,
            smooth
        }
        [HideInInspector]public BarType barType = BarType.smooth;
        //constructor useless
        public Bar(){}
        public Bar(float maxAmount, float currentAmount){
        	this.maxAmount = maxAmount;
        	this.currentAmount = currentAmount;
        }
        public float GetMaxAmount(){
            return this.maxAmount;
        }
        public float GetCurrentAmount(){
            return this.currentAmount;
        }
        public void SetmaxAmount(float amount){
            this.maxAmount = amount;
        }
        public void SetCurrentAmount(float amount){
            this.currentAmount = amount;
        }
        public void SetSmooth(float smooth){
            this.smoothRatio = smooth;
        }
        public void Consume(float amount, Image bar = null){
            switch (this.barType)
            {
                case BarType.direct:
                    DirectModify(-amount,bar);
                break;
                case BarType.smooth:
                    SmoothModify(-amount,bar);
                break;
                default:
                break;
            }
            if(this.currentAmount < 0){
                this.currentAmount = 0;
            }else if(this.currentAmount > this.maxAmount){
                this.currentAmount = this.maxAmount;
            }
        }
        public void FillBar(float amount, Image bar = null){
            switch (this.barType)
            {
                case BarType.direct:
                    DirectModify(amount,bar);
                break;
                case BarType.smooth:
                    SmoothModify(amount,bar);
                break;
                default:
                break;
            }
            if(this.currentAmount < 0){
                this.currentAmount = 0;
            }else if(this.currentAmount > this.maxAmount){
                this.currentAmount = this.maxAmount;
            }
        }
        public void DirectModify(float amount, Image bar = null)
        {
            this.currentAmount += amount;
            if(bar != null){
                bar.fillAmount = this.currentAmount / this.maxAmount;
            }
        }
        public void SmoothModify(float amount, Image bar = null) {
            this.currentAmount += amount;
            if(this.currentAmount < 0){
                this.currentAmount = 0;
            }
            StopAllCoroutines();
            if(bar != null){
                StartCoroutine(SmoothModifyingBar(amount,bar));
            }
        }
        IEnumerator SmoothModifyingBar(float amount, Image bar = null){
            float deltaSmooth;
            
                while(bar.fillAmount > (this.currentAmount/this.maxAmount) ){
                    deltaSmooth = CalculateSmooth(bar.fillAmount);
                    bar.fillAmount -= deltaSmooth;
                    yield return new WaitForEndOfFrame();
                }
                while(bar.fillAmount < (this.currentAmount/this.maxAmount) ){
                    deltaSmooth = CalculateSmooth(bar.fillAmount);
                    bar.fillAmount += deltaSmooth;
                    yield return new WaitForEndOfFrame();
                }
                bar.fillAmount = (this.currentAmount/this.maxAmount);
            yield return null;
        }
        float CalculateSmooth(float barFillAmount){
            float deltaSmooth;
            if(this.barType != BarType.smooth){
                deltaSmooth = ((barFillAmount - this.currentAmount/this.maxAmount)*(this.smoothRatio))*Time.deltaTime;
            }else
            {
                deltaSmooth = (((maxAmount)/(this.smoothRatio))*Time.deltaTime);
            }
            if(deltaSmooth <= 0.001f){
                deltaSmooth = 0.001f;
            }
            return deltaSmooth;
        }
        public virtual bool IsEmpty()
        {
            bool empty;
            empty = (currentAmount <= 0);
            return empty;
        }
        public virtual bool IsFull(){
            bool full;
            full = (currentAmount == maxAmount);
            return full;
        }
    }
}
