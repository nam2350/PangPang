using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    public Rigidbody2D playerRb;
    public Transform joypadTr;
    public Transform joystickTr;
    public float moveSpeed = 3.5f;

    private float limitJoystickPos = 80;
    private Vector2 defaultJoypadScreenPos;

    private void Start()
    {
        defaultJoypadScreenPos = UICamera.mainCamera.WorldToScreenPoint(joypadTr.position);
    }

    public void OnPress()
    {
        StartCoroutine("CoMoving");
    }

    public void OnRelease()
    {
        StopCoroutine("CoMoving");
        ResetController();
    }

    void ResetController()
    {
        joystickTr.localPosition = Vector3.zero;
        playerRb.velocity = Vector2.zero;
        PlayerManager.instance.SetAnimaiton(PlayerManager.AniType.Idle);
    }

    IEnumerator CoMoving()
    {
        while (true)
        {
            //Debug.Log("CoMoving");

            Vector2 touchScreenPos = UICamera.lastEventPosition;
            float distance = Vector2.Distance(defaultJoypadScreenPos, touchScreenPos);

            if (distance < limitJoystickPos)
            {
                joystickTr.position = UICamera.mainCamera.ScreenToWorldPoint(touchScreenPos);
            }
            else
            {
                float angle = GetAngle(defaultJoypadScreenPos, touchScreenPos);
                Vector2 limitPos = GetLimitPos(defaultJoypadScreenPos, angle, limitJoystickPos);
                joystickTr.position = UICamera.mainCamera.ScreenToWorldPoint(limitPos);
            }

            float disX = joystickTr.localPosition.x;
            if (disX > 5)
            {
                disX = moveSpeed;
                PlayerManager.instance.SetAnimaiton(PlayerManager.AniType.Move);
                PlayerManager.instance.SetFlip(false);
            }
            else if(disX < -5)
            {
                disX = -moveSpeed;
                PlayerManager.instance.SetAnimaiton(PlayerManager.AniType.Move);
                PlayerManager.instance.SetFlip(true);
            }
            else
            {
                disX = 0;
                PlayerManager.instance.SetAnimaiton(PlayerManager.AniType.Idle);
            }

            playerRb.velocity = new Vector2(disX, 0);

            yield return null;
        }   
    }

    float GetAngle(Vector2 startPos, Vector2 targetPos)
    {
        float disX = targetPos.x - startPos.x;
        float disY = targetPos.y - startPos.y;

        return Mathf.Atan2(disY, disX);
    }


    Vector2 GetLimitPos(Vector2 startPos, float angle, float distance)
    {
        Vector2 returnVector = Vector2.zero;
        returnVector.x = distance * Mathf.Cos(angle) + startPos.x;
        returnVector.y = distance * Mathf.Sin(angle) + startPos.y;

        return returnVector;
    }
}
