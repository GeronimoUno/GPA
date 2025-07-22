using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.Playables;   // для PlayableDirector
using UnityEngine.Timeline;    // для TimelineAsset, AnimationTrack, TimelineClip
using UnityEditor.Timeline;    // для MenuItem и работы с таймлайном

public class TimelineAutoAlign
{
    [MenuItem("Tools/Timeline/Batch Assign Clips By Name")]
    static void BatchAssign()
    {
        var go = Selection.activeGameObject;
        if (go == null)
        {
            Debug.LogError("Выберите GameObject с PlayableDirector!");
            return;
        }

        var director = go.GetComponent<PlayableDirector>();
        if (director == null)
        {
            Debug.LogError("На этом объекте нет PlayableDirector!");
            return;
        }

        var timeline = director.playableAsset as TimelineAsset;
        if (timeline == null)
        {
            Debug.LogError("PlayableDirector не содержит TimelineAsset!");
            return;
        }

        // Укажите здесь папку с вашими .anim
        string animFolder = "Assets/Animations";
        var animFiles = Directory.GetFiles(animFolder, "*.anim", SearchOption.AllDirectories);

        foreach (var track in timeline.GetOutputTracks())
        {
            if (!(track is AnimationTrack)) 
                continue;

            foreach (var clip in track.GetClips())
            {
                var animAsset = clip.asset as AnimationPlayableAsset;
                if (animAsset != null && animAsset.clip == null)
                {
                    string clipName = clip.displayName;
                    foreach (var path in animFiles)
                    {
                        if (Path.GetFileNameWithoutExtension(path) == clipName)
                        {
                            var anim = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
                            if (anim != null)
                            {
                                animAsset.clip = anim;
                                Debug.Log($"Assigned {clipName} → {path}");
                            }
                            break;
                        }
                    }
                }
            }
        }

        AssetDatabase.SaveAssets();
        Debug.Log("Batch assign complete.");
    }
}
