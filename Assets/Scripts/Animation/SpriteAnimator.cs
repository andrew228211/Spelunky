using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Timeline;

public class SpriteAnimator : MonoBehaviour
{
    public SpriteAnimation[] animations;
    // Thiet lap toc do animation
    public int fps;
    //Xay ra truong hop khong lap hinh anh animation
    public bool looping;
    public bool FrameAfterNonLoopAnimation;
    //Tao sprite mac dinh 
    [HideInInspector]
    public Sprite defaultSprite;
    //Frame hien tai cua animation 
    [HideInInspector]
    public int currentFrame;
    [HideInInspector]
    public SpriteAnimation currentAnimation;
    private SpriteRenderer _spriteRenderer;
    private float _timer;
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        defaultSprite = _spriteRenderer.sprite;
        currentAnimation = animations[0];
    }
    private void Update()
    {
        //Khong lam gi neu khong co animation
        if (currentAnimation == null)
        {
            return;
        }
        //khong lam gi neu animation hien tai khong co gi
        if (currentAnimation.frames.Length == 0)
        {
            return;
        }
        if (currentFrame == currentAnimation.frames.Length && !looping)
        {
            if (FrameAfterNonLoopAnimation)
            {
                _spriteRenderer.sprite = null;
            }
            return;
        }
        currentFrame %= currentAnimation.frames.Length;
        if (fps == 0)
        {
            _spriteRenderer.sprite = currentAnimation.frames[currentFrame];
            return;
        }
        _spriteRenderer.sprite = currentAnimation.frames[currentFrame];
        _timer += Time.deltaTime;
        if (_timer >= 1f / fps)
        {
            _timer = 0;
            currentFrame++;
        }
    }
    public void Play(string name,bool reset = false)
    {
        if (currentAnimation != null && currentAnimation.name == name)
        {
            return;
        }
        bool found = false;
        foreach(SpriteAnimation animation in animations)
        {
            if (animation.name == name)
            {
                currentAnimation = animation;
              
                if (reset)
                {
                    //Chuyen sang animation moi. Neu khong thi co do tre 1 frame
                    currentFrame = 0;
                    _spriteRenderer.sprite = currentAnimation.frames[currentFrame];
                }
                found = true;
                return;
            }
        }
        if (!found)
        {
            Debug.LogError("Animation " + name + " not found");
        }
    }
    public float GetAnimationLength(string name)
    {
        if(currentAnimation != null && currentAnimation.name == name)
        {
            return currentAnimation.frames.Length * (1f / fps);
        }
        foreach(SpriteAnimation animation in animations)
        {
            if (animation.name == name)
            {
                return animation.frames.Length * (1f / fps);
            }
        }
        Debug.LogError("Null");
        return 0f;
    }
}

