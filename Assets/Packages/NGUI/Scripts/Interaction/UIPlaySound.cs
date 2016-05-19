//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2015 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Plays the specified sound.
/// </summary>

[AddComponentMenu("NGUI/Interaction/Play Sound")]
public class UIPlaySound : MonoBehaviour
{
	public enum Trigger
	{
		OnClick,
		OnMouseOver,
		OnMouseOut,
		OnPress,
		OnRelease,
		Custom,
		OnEnable,
		OnDisable,
	}

    //!! SALLY -> ADDED
    public string AudioID;

    public AudioClip audioClip;
	public Trigger trigger = Trigger.OnClick;

	[Range(0f, 1f)] public float volume = 1f;
	[Range(0f, 2f)] public float pitch = 1f;

	bool mIsOver = false;

	bool canPlay
	{
		get
		{
			if (!enabled) return false;
			UIButton btn = GetComponent<UIButton>();
			return (btn == null || btn.isEnabled);
		}
	}

	void OnEnable ()
	{
		if (trigger == Trigger.OnEnable)
			Play();
            //NGUITools.PlaySound(audioClip, volume, pitch);
	}

	void OnDisable ()
	{
		if (trigger == Trigger.OnDisable)
            Play();
            //NGUITools.PlaySound(audioClip, volume, pitch);
	}

	void OnHover (bool isOver)
	{
		if (trigger == Trigger.OnMouseOver)
		{
			if (mIsOver == isOver) return;
			mIsOver = isOver;
		}

		if (canPlay && ((isOver && trigger == Trigger.OnMouseOver) || (!isOver && trigger == Trigger.OnMouseOut)))
            Play();
            //NGUITools.PlaySound(audioClip, volume, pitch);
	}

	void OnPress (bool isPressed)
	{
		if (trigger == Trigger.OnPress)
		{
			if (mIsOver == isPressed) return;
			mIsOver = isPressed;
		}

		if (canPlay && ((isPressed && trigger == Trigger.OnPress) || (!isPressed && trigger == Trigger.OnRelease)))
            Play();
            //NGUITools.PlaySound(audioClip, volume, pitch);
	}

	void OnClick ()
	{
		if (canPlay && trigger == Trigger.OnClick)
			Play();
            //NGUITools.PlaySound(audioClip, volume, pitch);
	}

	void OnSelect (bool isSelected)
	{
		if (canPlay && (!isSelected || UICamera.currentScheme == UICamera.ControlScheme.Controller))
			OnHover(isSelected);
	}

	public void Play ()
	{
        if (AudioID == string.Empty) {
            if (audioClip != null)
            {
                //Debug.Log(audioClip.name);
                //AudioController.Play(audioClip.name);
            }
            else {
                Debug.Log("Null Sound");
            }
        }
        else {
            //Debug.Log(AudioID);
            //AudioController.Play(AudioID);
        }

        //NGUITools.PlaySound(audioClip, volume, pitch);
	}
}
