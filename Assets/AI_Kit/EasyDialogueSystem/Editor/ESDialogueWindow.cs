using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


//
public class ESDialogueWindow : EditorWindow
{
    public ESDialgoueScriptableObj dialogueGraph;
    public SerializedObject serialized;
    
    public  static Texture2D editortexture;
    public static bool enableback_texture = true;
    public static Vector4 gridcolor = new Vector4(1,1,1,1);
    public static float coloropacity = 0.29f;
    public static Color linecolor = Color.white;
    public static bool useblack = false;
    //
    private ESDialgoueScriptableObj selectedobj;
    private ESDialgoueScriptableObj lastselectedobj;
    private  bool IsLayerArea = true;
    private string[] Layername ;
    private  int StateMentLayerIndex = 0;
    private int StateMentIndex = 0;
    private int decisionindex = 0;
    private int selectedlayer = 0;
    private bool Isdrag;
    private bool DragDecision;
    private int imgindex;
    private int img_in_index;
    private bool Isimgbox;
    private bool istransit;
    private bool Transistfromimgbox;
    private bool TransFromDecision;
    private int Out_int = 0;
    private int currentimgboxindex;
    private int In_Id = 0;
    private int Out_Id = 0;
    public int Stmt_Out_Int;
    private bool Centered =  false;
    private int In_int = 0;
    private Vector2 drag;
    private Vector2 offeset;
    private int des_int;
    Vector2 Button_scrollPosition = Vector2.zero;
    Rect boxrect;
    //statement settings
    private Rect StartRect;
    private Rect SettingsRect;
    //
    [MenuItem("Window/EasyDialogue/DialogueWindow")]
    public static void Init()
    {
        // Get existing open window or if none, make a new one:
        ESDialogueWindow window = (ESDialogueWindow)EditorWindow.GetWindow(typeof(ESDialogueWindow), false, "NodeEditor");
        window.autoRepaintOnSceneChange = true;
        window.minSize = new Vector2(300, 230);
        //forces editor refresh 
        EditorApplication.modifierKeysChanged += window.Repaint;
        window.Show();
       
    }
    //
    private const float kZoomMin = 0.5f;
    private const float kZoomMax = 3.0f;

    private readonly Rect _zoomArea = new Rect(0.0f, 50.0f, 10000, 10000);
    private float _zoom = 1.0f;
    private Vector2 _zoomCoordsOrigin = Vector2.zero;
    private Rect zoomrect;
    Vector2 convertedpos;
    private Vector2 ConvertScreenCoordsToZoomCoords(Vector2 screenCoords)
    {
        return (screenCoords - _zoomArea.TopLeft()) / _zoom + _zoomCoordsOrigin;
    }

    private void DrawZoomArea()
    {
        // Within the zoom area all coordinates are relative to the top left corner of the zoom area
        // with the width and height being scaled versions of the original/unzoomed area's width and height.
        if (!Centered)
        {
            _zoomCoordsOrigin = new Vector2(500, 500);
            Centered = true;
        }
        EditorZoomArea.Begin(_zoom, _zoomArea);
        
        GUI.Box(new Rect(0.0f - _zoomCoordsOrigin.x, 0.0f - _zoomCoordsOrigin.y, 10000, 10000f), "Zoomed Box");
        Rect arearect = new Rect(0.0f - _zoomCoordsOrigin.x, 30.0f - _zoomCoordsOrigin.y, 10000f, 10000f);
  
        GUIStyle gUI = new GUIStyle();
        if (enableback_texture)
        {
            gUI.normal.background = editortexture == null ? Resources.Load("Textures/ZoomBackground") as Texture2D : editortexture;
        }
        GUILayout.BeginArea(arearect,gUI);
       
        //Debug.Log(_zoomCoordsOrigin);
        DrawGrid(80.0f, coloropacity, gridcolor, new Vector2(10000,10000));
       
        LauchWindowContent(500,500);
        GUILayout.EndArea();

        EditorZoomArea.End();
    }
    //
    private void DrawGrid(float space, float opacity, Color mycolor, Vector2 size)
    {
        int width = Mathf.CeilToInt( size.x / space);
        int height = Mathf.CeilToInt(size.y / space);
       
        GUI.changed = true;
        //
        Handles.BeginGUI();
        Handles.color = new Color(mycolor.r, mycolor.g, mycolor.b, opacity);

        offeset += drag * 0.5f;
        Vector3 newoffset = new Vector3(offeset.x % space, offeset.y % space, 0);
        //
        for (int i = 0; i < width; i++)
        {
            Handles.DrawLine(new Vector3(space * i, -space, 0) + newoffset, new Vector3(space * i, size.y, 0f) + newoffset);
        }
        //
        for (int i = 0; i < width; i++)
        {
            Handles.DrawLine(new Vector3(-space, space * i, 0) + newoffset, new Vector3(size.x, space * i, 0f) + newoffset);
        }
        Handles.color = Color.white;
        Handles.EndGUI();
    }
    //
    private void DrawNonZoomArea()
    {
        if (IsLayerArea == false)
        {
            if (GUI.Button(new Rect(0, 0, 250f, 25f), "Back To StatementLayer"))
            {
                IsLayerArea = true;
            }
            if (GUI.Button(new Rect(300, 0f, 150f, 25f), "Black/White"))
            {
                useblack = !useblack;
            }
        }
        if (IsLayerArea == true)
        {
            if (GUI.Button(new Rect(0, 0, 100f, 20f), "grid color"))
            {


                gridcolor = new Vector4(Mathf.RoundToInt(Mathf.PingPong(Time.time , 9)),
                                        Mathf.RoundToInt(Mathf.PingPong(Time.time + gridcolor.x, 9)),
                                        Mathf.RoundToInt(Mathf.PingPong(Time.time + gridcolor.w, 9)),
                                        Mathf.RoundToInt(Mathf.PingPong(Time.time + gridcolor.y, 9)));
                //Debug.Log(gridcolor);
            }
            //
            GUI.Label(new Rect(170, 15, 150f, 20f), "BackGround Texture:");
            editortexture = (Texture2D)EditorGUI.ObjectField(new Rect(290, 0, 60f, 50f), "", editortexture, typeof(Texture2D), false);
            coloropacity = EditorGUI.Slider(new Rect(0.0f, 25.0f, 150.0f, 25.0f), coloropacity, 0.1f, 1.0f);
            enableback_texture = EditorGUI.Toggle(new Rect(380f, 15.0f, 15.0f, 15.0f), enableback_texture);
            GUI.Label(new Rect(410f, 15.0f, 200.0f, 25.0f), "Enable BackGround Texture");
        }
        
    }
    //
    private void HandleEvents(Event e)
    {
        if (Event.current.type == EventType.ScrollWheel)
        {
            Vector2 screenCoordsMousePos = Event.current.mousePosition;
            Vector2 delta = Event.current.delta;
            Vector2 zoomCoordsMousePos = ConvertScreenCoordsToZoomCoords(screenCoordsMousePos);
            float zoomDelta = -delta.y / 150.0f;
            float oldZoom = _zoom;
            _zoom += zoomDelta;
            _zoom = Mathf.Clamp(_zoom, kZoomMin, kZoomMax);
            _zoomCoordsOrigin += (zoomCoordsMousePos - _zoomCoordsOrigin) - (oldZoom / _zoom) * (zoomCoordsMousePos - _zoomCoordsOrigin);
            GUI.changed = true;
        }

        if (Event.current.type == EventType.MouseDrag &&
            (Event.current.button == 0 && Event.current.modifiers == EventModifiers.Alt) ||
            Event.current.button == 2)
        {
            Vector2 delta = Event.current.delta;
            delta /= _zoom;
            _zoomCoordsOrigin += delta;
            _zoomCoordsOrigin.x = Mathf.Clamp(_zoomCoordsOrigin.x, 500, 10000f - Screen.width * 2);
            _zoomCoordsOrigin.y = Mathf.Clamp(_zoomCoordsOrigin.y, 500, 10000f);
            GUI.changed = true;
        }
    }
    //
    public void OnGUI()
    {
        selectedobj = Selection.activeObject as ESDialgoueScriptableObj;
        if (selectedobj != null)
        {
            if (lastselectedobj != selectedobj)
            {
                ReloadWindow();
            }
            serialized = new SerializedObject(dialogueGraph);
            serialized.Update();
        }
        if(dialogueGraph != null)
        {
            
            HandleEvents(Event.current);

            DrawZoomArea();
      
            //LauchWindowContent();
            
            DrawNonZoomArea();
            NodeEvent(Event.current);

        }
        if (GUI.changed)
        {
            if (dialogueGraph != null)
                EditorUtility.SetDirty(dialogueGraph);
        }
    }
    //
    private void LauchWindowContent(float zoom_x,float zoom_y)
    {
        linecolor = useblack == true ? Color.black : Color.white;
        Array.Resize(ref Layername, dialogueGraph.statementLayers.Count);
        EditorGUI.BeginChangeCheck();
       
        if (IsLayerArea == false)
        {
            GUI.Box(new Rect(0, 0, Mathf.Infinity, Mathf.Infinity),"");
            //Start rect
            StartRect = new Rect(zoom_x, zoom_y , 150f, 50f);
            GUI.Box(StartRect, "Id : " + dialogueGraph.statementLayers[selectedlayer].ID);
            Rect StartRect_text = new Rect(StartRect.x, StartRect.y + 25f, 150f, 15f);
            GUIStyle Starttext_guistyle = new GUIStyle();
            Starttext_guistyle.border = new RectOffset(1, 1, 1, 1);
            //Starttext_guistyle.normal.background = GUI.skin.button.normal.background;
            Starttext_guistyle.alignment = TextAnchor.MiddleCenter;
            GUI.Label(StartRect_text,"Start",Starttext_guistyle);
            Rect con_rect = new Rect(StartRect.x + 150, (StartRect.y + 25f) - (25 * 0.5f), 10f, 25f);
            if (GUI.Button(con_rect, ""))
            {
                //: {}
            }
            //Settings rect
            SettingsRect = new Rect(StartRect.x + 180, StartRect.y + 50f, 170f, 150f);
            GUI.Box(SettingsRect, "Initialize");
            EditorGUI.LabelField(new Rect(SettingsRect.x + 10, SettingsRect.y + 25, 150, 25),
                "No. of statements <" + dialogueGraph.statementLayers[selectedlayer].statements.Count.ToString() + ">",EditorStyles.boldLabel);
            //
           

            Rect Setting_conRect_l = new Rect(SettingsRect.x - 10f, SettingsRect.y + SettingsRect.height / 2 - 25 / 2, 10f, 25f);
            Rect Setting_conRect_r= new Rect(SettingsRect.x + 170f, SettingsRect.y + SettingsRect.height / 2 - 25/2, 10f, 25f);
            if (GUI.Button(Setting_conRect_l, ""))
            {
                //: {}
            }
            if (GUI.Button(Setting_conRect_r, ""))
            {
                //: {}
            }
            Handles.BeginGUI();
            Handles.DrawBezier( Setting_conRect_l.center, con_rect.center,
                  Setting_conRect_l.center + Vector2.left * 50f,
                  con_rect.center - Vector2.left * 50f,
                  linecolor,
                  null,
                  2f
                  );
            Handles.EndGUI();
            //
            if (dialogueGraph.statementLayers[selectedlayer].statements.Count == 0)
            {
                ESDialgoueScriptableObj.StatementLayer.Statements statements = new ESDialgoueScriptableObj.StatementLayer.Statements();
                statements.rect = new Rect(SettingsRect.x + 220, SettingsRect.y + 100, 250, 180);
                statements.HasBuild = true;
                dialogueGraph.statementLayers[selectedlayer].statements.Add(statements);
            }
            //
            Draw_Char_image();
            for (int i = 0; i < dialogueGraph.statementLayers[selectedlayer].statements.Count; i++)
            {
                DrawStatement(i,Setting_conRect_r);
            }
        }
        else
        {
            bool breakafter = false;
            for (int i = 0; i < dialogueGraph.statementLayers.Count; i++)
            {
                if (dialogueGraph.statementLayers[i].HasBuild == true)
                {
                    Rect textrect = dialogueGraph.statementLayers[i].stmtRect;
                    Rect exitbutrect = dialogueGraph.statementLayers[i].stmtRect;
                   
                    GUI.Box(dialogueGraph.statementLayers[i].stmtRect,dialogueGraph.statementLayers[i].ID.ToString());
                    if (GUI.Button(new Rect(exitbutrect.x + 120f, textrect.y + 2f, 30f, 15f), "X"))
                    {
                        // code
                        
                        int removeme = i;
                        
                        if (dialogueGraph.statementLayers.Count > 1)
                        {
                            Undo.RegisterCompleteObjectUndo(dialogueGraph, "remove");
                            dialogueGraph.statementLayers.RemoveAt(removeme);
                            selectedlayer = dialogueGraph.statementLayers.Count - 1;
                        }
                      
                        if (i == dialogueGraph.statementLayers.Count)
                        {
                            breakafter = true;
                        }
                        if (breakafter)
                        {
                            break;
                        }
                    }
                    Layername[i] = GUI.TextField(new Rect(textrect.x + 5f, textrect.y + 20f, 130, 15),
                    dialogueGraph.statementLayers[i].layerName);
                    Rect enterbutrect = dialogueGraph.statementLayers[i].stmtRect;
                    if (GUI.Button(new Rect(enterbutrect.x + 20f, enterbutrect.y + 75f, 110f, 15f), "Enter"))
                    {
                        //open
                        selectedlayer = i;
                        dialogueGraph.statementLayers[i].Active = true;
                        IsLayerArea = false;

                    }
                }
            }
        }

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(dialogueGraph, "Changes");
            for (int i = 0; i < dialogueGraph.statementLayers.Count; i++)
            {
                dialogueGraph.statementLayers[i].layerName = Layername[i];
               
            }
        }
        DragNode(Event.current);
    }
    //
    private void AddLayer(Vector2 mousePos)
    {
        ESDialgoueScriptableObj.StatementLayer statementLayer = new ESDialgoueScriptableObj.StatementLayer();
        Vector2 zoomCoordsMousePos = ConvertScreenCoordsToZoomCoords(mousePos);
        statementLayer.stmtRect = new Rect(zoomCoordsMousePos.x, zoomCoordsMousePos.y - 100, 150, 150);
        statementLayer.HasBuild = true;
        statementLayer.ID = GenerateId(statementLayer.ID);
        dialogueGraph.statementLayers.Add(statementLayer);
        Undo.RegisterCompleteObjectUndo(dialogueGraph, "add layer");
    }
    //
    private void AddStmt(Vector2 mousepos,bool IsDecision)
    {
        ESDialgoueScriptableObj.StatementLayer.Statements statements = new ESDialgoueScriptableObj.StatementLayer.Statements();
        Vector2 zoomCoordsMousePos = ConvertScreenCoordsToZoomCoords(mousepos);
        statements.rect = new Rect(zoomCoordsMousePos.x, zoomCoordsMousePos.y -100, 250, 180);
        statements.HasBuild = true;
        statements.Isdecision = IsDecision;
        statements.Id = GenerateId(statements.Id);
        dialogueGraph.statementLayers[selectedlayer].statements.Add(statements);
     

        Undo.RegisterCompleteObjectUndo(dialogueGraph, "add stmt");
    }
    //
    private void AddImg(Vector2 mousepos)
    {
        ESDialgoueScriptableObj.StatementLayer.CharacterPicture characterPicture = new ESDialgoueScriptableObj.StatementLayer.CharacterPicture();
        Vector2 zoomCoordsMousePos = ConvertScreenCoordsToZoomCoords(mousepos);
        characterPicture.rect = new Rect(zoomCoordsMousePos.x, zoomCoordsMousePos.y - 100, 200, 150);
        characterPicture.HasBuild = true;
        dialogueGraph.statementLayers[selectedlayer].characterPictures.Add(characterPicture);

        Undo.RegisterCompleteObjectUndo(dialogueGraph, "add img");
    }
    //
    protected int GenerateId(int val)
    {
        int first = 0;
        first = UnityEngine.Random.Range(0,9);
        int second = 0;
        second = UnityEngine.Random.Range(0, 9);
        int third = 0;
        third = UnityEngine.Random.Range(0, 9);
        int forth = 0;
        forth = UnityEngine.Random.Range(0, 9);

        string addval = "100" + first + second + third + forth;
        val = int.Parse(addval);
        return val;
    }
    //
    private void Draw_Char_image()
    {
        Event @event = Event.current;
        if (dialogueGraph.statementLayers[selectedlayer].characterPictures.Count > 0)
        {
            for (int n = 0; n < dialogueGraph.statementLayers[selectedlayer].characterPictures.Count; n++)
            {
                if (dialogueGraph.statementLayers[selectedlayer].characterPictures[n].HasBuild)
                {
                    List<ESDialgoueScriptableObj.StatementLayer.CharacterPicture> characterPictures = dialogueGraph.statementLayers[selectedlayer].characterPictures;
                    List<ESDialgoueScriptableObj.StatementLayer.Statements> statements = dialogueGraph.statementLayers[selectedlayer].statements;
                    Rect rect = characterPictures[n].rect;
                    Rect outrect = characterPictures[n].outrect;
                    Rect texture_rect = new Rect(rect.x + 10, rect.y + 25, 50, 80);
                    outrect = new Rect(rect.x + rect.width, rect.y + rect.height/ 2 - 10.5f, 10, 25);
                    //
                    characterPictures[n].outrect = outrect;
                    //
                    EditorGUI.BeginChangeCheck();
                  
                    //
                    float width = 0;
                    float height =0;
                    Texture2D texture2d = characterPictures[n].texture; 
                    
                    GUI.Box(rect, "Texture2D");
                    //
                    texture2d = EditorGUI.ObjectField(texture_rect,characterPictures[n].texture,typeof(Texture2D),false) as Texture2D;
                    GUI.color = Color.black;
                    GUI.Box(new Rect(texture_rect.x + 53, texture_rect.y, 2,80), "");
                    GUI.color = Color.white;
                    
                    EditorGUI.LabelField(new Rect(texture_rect.x + 56, rect.y + 35, 80, 50), "Width :",EditorStyles.boldLabel);
                    EditorGUI.LabelField(new Rect(texture_rect.x + 56, rect.y + 70, 80, 50), "Height :", EditorStyles.boldLabel);
                    
                    width =  EditorGUI.FloatField(new Rect(texture_rect.x + 111, rect.y + 35, 70, 20), characterPictures[n].Size.x);
                    //EditorGUIUtility.AddCursorRect(widhtrect, MouseCursor.Text);
                    
                    height  =  EditorGUI.FloatField(new Rect(texture_rect.x + 114, rect.y + 70, 70, 20),characterPictures[n].Size.y);
                    EditorGUI.LabelField(new Rect(rect.x + 3, rect.y + 120, 70, 18), "Anchor >:",EditorStyles.boldLabel);
                    if (GUI.Button(new Rect(rect.x + 90, rect.y + 120, 100, 18), characterPictures[n].dropdowntext + "<>"))
                    {
                        characterPictures[n].dropdown = !characterPictures[n].dropdown;
                    }
                    if (characterPictures[n].dropdown)
                    {
                        GUI.Box(new Rect(rect.x + 90, rect.y + 140, 100, 50), "");
                        if (GUI.Button(new Rect(rect.x + 90, rect.y + 140, 100, 20),"Left"))
                        {
                            dialogueGraph.statementLayers[selectedlayer].characterPictures[n].dropdowntext = "Left";
                            dialogueGraph.statementLayers[selectedlayer].characterPictures[n].left = true;
                            dialogueGraph.statementLayers[selectedlayer].characterPictures[n].right = false;
                            dialogueGraph.statementLayers[selectedlayer].characterPictures[n].center = false;
                            dialogueGraph.statementLayers[selectedlayer].characterPictures[n].dropdown = false;
                        }
                        //
                        if (GUI.Button(new Rect(rect.x + 90, rect.y + 159.5f, 100, 20), "Right"))
                        {
                            dialogueGraph.statementLayers[selectedlayer].characterPictures[n].dropdowntext = "Right";
                            dialogueGraph.statementLayers[selectedlayer].characterPictures[n].left = false;
                            dialogueGraph.statementLayers[selectedlayer].characterPictures[n].right = true;
                            dialogueGraph.statementLayers[selectedlayer].characterPictures[n].center = false;
                            dialogueGraph.statementLayers[selectedlayer].characterPictures[n].dropdown = false;
                        }
                        //
                        if (GUI.Button(new Rect(rect.x + 90, rect.y + 179, 100, 20), "Center"))
                        {
                            dialogueGraph.statementLayers[selectedlayer].characterPictures[n].dropdowntext = "Center";
                            dialogueGraph.statementLayers[selectedlayer].characterPictures[n].left = false;
                            dialogueGraph.statementLayers[selectedlayer].characterPictures[n].right = false;
                            dialogueGraph.statementLayers[selectedlayer].characterPictures[n].center = true;
                            dialogueGraph.statementLayers[selectedlayer].characterPictures[n].dropdown = false;
                        }
                    }
                    //;{} dont try to understand hahahahahah

                    //
                    if (GUI.Button(outrect, ""))
                    {
                        if (!characterPictures[n].Connected)
                        {
                            Transistfromimgbox = true;
                            img_in_index = n;
                            currentimgboxindex = n;
                        }
                        else
                        {
                            statements[characterPictures[n].ConnectedIndex].connetedtoimg = false;
                            characterPictures[n].Connected = false;
                        }
                        
                    }
                    
                    if (Transistfromimgbox)
                    {  
                        Handles.DrawBezier(@event.mousePosition,characterPictures[img_in_index].outrect.center,
                         @event.mousePosition + Vector2.left * 50f,
                         characterPictures[img_in_index].outrect.center - Vector2.left * 50f,
                     linecolor,
                     null,
                     2f
                     );
                        GUI.changed = true;
                    }
                    //
                    for (int y = 0; y < dialogueGraph.statementLayers[selectedlayer].statements.Count; y++)
                    {
                        if (characterPictures[n].Connected)
                        {
                            if (characterPictures[n].connected_id == dialogueGraph.statementLayers[selectedlayer].statements[y].Id)
                            {
                                characterPictures[n].ConnectedIndex = y;
                                //statements[y].ConnectedImgIndex = currentimgboxindex;
                            }
                        }

                    }
                    if (characterPictures[n].Connected)
                    {
                        Handles.DrawBezier(statements[characterPictures[n].ConnectedIndex].img_rect.center, outrect.center,
                        statements[characterPictures[n].ConnectedIndex].img_rect.center + Vector2.left * 50f,
                        outrect.center - Vector2.left * 50f,
                        linecolor,
                        null,
                        2f
                        );
                    }
                    //
                    /*
                     * dont you dare freee this line of code. :{
                    if (characterPictures[n].Connected)
                    {
                        characterPictures[n].connected_id = statements[characterPictures[n].ConnectedIndex].Id;
                    }
                    */
                   
                    //
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RegisterCompleteObjectUndo(dialogueGraph, "changes belonging to character image");
                        dialogueGraph.statementLayers[selectedlayer].characterPictures[n].Size.x = width;
                        dialogueGraph.statementLayers[selectedlayer].characterPictures[n].Size.y = height;
                        
                        dialogueGraph.statementLayers[selectedlayer].characterPictures[n].texture = texture2d; 
                    }
                    if (n != 0)
                    {
                        if (GUI.Button(new Rect(rect.x + 150, rect.y, 50, 20), "X"))
                        {
                            if (dialogueGraph.statementLayers[selectedlayer].characterPictures[n].Connected)
                            {
                                int connectedint =  dialogueGraph.statementLayers[selectedlayer].characterPictures[n].ConnectedIndex;
                                dialogueGraph.statementLayers[selectedlayer].statements[connectedint].connetedtoimg = false;
                            }
                            Undo.RegisterCompleteObjectUndo(dialogueGraph, "remove imagebox");
                            dialogueGraph.statementLayers[selectedlayer].characterPictures.RemoveAt(n);
                        }
                    }
                }
            }
            //
           
            //drag
            Event e = Event.current;
            List<ESDialgoueScriptableObj.StatementLayer> eSDialgoue = dialogueGraph.statementLayers;
            switch (e.type)
            {
                case EventType.MouseDown:
                    {
                        if (e.button == 0)
                        {
                            if (!IsLayerArea)
                            {
                                for (int k = 0; k < eSDialgoue[selectedlayer].characterPictures.Count; ++k)
                                {
                                    if (eSDialgoue[selectedlayer].characterPictures[k].rect.Contains(e.mousePosition))
                                    {
                                        Isdrag = true;
                                        imgindex = k;
                                        Isimgbox = true;
                                        GUI.changed = true;
                                    }
                                    else
                                    {
                                        GUI.changed = true;
                                    }
                                }
                                    
                            }

                        }

                        break;
                    }
                case EventType.MouseUp:
                    Isdrag = false;
                    Isimgbox = false;

                    break;

                case EventType.MouseDrag:
                    if (e.button == 0 && Isdrag && Isimgbox)
                    {
                        dialogueGraph.statementLayers[selectedlayer].characterPictures[imgindex].rect.position += e.delta;
                        e.Use();
                    }
                    break;
            }

        }
        
    }
    //
    private void DrawStatement(int i,Rect settings_out)
    {
        if (dialogueGraph.statementLayers[selectedlayer].statements[i].HasBuild)
        {
            Rect rect = dialogueGraph.statementLayers[selectedlayer].statements[i].rect;
            GUIStyle stmt_cap_styles = new GUIStyle();
            stmt_cap_styles.border = GUI.skin.button.border;
            stmt_cap_styles.normal.background = GUI.skin.button.normal.background;
            stmt_cap_styles.alignment = TextAnchor.MiddleCenter;
           
            GUI.Box(new Rect(rect.x, rect.y - 30, rect.width, 30),"Statement Content",stmt_cap_styles);
            GUI.Box(dialogueGraph.statementLayers[selectedlayer].statements[i].rect,
                "ID: " + dialogueGraph.statementLayers[selectedlayer].statements[i].Id.ToString());
            //draw side statment box
           
            Rect stmt_side_box = dialogueGraph.statementLayers[selectedlayer].statements[i].Side_Rect;
            //start
            Rect Text_Rect = new Rect(rect.x + (10/2),rect.y + 50,240,120);
            string stmt_content = "";
            EditorGUI.BeginChangeCheck();
            //EditorGUIUtility.AddCursorRect(Text_Rect, MouseCursor.Text);
            stmt_content = EditorGUI.TextArea(Text_Rect, dialogueGraph.statementLayers[selectedlayer].statements[i].StatementContent);
            //
            stmt_side_box = new Rect(rect.x + 300, rect.y + 80, 250, 150);
            float timer = new float();
            bool click = new bool();
            AudioClip audioClip = dialogueGraph.statementLayers[selectedlayer].statements[i].audio;
            string tagname = dialogueGraph.statementLayers[selectedlayer].statements[i].CharacterTagName;
            //...........
            object texture2D = dialogueGraph.statementLayers[selectedlayer].statements[i].buttontexture;
            Vector2 size = dialogueGraph.statementLayers[selectedlayer].statements[i].Size;
            string content = dialogueGraph.statementLayers[selectedlayer].statements[i].ButtonText;

            //:{}
            GUI.Box(stmt_side_box, "Transit To >");
            if (!dialogueGraph.statementLayers[selectedlayer].statements[i].Isdecision)
            {
                GUI.Box(new Rect(rect.x + 300, rect.y + 115, 250, 5), "");
                click = GUI.Toggle(new Rect(stmt_side_box.x + 80, stmt_side_box.y + 20, 15, 15), dialogueGraph.statementLayers[selectedlayer].statements[i].Click, "");
                GUI.Label(new Rect(stmt_side_box.x, stmt_side_box.y + 20, 100, 20), "Use Button");
                if (click)
                {


                    if (GUI.Button(new Rect(stmt_side_box.x + 10, stmt_side_box.y + 50, 200, 15), "Button Settings <>"))
                    {
                        dialogueGraph.statementLayers[selectedlayer].statements[i].dropdown = !dialogueGraph.statementLayers[selectedlayer].statements[i].dropdown;
                    }

                    Rect audiorect = new Rect(stmt_side_box.x + 60, stmt_side_box.y + 80, 150, 20);
                    Rect tagrect = new Rect(stmt_side_box.x + 85, stmt_side_box.y + 118, 150, 20);
                    GUI.Label(new Rect(stmt_side_box.x, audiorect.y, 100, 20), "Audio = ");
                    GUI.Label(new Rect(stmt_side_box.x, tagrect.y, 100, 20), "TagName = ");
                    tagname = EditorGUI.TextField(tagrect, dialogueGraph.statementLayers[selectedlayer].statements[i].CharacterTagName);
                    audioClip = (AudioClip)EditorGUI.ObjectField(audiorect, "", dialogueGraph.statementLayers[selectedlayer].statements[i].audio, typeof(AudioClip), false);
                    if (dialogueGraph.statementLayers[selectedlayer].statements[i].dropdown)
                    {

                        Rect dropdown_rect = new Rect(new Rect(stmt_side_box.x + 10, stmt_side_box.y + 65, 200, 170));
                        GUI.Box(dropdown_rect, "Button Settings");
                        GUI.Box(new Rect(dropdown_rect.x, dropdown_rect.y + 17, 200, 3), "");
                        boxrect = new Rect(dropdown_rect.x, dropdown_rect.y + 20, dropdown_rect.width, dropdown_rect.height - 20);
                        Button_scrollPosition = GUI.BeginScrollView(boxrect, Button_scrollPosition, new Rect(dropdown_rect.x, dropdown_rect.y, 210f, 290));
                        GUI.Label(new Rect(dropdown_rect.x + 5, dropdown_rect.y + 20, 150, 30), "Button Texture >");
                        texture2D = EditorGUI.ObjectField(new Rect(dropdown_rect.x + 120, dropdown_rect.y + 20, 80, 100), "", dialogueGraph.statementLayers[selectedlayer].statements[i].buttontexture, typeof(Texture2D), false);
                        GUI.Box(new Rect(dropdown_rect.x, dropdown_rect.y + 130, 200, 3), "");
                        GUI.Label(new Rect(dropdown_rect.x, dropdown_rect.y + 132, 150, 30), "Button Size");
                        size = EditorGUI.Vector2Field(new Rect(dropdown_rect.x, dropdown_rect.y + 150, 150, 50), "", dialogueGraph.statementLayers[selectedlayer].statements[i].Size);
                        GUI.Box(new Rect(dropdown_rect.x, dropdown_rect.y + 172, 200, 3), "");
                        GUI.Label(new Rect(dropdown_rect.x + 5, dropdown_rect.y + 175, 150, 30), "ButtonText");
                        content = EditorGUI.TextField(new Rect(dropdown_rect.x + 5, dropdown_rect.y + 195, 150, 15), dialogueGraph.statementLayers[selectedlayer].statements[i].ButtonText);
                        GUI.Label(new Rect(dropdown_rect.x + 5, dropdown_rect.y + 230, 150, 30), "Button Anchor");
                        //
                        Rect anchorrect = new Rect(dropdown_rect.x + 5, dropdown_rect.y + 250, 150, 16);
                        if (GUI.Button(anchorrect, dialogueGraph.statementLayers[selectedlayer].statements[i].anchortext + "<>"))
                        {
                            dialogueGraph.statementLayers[selectedlayer].statements[i].anchordropdown = !dialogueGraph.statementLayers[selectedlayer].statements[i].anchordropdown;

                        }
                        GUI.EndScrollView();
                        if (dialogueGraph.statementLayers[selectedlayer].statements[i].anchordropdown)
                        {
                            GUI.Box(new Rect(anchorrect.x, anchorrect.y - 80, 150, 57), "");
                            Rect topleft = new Rect(anchorrect.x, anchorrect.y - 80, 150, 15);
                            Rect topright = new Rect(anchorrect.x, anchorrect.y - 66, 150, 15);
                            Rect downleft = new Rect(anchorrect.x, anchorrect.y - 52, 150, 15);
                            Rect downright = new Rect(anchorrect.x, anchorrect.y - 38f, 150, 15);
                            if (GUI.Button(topleft, "TopLeft"))
                            {
                                dialogueGraph.statementLayers[selectedlayer].statements[i].anchortext = "TopLeft";
                                dialogueGraph.statementLayers[selectedlayer].statements[i].TopLeft = true;
                                dialogueGraph.statementLayers[selectedlayer].statements[i].TopRight = false;
                                dialogueGraph.statementLayers[selectedlayer].statements[i].DownLeft = false;
                                dialogueGraph.statementLayers[selectedlayer].statements[i].DownRight = false;
                                dialogueGraph.statementLayers[selectedlayer].statements[i].anchordropdown = false;
                            }
                            //
                            if (GUI.Button(topright, "TopRight"))
                            {
                                dialogueGraph.statementLayers[selectedlayer].statements[i].anchortext = "TopRight";
                                dialogueGraph.statementLayers[selectedlayer].statements[i].TopLeft = false;
                                dialogueGraph.statementLayers[selectedlayer].statements[i].TopRight = true;
                                dialogueGraph.statementLayers[selectedlayer].statements[i].DownLeft = false;
                                dialogueGraph.statementLayers[selectedlayer].statements[i].DownRight = false;
                                dialogueGraph.statementLayers[selectedlayer].statements[i].anchordropdown = false;
                            }
                            //
                            if (GUI.Button(downleft, "DownLeft"))
                            {
                                dialogueGraph.statementLayers[selectedlayer].statements[i].anchortext = "DownLeft";
                                dialogueGraph.statementLayers[selectedlayer].statements[i].TopLeft = false;
                                dialogueGraph.statementLayers[selectedlayer].statements[i].TopRight = false;
                                dialogueGraph.statementLayers[selectedlayer].statements[i].DownLeft = true;
                                dialogueGraph.statementLayers[selectedlayer].statements[i].DownRight = false;
                                dialogueGraph.statementLayers[selectedlayer].statements[i].anchordropdown = false;
                            }
                            //
                            if (GUI.Button(downright, "DownRight"))
                            {
                                dialogueGraph.statementLayers[selectedlayer].statements[i].anchortext = "DownRight";
                                dialogueGraph.statementLayers[selectedlayer].statements[i].TopLeft = false;
                                dialogueGraph.statementLayers[selectedlayer].statements[i].TopRight = false;
                                dialogueGraph.statementLayers[selectedlayer].statements[i].DownLeft = false;
                                dialogueGraph.statementLayers[selectedlayer].statements[i].DownRight = true;
                                dialogueGraph.statementLayers[selectedlayer].statements[i].anchordropdown = false;
                            }
                        }
                        //
                     
                        //

                    }
                }
                else
                {
                   

                    timer = EditorGUI.FloatField(new Rect(stmt_side_box.x + 60, stmt_side_box.y + 50, 150, 20), dialogueGraph.statementLayers[selectedlayer].statements[i].Timer);
                    EditorGUIUtility.AddCursorRect(new Rect(stmt_side_box.x + 60, stmt_side_box.y + 50, 150, 20), MouseCursor.Text);
                   
                    Rect audiorect = new Rect(stmt_side_box.x + 60, stmt_side_box.y + 80, 150, 20);
                    Rect tagrect = new Rect(stmt_side_box.x + 85, stmt_side_box.y + 118, 150, 20);
                    GUI.Label(new Rect(stmt_side_box.x, audiorect.y, 100, 20), "Audio = ");
                    GUI.Label(new Rect(stmt_side_box.x, tagrect.y, 100, 20), "TagName = ");
                    audioClip = (AudioClip)EditorGUI.ObjectField(audiorect, "", dialogueGraph.statementLayers[selectedlayer].statements[i].audio, typeof(AudioClip), false);
                    tagname = EditorGUI.TextField(tagrect, dialogueGraph.statementLayers[selectedlayer].statements[i].CharacterTagName);
                    GUI.Label(new Rect(stmt_side_box.x, stmt_side_box.y + 50, 100, 20), "Value = ");
                }
            }
           
            //in
            Rect InRect = dialogueGraph.statementLayers[selectedlayer].statements[i].InRect;
            Rect OutRect = dialogueGraph.statementLayers[selectedlayer].statements[i].OutRect;
            Rect img_rect = dialogueGraph.statementLayers[selectedlayer].statements[i].img_rect;
            Rect mid_in_rect  = new Rect(new Rect(stmt_side_box.x - 10,
            (stmt_side_box.y + stmt_side_box.height / 2) - (25 / 2), 10, 25));
            Rect mid_out_rect = new Rect(new Rect(rect.x + rect.width,
            (rect.y + rect.height / 2) - (25 / 2), 10, 25));
            
            InRect = new Rect(rect.x - 10, (rect.y + rect.height/2) - (25/2), 10, 25);
            OutRect = new Rect(stmt_side_box.x + stmt_side_box.width,
            (stmt_side_box.y + stmt_side_box.height / 2) - (25 / 2), 10, 25);
            img_rect = new Rect(rect.x - 10, rect.y + 20, 10, 25);
            //draw contents
            
            //
            dialogueGraph.statementLayers[selectedlayer].statements[i].InRect = InRect;
            dialogueGraph.statementLayers[selectedlayer].statements[i].OutRect = OutRect;
            dialogueGraph.statementLayers[selectedlayer].statements[i].Side_Rect = stmt_side_box;
            dialogueGraph.statementLayers[selectedlayer].statements[i].img_rect = img_rect;
            //
            Handles.BeginGUI();
            Handles.DrawBezier(mid_in_rect.center, mid_out_rect.center,
                  mid_in_rect.center + Vector2.left * 50f,
                  mid_out_rect.center - Vector2.left * 50f,
                  linecolor,
                  null,
                  2f
                  );
            //
          
            //
            if (i == 0)
            {
                Handles.DrawBezier( InRect.center, settings_out.center,
                     InRect.center + Vector2.left * 50f,
                     settings_out.center - Vector2.left * 50f,
                     linecolor,
                     null,
                     2f
                     );
            }
           Handles.EndGUI();
            //
            List<ESDialgoueScriptableObj.StatementLayer.Statements> layer = dialogueGraph.statementLayers[selectedlayer].statements;
            //
            if (layer[i].Isdecision)
            {
                Rect Add_rect = new Rect(stmt_side_box.center - new Vector2(100 / 2,50), new Vector2(100, 20));
                //
              
               
                Rect align_dropdown = new Rect(stmt_side_box.center - new Vector2(150 / 2, 2), new Vector2(150, 20));
               
                
                if (GUI.Button(Add_rect, "+"))
                {
                    ESDialgoueScriptableObj.StatementLayer.Statements.Decision decision = new ESDialgoueScriptableObj.StatementLayer.Statements.Decision();
                    decision.rect = new Rect( layer[i].Side_Rect.x + 500,layer[i].Side_Rect.y,350, 80);
                    string first, second;
                    int alpha_first = 0;
                    alpha_first= UnityEngine.Random.Range(0, 9);
                    int alpha_second = 0;
                    alpha_second = UnityEngine.Random.Range(0,9);
                    first = alpha_first == 0 ? "A" : alpha_first == 1 ? "B" : alpha_first == 2 ? "C" :
                        alpha_first == 3 ? "D" : alpha_first == 4 ? "E" : alpha_first == 5 ? "F" : alpha_first == 6 ?
                        "G" : alpha_first == 8 ? "F" : "I";
                    //
                    second = alpha_second == 0 ? "K" : alpha_second == 1 ? "V" : alpha_second == 3 ? "Y" :
                       alpha_second == 3 ? "R" : alpha_second == 4 ? "T" : alpha_second == 5 ? "O" : alpha_second == 6 ?
                       "R" : alpha_second == 8 ? "X" : "Q";
                    //
                    int g = 0; 
                      g =   GenerateId(g);
                    decision.ID = g.ToString() + first + second;
                    decision.HasBuild = true;
                    layer[i].decisions.Add(decision);
                }
                //
                GUI.Label(new Rect(stmt_side_box.center - new Vector2(150 / 2, 23), new Vector2(150, 20)), "        Alignment <>");
                //
                if (GUI.Button(align_dropdown,layer[i].align_dropdown_text))
                {
                    //do somthing
                    layer[i].align_dropdown = !layer[i].align_dropdown;
                }
                //
                Rect audiorect = new Rect(Add_rect.x - 10, align_dropdown.y + 25, 140, 20);
                Rect tagrect = new Rect(Add_rect.x + 13, align_dropdown.y + 50, 140, 20);
                GUI.Label(new Rect(stmt_side_box.x + 10, tagrect.y, 100, 20), "TagName = ");
                GUI.Label(new Rect(stmt_side_box.x + 10, audiorect.y, 100, 20), "Audio = ");
                audioClip = (AudioClip)EditorGUI.ObjectField(audiorect, "", dialogueGraph.statementLayers[selectedlayer].statements[i].audio, typeof(AudioClip), false);
                tagname = EditorGUI.TextField(tagrect, dialogueGraph.statementLayers[selectedlayer].statements[i].CharacterTagName);
                if (layer[i].align_dropdown == true)
                {
                    if(GUI.Button(new Rect(stmt_side_box.center - new Vector2(150 / 2, -40), new Vector2(150, 20)),"Horizontal"))
                    {
                        layer[i].horizontal = true;
                        layer[i].vertical = false;

                        layer[i].align_dropdown_text = "Horizontal";
                        layer[i].align_dropdown = false;
                    }
                    //
                    if (GUI.Button(new Rect(stmt_side_box.center - new Vector2(150 / 2, -60), new Vector2(150, 20)), "Vertical"))
                    {
                        layer[i].horizontal = false;
                        layer[i].vertical = true;
                       
                        layer[i].align_dropdown_text = "Vertical";
                        layer[i].align_dropdown = false;
                    }
                   
                }
            }
            //
            if(GUI.Button(img_rect,""))
            {
                if (Transistfromimgbox)
                {
                    if (layer[i].connetedtoimg == false)
                    {
                        layer[i].ConnectedImgIndex = currentimgboxindex;
                        dialogueGraph.statementLayers[selectedlayer].characterPictures[currentimgboxindex].ConnectedIndex = i;
                        dialogueGraph.statementLayers[selectedlayer].characterPictures[currentimgboxindex].Connected = true;
                        dialogueGraph.statementLayers[selectedlayer].characterPictures[currentimgboxindex].connected_id = layer[i].Id;
                        layer[i].connetedtoimg = true;
                        Transistfromimgbox = false;
                    }
                   
                   
                }
            }
            //
            if (GUI.Button(InRect, ""))
            {
                //in
                if (TransFromDecision)
                {
                    if (layer[i].IsconnectedIn == false)
                    {
                        if (istransit)
                        {
                            In_int = i;
                            layer[i].ConnectedIndexOut = Out_int;

                            //Debug.Log(Stmt_Out_Int);
                            layer[i].desparentindex = Stmt_Out_Int;
                            layer[Stmt_Out_Int].decisions[Out_int].ConnectedIndexIn = In_int;
                            layer[Stmt_Out_Int].decisions[Out_int].connected_id = layer[i].Id;
                            layer[Stmt_Out_Int].decisions[Out_int].IsconnectedOut = true;
                            layer[i].ConnectedOutId = layer[Stmt_Out_Int].Id;
                            layer[i].connectedtodecision = true;
                            layer[i].IsconnectedIn = true;
                            istransit = false;
                        }
                    }
                
                }
                else
                {
                 
                    if (layer[i].IsconnectedIn == false)
                    {
                        if (istransit)
                        {
                            In_int = i;
                            In_Id = layer[i].Id;
                            layer[i].ConnectedIndexOut = Out_int;
                            layer[i].ConnectedOutId = Out_Id;
                            layer[Out_int].ConnectedIndexIn = In_int;
                            layer[Out_int].ConnectedInId = In_Id;
                            layer[Out_int].IsconnectedOut = true;
                            
                            layer[i].connectedtodecision = false;
                            layer[i].IsconnectedIn = true;
                            TransFromDecision = false;
                            Transistfromimgbox = false;
                            istransit = false;
                        }
                        
                    }
                }
               
            }
            //
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RegisterCompleteObjectUndo(dialogueGraph, "changes");
                dialogueGraph.statementLayers[selectedlayer].statements[i].StatementContent = stmt_content;
                dialogueGraph.statementLayers[selectedlayer].statements[i].Click = click;
                dialogueGraph.statementLayers[selectedlayer].statements[i].buttontexture = texture2D as Texture2D;
                dialogueGraph.statementLayers[selectedlayer].statements[i].Size = size;
                dialogueGraph.statementLayers[selectedlayer].statements[i].ButtonText = content;
                dialogueGraph.statementLayers[selectedlayer].statements[i].audio = audioClip;
                dialogueGraph.statementLayers[selectedlayer].statements[i].CharacterTagName = tagname;
                if (!dialogueGraph.statementLayers[selectedlayer].statements[i].Isdecision)
                {
                    if (click == false)
                        dialogueGraph.statementLayers[selectedlayer].statements[i].Timer = timer;
                }
            }
            //
            if (GUI.Button(OutRect, ""))
            {
                //out
                if (layer[i].Isdecision == false)
                {
                    TransFromDecision = false;
                    Transistfromimgbox = false;
                    if (layer[i].IsconnectedOut == false)
                    {
                        istransit = true;
                        Out_int = i;
                        Out_Id = layer[i].Id;
                       // Debug.Log(Out_int);
                    }
                    if (layer[i].IsconnectedOut == true)
                    {
                        layer[layer[i].ConnectedIndexIn].IsconnectedIn = false;
                        layer[i].IsconnectedOut = false;
                    }
                }
               
            }
            if (GUI.Button(mid_in_rect, ""))
            {
                //midoutstmt
            }
            if (GUI.Button(mid_out_rect, ""))
            {
                //midoutstmt
            }
            //
            
            if (layer[i].decisions.Count > 0)
            {
                for (int h = 0; h < layer[i].decisions.Count; h++)
                {
                    if (layer[i].Isdecision == true)
                    {
                        if (layer[i].decisions[h].HasBuild == true)
                        {

                            GUI.Box(layer[i].decisions[h].rect, "");
                            Rect des_rect = layer[i].decisions[h].rect;
                            Rect des_rect_in = new Rect(des_rect.x - 10f, (des_rect.y + des_rect.height / 2) - (25 / 2), 10, 25f);
                            Rect des_rect_out = new Rect(des_rect.x + des_rect.width, (des_rect.y + des_rect.height / 2) - (25 / 2), 10, 25f);
                            layer[i].decisions[h].OutRect = des_rect_out;
                            //
                            /*
                             * not important do not unclock hahahahahahaha
                            if (layer[i].decisions[h].ID == "")
                            {
                                string first, second;
                                int alpha_first = 0;
                                alpha_first = UnityEngine.Random.Range(0, 9);
                                int alpha_second = 0;
                                alpha_second = UnityEngine.Random.Range(0, 9);
                                first = alpha_first == 0 ? "A" : alpha_first == 1 ? "B" : alpha_first == 2 ? "C" :
                                    alpha_first == 3 ? "D" : alpha_first == 4 ? "E" : alpha_first == 5 ? "F" : alpha_first == 6 ?
                                    "G" : alpha_first == 8 ? "F" : "I";
                                //
                                second = alpha_second == 0 ? "K" : alpha_second == 1 ? "V" : alpha_second == 3 ? "Y" :
                                   alpha_second == 3 ? "R" : alpha_second == 4 ? "T" : alpha_second == 5 ? "O" : alpha_second == 6 ?
                                   "R" : alpha_second == 8 ? "X" : "Q";
                                //
                                int g = 0;
                                g = GenerateId(g);
                                layer[i].decisions[h].ID = g.ToString() + first + second;
                            }
                           */
                            //
                            GUI.Box(new Rect(des_rect.x, des_rect.y - 20, 270, 20), "ID:" + layer[i].decisions[h].ID);
                            string decisioncontent = "";
                            EditorGUI.BeginChangeCheck();
                            decisioncontent = GUI.TextArea(new Rect(des_rect.x + 25, des_rect.y+10, 300, 60),layer[i].decisions[h].content);
                            //
                            if (GUI.Button(des_rect_in, ""))
                            {
                                //desrectin
                            }
                            //
                            if (GUI.Button(des_rect_out, ""))
                            {
                                //desrectout
                                if (layer[i].Isdecision == true)
                                {
                                    if (layer[i].decisions[h].IsconnectedOut == false)
                                    {
                                        TransFromDecision = true;
                                        Transistfromimgbox = false;
                                        istransit = true;
                                        Out_int = h;
                                        Stmt_Out_Int = i;
                                        
                                    }
                                    if (layer[i].decisions[h].IsconnectedOut == true)
                                    {
                                        layer[layer[i].decisions[h].ConnectedIndexIn].IsconnectedIn = false;
                                        layer[layer[i].decisions[h].ConnectedIndexIn].connectedtodecision = false;
                                        layer[i].decisions[h].IsconnectedOut = false;

                                    }
                                }
                            }
                            Handles.DrawBezier(des_rect_in.center, OutRect.center,
                            des_rect_in.center + Vector2.left * 50f,
                            OutRect.center - Vector2.left * 200f,
                            linecolor,
                            null,
                            2f
                            );
                            if (EditorGUI.EndChangeCheck())
                            {
                                Undo.RegisterCompleteObjectUndo(dialogueGraph, "Changes in des");
                                layer[i].decisions[h].content = decisioncontent;
                            }
                            //remove desicion
                            Rect delete_rect = new Rect(des_rect.x + des_rect.width - 80, des_rect.y - 20, 80, 20);
                            if (GUI.Button(delete_rect, "X"))
                            {
                                //remove a decision  
                                Undo.RegisterCompleteObjectUndo(dialogueGraph, "remove decision");
                                if (layer[i].decisions[h].IsconnectedOut)
                                {
                                    layer[layer[i].decisions[h].ConnectedIndexIn].IsconnectedIn = false;
                                    layer[layer[i].decisions[h].ConnectedIndexIn].connectedtodecision = false;
                                    layer[i].decisions[h].IsconnectedOut = false;
                                }
                                layer[i].decisions.RemoveAt(h);
                            }

                          
                        }
                      
                    }
                }
            }
            //
          
            layer[0].IsconnectedIn = true;
            Event e = Event.current;
            if (i != 0)
            {
                if (layer[i].IsconnectedIn == true)
                {
                    Handles.BeginGUI();
                    for (int k = 0; k < dialogueGraph.statementLayers[selectedlayer].statements.Count; k++)
                    {
                        if (layer[i].connectedtodecision)
                        {
                            if (layer[i].ConnectedOutId == dialogueGraph.statementLayers[selectedlayer].statements[k].Id)
                            {
                                layer[i].desparentindex = k;
                                for (int f = 0; f < dialogueGraph.statementLayers[selectedlayer].statements[k].decisions.Count; f++)
                                {
                                    if (layer[i].Id == dialogueGraph.statementLayers[selectedlayer].statements[k].decisions[f].connected_id)
                                    {
                                       dialogueGraph.statementLayers[selectedlayer].statements[k].decisions[f].ConnectedIndexIn = i;
                                      //  des_int = f;
                                    }
                                    if (i == dialogueGraph.statementLayers[selectedlayer].statements[k].decisions[f].ConnectedIndexIn)
                                    {
                                        layer[i].ConnectedIndexOut = f;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (layer[i].ConnectedOutId == dialogueGraph.statementLayers[selectedlayer].statements[k].Id)
                            {
                                layer[i].ConnectedIndexOut = k;
                            }
                        }
                    }
                    if (!layer[i].connectedtodecision)
                    {
                        Handles.DrawBezier(layer[i].InRect.center, layer[layer[i].ConnectedIndexOut].OutRect.center,
                        layer[i].InRect.center + Vector2.left * 50f,
                        layer[layer[i].ConnectedIndexOut].OutRect.center - Vector2.left * 50f,
                        linecolor,
                        null,
                        2f
                        );
                        //

                    }
                    else
                    {
                      
                      Rect tempoutrect = layer[layer[i].desparentindex].decisions[layer[i].ConnectedIndexOut].OutRect;
                      Handles.DrawBezier(layer[i].InRect.center, tempoutrect.center,
                      layer[i].InRect.center + Vector2.left * 50f,
                      tempoutrect.center - Vector2.left * 50f,
                      linecolor,
                      null,
                      2f
                      );
                    }
                   
                  
                    Handles.EndGUI();
                }
                if (layer[i].IsconnectedOut)
                {
                    for (int k = 0; k < dialogueGraph.statementLayers[selectedlayer].statements.Count; k++)
                    {
                        if (layer[i].ConnectedInId == dialogueGraph.statementLayers[selectedlayer].statements[k].Id)
                        {
                            layer[i].ConnectedIndexIn = k;
                        }
                    }
                }
            }
            //
            
            //

            if (istransit == true)
            {
                Handles.BeginGUI();
                if (!TransFromDecision)
                {
                    Handles.DrawBezier(e.mousePosition, layer[Out_int].OutRect.center,
                     e.mousePosition + Vector2.left * 50f,
                    layer[Out_int].OutRect.center - Vector2.left * 50f,
                    linecolor,
                    null,
                    2f
                    );
                }
                else
                {
                    Handles.DrawBezier(e.mousePosition, layer[Stmt_Out_Int].decisions[Out_int].OutRect.center,
                    e.mousePosition + Vector2.left * 50f,
                   layer[Stmt_Out_Int].decisions[Out_int].OutRect.center - Vector2.left * 50f,
                   linecolor,
                   null,
                   2f
                   );
                }
             
               
                GUI.changed = true;
                Handles.EndGUI();
            }
            if (i != 0)
            {
                if (GUI.Button(new Rect(rect.x + rect.width - 50, rect.y, 50, 15),"X"))
                {
                    //remove this 
                    dialogueGraph.statementLayers[selectedlayer].reset = true;
                    Undo.RegisterCompleteObjectUndo(dialogueGraph, "remove stmt");
                    if (layer[i].connetedtoimg)
                    {
                        dialogueGraph.statementLayers[selectedlayer].characterPictures[layer[i].ConnectedImgIndex].Connected = false;
                    }
                    if (layer[i].connectedtodecision == false)
                    {  
                      if (layer[i].IsconnectedIn)
                      {
                          layer[layer[i].ConnectedIndexOut].IsconnectedOut = false;
                       
                      }
                      if (layer[i].IsconnectedOut)
                      {
                          layer[layer[i].ConnectedIndexIn].IsconnectedIn = false;
                        
                      }
                    
                    }
                    if(layer[i].connectedtodecision == true)
                    {
                        if (layer[i].IsconnectedIn)
                        {
                            layer[layer[i].desparentindex].decisions[layer[i].ConnectedIndexOut].IsconnectedOut = false;
                            layer[layer[i].desparentindex].decisions[layer[i].ConnectedIndexOut].ConnectedIndexIn = -1;
                         
                        }
                        if (layer[i].IsconnectedOut)
                        {
                            layer[layer[i].ConnectedIndexIn].IsconnectedIn = false;
                          
                        }
                    }
                    if (layer[i].Isdecision == true)
                    {
                       
                        if (layer[i].decisions.Count > 0)
                        {   
                            for (int h = 0; h < layer[i].decisions.Count; ++h)
                            {   
                                if (layer[i].decisions[h].IsconnectedOut)
                                {
                                    Debug.Log(h);
                                    int myindex = layer[i].decisions[h].ConnectedIndexIn;
                                    
                                    layer[myindex].IsconnectedIn = false;
                                    layer[myindex].connectedtodecision = false;
                                    layer[i].decisions[h].IsconnectedOut = false;
                                }
                            }
                        }
                    }
                   dialogueGraph.statementLayers[selectedlayer].statements.RemoveAt(i);
                }
            }
        }
    }
    //
    protected void NodeEvent(Event e)
    {
        if (dialogueGraph.statementLayers.Count == 0)
        {
            AddLayer(new Vector2(150, 150));
        }
        switch (e.type)
        {
            case EventType.MouseDown:
            if (e.button == 1)
            {
               CallGenericMenu(e.mousePosition);
               
            }
            if (e.button == 0)
            {
               istransit = false;
               Transistfromimgbox = false;
            }
            break;
        }
    }
    //
    protected void DragNode(Event e)
    {
        //
        List<ESDialgoueScriptableObj.StatementLayer> eSDialgoue = dialogueGraph.statementLayers;    
        #region Drag_Seleted_STMTs_Block
        switch (e.type)
        {
            case EventType.MouseDown:
                {
                    if (e.button == 0)
                    {
                        if(IsLayerArea)
                        for (int i = 0; i < eSDialgoue.Count; ++i)
                        {
                            if (eSDialgoue[i].stmtRect.Contains(e.mousePosition))
                            {
                                Isdrag = true;
                               
                                StateMentLayerIndex = i;
                                GUI.changed = true;
                            }
                            else
                            {
                                GUI.changed = true;
                            }
                        }
                        else
                            for (int i = 0; i < eSDialgoue[selectedlayer].statements.Count; ++i)
                            {
                                if (eSDialgoue[selectedlayer].statements[i].rect.Contains(e.mousePosition))
                                {
                                    Isdrag = true;
                                 
                                    StateMentIndex = i;
                                    GUI.changed = true;
                                }
                                else
                                {
                                    GUI.changed = true;
                                }
                                for (int h = 0; h < eSDialgoue[selectedlayer].statements[i].decisions.Count; ++h)
                                {
                                    if (eSDialgoue[selectedlayer].statements[i].decisions[h].rect.Contains(e.mousePosition))
                                    {
                                        Isdrag = true;
                                        DragDecision = true;
                                        StateMentIndex = i;
                                        decisionindex = h;
                                        GUI.changed = true;
                                    }
                                    else
                                    {
                                        GUI.changed = true;
                                    }
                                }
                            }
                    }
                    break;
                }
            case EventType.MouseUp:
                Isdrag = false;
                DragDecision = false;
                break;

            case EventType.MouseDrag:
                if (e.button == 0 && Isdrag)
                {

                    if (IsLayerArea)
                    {
                        dialogueGraph.statementLayers[StateMentLayerIndex].stmtRect.position += e.delta;
                        e.Use();
                    }
                    else
                    {
                        if (DragDecision == false)
                        {
                            dialogueGraph.statementLayers[selectedlayer].statements[StateMentIndex].rect.position += e.delta;
                            if (dialogueGraph.statementLayers[selectedlayer].statements[StateMentIndex].decisions.Count > 0)
                            {
                                for (int i = 0; i < dialogueGraph.statementLayers[selectedlayer].statements[StateMentIndex].decisions.Count; ++i)
                                {
                                    dialogueGraph.statementLayers[selectedlayer].statements[StateMentIndex].decisions[i].rect.position += e.delta;
                                }
                            }
                        }
                        else
                        {
                            dialogueGraph.statementLayers[selectedlayer].statements[StateMentIndex].decisions[decisionindex].rect.position += e.delta;
                        }
                        
                        e.Use();
                    }
                }
                break;
        }
        #endregion
        //

    }
    //
    private void CallGenericMenu(Vector2 mousepos)
    {
        GenericMenu genericMenu = new GenericMenu();
        if (IsLayerArea)
        {
            genericMenu.AddItem(new GUIContent("Add Statement Layer"), false, () => AddLayer(mousepos));
        }
        else
        {
            genericMenu.AddItem(new GUIContent("Add/Statement Block"), false, () => AddStmt(mousepos,false));
            genericMenu.AddItem(new GUIContent("Add/Decision Block"), false, () => AddStmt(mousepos, true));
            genericMenu.AddItem(new GUIContent("Content/CharacterImage"), false, () => AddImg(mousepos));
        }

       
        genericMenu.ShowAsContext();
    }
    //
    private void ReloadWindow()
    {
        dialogueGraph = Selection.activeObject as ESDialgoueScriptableObj;
        if (dialogueGraph == null)
        {
             Debug.Log("There is no Dialogue Graph seleted please select a dialogue graph :)");
        }
        else
        {
            serialized = new SerializedObject(dialogueGraph);
            Debug.Log(dialogueGraph.name);

        }
        lastselectedobj = selectedobj;
    }
}
