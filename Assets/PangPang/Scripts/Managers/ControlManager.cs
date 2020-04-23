using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    public Rigidbody2D playerRb;
    public Transform joypadTr;
    public Transform joystickTr;
    public Transform joysticBgTr;
    public float moveSpeed = 3.5f;

    private float limitJoystickPos = 80;

    //private Vector2 defaultJoypadScreenPos;
    private Vector2 tempJoystickBgPos;

    private bool isEditorMoving = false;

    private void Start()
    {
        // 조이스틱 고정형
        // defaultJoypadScreenPos = UICamera.mainCamera.WorldToScreenPoint(joypadTr.position);
    }

    public void OnPress()
    {
        tempJoystickBgPos = UICamera.lastEventPosition;
        joypadTr.position = UICamera.mainCamera.ScreenToWorldPoint(tempJoystickBgPos);
        joypadTr.gameObject.SetActive(true);

        StartCoroutine("CoMoving");
    }

    public void OnRelease()
    {
        StopCoroutine("CoMoving");
        ResetController();
    }

    private void Update()
    {
        float axisX = Input.GetAxisRaw("Horizontal");
        if (axisX != 0 && isEditorMoving == false)
        {
            isEditorMoving = true;
            joypadTr.gameObject.SetActive(true);
            StartCoroutine("CoMoving");
        }
        else if (axisX == 0 && isEditorMoving == true)
        {
            isEditorMoving = false;
            StopCoroutine("CoMoving");
            ResetController();
        }

        if (axisX > 0)
        {
            joystickTr.localPosition = new Vector3(75, 0, 0);
        }
        else if (axisX < 0)
        {
            joystickTr.localPosition = new Vector3(-75, 0, 0);
        }
        else
        {
            joystickTr.localPosition = Vector2.zero;
        }
    }

    void ResetController()
    {
        joystickTr.localPosition = Vector3.zero;
        joysticBgTr.localPosition = Vector3.zero;
        joypadTr.gameObject.SetActive(false);
        playerRb.velocity = Vector2.zero;
        PlayerManager.instance.SetAnimaiton(PlayerManager.AniType.Idle);
    }

    IEnumerator CoMoving()
    {
        while (true)
        {
            //Debug.Log("CoMoving");
            if ( !isEditorMoving )
            {
                Vector2 touchScreenPos = UICamera.lastEventPosition;
                joystickTr.position = UICamera.mainCamera.ScreenToWorldPoint(touchScreenPos);

                //고정형
                //float distance = Vector2.Distance(defaultJoypadScreenPos, touchScreenPos);

                float distance = Vector2.Distance(tempJoystickBgPos, touchScreenPos);

                if (distance > limitJoystickPos)
                {
                    float angle = GetAngle(touchScreenPos, tempJoystickBgPos);
                    tempJoystickBgPos = GetLimitPos(touchScreenPos, angle, limitJoystickPos);
                    joysticBgTr.position = UICamera.mainCamera.ScreenToWorldPoint(tempJoystickBgPos);
                }
            }

            //if (distance < limitJoystickPos)
            //{
            //    joystickTr.position = UICamera.mainCamera.ScreenToWorldPoint(touchScreenPos);
            //}
            //else
            //{
            //    //고정형
            //    //float angle = GetAngle(defaultJoypadScreenPos, touchScreenPos);
            //    //Vector2 limitPos = GetLimitPos(defaultJoypadScreenPos, angle, limitJoystickPos);

            //    float angle = GetAngle(tempJoystickBgPos, touchScreenPos);
            //    Vector2 limitPos = GetLimitPos(tempJoystickBgPos, angle, limitJoystickPos);
            //    joystickTr.position = UICamera.mainCamera.ScreenToWorldPoint(limitPos);
            //}

            // 고정형
            //float disX = joystickTr.localPosition.x
            float disX = joystickTr.localPosition.x - joysticBgTr.localPosition.x;
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
