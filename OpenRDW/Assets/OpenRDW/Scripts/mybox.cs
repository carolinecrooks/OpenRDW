using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mybox : MonoBehaviour
{
    [HideInInspector]
    public int avatarId;
    
    private GlobalConfiguration globalConfiguration;
    private RedirectionManager redirectionManager;
    private Vector3 prePos;
    private GameObject path_viewer;
    private bool on_path;
    private bool redirecting; 
    void Start()
    {
        path_viewer = GameObject.FindWithTag("PathViewer");
        globalConfiguration = GetComponentInParent<GlobalConfiguration>();
        try
        {
            redirectionManager = globalConfiguration.redirectedAvatars[avatarId].GetComponent<RedirectionManager>();
        }
        catch {

        }        
    }

    void Update() 
    {
        float path_viewer_val = path_viewer.transform.rotation.y;
        float box_val = gameObject.transform.rotation.y;
        float facing_towards_threshold = Mathf.Abs(path_viewer_val - box_val);
        //Debug.Log("angle is: " + path_viewer_val.ToString() + "and " + box_val.ToString() + ", threshold is " + facing_towards_threshold.ToString());
        
        // if on the path but not facing forward, stop the redirection
        if ((facing_towards_threshold > 0.30) & redirecting){
            redirectionManager.UpdateRedirector(typeof(NullRedirector));
            redirecting = false;
        } else if (!redirecting & (facing_towards_threshold <= 0.30)) {
            redirectionManager.UpdateRedirector(typeof(S2ORedirector));
            redirecting = true;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
         Debug.Log("Entered collision with " + collision.gameObject.name);
        //Check for a match with the specified name on any GameObject that collides with your GameObject
        if (collision.gameObject.tag == "CarolinePath")
        {
            //If the GameObject's name matches the one you suggest, output this message in the console
            Debug.Log("Enter found");
            //redirectionManager.UpdateRedirector(typeof(S2ORedirector));
            on_path = true;
            redirecting = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
         Debug.Log("Exiting " + collision.gameObject.name);
        //Check for a match with the specified name on any GameObject that collides with your GameObject
        if (collision.gameObject.tag == "CarolinePath")
        {
            //If the GameObject's name matches the one you suggest, output this message in the console
            Debug.Log("Exit found");
            //redirectionManager.UpdateRedirector(typeof(NullRedirector));
            on_path = false;
            redirecting = false;
        }

    }
}
