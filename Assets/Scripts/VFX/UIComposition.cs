using UnityEngine;

/// <summary>
/// Amplify Bloom provided class to marge main camera with glow camera
/// </summary>
public class UIComposition : MonoBehaviour
{
	public Camera MainCamera;
	private RenderTexture m_mainCameraRT;
	private Material m_combineAllMat;

	void Start()
	{
		m_combineAllMat = new Material( Shader.Find( "Hidden/BloomUICombineAll" ));
		m_combineAllMat.hideFlags = HideFlags.HideAndDontSave;
		m_mainCameraRT = new RenderTexture( MainCamera.pixelWidth, MainCamera.pixelHeight,16, ( MainCamera.hdr ? RenderTextureFormat.DefaultHDR : RenderTextureFormat.Default ) );
		MainCamera.targetTexture = m_mainCameraRT;
		m_combineAllMat.SetTexture( "_GameTex", m_mainCameraRT );
	}

	void OnRenderImage( RenderTexture source, RenderTexture dest )
	{
		Graphics.Blit( source, dest, m_combineAllMat, 0 );
	}
}
