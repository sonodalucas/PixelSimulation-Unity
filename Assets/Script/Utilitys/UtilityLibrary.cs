using TMPro;
using UnityEngine;

public static class UtilityLibrary
{
    public const int sortingOrderDefault = 5000;

    // Create a sprite in the world
    public static GameObject CreateWorldSprite(Transform parent, string name, Sprite sprite, Vector3 localPosition,
        Vector3 localScale, int sortingOrder, Color color)
    {
        var gameObject = new GameObject(name, typeof(SpriteRenderer));

        var transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        transform.localScale = localScale;

        var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        spriteRenderer.sortingOrder = sortingOrder;
        spriteRenderer.color = color;

        return gameObject;
    }

    // Get Mouse Position in world with Z = 0
    public static Vector3 GetMouseWorldPosition(Camera mainCamera)
    {
        var vector = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        vector.z = 0;
        return vector;
    }

    // Create Text Pro in the World
    public static TextMeshPro CreateWorldText(string text, Transform parent = null,
        Vector3 localPosition = default, int fontSize = 40, Color? color = null, 
        VerticalAlignmentOptions verticalTextAlignment = VerticalAlignmentOptions.Middle,
        HorizontalAlignmentOptions horizontalTextAlignment = HorizontalAlignmentOptions.Left,
        int sortingOrder = sortingOrderDefault)
    {
        if (color == null) color = Color.white;
        return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, verticalTextAlignment,
            horizontalTextAlignment, sortingOrder);
    }

    // Create Text Pro in the World
    public static TextMeshPro CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize,
        Color color,  VerticalAlignmentOptions verticalTextAlignment, HorizontalAlignmentOptions horizontalTextAlignment,
        int sortingOrder)
    {
        var gameObject = new GameObject("World_Text", typeof(TextMeshPro));
        var transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;

        var textMesh = gameObject.GetComponent<TextMeshPro>();
        textMesh.horizontalAlignment = horizontalTextAlignment;
        textMesh.verticalAlignment = verticalTextAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }
    
    // Create Text in the World
    public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = sortingOrderDefault) {
        if (color == null) color = Color.white;
        return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
    }
        
    // Create Text in the World
    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder) {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }
}