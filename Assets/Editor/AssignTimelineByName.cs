using UnityEngine;
using UnityEditor;
using UnityEngine.Timeline;
using UnityEditor.Timeline;
using System.IO;

public class AssignTimelineByName
{
    [MenuItem("Tools/Timeline/Batch Assign Clips By Name")]
    static void BatchAssign()
    {
        // Получаем выбранный TimelineAsset
        var go = Selection.activeGameObject;
        var director = go?.GetComponent<PlayableDirector>();
        if (director == null)
        {
            Debug.LogError("Выберите GameObject с PlayableDirector!");
            return;
        }
        var timeline = director.playableAsset as TimelineAsset;
        if (timeline == null)
        {
            Debug.LogError("На этом объекте нет TimelineAsset!");
            return;
        }

        // Папка, где лежат .anim (в корне Assets)
        var animFolder = "Assets/Animations";
        var animFiles = Directory.GetFiles(animFolder, "*.anim", SearchOption.AllDirectories);

        foreach (var track in timeline.GetOutputTracks())
        {
            if (!(track is AnimationTrack)) continue;

            foreach (var clip in track.GetClips())
            {
                if (clip.animationClip == null)
                {
                    var clipName = clip.displayName;
                    // ищем файл с таким же именем
                    foreach (var path in animFiles)
                    {
                        if (Path.GetFileNameWithoutExtension(path) == clipName)
                        {
                            var anim = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
                            clip.animationClip = anim;
                            Debug.Log($"Assigned {clipName} → {path}");
                            break;
                        }
                    }
                }
            }
        }

        // Сохраняем изменения
        AssetDatabase.SaveAssets();
        Debug.Log("Batch assign complete.");
    }
}
