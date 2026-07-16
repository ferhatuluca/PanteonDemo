using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public sealed class TinySwordsAnimationExtractorWindow : EditorWindow
{
    private const string UnitsPath = "Assets/ThirdParty/Tiny Swords (Free Pack)/Units";
    private const string AnimationsPath = "Assets/Animations";

    [Serializable]
    private sealed class UnitSelection
    {
        public string unitType;
        public List<string> sheets = new List<string>();
    }

    [SerializeField] private List<string> selectedColors = new List<string>();
    [SerializeField] private List<UnitSelection> selections = new List<UnitSelection>();
    [SerializeField] private float framesPerSecond = 12f;
    [SerializeField] private bool loopAnimations = true;

    [MenuItem("Tools/Tiny Swords/Animation Extractor")]
    private static void Open()
    {
        GetWindow<TinySwordsAnimationExtractorWindow>("Animation Extractor");
    }

    private void OnEnable()
    {
        string[] colors = GetColors();
        selectedColors = selectedColors.Where(colors.Contains).ToList();
        if (selectedColors.Count == 0 && colors.Length > 0) selectedColors.Add(colors[0]);
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Tiny Swords Animation Extractor", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("Add the unit types and animation sheets to export. The available colors, units, and sheets are read directly from the Units folders.", MessageType.Info);

        DrawColorSelector();
        if (selectedColors.Count == 0)
        {
            EditorGUILayout.HelpBox("No color folders were found.", MessageType.Warning);
            return;
        }

        EditorGUILayout.Space();
        DrawUnitTypeAdder();
        DrawSelections();

        EditorGUILayout.Space();
        framesPerSecond = EditorGUILayout.Slider("Frames Per Second", framesPerSecond, 1f, 60f);
        loopAnimations = EditorGUILayout.Toggle("Loop Animations", loopAnimations);

        using (new EditorGUI.DisabledScope(!selections.Any(selection => selection.sheets.Count > 0)))
        {
            if (GUILayout.Button("Extract Animations", GUILayout.Height(30)))
            {
                ExtractAnimations();
            }
        }
    }

    private void DrawColorSelector()
    {
        string[] colors = GetColors();
        int mask = 0;
        for (int index = 0; index < colors.Length; index++)
        {
            if (selectedColors.Contains(colors[index])) mask |= 1 << index;
        }

        int nextMask = EditorGUILayout.MaskField("Unit Colors", mask, colors);
        if (nextMask != mask)
        {
            selectedColors = colors.Where((color, index) => (nextMask & (1 << index)) != 0).ToList();
        }
    }

    private void DrawUnitTypeAdder()
    {
        using (new EditorGUI.DisabledScope(GetAvailableUnitTypes().Length == 0))
        {
            if (GUILayout.Button("+ Add Unit Type"))
            {
                var menu = new GenericMenu();
                foreach (string unitType in GetAvailableUnitTypes())
                {
                    string capturedType = unitType;
                    bool alreadyAdded = selections.Any(selection => selection.unitType == capturedType);
                    menu.AddItem(new GUIContent(capturedType), false, () =>
                    {
                        if (!alreadyAdded)
                        {
                            selections.Add(new UnitSelection { unitType = capturedType });
                        }
                    });
                }
                menu.ShowAsContext();
            }
        }
    }

    private void DrawSelections()
    {
        for (int unitIndex = selections.Count - 1; unitIndex >= 0; unitIndex--)
        {
            UnitSelection selection = selections[unitIndex];
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(selection.unitType, EditorStyles.boldLabel);
            if (GUILayout.Button("Remove", GUILayout.Width(65)))
            {
                selections.RemoveAt(unitIndex);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                continue;
            }
            EditorGUILayout.EndHorizontal();

            foreach (string sheet in selection.sheets.ToArray())
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(sheet);
                if (GUILayout.Button("-", GUILayout.Width(24)))
                {
                    selection.sheets.Remove(sheet);
                }
                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("+ Add Sprite Sheet"))
            {
                ShowSheetMenu(selection);
            }
            EditorGUILayout.EndVertical();
        }
    }

    private void ShowSheetMenu(UnitSelection selection)
    {
        var menu = new GenericMenu();
        foreach (string sheet in GetSheets(selection.unitType))
        {
            string capturedSheet = sheet;
            bool alreadyAdded = selection.sheets.Contains(capturedSheet);
            menu.AddItem(new GUIContent(capturedSheet), alreadyAdded, () =>
            {
                if (!alreadyAdded)
                {
                    selection.sheets.Add(capturedSheet);
                }
            });
        }
        menu.ShowAsContext();
    }

    private void ExtractAnimations()
    {
        EnsureFolder(AnimationsPath);
        string baseFolder = Path.Combine(AnimationsPath, "_Base").Replace('\\', '/');
        EnsureFolder(baseFolder);
        string baseControllerPath = Path.Combine(AnimationsPath, "unit_animator.controller").Replace('\\', '/');
        AnimatorController baseController = AssetDatabase.LoadAssetAtPath<AnimatorController>(baseControllerPath)
            ?? AnimatorController.CreateAnimatorControllerAtPath(baseControllerPath);

        int created = 0;
        foreach (string color in selectedColors)
        {
            string colorName = GetColorName(color);
            string assetColorName = ToAssetName(colorName);
            string colorFolder = Path.Combine(AnimationsPath, colorName).Replace('\\', '/');
            EnsureFolder(colorFolder);
            string overridePath = Path.Combine(colorFolder, $"{assetColorName}_animator.overrideController").Replace('\\', '/');
            var overrideController = AssetDatabase.LoadAssetAtPath<AnimatorOverrideController>(overridePath);
            if (overrideController == null)
            {
                overrideController = new AnimatorOverrideController(baseController);
                AssetDatabase.CreateAsset(overrideController, overridePath);
            }
            else
            {
                overrideController.runtimeAnimatorController = baseController;
            }

            foreach (UnitSelection selection in selections)
            {
                foreach (string sheet in selection.sheets)
                {
                    List<Sprite> sprites = LoadSprites(color, selection.unitType, sheet);
                    if (sprites.Count == 0)
                    {
                        Debug.LogWarning($"No sliced sprites found for {color}/{selection.unitType}/{sheet}. Run the sprite slicer first.");
                        continue;
                    }

                    string animationKey = GetAnimationKey(selection.unitType, sheet);
                    string colorClipPath = Path.Combine(colorFolder, $"{assetColorName}_{animationKey}.anim").Replace('\\', '/');
                    string baseClipPath = Path.Combine(baseFolder, $"{animationKey}.anim").Replace('\\', '/');
                    AnimationClip colorClip = CreateOrUpdateClip(colorClipPath, sprites);
                    AnimationClip baseClip = CreateOrUpdateClip(baseClipPath, sprites);
                    EnsureState(baseController, animationKey, baseClip);
                    overrideController[baseClip] = colorClip;
                    created++;
                }
            }

            EditorUtility.SetDirty(overrideController);
        }

        EditorUtility.SetDirty(baseController);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("Animation extraction complete", $"Created or updated {created} animation clip(s) for {selectedColors.Count} color(s).", "OK");
    }

    private AnimationClip CreateOrUpdateClip(string assetPath, List<Sprite> sprites)
    {
        AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(assetPath);
        if (clip == null)
        {
            clip = new AnimationClip();
            AssetDatabase.CreateAsset(clip, assetPath);
        }

        clip.frameRate = framesPerSecond;
        var binding = EditorCurveBinding.PPtrCurve(string.Empty, typeof(SpriteRenderer), "m_Sprite");
        var frames = sprites.Select((sprite, index) => new ObjectReferenceKeyframe
        {
            time = index / framesPerSecond,
            value = sprite
        }).ToArray();
        AnimationUtility.SetObjectReferenceCurve(clip, binding, frames);
        SetLooping(clip, loopAnimations);
        EditorUtility.SetDirty(clip);
        return clip;
    }

    private static void EnsureState(AnimatorController controller, string stateName, AnimationClip motion)
    {
        AnimatorStateMachine stateMachine = controller.layers[0].stateMachine;
        AnimatorState state = stateMachine.states.Select(child => child.state).FirstOrDefault(candidate => candidate.name == stateName);
        if (state == null)
        {
            state = stateMachine.AddState(stateName);
        }
        state.motion = motion;
    }

    private static List<Sprite> LoadSprites(string color, string unitType, string sheet)
    {
        string path = Path.Combine(UnitsPath, color, unitType, sheet + ".png").Replace('\\', '/');
        return AssetDatabase.LoadAllAssetsAtPath(path)
            .OfType<Sprite>()
            .OrderBy(sprite => sprite.rect.x)
            .ToList();
    }

    private string[] GetColors()
    {
        if (!Directory.Exists(UnitsPath)) return Array.Empty<string>();
        return Directory.GetDirectories(UnitsPath)
            .Select(Path.GetFileName)
            .Where(name => name.EndsWith(" Units", StringComparison.OrdinalIgnoreCase))
            .OrderBy(name => name)
            .ToArray();
    }

    private string[] GetAvailableUnitTypes()
    {
        string colorPath = Path.Combine(UnitsPath, selectedColors.FirstOrDefault() ?? string.Empty);
        if (!Directory.Exists(colorPath)) return Array.Empty<string>();
        return Directory.GetDirectories(colorPath)
            .Select(Path.GetFileName)
            .OrderBy(name => name)
            .ToArray();
    }

    private string[] GetSheets(string unitType)
    {
        string unitPath = Path.Combine(UnitsPath, selectedColors.FirstOrDefault() ?? string.Empty, unitType);
        if (!Directory.Exists(unitPath)) return Array.Empty<string>();
        return Directory.GetFiles(unitPath, "*.png")
            .Select(Path.GetFileNameWithoutExtension)
            .OrderBy(name => name)
            .ToArray();
    }

    private static void EnsureFolder(string assetFolder)
    {
        if (AssetDatabase.IsValidFolder(assetFolder)) return;
        string parent = Path.GetDirectoryName(assetFolder).Replace('\\', '/');
        string name = Path.GetFileName(assetFolder);
        if (!AssetDatabase.IsValidFolder(parent)) EnsureFolder(parent);
        AssetDatabase.CreateFolder(parent, name);
    }

    private static string ToAssetName(string value)
    {
        return string.Concat(value.ToLowerInvariant().Select(character => char.IsLetterOrDigit(character) ? character : '_'))
            .Trim('_');
    }

    private static string GetAnimationKey(string unitType, string sheet)
    {
        string unitPrefix = unitType + "_";
        string descriptiveName = sheet.StartsWith(unitPrefix, StringComparison.OrdinalIgnoreCase)
            ? sheet
            : unitPrefix + sheet;
        return ToAssetName(descriptiveName);
    }

    private static string GetColorName(string colorFolderName)
    {
        const string suffix = " Units";
        return colorFolderName.EndsWith(suffix, StringComparison.OrdinalIgnoreCase)
            ? colorFolderName.Substring(0, colorFolderName.Length - suffix.Length)
            : colorFolderName;
    }

    private static void SetLooping(AnimationClip clip, bool loop)
    {
        var serializedClip = new SerializedObject(clip);
        SerializedProperty settings = serializedClip.FindProperty("m_AnimationClipSettings");
        if (settings != null)
        {
            SerializedProperty loopTime = settings.FindPropertyRelative("m_LoopTime");
            if (loopTime != null) loopTime.boolValue = loop;
            serializedClip.ApplyModifiedProperties();
        }
    }
}
