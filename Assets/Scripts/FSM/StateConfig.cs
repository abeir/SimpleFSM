using System;
using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;


namespace FSM
{
    
    [CreateAssetMenu(menuName = "FSM/State Config")]
    public class StateConfig : ScriptableObject
    {
        
        public List<Node> nodes;
        
        public List<Translate> translates;
        public List<string> anyTranslates;

        
        public static StateConfig ParseJSON(string json)
        {
            return SerializationUtility.DeserializeValue<StateConfig>(System.Text.Encoding.UTF8.GetBytes(json), DataFormat.JSON);
        }
        
        public string ToJSON()
        {
            var json = SerializationUtility.SerializeValue(this, DataFormat.JSON);
            return System.Text.Encoding.UTF8.GetString(json);
        }



        [Serializable]
        public class Node
        {
            public string value;
            public NodeStyle style;
        }

        [Serializable]
        public class NodeStyle
        {
            public float left;
            public float right;
            public float top;
            public float bottom;
            public float width;
            public float height;
        }

        [Serializable]
        public class Translate
        {
            public string source;
            public List<string> targets;
        }
    }
}