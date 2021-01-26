using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AngryProject
{
    
    public class AnimatorFonctions : MonoBehaviour
    {
        FireSkullController fireSkullController;
        // Start is called before the first frame update
        void Start()
        {
            fireSkullController = GetComponentInParent<FireSkullController>();
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        public void ParentSetIsDashing(bool state){
            fireSkullController.SetIsDashing(state);
        }
        public void ParentSetIsReachableTrue(){
            fireSkullController.SetIsReachable(true);
        }
        public void ParentSetIsReachableFalse(){
            fireSkullController.SetIsReachable(false);
        }
        public bool ParentGetIsReachable(){
            return fireSkullController.getIsReachable();
        }

        public void SetDashingAnimator(){
            fireSkullController.GetAnimator().SetTrigger("Dash");
        }
        public void ParentSetIsIdleTrue(){
            fireSkullController.SetIsIdle(true);
        }
        public void ParentSetIsIdleFalse(){
            fireSkullController.SetIsIdle(false);
        }
        public void ParentDashThroughPlayer(){
            fireSkullController.DashThroughPlayer();
        }
    }
}
