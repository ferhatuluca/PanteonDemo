using System;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Converts every PNG spritesheet in Tiny Swords/Units into a horizontal
/// multiple-sprite sheet. Frame count is inferred from imageWidth / imageHeight.
/// </summary>
public static class TinySwordsUnitSpriteSlicer
{
    private const string UnitsPath = "Assets/ThirdParty/Tiny Swords (Free Pack)/Units";

    [MenuItem("Tools/Tiny Swords/Slice All Units")]
    public static void SliceAllUnitSpritesheets()
    {
        string absoluteUnitsPath = Path.Combine(Directory.GetCurrentDirectory(), UnitsPath);
        string[] files = Directory.GetFiles(absoluteUnitsPath, "*.png", SearchOption.AllDirectories);
        int sliced = 0;
        int skipped = 0;

        try
        {
            foreach (string file in files)
            {
                string assetPath = file.Replace('\\', '/');
                int assetsIndex = assetPath.IndexOf("Assets/", StringComparison.Ordinal);
                if (assetsIndex < 0)
                {
                    skipped++;
                    continue;
                }

                assetPath = assetPath.Substring(assetsIndex);
                var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
                var importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;

                if (texture == null || importer == null || texture.height == 0 || texture.width % texture.height != 0)
                {
                    Debug.LogWarning($"Skipped {assetPath}: expected a horizontal strip whose width is a multiple of its height.");
                    skipped++;
                    continue;
                }

                int columns = texture.width / texture.height;
                var sprites = new SpriteMetaData[columns];
                string baseName = Path.GetFileNameWithoutExtension(assetPath);

                for (int column = 0; column < columns; column++)
                {
                    sprites[column] = new SpriteMetaData
                    {
                        name = $"{baseName}_{column}",
                        rect = new Rect(column * texture.height, 0, texture.height, texture.height),
                        alignment = (int)SpriteAlignment.Center,
                        pivot = new Vector2(0.5f, 0.5f)
                    };
                }

                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Multiple;
                importer.spritesheet = sprites;
                importer.SaveAndReimport();
                sliced++;
            }
        }
        finally
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        Debug.Log($"Tiny Swords unit slicing complete. Sliced {sliced} sheet(s); skipped {skipped} file(s).");
    }
}
