
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace AngryProject
{  
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class DragController : MonoBehaviour
    {
        public static DragController instance;
        PlayerHealthController healthController;
        private Animator catAnimator;
        public float maxDragRange = 5;
        public float force = 10;
        Vector2 clickPoint = Vector3.zero;
        Vector2 dragPoint = Vector3.zero;
        Rigidbody2D rigid;
        bool isAlive = true;
        public bool canMove = true;
        #region Display
        [SerializeField]private GameObject arrowObject = null;
        bool isShowingDirection = false;

        #endregion Display

        private bool isDraging = false;
        private bool isDragLenghGood = false;
        RaycastHit2D downRay = new RaycastHit2D();
        bool lookRight = true;

        #region modifiable variables

        [Header("Parameters")]
        [SerializeField]private float dragOffset = 0.1f;
        [SerializeField]private float rayDistance = 2;
        [SerializeField]private LayerMask GroundMask = new LayerMask();
        [SerializeField]private float minimalVelocityY = 2;
        [SerializeField]private float frictionOffset = 1;
        [Header("Jump")]
        public float lastJumpForceRatio = 0.8f;
        public float timeToIdle = 1;
        private float timerBeforeIdle = 1;
        ///////
        Image enduranceBarImage = null;
        public float timeToFillEndurence = 10;
        public float amountForJump = 0.40f;
        public Color colorNormalJump = Color.blue;
        public Color colorSmallJump = Color.red;
        ///////

        #endregion
        [Header("Joystick")]
        public LineRenderer lrJoystick;
        private void Start() {
            if(instance != this){
                instance = this;
            }
            
            timerBeforeIdle = timeToIdle;
            catAnimator = GetComponent<Animator>();
            rigid = GetComponent<Rigidbody2D>();
            enduranceBarImage = GameObject.FindWithTag("enduranceBar").GetComponent<Image>();

            lrJoystick.positionCount = 2;
            GetComponent<CollisionController>().enabled = true;
            healthController = GetComponent<PlayerHealthController>();
        }
        void Update(){
            if(isAlive && canMove){    
                if(isDraging){
                    dragPoint = Input.mousePosition;
                    isDragLenghGood = Mathf.Abs((clickPoint-dragPoint).magnitude) > dragOffset;
                    //if condition are okey
                    if(enduranceBarImage.fillAmount >= amountForJump){
                        catAnimator.SetBool("isFocusing", true);
                    }
                }else
                {
                    catAnimator.SetBool("isFocusing", false);
                }
                if (Input.GetMouseButtonDown(0)) {
                    clickPoint = Input.mousePosition;
                    isDraging = true;
                }else if (Input.GetMouseButtonUp(0)) {
                    isDraging = false;
                    if(isDragLenghGood && enduranceBarImage.fillAmount > 0){
                        float endToConsume = amountForJump;
                        //Jump
                        if(enduranceBarImage.fillAmount >= amountForJump){
                            rigid.velocity = ((Vector2.ClampMagnitude(GetDragLength(), maxDragRange)/maxDragRange) * force);
                        }else if(enduranceBarImage.fillAmount >= amountForJump/2){
                            rigid.velocity = ((Vector2.ClampMagnitude(GetDragLength(), maxDragRange)/maxDragRange) * force * lastJumpForceRatio);   
                        }else{
                            endToConsume = 0;
                        }
                        enduranceBarImage.fillAmount -= endToConsume;
                        isDragLenghGood = false;
                    }
                }
                if(isDraging){
                    TimeManager.instance.SlowTime();
                }else if(Time.timeScale != 1)
                {
                    TimeManager.instance.SetNormalTime();
                }
                if(rigid.velocity.magnitude > 0.02f)
                    SetRotationFromVelocity();
                ShowDirection();
                SpeedXScaler();
                JumpManagerEndurance();
                AnimateIdling();
            }
        }

        private void LateUpdate() {
            JoystickDisplay(clickPoint,Input.mousePosition,isDraging,maxDragRange);    
        }
        private void FixedUpdate() {
            JoystickDisplay(clickPoint,Input.mousePosition,isDraging,maxDragRange);    
        }
        Vector2 GetDragLength(){
            return (Vector2)Camera.main.ScreenToWorldPoint(clickPoint) - (Vector2)Camera.main.ScreenToWorldPoint(dragPoint);
        }
        public float GetDragLengthClamped(){
            return (Vector2.ClampMagnitude(GetDragLength(), maxDragRange)/maxDragRange).magnitude;
        }
        public bool getIsDraging(){
            return isDraging;
        }
        public RaycastHit2D GetDownRay(){
            return downRay;
        }
        void SetRotationFromVelocity(){
            downRay = Physics2D.Raycast((Vector2)transform.position, Vector2.down , rayDistance, GroundMask);
            if(rigid.velocity.magnitude > 0.02f){
                if(rigid.velocity.x < 0){
                    transform.localScale = new Vector3(1,-1,0);
                    arrowObject.transform.localScale = new Vector3(1,-1,0);
                    lookRight = false;
                }else if(rigid.velocity.x > 0)
                {
                    transform.localScale = new Vector3(1,1,0);
                    arrowObject.transform.localScale = new Vector3(1,1,0);
                    lookRight = true;
                }
            }else
            {
                transform.localScale = new Vector3(1,lookRight ?1:-1,0);                
            }
            if(Mathf.Abs(rigid.velocity.magnitude) > 0.02f){
                Vector2 dir = rigid.velocity.normalized;
                float angle = Mathf.Atan2(dir.x,dir.y)*Mathf.Rad2Deg;
                //Si Velocité.y < seuil && ray hit touche un sol, rotation.z = 0 || 180


                if(Mathf.Abs(rigid.velocity.y) < minimalVelocityY && downRay.collider != null){
                    transform.eulerAngles = new Vector3(0,0, lookRight?0:180);
                }else if(rigid.velocity.x != 0 || Mathf.Abs(rigid.velocity.magnitude) > 5f) {
                    transform.eulerAngles = new Vector3(0,0,-angle+90);
                }
            }else{
                rigid.velocity = Vector2.zero;
            }
        }
        void ShowDirection(){
            float angle = 0;
            if(Input.GetMouseButton(0)){
                if(!isShowingDirection && isDragLenghGood){
                    isShowingDirection = true;
                    arrowObject.SetActive(true);
                }
                Vector2 dir = (GetDragLength()).normalized;
                angle = Mathf.Atan2(dir.x,dir.y)*Mathf.Rad2Deg;

                arrowObject.transform.eulerAngles = new Vector3(0,0,-angle);
            }
            if(!Input.GetMouseButton(0) || !isDragLenghGood){
                isShowingDirection = false;
                arrowObject.SetActive(false);
            }
        }
        public Rigidbody2D GetRigidbody2D(){
            return this.rigid;
        }

        public void SpeedXScaler(){
            if(downRay.collider != null && downRay.collider.CompareTag("Ground")){
                Vector2 velo = rigid.velocity;
                rigid.velocity = new Vector2(velo.x * frictionOffset,velo.y);
                if(Mathf.Abs(rigid.velocity.magnitude) < 0.02f && (transform.rotation.z == 0 || transform.rotation.z == 180)){
                    rigid.velocity = Vector2.zero;
                }
            }
        }
        void JumpManagerEndurance(){
            if(enduranceBarImage.fillAmount < 1){
                enduranceBarImage.fillAmount += Time.deltaTime/timeToFillEndurence;
            }
            if(enduranceBarImage.fillAmount <= 0.99f && enduranceBarImage.fillAmount >= amountForJump){
                enduranceBarImage.color = colorNormalJump;
            }else if(enduranceBarImage.fillAmount < amountForJump){
                enduranceBarImage.color = colorSmallJump;
            }
        }
        public void AddJumpCount(){
            if(enduranceBarImage.fillAmount < 1){
                enduranceBarImage.fillAmount += amountForJump;
            }
        }
        void AnimateIdling(){
            if(!isDraging && timerBeforeIdle > 0 && Mathf.Abs(rigid.velocity.magnitude) < 0.05f){
                timerBeforeIdle -= Time.deltaTime;
            }else if(isDraging || Mathf.Abs(rigid.velocity.magnitude) > 0.05f)
            {
                timerBeforeIdle = timeToIdle;
                catAnimator.SetBool("isIdling",false);
            }
            if(timerBeforeIdle <= 0){
                catAnimator.SetBool("isIdling",true);
            }
        }
        void JoystickDisplay(Vector2 initialPos,Vector2 mouseDrag, bool isDraging, float magnClamp){
            if(isDraging){
                if(!lrJoystick.gameObject.activeInHierarchy) {
                    lrJoystick.gameObject.SetActive(true);
                }
                initialPos = Camera.main.ScreenToWorldPoint(initialPos);
                mouseDrag = Camera.main.ScreenToWorldPoint(mouseDrag);
                Vector3 newInitPos = new Vector3(initialPos.x, initialPos.y, -5);
                Vector3 newMouseDrag = new Vector3(mouseDrag.x, mouseDrag.y, -5);
                lrJoystick.SetPosition(1,newInitPos);

                if((newInitPos - newMouseDrag).magnitude > magnClamp){
                    newMouseDrag = (Vector3.ClampMagnitude((newMouseDrag - newInitPos), magnClamp) + newInitPos);
                }
                lrJoystick.SetPosition(0,newMouseDrag);


            }else{
                if(lrJoystick.gameObject.activeInHierarchy)
                    lrJoystick.gameObject.SetActive(false);
            }
        }

        public void SetIsAlive(bool state){
            isAlive = state;
        }
        public bool GetLookingRight(){
            return lookRight;
        }
    }
}