using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace BulletSteam.GameFramework.Graph.Editor
{
    [CustomEditor(typeof(StateData))]
    public class StateDataEditor : UnityEditor.Editor
    {
        private StateData _stateNode;
        public VisualTreeAsset _stateDataEditorUxml;
        
        private List<SerializedObject> _actions = new();

        private void OnEnable()
        {
            _stateNode = (StateData)target;
            if(_stateNode.Actions != null)
                _actions = _stateNode.Actions.Select(a => new SerializedObject(a)).ToList();
        }

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new();

            _stateDataEditorUxml.CloneTree(root);

            ListView list = root.Q<ListView>("actions-list");
            list.makeItem = () => new VisualElement();
            list.bindItem = (e, i) =>
            {
                if (i >= _stateNode.Actions.Count)
                    return;
                SerializedObject so = new(_stateNode.Actions[i]);
                FillDefaultInspector(e, so);
                // InspectorElement inspectorElement = new(_stateNode.Actions[i]);
                e.Bind(so);
                //Hide m_Script field
            };
            list.unbindItem = (e, i)=>
            {
                e.Unbind();
            };

            // Button button = root.Q<Button>("new-action-button");
            // button.RegisterCallback<ClickEvent>(_ =>
            // {
            //     _stateNode.Actions.Add(CreateInstance<PrintInfoAction>());
            // });

            return root;
        }

        private static void FillDefaultInspector(VisualElement container, SerializedObject serializedObject)
        {
            container.Clear();
            if (serializedObject == null)
                return;
 
            SerializedProperty iterator = serializedObject.GetIterator();
            Type type = serializedObject.targetObject.GetType();
            Label label = new(ObjectNames.NicifyVariableName(type.Name));
            container.Add(label);

            if (iterator.NextVisible(true))
            {
                do
                {
                    if (iterator.propertyPath == "m_Script")
                        continue;


                    PropertyField propertyField = new(iterator)
                    {
                        name = "PropertyField:" + iterator.propertyPath
                    };
                    propertyField.AddToClassList("unity-disabled unity-property-field__inspector-property");

                    container.Add(propertyField);
                } while (iterator.NextVisible(false));
            }
        }
    }
}