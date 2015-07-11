using UnityEngine;

namespace Slerpy.Unity3D.UI
{
    public sealed class StaticHelper : MonoBehaviour
    {
        public void LoadLevel(string levelName)
        {
            Application.LoadLevel(levelName);
        }

        public void Quit()
        {
            Application.Quit();
        }

        public void Destroy(GameObject target)
        {
            GameObject.Destroy(target);
        }

        public void Destroy(GameObject target, float time)
        {
            GameObject.Destroy(target, time);
        }

        public void Destroy(Component target)
        {
            Component.Destroy(target);
        }

        public void Destroy(Component target, float time)
        {
            Component.Destroy(target, time);
        }

        public void Instantiate(GameObject prefab)
        {
            GameObject.Instantiate(prefab);
        }

        public void Instantiate(GameObject prefab, Vector3 position)
        {
            GameObject.Instantiate(prefab, position, Quaternion.identity);
        }

        public void Instantiate(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            GameObject.Instantiate(prefab, position, rotation);
        }

        public void DetatchParent(Transform target)
        {
            target.parent = null;
        }
    }
}
