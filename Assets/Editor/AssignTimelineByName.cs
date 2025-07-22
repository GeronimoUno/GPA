// Assets/Editor/AssignTimelineByName.cs
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.Playables;   // <-- PlayableDirector тут
using UnityEngine.Timeline;    // TimelineAsset, AnimationTrack, AnimationPlayableAsset
using UnityEditor.Timeline;    // Опционально, но можно оставить

public static class AssignTimelineByName
{
    [MenuItem("Tools/Timeline/Batch Assign Clips By Name")]
    private static void BatchAssign()
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
            Debug.LogError("На выбранном объекте нет PlayableDirector!");
            return;
        }

        var timeline = director.playableAsset as TimelineAsset;
        if (timeline == null)
        {
            Debug.LogError("У PlayableDirector не назначен TimelineAsset!");
            return;
        }

        // Папка, где лежат ваши .anim
        const string animFolder = "Assets/Animations";
        if (!Directory.Exists(animFolder))
        {
            Debug.LogError($"Папка с анимациями не найдена: {animFolder}");
            return;
        }

        var animFiles = Directory.GetFiles(animFolder, "*.anim", SearchOption.AllDirectories);

        foreach (var track in timeline.GetOutputTracks())
        {
            if (track is not AnimationTrack) continue;

            foreach (var clip in track.GetClips())
            {
                // Достаём AnimationPlayableAsset
                var animAsset = clip.asset as AnimationPlayableAsset;
                if (animAsset == null) continue;

                if (animAsset.clip != null) continue; // уже привязано

                var clipName = clip.displayName;
                foreach (var path in animFiles)
                {
                    if (Path.GetFileNameWithoutExtension(path) != clipName) continue;

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

        EditorUtility.SetDirty(timeline);
        AssetDatabase.SaveAssets();
        Debug.Log("Batch assign complete.");
    }
}
