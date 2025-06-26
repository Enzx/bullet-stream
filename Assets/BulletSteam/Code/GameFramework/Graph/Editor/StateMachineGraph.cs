using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace BulletSteam.GameFramework.Graph.Editor
{
    [UxmlElement("state-machine-graph")]
    public partial class StateMachineGraph : GraphView
    {
  
        public GraphData GraphData => _graphData;

        private StateMachineEditorWindow _window;
        private Vector2 _mouseWorldPosition;
        private Vector2 _mouseLocalPosition;
        private GraphData _graphData;
        private readonly NodeFactory _nodeFactory;

        private readonly MiniMap _miniMap;
        private readonly Blackboard _blackboard;
        VisualElement _propertiesPanel;


        //Expose StateMachineGraph uxml


        public StateMachineGraph()
        {
            this.StretchToParentSize();
            GridBackground gridBackground = new();
            Insert(0, gridBackground);
            gridBackground.SendToBack();
            gridBackground.StretchToParentSize();
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new ContextualMenuManipulator(BuildContextualMenu));

            //Create a mini map
            _miniMap = new MiniMap();
            _miniMap.SetPosition(new Rect(10, 30, 200, 140));
            Add(_miniMap);


            //Create a blackboard
            _blackboard = new Blackboard(this);
            _blackboard.Add(new BlackboardSection { title = "Exposed Properties" });
            _blackboard.SetPosition(new Rect(10, 200, 200, 300));
            Add(_blackboard);


            RegisterCallback<MouseUpEvent>(MouseUp);

            nodeCreationRequest = NodeCreationRequest;

            _nodeFactory = new NodeFactory();
        }

        private void OnNodeSelected(BaseNode node)
        {
            _propertiesPanel.Clear();
            InspectorElement inspectorElement = new(node.Data);
            _propertiesPanel.Add(inspectorElement);
        }

        private void OnMinimapButtonClicked(ClickEvent _)
        {
            _miniMap.visible = !_miniMap.visible;
        }

        private void OnBlackboardButtonClicked(ClickEvent _)
        {
            _blackboard.visible = !_blackboard.visible;
        }


        private void NodeCreationRequest(NodeCreationContext ctx)
        {
            AddNode(typeof(StateData));
        }

        public void SetGraphData(GraphData graphData)
        {
            _graphData = graphData;
            if (_graphData == null) return;
            _graphData.NodesData ??= new List<NodeData>();
            _graphData.Transitions ??= new List<Transition>();
            if (_graphData.StartNodeData == null && _graphData.NodesData.Count > 0)
            {
                _graphData.StartNodeData = _graphData.NodesData[0];
            }

            DrawElements();
        }

        private void DrawElements()
        {
            _graphData.NodesData.ForEach(nodeData =>
            {
                BaseNode node = _nodeFactory.Create(nodeData);
                if (nodeData.EditorData is GraphNodeState nodeState)
                {
                    node.SetPosition(new Rect(nodeState.Position, Vector2.one));
                }

                node.title = nodeData.name;
                AddElement(node);
            });
            _graphData.Transitions.ForEach(transition =>
            {
                BaseNode sourceNode = (BaseNode)GetNodeByGuid(transition.Source.Id.ToString());
                BaseNode destinationNode = (BaseNode)GetNodeByGuid(transition.Destination.Id.ToString());
                if (sourceNode == null || destinationNode == null)
                    return;
                if (sourceNode.outputContainer[0] is not Port startPort ||
                    destinationNode.inputContainer[0] is not Port endPort)
                    return;
                Edge edge = startPort.ConnectTo(endPort);
                AddElement(edge);
            });
        }

        private void MouseUp(MouseUpEvent evt)
        {
            _mouseWorldPosition = evt.mousePosition;
            _mouseLocalPosition = contentViewContainer.WorldToLocal(_mouseWorldPosition);
        }


        public void Init(StateMachineEditorWindow window)
        {
            _window = window;

            Button miniMapButton = _window.rootVisualElement.Q<Button>("minimap-button");
            miniMapButton?.RegisterCallback<ClickEvent>(OnMinimapButtonClicked);
            Button blackboardButton = _window.rootVisualElement.Q<Button>("blackboard-button");
            blackboardButton?.RegisterCallback<ClickEvent>(OnBlackboardButtonClicked);
            _propertiesPanel = _window.rootVisualElement.Q<VisualElement>("preview-panel");
            BaseNode.OnNodeSelected += OnNodeSelected;
        }

        public void UnInit()
        {
            BaseNode.OnNodeSelected -= OnNodeSelected;
        }


        private void AddNode(Type type)
        {
            NodeData nodeData = NodeData.Create(type);
            BaseNode node = _nodeFactory.Create(nodeData);
            node.SetPosition(new Rect(_mouseLocalPosition, Vector2.one));
            AddElement(node);
            _graphData.NodesData ??= new List<NodeData>();
            _graphData.NodesData.Add(nodeData);
            nodeData.name = node.title;
            AssetDatabase.AddObjectToAsset(nodeData, _graphData);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new();
            ports.ForEach(port =>
            {
                if (startPort != port && startPort.node != port.node)
                {
                    compatiblePorts.Add(port);
                }
            });
            return compatiblePorts;
        }

        public void Serialize()
        {
            _graphData.NodesData.Clear();
            _graphData.Transitions.Clear();
            foreach (GraphElement element in graphElements)
            {
                if (element is not BaseNode node) continue;
                GraphNodeState nodeState = new()
                {
                    Position = node.GetPosition().position
                };
                node.Data.EditorData = nodeState;
                _graphData.NodesData.Add(node.Data);

                foreach (VisualElement visualElement in node.outputContainer.Children())
                {
                    Port port = (Port)visualElement;
                    foreach (Edge edge in port.connections)
                    {
                        BaseNode targetNode = (BaseNode)edge.input.node;
                        Transition transition = new(node.Data.Key, targetNode.Data.Key);
                        _graphData.Transitions.Add(transition);
                    }
                }
            }
        }
    }
}