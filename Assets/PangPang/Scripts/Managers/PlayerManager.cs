using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    public enum AniType
    {
        Idle, Move, Attack
    }

    private void Awake()
    {
        instance = this;
    }

    public void SetAnimaiton(AniType type)
    {

        switch (type)
        {
            case AniType.Attack:
                animator.Play(type.ToString(), -1, 0);
                break;
            default:
                animator.Play(type.ToString());
                break;
        }
    }

    public void SetFlip(bool isFlip)
    {
        spriteRenderer.flipX = isFlip;
    }

    private void OnDestroy()
    {
        instance = null;
    }
}
