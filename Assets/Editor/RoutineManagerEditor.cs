using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EditorManager))]
public class RoutineManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (!Application.isPlaying)
        {
            EditorGUILayout.HelpBox("RoutineManagerEditor sadece oyun çalıştığında aktif olur.", MessageType.Info);
            return;
        }

        EditorManager editorManager = (EditorManager)target;

        DayManager dayManager = editorManager.dayManager;
        if (dayManager == null)
        {
            EditorGUILayout.HelpBox("DayManager referansı atanmadı. Lütfen EditorManager'da DayManager referansını ayarlayın.", MessageType.Warning);
        }
        else
        {
            EditorGUILayout.LabelField("Day Manager", EditorStyles.boldLabel);
            Day newDay = (Day)EditorGUILayout.EnumPopup("Current Day", dayManager.currentDay);
            if (newDay != dayManager.currentDay)
            {
                Undo.RecordObject(dayManager, "Change Current Day");
                dayManager.currentDay = newDay;
                EditorUtility.SetDirty(dayManager);
            }
        }

        DailyRoutineManager routineManager = editorManager.dailyRoutineManager;
        if (routineManager == null)
        {
            EditorGUILayout.HelpBox("DailyRoutineManager referansı atanmadı. Lütfen EditorManager'da DailyRoutineManager referansını ayarlayın.", MessageType.Warning);
        }
        else
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Daily Routine Manager", EditorStyles.boldLabel);
            DailyRoutineBaseState newState = null;

            string[] stateOptions = { "HeadcountState", "ChowtimeState", "RectimeState", "BedtimeState" };
            int currentStateIndex = System.Array.IndexOf(stateOptions, routineManager.currentState.GetType().Name);
            int newStateIndex = EditorGUILayout.Popup("Current State", currentStateIndex, stateOptions);

            if (newStateIndex != currentStateIndex)
            {
                Undo.RecordObject(routineManager, "Change Current State");
                switch (newStateIndex)
                {
                    case 0:
                        newState = routineManager.headcountState;
                        break;
                    case 1:
                        newState = routineManager.chowtimeState;
                        break;
                    case 2:
                        newState = routineManager.rectimeState;
                        break;
                    case 3:
                        newState = routineManager.bedtimeState;
                        break;
                }

                if (newState != null)
                {
                    routineManager.SwitchState(newState);
                    EditorUtility.SetDirty(routineManager);
                }
            }
        }

        EditorGUILayout.Space();
        DrawDefaultInspector();
    }
}
