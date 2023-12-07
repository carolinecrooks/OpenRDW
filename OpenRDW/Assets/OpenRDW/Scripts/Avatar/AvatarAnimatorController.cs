using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarAnimatorController : MonoBehaviour
{
    [HideInInspector]
    public int avatarId;
    
    private GlobalConfiguration globalConfiguration;
    private RedirectionManager redirectionManager;
    private Vector3 prePos;
    void Start()
    {
        globalConfiguration = GetComponentInParent<GlobalConfiguration>();
        try
        {
            redirectionManager = globalConfiguration.redirectedAvatars[avatarId].GetComponent<RedirectionManager>();
        }
        catch {

        }        
    }

    // Update is called once per frame
    void Update()
    {
        var animator = GetComponentInChildren<Animator>();        
        var walkSpeed = (redirectionManager.currPos - prePos).magnitude / globalConfiguration.GetDeltaTime();
        UpdateAnimator(animator, walkSpeed, globalConfiguration.GetDeltaTime());
        prePos = redirectionManager.currPos;
    }
    public static void UpdateAnimator(Animator animator, float walkSpeed, float time)
    {
        if (animator == null)
            return;        
        //Debug.Log("Updating");
        animator.SetFloat("walkSpeed", walkSpeed);

        //update animator
        animator.speed = 1;

        //Can't call Animator.Update on inactive object
        if (animator.gameObject.activeSelf)
            animator.Update(time);
        
        animator.speed = 0;        
    }

/*    private void OnTriggerEnter(Collider collision)
    {
        //Check for a match with the specified name on any GameObject that collides with your GameObject
        if (collision.gameObject.tag == "CarolinePath")
        {
            //If the GameObject's name matches the one you suggest, output this message in the console
            Debug.Log("Do something here");
        }

        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        if (collision.gameObject.tag == "MyGameObjectTag")
        {
            //If the GameObject has the same tag as specified, output this message in the console
            Debug.Log("Do something else here");
        }
    }
*/
    
}
