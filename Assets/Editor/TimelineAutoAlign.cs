using UnityEngine;
using UnityEditor;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public static class TimelineAutoAlign
{
    [MenuItem("Tools/Timeline/Align Phase Clips To Sequence")]
    public static void AlignPhaseClips()
    {
        if (Selection.activeGameObject == null)
        {
            Debug.LogError("Select the GameObject with the PlayableDirector.");
            return;
        }
        var director = Selection.activeGameObject.GetComponent<PlayableDirector>();
        if (director == null)
        {
            Debug.LogError("No PlayableDirector on selected GameObject.");
            return;
        }
        var timeline = director.playableAsset as TimelineAsset;
        if (timeline == null)
        {
            Debug.LogError("PlayableAsset is not a TimelineAsset.");
            return;
        }

        const double clipDuration = 2.0;
        int index = 0;
        // Пробегаем все треки, ищем AnimationTrack
        foreach (var track in timeline.GetOutputTracks())
        {
            if (!(track is AnimationTrack)) continue;
            foreach (var clip in track.GetClips())
            {
                clip.start = index * clipDuration;
                clip.duration = clipDuration;
                index++;
            }
        }

        // Сохраняем изменения
        EditorUtility.SetDirty(timeline);
        Debug.Log($"Aligned {index} clips: each {clipDuration}s apart.");
    }
}
