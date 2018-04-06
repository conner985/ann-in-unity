using UnityEngine;

public class MouseCreator{
    
	public static GameObject CreateMouse (Sprite mouse, string name, string tag) {

        float org_x = 0.64f;
        float org_y = -0.64f;

        GameObject mice = GameObject.Find("Mice");
        if (!mice) mice = new GameObject("Mice");

        GameObject mouse_obj = new GameObject(name);
        mouse_obj.transform.SetParent(mice.transform);
        mouse_obj.layer = LayerMask.NameToLayer("Mouse");
        mouse_obj.tag = tag;
        mouse_obj.transform.position = new Vector3(org_x, org_y,-1);
        mouse_obj.transform.Rotate(0,0,-90);
        SpriteRenderer renderer = mouse_obj.AddComponent<SpriteRenderer>();
        renderer.sprite = mouse;
        mouse_obj.AddComponent<Rigidbody2D>().gravityScale = 0;
        mouse_obj.AddComponent<MouseScript>();

        return mouse_obj;
    }
}
