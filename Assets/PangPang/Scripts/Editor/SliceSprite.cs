using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SliceSprite
{
    [MenuItem("Assets/Slice Sprite/Center Pivot")]
    static void SliceSpriteCenterPivot()
    {
        var selectGUIDs = Selection.assetGUIDs; // 클릭한 파일의 id 를 받아옴
        for (int i = 0; i < selectGUIDs.Length; i++)
        {
            Slice(AssetDatabase.GUIDToAssetPath(selectGUIDs[i]), new Vector2(0.5f, 0.5f));
        }
    }

    [MenuItem("Assets/Slice Sprite/Top Pivot")]
    static void SliceSpriteTopPivot()
    {
        var selectGUIDs = Selection.assetGUIDs; // 클릭한 파일의 id 를 받아옴
        for (int i = 0; i < selectGUIDs.Length; i++)
        {
            Slice(AssetDatabase.GUIDToAssetPath(selectGUIDs[i]), new Vector2(0.5f, 1f));
        }
    }

    static void Slice(string imageFilePath, Vector2 pivot)
    {
        TextureImporter importer = null;    // 이미지 파일을 클릭했을 때 인스펙터에 나타나는 세팅창 클래스
        string assetFilePath = string.Format("{0}.asset", imageFilePath.Remove(imageFilePath.LastIndexOf('.')));

        var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(imageFilePath);
        var asset = AssetDatabase.LoadAssetAtPath<NGUIAtlas>(assetFilePath);

        importer = (TextureImporter)AssetImporter.GetAtPath(imageFilePath); // 해당 경로의 이미지 인스펙터 정보를 받아옴

        importer.spriteImportMode = SpriteImportMode.Single;
        AssetDatabase.ImportAsset(imageFilePath, ImportAssetOptions.ForceUpdate);

        importer.spriteImportMode = SpriteImportMode.Multiple;
        List<SpriteMetaData> metaList = new List<SpriteMetaData>();

        for (int i = 0; i < asset.spriteList.Count; i++)
        {
            SpriteMetaData meta = new SpriteMetaData();
            meta.name = asset.spriteList[i].name;
            meta.border = new Vector4(asset.spriteList[i].borderLeft,
                asset.spriteList[i].borderBottom,
                asset.spriteList[i].borderRight,
                asset.spriteList[i].borderTop);

            meta.rect = new Rect(asset.spriteList[i].x,
                texture.height - asset.spriteList[i].y - asset.spriteList[i].height,
                asset.spriteList[i].width,
                asset.spriteList[i].height);

            meta.alignment = 9; // Custom을 의미
            meta.pivot = pivot;
            metaList.Add(meta);
        }

        importer.spritesheet = metaList.ToArray();

        AssetDatabase.ImportAsset(imageFilePath, ImportAssetOptions.ForceUpdate);

    }
}
