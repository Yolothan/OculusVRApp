using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//
[CreateAssetMenu(fileName = "Data", menuName = "ESDialogue/NewDialogueGraph", order = 1)]
public class ESDialgoueScriptableObj:ScriptableObject
{
    //
    [Serializable]
    public class StatementLayer
    {
        [Serializable]
        public class CharacterPicture
        {
            public int ConnectedIndex;
            public bool Connected = false;
            public Vector2 Size;
            public Texture2D texture;
            public bool center, left = true, right;
            public Rect rect;
            public string dropdowntext = "Left";
            public bool dropdown = false;
            public Rect outrect;
            public bool HasBuild;
            public int connected_id;
        }
        [Serializable]
        public class Statements
        {
            public bool IsconnectedIn;
            public bool IsconnectedOut;
            public int ConnectedIndexIn = 0;
            public int ConnectedIndexOut = 0;
            public string StatementContent;
            public Rect rect;
            public Rect InRect;
            public Rect OutRect;
            public Rect Side_Rect;
            public Rect img_rect;
            public bool connectedtodecision;
            public bool connetedtoimg;
            public int ConnectedInId = 100001;
            //
            public int ConnectedOutId = 100001;
            public int Id = 100001;
            public int ConnectedImgIndex = 0;
            //next button settings
            public string ButtonText;
            public bool dropdown;
            public bool anchordropdown;
            public string anchortext = "TopLeft";
            public Texture2D buttontexture;
            public Vector2 Size;
            public bool align_dropdown = false;
            public string align_dropdown_text = "Horizontal";
            public bool horizontal = true;
            public bool vertical;
            public string CharacterTagName = "Untagged";
            //
            public AudioClip audio;
            public Rect Statement_rect;
            public float stmt_posx;
            public float stmt_posy;
            public float des_spacing;
            public bool enable_scroll;
            //
            public bool TopLeft = true, TopRight, DownLeft, DownRight;
            //
            //if connectedtodecision returns true
            public int desparentindex;
            //S
            [Serializable]
            public class Decision
            {
                public string content;
                public Rect rect;
                public Rect OutRect;
                public Rect display_rect;
                public int connected_id;
                public int spacing = 170;
                public bool updatedspacing = false;
                public bool HasBuild = false;
                public bool HasSpaced = false;
                //
                public string ID;
              
                public bool IsconnectedIn;
                public bool IsconnectedOut;
                public int ConnectedIndexIn = 0;
                public int ConnectedIndexOut = 0;
            }
            
            public bool HasBuild;
            public bool Click;
            public float Timer = 0.3f;
            public List<Decision> decisions = new List<Decision>(1);
            public bool Isdecision  = false;
            public int DecisionAmount = 3;
            //editor only(ver1.1)
            public bool BySelection;
            public bool closemsgbox;
            //end
        }
        public List<Statements> statements = new List<Statements>(1);
        public List<CharacterPicture> characterPictures = new List<CharacterPicture>();
        public Rect stmtRect;
        public bool HasBuild = false;
        public string layerName = "New Layer";
        public int ID = 1010001;
        public bool Active;
        public bool Play = false;
        public bool isplaying = false;
        public Vector2  panelsize = new Vector2(800,360);
       
        public bool reset;
    }
    //
    public List<StatementLayer> statementLayers = new List<StatementLayer>(1);
}
