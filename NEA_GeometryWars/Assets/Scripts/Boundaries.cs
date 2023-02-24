using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundaries : MonoBehaviour
{
    //source: https://youtu.be/ailbszpt_AI

    private Vector2 screenBound;
    private float radius;
    
    void Start()
    {
        screenBound = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        radius = GetComponent<CircleCollider2D>().radius;
        //to set up boundaries of the world view and radius
    }

    //This function is used to set the position to always be within the screen boundaries
    //This function is run after all other update functions has been called
    //The LateUpdate function helps with keeping the objects in screen as it reflects the objects movement after updates so checks if they're still within screen
    private void LateUpdate()
    {
        Vector2 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, screenBound.x*-1 + radius, screenBound.x - radius);
        viewPos.y = Mathf.Clamp(viewPos.y, screenBound.y*-1 + radius, screenBound.y - radius);
        //Mathf.Clamp takes 3 parameters, if the 1st parameter  is within the 2nd parameter (min) and 3rd (max) then it doesnt change.
        //Otherwise, it takes the min value if its below the minimum or max if its above the maximum
        transform.position = viewPos;
    }
}
