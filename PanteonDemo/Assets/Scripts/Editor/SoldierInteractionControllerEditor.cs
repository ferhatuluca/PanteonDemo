using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Core.GameUnits.Soldiers;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(SoldierInteractionController))]
    public class SoldierInteractionControllerEditor : UnityEditor.Editor
    {
        private const BindingFlags Flags = BindingFlags.NonPublic | BindingFlags.Instance;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Interacts (Runtime)", EditorStyles.boldLabel);

            if (!Application.isPlaying)
            {
                EditorGUILayout.HelpBox("Only available in Play Mode.", MessageType.Info);
                return;
            }

            var controller = (SoldierInteractionController)target;
            var field = typeof(SoldierInteractionController).BaseType?.GetField("Interacts", Flags);

            if (field == null)
            {
                EditorGUILayout.HelpBox("Could not find 'Interacts' field via reflection.", MessageType.Warning);
                return;
            }

            var interacts = field.GetValue(controller) as IEnumerable;

            if (interacts == null)
            {
                EditorGUILayout.HelpBox("Interacts is null.", MessageType.Info);
                return;
            }

            int count = 0;
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                foreach (var item in interacts)
                {
                    count++;
                    if (item is Component component)
                    {
                        EditorGUILayout.ObjectField(
                            $"[{count}]",
                            component.gameObject,
                            typeof(GameObject),
                            true
                        );
                    }
                    else
                    {
                        EditorGUILayout.LabelField($"[{count}]", item?.ToString() ?? "null");
                    }
                }
            }

            if (count == 0)
            {
                EditorGUILayout.HelpBox("Interacts is empty.", MessageType.Info);
            }

            Repaint();
        }
    }
}
