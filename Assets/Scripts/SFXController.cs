using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXController : MonoBehaviour
{
    public RuntimeAnimatorController controller;

    public struct SFXRenderer
    {
        public Transform transform;
        public Animator animator;
        public SpriteRenderer spriteRenderer;
        public Coroutine currentAction;
    }

    public delegate void SFXRendererCallback(SFXRenderer sfx);

    private Transform _anchorPoint;

    private List<SFXRenderer> _persistentRenderers;

    private List<SFXRenderer> _renderers;
    private int _current = 0;
    private int _limit;

    public void Init(int rendererLimit, Transform anchor)
    {
        _renderers = new List<SFXRenderer>();
        _persistentRenderers = new List<SFXRenderer>();
        _limit = rendererLimit;
        _anchorPoint = anchor;
    }

    public void SpawnSFXAnimation(Vector2 offset, string anim, bool flip = false)
    {
        if(_renderers.Count < _limit)
        {
            _renderers.Add(CreateSFXUnit());
        }

        SFXRenderer sfx = _renderers[_current];

        sfx.transform.position = _anchorPoint.position + (Vector3)offset;
        PlayAnimation(sfx, anim);
        _current = (_current + 1) % _limit;
    }

    public void SpawnPersistentSFXAnimation(Vector2 offset, Vector2 scale, string anim, bool flip = false)
    {
        _persistentRenderers.Add(CreateSFXUnit());

        var sfx = _persistentRenderers[_persistentRenderers.Count - 1];
        sfx.transform.parent = _anchorPoint.transform;

        sfx.transform.localPosition = (Vector3)offset;
        sfx.transform.localScale = scale;
        sfx.spriteRenderer.flipX = flip;

        PlayLoopingAnimation(sfx, anim);
    }

    //=========== PRIVATE ================/
    
    private void PlayAnimation(SFXRenderer sfx, string anim)
    {
        if (sfx.currentAction != null)
            StopCoroutine(sfx.currentAction);

        SFXRendererCallback callback = delegate (SFXRenderer sfxR)
        {
            sfxR.spriteRenderer.enabled = false;
            sfxR.animator.enabled = false;
        };

        sfx.spriteRenderer.enabled = true;
        sfx.animator.enabled = true;

        sfx.currentAction =
            StartCoroutine(
                PlayTimedAnim(anim, sfx, callback));
    }

    private void PlayLoopingAnimation(SFXRenderer sfx, string anim)
    {
        if (sfx.currentAction != null)
            StopCoroutine(sfx.currentAction);

        SFXRendererCallback callback = delegate (SFXRenderer sfxR) {};

        sfx.spriteRenderer.enabled = true;
        sfx.animator.enabled = true;

        sfx.currentAction =
            StartCoroutine(
                PlayLoopedAnim(anim, sfx, callback));
    }

    private IEnumerator PlayTimedAnim(string animation, SFXRenderer renderer, SFXRendererCallback callback)
    {
        renderer.animator.Play(animation, -1, 0);
        yield return new WaitForEndOfFrame();

        float time = renderer.animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(time);

        callback(renderer);
    }

    private IEnumerator PlayLoopedAnim(string animation, SFXRenderer renderer, SFXRendererCallback callback)
    {
        renderer.animator.Play(animation, -1, 0);
        yield return new WaitForEndOfFrame();

        while (true)
        {
            float time = renderer.animator.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(time);

            callback(renderer);
        }
    }

    private SFXRenderer CreateSFXUnit()
    {
        GameObject obj = GameObject.Instantiate(new GameObject(), transform);

        SpriteRenderer sprite = obj.AddComponent<SpriteRenderer>();
        sprite.sortingOrder = 1;
        sprite.enabled = false;

        Animator attachedAnimator = obj.AddComponent<Animator>();
        attachedAnimator.runtimeAnimatorController = controller;
        attachedAnimator.enabled = false;

        SFXRenderer r;
        r.transform = obj.transform;
        r.animator = attachedAnimator;
        r.spriteRenderer = sprite;
        r.currentAction = null;

        return r;
    }

}
