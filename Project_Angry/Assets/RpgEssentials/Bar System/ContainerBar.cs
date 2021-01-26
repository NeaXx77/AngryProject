using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace RpgEssentials
{
    [ExecuteInEditMode]
    public class ContainerBar : MonoBehaviour
    {
        //Debug
        public TextMeshProUGUI HPtext;
        [Tooltip("The prefab with the BarManager component in it")]
        [SerializeField]private GameObject barPrefab = null;
        [Header("Parameters")]
        [SerializeField]private int numberOfElement = 1;
        [SerializeField]private float totalAmount = 1;
        public float currentAmount = 1;
        public Bar.BarType barType = Bar.BarType.direct;
        public float smooth = 0.1f;
        public enum FillType
        {
            filled,
            fragment
        }
        public FillType fillType;
        [SerializeField]private Vector2 toScale = Vector2.one; 
        [Header("UI")]
        [Tooltip("The size will affect total amount if the type is on \"fragment\". Don't forget to count the full fill and the empty sprite")]
        public Sprite[] FragmentSprite = null;
        [SerializeField]private bool customContainer = true;
        private int fragmentPerBar;
        public Bar[] container;
        private int notEmptyIndex;
        public Image[] barImageInstance;
        private float amountLeftToConsume = 0;
        private bool coroutineConsumeStarted = false;
        /*
        Awake create all objects in the container and initialize them
         */
        void Awake(){
            container = GetComponentsInChildren<Bar>();
            barImageInstance = GetComponentsInChildren<Image>();
            notEmptyIndex = (int)currentAmount - 1;
        }
        private void Update() {
            HPtext.text = "HP : "+currentAmount.ToString();
            #region Edit Mode Preview
            if(numberOfElement != container.Length){
                //Awake();
            }
            #endregion
            //To arrange a Fragment + smooth problem that sometimes not fill the bar at the current amount
            if(!coroutineConsumeStarted && CheckContainer() != this.currentAmount){
                ChangeBarCurrentAmount(-CheckContainer()+this.currentAmount);
            }
        }
        void InitializeBar(Image barImage){
            switch (fillType)
            {
                case FillType.filled:
                    barImage.type = Image.Type.Filled;
                break;
                case FillType.fragment:
                    barImage.type = Image.Type.Simple;
                break;
                default:
                break;
            }
        }
        //To use directly when you want to modify the bar, amount is the amount you want to reduce or to add
        public void ChangeBarCurrentAmount(float amount){
            currentAmount += amount;
            switch (fillType)
            {
                case FillType.filled:
                    StartCoroutine(FilledMode(amount));
                break;
                case FillType.fragment:
                    StartCoroutine(FragmentMode(amount));
                break;
                default:
                break;
            }
            if(currentAmount > totalAmount){
                currentAmount = totalAmount;
            }else if(currentAmount < 0)
            {
                currentAmount = 0;
            }
        }
        public float GetCurrentAmount(){
            return this.currentAmount;
        }
        //Is the notEmptyIndex bar empty ?
        public bool IsLastEmpty(){
            if(fillType == FillType.filled)
                return (container[notEmptyIndex].IsEmpty() && barImageInstance[notEmptyIndex].fillAmount <= 0);
            else
                return (container[notEmptyIndex].IsEmpty());

        }
        //Is the notEmptyIndex bar full ?
        public bool LastIsFull(){
            if(fillType == FillType.filled)
                return (container[notEmptyIndex].IsFull() && barImageInstance[notEmptyIndex].fillAmount == 1);
            else
            {
                return (container[notEmptyIndex].IsFull());
            }
        }
        //Change the bar visual and amount for filled mode
        IEnumerator FilledMode(float amount){
            amountLeftToConsume -= amount;
            amountLeftToConsume = -(this.currentAmount - CheckContainer());
            if(!coroutineConsumeStarted){
                coroutineConsumeStarted = true;
                while (amountLeftToConsume != 0)
                {
                    if(notEmptyIndex != 0 && IsLastEmpty()){
                        notEmptyIndex--;
                    }else if(notEmptyIndex < numberOfElement-1 && LastIsFull()){
                        notEmptyIndex++;
                    }else if(amountLeftToConsume < 0 && notEmptyIndex == numberOfElement-1 && LastIsFull()){
                        amountLeftToConsume = 0;
                    }else if(amountLeftToConsume > 0 && notEmptyIndex == 0 && IsLastEmpty()){
                        amountLeftToConsume = 0;
                    }
                    if(notEmptyIndex != -1){
                        if(container[notEmptyIndex].GetCurrentAmount()-amountLeftToConsume <= 0){
                            amountLeftToConsume -= container[notEmptyIndex].GetCurrentAmount();
                            container[notEmptyIndex].Consume(container[notEmptyIndex].GetCurrentAmount(), barImageInstance[notEmptyIndex]);
                        }else if(container[notEmptyIndex].GetCurrentAmount()-amountLeftToConsume >= container[notEmptyIndex].GetMaxAmount()){
                            float barAmount = container[notEmptyIndex].GetMaxAmount()-container[notEmptyIndex].GetCurrentAmount();
                            amountLeftToConsume += barAmount;
                            container[notEmptyIndex].Consume(-barAmount, barImageInstance[notEmptyIndex]);
                        }else{
                            container[notEmptyIndex].Consume(amountLeftToConsume, barImageInstance[notEmptyIndex]);
                            amountLeftToConsume = 0;
                        }
                    }
                    yield return new WaitForEndOfFrame();
                }
                coroutineConsumeStarted = false;
            }
        }
        //Change the bar visual and amount for Fragment mode
        IEnumerator FragmentMode(float amount){
            amountLeftToConsume -= amount;
            if(!coroutineConsumeStarted){
                coroutineConsumeStarted = true;
                while (amountLeftToConsume != 0)
                {
                    if(notEmptyIndex != 0 && container[notEmptyIndex].IsEmpty()){
                        notEmptyIndex--;
                    }else if(notEmptyIndex < numberOfElement-1 && LastIsFull()){
                        notEmptyIndex++;
                    }else if(amountLeftToConsume < 0 && notEmptyIndex == numberOfElement-1 && LastIsFull()){
                        amountLeftToConsume = 0;
                    }else if(amountLeftToConsume > 0 && notEmptyIndex == 0 && IsLastEmpty()){
                        amountLeftToConsume = 0;
                    }
                    //Consume fragment for each BarType
                    if(barType == Bar.BarType.direct){
                        //If one element still
                        if(notEmptyIndex != -1){
                            //Check if the last element have enough amount to consume the entire amount
                            if(container[notEmptyIndex].GetCurrentAmount()-amountLeftToConsume <= 0){
                                amountLeftToConsume -= container[notEmptyIndex].GetCurrentAmount();
                                container[notEmptyIndex].Consume(container[notEmptyIndex].GetCurrentAmount());
                                container[notEmptyIndex].GetComponent<Image>().sprite = FragmentSprite[0];
                            }
                            else if(container[notEmptyIndex].GetCurrentAmount()-amountLeftToConsume >= container[notEmptyIndex].GetMaxAmount()){
                                float barAmount = container[notEmptyIndex].GetMaxAmount()-container[notEmptyIndex].GetCurrentAmount();
                                amountLeftToConsume += barAmount;
                                container[notEmptyIndex].Consume(-barAmount, barImageInstance[notEmptyIndex]);
                                container[notEmptyIndex].GetComponent<Image>().sprite = FragmentSprite[(int)container[notEmptyIndex].GetMaxAmount()];
                            }
                            else{
                                container[notEmptyIndex].Consume(amountLeftToConsume);
                                amountLeftToConsume = 0;
                                container[notEmptyIndex].GetComponent<Image>().sprite = FragmentSprite[(int)container[notEmptyIndex].GetCurrentAmount()];
                            }
                        }
                        //yield return null;
                    }
                    //Curved to do
                    else if(barType != Bar.BarType.direct && amountLeftToConsume != 0){
                        if(notEmptyIndex != -1){
                            int amountSign = this.currentAmount - CheckContainer() > 0 ? 1:(-1);
                            print("Verified "+amountSign);
                            container[notEmptyIndex].FillBar(1*amountSign);
                            if(this.currentAmount - CheckContainer() == 0)
                                amountLeftToConsume = 0;
                            container[notEmptyIndex].GetComponent<Image>().sprite = FragmentSprite[(int)container[notEmptyIndex].GetCurrentAmount()];
                        }
                        yield return new WaitForSeconds(smooth/100);
                    }
                }
                coroutineConsumeStarted = false;
            }
            yield return null;
        }
        //Check the actual amount on the container
        public float CheckContainer(){
            float amountOnContainer = 0, amountLeft = this.currentAmount;
            for (int i = 0; i < container.Length; i++)
            {
                amountOnContainer += container[i].GetCurrentAmount();
            }
            return amountOnContainer;
        }
    //container with 2 state items
    //container with items to fill
        //modular bar that could contain any type of contain (other than bar)
        
    //Do Health and an other modular bar class
    }
}