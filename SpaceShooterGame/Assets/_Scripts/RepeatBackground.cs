using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 2f;

    private Vector3 startPos;
    private float repeatWidth;

    private SpriteRenderer _sr;

    private void Start()
    {
        startPos = transform.position;
        _sr = GetComponent<SpriteRenderer>();

        repeatWidth = _sr.bounds.size.x;
    }

    private void Update()
    {
        //Move background left
        transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);

        //Reset when off screen
        if (transform.position.x < startPos.x - repeatWidth)
        {
            transform.position = startPos;
        }
    }
}
