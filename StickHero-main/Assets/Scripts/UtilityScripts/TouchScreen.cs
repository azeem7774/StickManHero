using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchScreen : MonoBehaviour
{
    private void Update()
    {
        // Check if at least one touch is detected
        if (Input.touchCount > 0)
        {
            // Get the first touch (index 0) in the array of touches
            Touch touch = Input.GetTouch(0);

            // Check the phase of the touch
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // The touch has just started (finger down)
                    Debug.Log("Touch began");
                    break;

                case TouchPhase.Moved:
                    // The touch is moving (finger dragging)
                    Debug.Log("Touch moved");
                    break;

                case TouchPhase.Ended:
                    // The touch has ended (finger up)
                    Debug.Log("Touch ended");
                    break;
            }
        }
    }
}
