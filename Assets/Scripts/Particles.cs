using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        // Debug.Log("Particle spawned.");
        yield return new WaitForSeconds(gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }

    public static void Spawn(GameObject obj, Type type, float x = 0f, float y = 0f, float z = 0f, Transform parent = null)
    {
        Spawn(obj, type, new Vector3(x, y, z), parent);
    }

    public static void Spawn(GameObject obj, Type type, Vector3 offset, Transform parent = null)
    {
        string path = "Prefabs/";
        switch (type)
        {
            case Type.BloodLight:
                path += "Blood Splatter Light";
                break;
            case Type.BloodHeavy:
                path += "Blood Splatter Heavy";
                break;
            case Type.Explosion:
                path += "Explosion";
                break;
            case Type.Sparks:
                path += "Sparks";
                break;
            default:
                throw new System.ArgumentException("Invalid argument.");
        }
        GameObject particle = Instantiate(Resources.Load<GameObject>(path), obj.transform.position, obj.transform.rotation, parent);
        particle.transform.localPosition += offset;
        if (parent != null)
            particle.transform.localScale = new Vector3(particle.transform.localScale.x / parent.localScale.x, particle.transform.localScale.y / parent.localScale.y, particle.transform.localScale.z);
    }

    public enum Type
    {
        BloodLight,
        BloodHeavy,
        Explosion,
        Sparks
    }
}
