using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace FSM.Editor
{
    
    public class FSMEditorWindow : OdinEditorWindow
    {
        public const int Width = 800;
        public const int Height = 600;

        private FSMGraphView _view;
        
        [MenuItem("Tools/FSM/Editor")]
        public static void OpenFSMEditorWindow()
        {
            var win = GetWindow<FSMEditorWindow>();
            win.titleContent = new GUIContent("Study GraphView");
            win.position = GUIHelper.GetEditorWindowRect().AlignCenter(Width, Height);
        }

        [LabelText("保存路径"), FolderPath(ParentFolder = "Assets", AbsolutePath = false, RequireExistingPath = true)]
        public string saveDir;
        [LabelText("文件名"), ReadOnly]
        public string fileName = "FSM_StateConfig.asset";

        [Button("保存"), GUIColor(0.4f, 0.8f, 1)]
        public void Save()
        {
            _view.Execute(out var nodeInfos, out var translates, out var anyTranslates);

            SaveAsset(nodeInfos, translates, anyTranslates);
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();

            var guids = Selection.assetGUIDs;
            var path = AssetDatabase.GUIDToAssetPath(guids[0]);
            Debug.Log($">>>>> {path}");

            var conf = AssetDatabase.LoadAssetAtPath<StateConfig>(path);
            Debug.Log($">>>>>> {conf}");
            
            _view = new FSMGraphView(this)
            {
                style =
                {
                    flexGrow = 1,
                    marginTop = 100
                }
            };
            
            rootVisualElement.Add(_view);
        }


        private void SaveAsset(List<NodeInfo> nodeInfos, Dictionary<string, List<string>> translates, List<string> anyTranslates)
        {
            var conf = ScriptableObject.CreateInstance<StateConfig>();
            conf.nodes = new List<StateConfig.Node>();
            conf.translates = new List<StateConfig.Translate>();
            conf.anyTranslates = anyTranslates;
            
            foreach (var nodeInfo in nodeInfos)
            {
                var style = nodeInfo.Node.style;
                var node = new StateConfig.Node
                {
                    value = nodeInfo.Value,
                    style = new StateConfig.NodeStyle
                    {
                        left = style.left.value.value,
                        right = style.right.value.value,
                        top = style.top.value.value,
                        bottom = style.bottom.value.value
                    }
                };
                conf.nodes.Add(node);
            }

            foreach (var t in translates)
            {
                var translate = new StateConfig.Translate();
                translate.source = t.Key;
                translate.targets = new List<string>();
                foreach (var target in t.Value)
                {
                    translate.targets.Add(target);
                }
                conf.translates.Add(translate);
            }

            AssetDatabase.CreateAsset(conf, "Assets/" + saveDir + "/" + fileName);
        }
    }
}   