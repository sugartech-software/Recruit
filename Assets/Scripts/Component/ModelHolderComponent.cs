using UnityEngine;
using System.Collections;
using Motto.Domain;

public class ModelHolderComponent : MonoBehaviour
{
    [SerializeField]
    public BaseEntity model;

    public void AdjustTransformFormEntity()
    {
        gameObject.transform.position = model.position.ToVector();
        gameObject.transform.rotation = Quaternion.Euler(model.rotation.ToVector());
    }
}
