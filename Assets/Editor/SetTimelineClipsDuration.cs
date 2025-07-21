using UnityEngine;
using UnityEditor;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public static class SetTimelineClipsDuration
{
    [MenuItem("Tools/Timeline/Set All Clips Duration to 2s")]
    public static void Execute()
    {
        if (Selection.activeGameObject == null)
        {
            Debug.LogError("Выберите GameObject с PlayableDirector.");
            return;
        }
        var director = Selection.activeGameObject.GetComponent<PlayableDirector>();
        if (director == null)
        {
            Debug.LogError("У выбранного объекта нет PlayableDirector.");
            return;
        }
        var timeline = director.playableAsset as TimelineAsset;
        if (timeline == null)
        {
            Debug.LogError("Привязанный PlayableAsset не является TimelineAsset.");
            return;
        }

        const double newDuration = 2.0;
        int count = 0;
        foreach (var track in timeline.GetOutputTracks())
        {
            if (track is AnimationTrack)
            {
                foreach (var clip in track.GetClips())
                {
                    clip.duration = newDuration;
                    count++;
                }
            }
        }
        EditorUtility.SetDirty(timeline);
        Debug.Log($"Установлена длительность {newDuration}s для {count} клипов.");
    }
}
