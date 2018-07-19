using UnityEngine;

public struct Info {
    public string name;
    public float? health;
    public string desc;
}

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(HasHealth))]
public class Targetable : MonoBehaviour {
    private HasHealth health;

    void Start() {
        health = GetComponent<HasHealth>();
    }

    public Info GetInfo() {
       return new Info { health = health.health, desc = "An asteroid", name = gameObject.name };
    }
    void OnMouseDown() {
        GameManager.Instance.SetTarget(this);
    }
}
