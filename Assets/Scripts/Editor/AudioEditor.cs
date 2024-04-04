using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AudioSO))]
public class AudioEditor : Editor
{
    private AudioSource previewSource;

    public void OnEnable()
    {
        // Create an AudioSource for previewing. This does not get saved into the scene.
        previewSource = EditorUtility.CreateGameObjectWithHideFlags("AudioPreview", HideFlags.HideAndDontSave, typeof(AudioSource)).GetComponent<AudioSource>();
    }

    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        DrawDefaultInspector();

        AudioSO audio = (AudioSO)target;

        // Only show the play button if an AudioClip is assigned
        if (audio.clip != null)
        {
            if (GUILayout.Button("Play Sound"))
            {
                PlayClip(audio.clip, audio.volume, audio.pitch);
            }
        }
    }

    private void PlayClip(AudioClip clip, float volume, float pitch)
    {
        if (previewSource != null)
        {
            previewSource.Stop(); // Stop the current clip if playing
            previewSource.clip = clip;
            previewSource.volume = volume;
            previewSource.pitch = pitch;
            previewSource.Play();
        }
    }

    void OnDestroy()
    {
        // Clean up the AudioSource when the editor is closed
        if (previewSource != null)
        {
            DestroyImmediate(previewSource.gameObject); // Use DestroyImmediate since this is editor code
        }
    }
}
