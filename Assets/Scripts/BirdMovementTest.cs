using UnityEngine;
using UnityEngine.SceneManagement;

public class BirdMovementTest : MonoBehaviour
{
    private bool _birdClicked;
    private Vector3 _catapultPosition;
    private bool _chanceOver = false;
    
    // Set value from editor
    public float throwForce;
    public GameObject bird;
    public GameObject catapultArmOne;
    public GameObject catapultArmTwo;
    public float catapultRadius;

    // Start is called before the first frame update
    void Start()
    {
        _birdClicked = false;
        _catapultPosition = bird.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        DrawCatapultStrings();
        
        if (Input.GetMouseButton(0) && !_chanceOver)
        {
            if (!_birdClicked)
            {
                GetBirdClick();
            }

            if (_birdClicked)
            {
                FollowBird();
            }
        }
        else
        {
            if (_birdClicked)
            {
                ThrowBird();
            }
        }

        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void GetBirdClick()
    {
        var clickPositionVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var directionVector = (clickPositionVector - Camera.main.transform.position).normalized;

        var raycast2DHit = Physics2D.Raycast(Camera.main.transform.position, directionVector);
        if (raycast2DHit.collider != null && raycast2DHit.collider.gameObject == bird)
        {
            _birdClicked = true;
        }
    }

    private void FollowBird()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        var distanceBetweenMouseAndCatapult = (mousePosition - _catapultPosition).magnitude;

        if (distanceBetweenMouseAndCatapult < catapultRadius)
        {
            bird.transform.position = mousePosition;
            return;
        }

        var maxX = (mousePosition.x * catapultRadius + _catapultPosition.x *
            (distanceBetweenMouseAndCatapult - catapultRadius)) / distanceBetweenMouseAndCatapult;
        var maxY = (mousePosition.y * catapultRadius + _catapultPosition.y *
            (distanceBetweenMouseAndCatapult - catapultRadius)) / distanceBetweenMouseAndCatapult;

        bird.transform.position = new Vector3(maxX, maxY,0);
    }

    private void ThrowBird()
    {
        var birdPosition = bird.transform.position;
        var directionVector = (_catapultPosition - birdPosition);

        bird.AddComponent<Rigidbody2D>();

        bird.GetComponent<Rigidbody2D>().mass = 3f;
        bird.GetComponent<Rigidbody2D>().AddForce(directionVector * throwForce, ForceMode2D.Impulse);
        _birdClicked = false;
        _chanceOver = true;
    }

    private void DrawCatapultStrings()
    {
        var firstEndPosition = (_chanceOver) ? catapultArmOne.transform.position : bird.transform.position;
        var secondEndPosition = (_chanceOver) ? catapultArmTwo.transform.position : bird.transform.position;
        
        var lineRendererFirst = catapultArmOne.GetComponent<LineRenderer>();
        var lineRendererSecond = catapultArmTwo.GetComponent<LineRenderer>();

        if (lineRendererFirst == null)
        {
            lineRendererFirst = catapultArmOne.AddComponent<LineRenderer>();
        }

        if (lineRendererSecond == null)
        {
            lineRendererSecond = catapultArmTwo.AddComponent<LineRenderer>();
        }
        
        lineRendererFirst.startWidth = 0.07f;
        lineRendererFirst.endWidth = 0.07f;
        
        lineRendererSecond.startWidth = 0.07f;
        lineRendererSecond.endWidth = 0.07f;
        
        lineRendererFirst.SetPosition(0, catapultArmOne.transform.position);
        lineRendererFirst.SetPosition(1, firstEndPosition);
        
        lineRendererSecond.SetPosition(0, catapultArmTwo.transform.position);
        lineRendererSecond.SetPosition(1, secondEndPosition);
    }
}
