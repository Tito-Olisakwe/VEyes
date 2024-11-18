using UnityEngine;

public class SwipeNavigationLeanTween : MonoBehaviour
{
    public RectTransform[] pages;
    private int currentPage = 0;
    private Vector2 touchStartPos, touchEndPos;
    private bool isSwiping = false;

    public float swipeThreshold = 50f;
    public float transitionDuration = 0.5f;

    private void Start()
    {
       for (int i = 0; i < pages.Length; i++)
      {
        float targetPositionX = (i - currentPage) * Screen.width;
        pages[i].anchoredPosition = new Vector2(targetPositionX, pages[i].anchoredPosition.y);
      }
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                isSwiping = true;
                touchStartPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved && isSwiping)
            {
                touchEndPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended && isSwiping)
            {
                isSwiping = false;
                HandleSwipe();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            isSwiping = true;
            touchStartPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0) && isSwiping)
        {
            isSwiping = false;
            touchEndPos = Input.mousePosition;
            HandleSwipe();
        }
    }

    private void HandleSwipe()
    {
        float swipeDistance = touchEndPos.x - touchStartPos.x;

        if (Mathf.Abs(swipeDistance) > swipeThreshold)
        {
            if (swipeDistance > 0)
            {
                NavigateToPage(currentPage - 1);
            }
            else
            {
                NavigateToPage(currentPage + 1);
            }
        }
    }

    private void NavigateToPage(int newPage)
    {
        if (newPage >= 0 && newPage < pages.Length)
        {
            currentPage = newPage;

            for (int i = 0; i < pages.Length; i++)
            {
                float targetPositionX = (i - currentPage) * Screen.width;
                LeanTween.moveX(pages[i], targetPositionX, transitionDuration).setEase(LeanTweenType.easeInOutQuad);
            }
        }
    }
}
