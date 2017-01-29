using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Collections.Generic;


public class PropertyID
{
    public SerializedProperty Property { get; set; }
    public string Id { get; set; }
    public FieldInfo Field { get; set; }
    public object Target { get; set; }

    public PropertyID(string id, SerializedProperty property, FieldInfo field, object target)
    {
        this.Id = id;
        this.Property = property;
        this.Field = field;
        this.Target = target;
    }
}


public class PrefabInterface : MonoBehaviour
{
    public List<SerializedObject> objects = new List<SerializedObject>();
    public List<PropertyID> propertyIds = new List<PropertyID>();
}


[CustomEditor(typeof(PrefabInterface))]
public class PrefabInterfaceInspector : Editor
{
    private PrefabInterface myTarget;

    public void OnEnable()
    {
        // Initail property scan
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

        this.myTarget = (target as PrefabInterface);

        foreach (PropertyID propertyId in this.myTarget.propertyIds)
        {
            // Build Interface from Properties List
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
        StoreInterfaces(start.transform);
        RecursiveScan(start.transform);
    }

    private void SetDependancies()
    {
        foreach(PropertyID prop in this.myTarget.propertyIds)
        {
            object value = null;

            switch(prop.Property.propertyType)
            {
                case SerializedPropertyType.Integer:
                    value = prop.Property.intValue;
                    break;
                case SerializedPropertyType.Boolean:
                    value = prop.Property.boolValue;
                    break;
                case SerializedPropertyType.Float:
                    value = prop.Property.floatValue;
                    break;
                case SerializedPropertyType.String:
                    value = prop.Property.stringValue;
                    break;
                case SerializedPropertyType.Color:
                    value = prop.Property.colorValue;
                    break;
                case SerializedPropertyType.ObjectReference:
                    value = prop.Property.objectReferenceValue;
                    break;
                case SerializedPropertyType.LayerMask:
                    value = prop.Property.stringValue;
                    break;
                case SerializedPropertyType.Enum:
                    value = prop.Property.enumValueIndex;
                    break;
                case SerializedPropertyType.Vector2:
                    value = prop.Property.vector2Value;
                    break;
                case SerializedPropertyType.Vector3:
                    value = prop.Property.vector3Value;
                    break;
                case SerializedPropertyType.Vector4:
                    value = prop.Property.vector4Value;
                    break;
                case SerializedPropertyType.Rect:
                    value = prop.Property.rectValue;
                    break;
                case SerializedPropertyType.ArraySize:
                    value = prop.Property.arraySize;
                    break;
                case SerializedPropertyType.Character:
                    value = prop.Property.stringValue[0];
                    break;
                case SerializedPropertyType.AnimationCurve:
                    value = prop.Property.animationCurveValue;
                    break;
                case SerializedPropertyType.Bounds:
                    value = prop.Property.boundsValue;
                    break;
                case SerializedPropertyType.Gradient:
                    value = prop.Property.boolValue;
                    break;
                case SerializedPropertyType.Quaternion:
                    value = prop.Property.quaternionValue;
                    break;
            }

            prop.Field.SetValue(prop.Target, value);
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
                    PropertyID propId = new PropertyID(b.name + ":" + childClass.ToString() + " " + prop.displayName, prop, field, b);
                    this.myTarget.propertyIds.Add(propId);
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