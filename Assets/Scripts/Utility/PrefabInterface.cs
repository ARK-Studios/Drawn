using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Collections.Generic;


public class PropertyID
{
    public SerializedProperty Property { get; set; }
    public string Id { get; set; }

    public PropertyID(string id, SerializedProperty property)
    {
        this.Id = id;
        this.Property = property;
    }
}


public class ReferenceID
{
    public SerializedProperty Property { get; set; }
    public FieldInfo Field { get; set; }
    public object Target { get; set; }

    public ReferenceID(SerializedProperty property, FieldInfo field, object target)
    {
        this.Property = property;
        this.Field = field;
        this.Target = target;
    }
}


public class PrefabInterface : MonoBehaviour
{
    // Storage of Serialized objects so they don't go out of scope
    public List<SerializedObject> objects = new List<SerializedObject>();

    // List of shown Properties in the Inspector
    public List<PropertyID> propertyIds = new List<PropertyID>();

    // List of Attribute variables to update
    public List<ReferenceID> referenceIds = new List<ReferenceID>();
}


[CustomEditor(typeof(PrefabInterface))]
public class PrefabInterfaceInspector : Editor
{
    private PrefabInterface myTarget;

    public void OnEnable()
    {
        this.myTarget = (target as PrefabInterface);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if(GUILayout.Button("Scan Dependancies"))
        {
            ScanPrefab();
        }

        if(GUILayout.Button("Set Dependancies"))
        {
            SetDependancies();
        }

        foreach (PropertyID propertyId in this.myTarget.propertyIds)
        {
            EditorGUILayout.PropertyField(propertyId.Property, new GUIContent(propertyId.Id));
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void ScanPrefab()
    {
        // Traverse the GameObject and look for InterfaceFields
        Debug.Log("Scanning Prefab Dependancies");

        MonoBehaviour start = (target as MonoBehaviour);
        this.myTarget.propertyIds.Clear();
        this.myTarget.referenceIds.Clear();
        StoreInterfaces(start.transform);
        RecursiveScan(start.transform);
    }

    private void SetDependancies()
    {
        foreach(ReferenceID refId in this.myTarget.referenceIds)
        {
            object value = null;

            switch(refId.Property.propertyType)
            {
                case SerializedPropertyType.Integer:
                    value = refId.Property.intValue;
                    break;
                case SerializedPropertyType.Boolean:
                    value = refId.Property.boolValue;
                    break;
                case SerializedPropertyType.Float:
                    value = refId.Property.floatValue;
                    break;
                case SerializedPropertyType.String:
                    value = refId.Property.stringValue;
                    break;
                case SerializedPropertyType.Color:
                    value = refId.Property.colorValue;
                    break;
                case SerializedPropertyType.ObjectReference:
                    value = refId.Property.objectReferenceValue;
                    break;
                case SerializedPropertyType.LayerMask:
                    value = refId.Property.stringValue;
                    break;
                case SerializedPropertyType.Enum:
                    value = refId.Property.enumValueIndex;
                    break;
                case SerializedPropertyType.Vector2:
                    value = refId.Property.vector2Value;
                    break;
                case SerializedPropertyType.Vector3:
                    value = refId.Property.vector3Value;
                    break;
                case SerializedPropertyType.Vector4:
                    value = refId.Property.vector4Value;
                    break;
                case SerializedPropertyType.Rect:
                    value = refId.Property.rectValue;
                    break;
                case SerializedPropertyType.ArraySize:
                    value = refId.Property.arraySize;
                    break;
                case SerializedPropertyType.Character:
                    value = refId.Property.stringValue[0];
                    break;
                case SerializedPropertyType.AnimationCurve:
                    value = refId.Property.animationCurveValue;
                    break;
                case SerializedPropertyType.Bounds:
                    value = refId.Property.boundsValue;
                    break;
                case SerializedPropertyType.Gradient:
                    value = refId.Property.rectValue;
                    break;
                case SerializedPropertyType.Quaternion:
                    value = refId.Property.quaternionValue;
                    break;
            }

            refId.Field.SetValue(refId.Target, value);
        }
    }

    private void StoreInterfaces(Transform item)
    {
        MonoBehaviour[] behaviors = item.GetComponents<MonoBehaviour>();
        
        foreach (MonoBehaviour b in behaviors)
        {
            SerializedObject serObj = new SerializedObject(b);
            this.myTarget.objects.Add(serObj);

            Type childClass = b.GetType();

            FieldInfo[] fields = childClass.GetFields();
            foreach(FieldInfo field in fields)
            {
                InterfaceFieldAttribute interfaceField = (InterfaceFieldAttribute) Attribute.GetCustomAttribute(field, typeof(InterfaceFieldAttribute));
                if (null != interfaceField)
                {
                    SerializedProperty prop = serObj.FindProperty(field.Name);
                    PropertyID propId = new PropertyID(b.name + ":" + childClass.ToString() + " " + prop.displayName, prop);
                    this.myTarget.propertyIds.Add(propId);

                    ReferenceID refId = new ReferenceID(prop, field, b);
                    this.myTarget.referenceIds.Add(refId);
                }
            }
        }
    }

    private void RecursiveScan(Transform item)
    {
        foreach(Transform t in item)
        {
            StoreInterfaces(t);

            RecursiveScan(t);
        }
    }
}