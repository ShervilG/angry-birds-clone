using UnityEngine;

public class BirdForceTest : MonoBehaviour
{
    private bool spaceClicked = false;
    
    public float forceToApply;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && !spaceClicked)
        {
            ApplyForceToBird();
            spaceClicked = true;
        }
    }

    private void ApplyForceToBird()
    {
        Vector2 force = Vector2.right;
        force *= forceToApply;
        
        gameObject.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
    }
}
