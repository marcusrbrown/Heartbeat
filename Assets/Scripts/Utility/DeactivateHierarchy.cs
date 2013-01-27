using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class DeactivateHierarchy : MonoBehaviour
{
    public bool deactivateHierarchy = false;
    public bool activateInPlayMode = true;

    private bool deactivated_;

    private void Start()
    {
        if (!(EditorApplication.isPlaying && this.activateInPlayMode) && deactivateHierarchy)
        {
            SetActive(false, this.transform);
        }
        else
        {
            SetActive(true, this.transform);
        }
    }

    private void SetActive(bool active, Transform parent)
    {
        foreach (Transform child in parent)
        {
            SetActive(active, child);
            child.gameObject.active = active;
        }

        deactivated_ = active == false;
    }

    private void Update()
    {
        if (this.deactivateHierarchy != deactivated_)
        {
            SetActive(!this.deactivateHierarchy, this.transform);
        }
    }
}
