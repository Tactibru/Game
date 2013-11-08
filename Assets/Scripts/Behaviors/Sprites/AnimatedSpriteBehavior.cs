using UnityEngine;
using System.Collections;

/// <summary>
/// Implements animated sprite functionality, to be applied to a Quad.
/// 
/// Author: Ken Murray
/// </summary>
[ExecuteInEditMode]
[AddComponentMenu("Tactibru/Sprites/Animated Sprite")]
public class AnimatedSpriteBehavior : MonoBehaviour
{
	/// <summary>
	/// Speed, in frames per second, at which the animation will play.
	/// </summary>
	public float AnimationSpeed = 10.0f;

	/// <summary>
	/// Texture sheet containing the animations.
	/// </summary>
	public Texture2D AnimationSheet;

	/// <summary>
	/// Number of frames contained in the image.
	/// </summary>
	public int FrameCount;

	/// <summary>
	/// Whether or not the image should be flipped.
	/// </summary>
	public bool IsFlipped;

	/// <summary>
	/// Stores a list of offsets for each frame.
	/// </summary>
	private Vector2[] frameOffsets;

	/// <summary>
	/// Current animation frame being displayed.
	/// </summary>
	private int currentFrame;

	/// <summary>
	/// Amount of time that has passed since the last frame.
	/// </summary>
	private float frameDelta;

	/// <summary>
	/// Size of each frame.
	/// </summary>
	private Vector2 frameSize;

	/// <summary>
	/// Size of each frame, flipped along the horizontal axis.
	/// </summary>
	private Vector2 flippedFrameSize;

	/// <summary>
	/// Tracks the last "IsFlipped" state.
	/// </summary>
	private bool lastFlipState;

	/// <summary>
	/// Calculates the total number of frames in the image, based on the width and height of the texture and frames.
	/// </summary>
	public void Start()
	{
		if (AnimationSheet == null)
			return;

		if (FrameCount <= 0)
			FrameCount = 1;

		frameOffsets = new Vector2[FrameCount];

		float frameWidth = 1.0f / FrameCount;

		for (int _i = 0; _i < FrameCount; _i++)
			frameOffsets[_i] = new Vector2(_i * frameWidth, 0.0f);

		frameSize = new Vector2(1.0f / FrameCount, 1.0f);
		flippedFrameSize = new Vector2(-(1.0f / FrameCount), 1.0f);

		gameObject.renderer.sharedMaterial.SetTextureScale("_MainTex", (IsFlipped ? flippedFrameSize : frameSize));
	}

	/// <summary>
	/// Increments the current frame if the frame time has been passed.
	/// </summary>
	public void Update()
	{
		frameDelta += Time.deltaTime;

		if (frameDelta >= (1.0f / AnimationSpeed)) {
			frameDelta -= (1.0f / AnimationSpeed);

			currentFrame++;

			if (currentFrame >= FrameCount)
				currentFrame = 0;

			gameObject.renderer.sharedMaterial.SetTextureOffset("_MainTex", frameOffsets[currentFrame]);
		}

		if (lastFlipState != IsFlipped) {
			gameObject.renderer.sharedMaterial.SetTextureScale("_MainTex", (IsFlipped ? flippedFrameSize : frameSize));
			lastFlipState = IsFlipped;
		}
	}
}
