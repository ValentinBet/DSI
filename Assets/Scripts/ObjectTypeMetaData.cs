using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectTypeMetaData.asset", menuName = "Tools/Object Type Meta Data", order = 100)]
public class ObjectTypeMetaData : ScriptableObject
{
    public Sprite icon;
    public Sprite sprite;
    public string typeDescription;
}
