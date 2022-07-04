using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CanEditMultipleObjects]
[CustomEditor(typeof(ESDialogueManager))]
public class ESDialogueManagerEditor : Editor
{
    public ESDialogueManager manager;
    SerializedObject serialized;
    int selectedindex;
    int CurrentMessageIndex;
    GameObject[] tagobj;
   
    string[] tagname;
    int[] sceneindex;
    int[] triggerid ;
    float[] cooldowntime;
    ESDialogueManager.EventClass.LoadSceneType[] loadSceneType;
    ESDialogueManager.EventClass.SceneLoadType[] LoadType;
    string[] scenename;
    bool[] destroythis;
    ESDialogueManager[] dialogueManagers;
    string[] decisionid;
    float[] repeattime;
    int[] repeatamount;

    

    public override void OnInspectorGUI()
    {
        CustomInspector(manager);
      
    }
    //
    private void CustomInspector(ESDialogueManager eSDialogueManager)
    {
        eSDialogueManager = target as ESDialogueManager;
        serialized = new SerializedObject(eSDialogueManager);
        ESDialgoueScriptableObj eSDialgoueScriptable = eSDialogueManager.DialogueGraph;
        
        EditorGUI.BeginChangeCheck();
        //
        GUILayout.Label("DialogueDataBase",EditorStyles.boldLabel);
        GUILayout.Space(5.5f);
        if (eSDialogueManager.DialogueGraph == null)
        {
            EditorGUILayout.HelpBox("Dialogue DataBase Is Empty :{}",MessageType.Warning);
        
        }
        GUILayout.Space(5.5f);
        eSDialgoueScriptable = EditorGUILayout.ObjectField("", eSDialogueManager.DialogueGraph, typeof(ESDialgoueScriptableObj), true) as ESDialgoueScriptableObj;
        //
        ESDialogueManager.TriggerType triggerType = eSDialogueManager.triggerType;
        ESDialogueManager.PanelAnchor panelAnchor = eSDialogueManager.panelAnchor;
        ESDialogueManager.WrapMode wrapMode = eSDialogueManager.wrapMode;
        bool visible = eSDialogueManager.Visible;
        Vector2 panelsize = eSDialogueManager.PanelSize;
        float bordersize = eSDialogueManager.BorderSize;
        float scroll_v_val = eSDialogueManager.scroll_V_value;
        float scroll_h_val = eSDialogueManager.scroll_H_value;
        string Tagname = eSDialogueManager.ObjectTagName;
        float posx = eSDialogueManager.posx;
        float posy = eSDialogueManager.posy;
        float center = eSDialogueManager.Center;
        float Spacing = eSDialogueManager.spacing;
        float stmt_posx = eSDialogueManager.stmt_posx;
        float stmt_posy = eSDialogueManager.stmt_posy;
        bool freezetime = eSDialogueManager.FreezeTime;
        bool _3d_audio = eSDialogueManager.Use_3D_Audio;
        bool usedisplaywidth = eSDialogueManager.ScaleWithDisplayWidth;
        int fontsize = eSDialogueManager.FontSize;
        Color fontcolor = eSDialogueManager.FontColor;
      
        Vector2 addsize = eSDialogueManager.addsize;
        Texture2D des_act_texture = eSDialogueManager.des_act_texture;
        Texture2D des_bck_texture = eSDialogueManager.des_bck_texture;
        Texture2D stmt_bck_texture = eSDialogueManager.Stmt_bck_texture;
        Texture2D panel_texture = eSDialogueManager.panel_texture;
        bool enable_scroll = eSDialogueManager.Enablr_Scroll;
      
        //ver1.1
        ESDialogueManager.TriggerEvent triggerEvent = eSDialogueManager.triggerEvent;
        KeyCode TriggerKey = eSDialogueManager.TriggerKey;
        //end

        //
        if (eSDialogueManager.DialogueGraph != null)
        {
             selectedindex = eSDialogueManager.GetCurrentLayerIndex();
             CurrentMessageIndex = eSDialogueManager.GetCurrentMessageIndex();
            //int clicked_des_index = eSDialogueManager.GetCurrentDecision();
            if (selectedindex > eSDialogueManager.DialogueGraph.statementLayers.Count - 1)
            {
               eSDialogueManager.SetCurrentLayerIndex(eSDialogueManager.DialogueGraph.statementLayers.Count - 1);
                selectedindex = eSDialogueManager.GetCurrentLayerIndex();
            }

           
            if (!eSDialogueManager.DialogueGraph.statementLayers[selectedindex].Active)
            {
               
                EditorGUILayout.HelpBox("Seleted Layer with ID NO : " +"("+ eSDialogueManager.DialogueGraph.statementLayers[selectedindex].ID +")"+ " is inactive please go to layer editor window and activate the layer :)", MessageType.Info);
            }
            if (eSDialogueManager.DialogueGraph.statementLayers[selectedindex].Active)
            {
                Array.Resize(ref eSDialogueManager.listme, eSDialogueManager.DialogueGraph.statementLayers.Count);
                selectedindex = EditorGUILayout.Popup(selectedindex, eSDialogueManager.listme, EditorStyles.miniPullDown);
                eSDialogueManager.SetCurrentLayerIndex(selectedindex);
                selectedindex = eSDialogueManager.GetCurrentLayerIndex();
                for (int i = 0; i < eSDialogueManager.DialogueGraph.statementLayers.Count; i++)
                {
                    // Debug.Log(eSDialogueManager.selectedindex);
                    if (eSDialogueManager.DialogueGraph.statementLayers[i].Active)
                        eSDialogueManager.listme[i] = eSDialogueManager.DialogueGraph.statementLayers[i].layerName + "___" + (i).ToString();
                }
                //
              
                GUILayout.Space(5.5f);
                //
                EditorGUILayout.BeginVertical("Box");
                //
                triggerType = (ESDialogueManager.TriggerType)EditorGUILayout.EnumPopup("TriggerType", eSDialogueManager.triggerType);
                //
                wrapMode = (ESDialogueManager.WrapMode)EditorGUILayout.EnumPopup("WrapMode", eSDialogueManager.wrapMode);
                if(triggerType == ESDialogueManager.TriggerType.Collider)
                {
                    Tagname = EditorGUILayout.TextField("TagName", eSDialogueManager.ObjectTagName);
                }
                //
                _3d_audio = EditorGUILayout.Toggle("3D Audio", eSDialogueManager.Use_3D_Audio);
                //
                usedisplaywidth = EditorGUILayout.Toggle("UseDisplayWidth", eSDialogueManager.ScaleWithDisplayWidth);
                //
                GUILayout.Space(10f);
                GUILayout.Label("Display Panel Settings", EditorStyles.boldLabel);
                GUILayout.Space(5.5f);
                //
                panelAnchor = (ESDialogueManager.PanelAnchor)EditorGUILayout.EnumPopup("Anchor", eSDialogueManager.panelAnchor);
                visible = EditorGUILayout.Toggle("Visible", eSDialogueManager.Visible);
                freezetime = EditorGUILayout.Toggle("FreezeTime", eSDialogueManager.FreezeTime);
                panelsize = EditorGUILayout.Vector2Field("Size", eSDialogueManager.DialogueGraph.statementLayers[selectedindex].panelsize);

                GUILayout.Space(10.0f);
                GUILayout.Label("statement position", EditorStyles.miniBoldLabel);
                stmt_posx = EditorGUILayout.FloatField("posx", eSDialogueManager.stmt_posx);
                stmt_posy = EditorGUILayout.FloatField("Posy", eSDialogueManager.stmt_posy);

                //
                GUILayout.Space(6.5f);
                GUILayout.Label("Panel Editor", EditorStyles.boldLabel);
                GUILayout.Space(6.5f);
                enable_scroll = EditorGUILayout.Toggle("EnableScrollBar", eSDialogueManager.DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].enable_scroll);
                if (enable_scroll)
                {
                    bordersize = EditorGUILayout.FloatField("BorderSize", eSDialogueManager.BorderSize);
                    center = EditorGUILayout.FloatField("Center", eSDialogueManager.Center);
                    GUILayout.Space(5.5f);
                    Spacing = EditorGUILayout.FloatField("Spacing", eSDialogueManager.DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].des_spacing);

                    GUILayout.Space(5.5f);
                    GUILayout.Label("Vertical", EditorStyles.miniBoldLabel);
                    scroll_v_val = EditorGUILayout.Slider("ScrollValue :", eSDialogueManager.scroll_V_value, 0.0f, eSDialogueManager.scrollrect.height);
                    GUILayout.Label("Horizontal", EditorStyles.miniBoldLabel);
                    scroll_h_val = EditorGUILayout.Slider("ScrollValue :", eSDialogueManager.scroll_H_value, 0.0f, eSDialogueManager.scrollrect.width);
                    //

                    //
                }
                GUILayout.Space(5.8f);
                EditorGUILayout.HelpBox("posistion fields can be used to edit position of desicion boxes, if any exist", MessageType.Info, EditorStyles.miniBoldFont);
                EditorGUILayout.BeginHorizontal();

                posx = EditorGUILayout.FloatField("PosX", eSDialogueManager.posx);
                GUILayout.Space(5);
                posy = EditorGUILayout.FloatField("PosY", eSDialogueManager.posy);
                EditorGUILayout.EndHorizontal();
                GUILayout.Space(7.5f);
                GUILayout.Label("Add size to decision boxes", EditorStyles.miniLabel);
                addsize = EditorGUILayout.Vector2Field("Decision Size", eSDialogueManager.addsize);
                GUILayout.Space(7.8f);
                GUILayout.Label("Move to next statement", EditorStyles.boldLabel);
                EditorGUILayout.HelpBox("Editor only", MessageType.Info, EditorStyles.miniBoldFont);

                if (Application.isPlaying == false)
                {
                    //
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("Next Statement", GUILayout.Width(110), GUILayout.Height(20)))
                    {
                        // move to next editor only
                        if (Application.isPlaying == false)
                        {
                            if (eSDialogueManager.DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].Isdecision == false)
                            {
                                if (eSDialogueManager.DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].IsconnectedOut)
                                {
                                    int temp_index = eSDialogueManager.DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].ConnectedIndexIn;
                                    eSDialogueManager.SetCurrentMessageIndex(temp_index);
                                    CurrentMessageIndex = eSDialogueManager.GetCurrentMessageIndex();
                                   
                                }
                            }
                            else
                            {
                                List<ESDialgoueScriptableObj.StatementLayer.Statements.Decision> decisions = eSDialogueManager.DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].decisions;
                               
                                if (eSDialogueManager.clicked_des_index < decisions.Count - 1)
                                {
                                    eSDialogueManager.clicked_des_index++;
                                }
                               
                            }
                            stmt_posx = eSDialogueManager.DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].stmt_posx;
                            stmt_posy = eSDialogueManager.DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].stmt_posy;
                            enable_scroll = eSDialogueManager.DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].enable_scroll;

                            Spacing = eSDialogueManager.DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].des_spacing;
                        }
                     //   Debug.Log(eSDialogueManager.move);
                    }
                    //
                    GUILayout.Space(5.5f);
                    if (eSDialogueManager.DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].Isdecision)
                    {
                        if (GUILayout.Button("Enter", GUILayout.Width(65), GUILayout.Height(20)))
                        {
                            List<ESDialgoueScriptableObj.StatementLayer.Statements.Decision> decisions = eSDialogueManager.DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].decisions;
                            if (decisions[eSDialogueManager.clicked_des_index].IsconnectedOut)
                            {
                                int temp_index = decisions[eSDialogueManager.clicked_des_index].ConnectedIndexIn;
                                eSDialogueManager.SetCurrentMessageIndex(temp_index);
                               CurrentMessageIndex =  eSDialogueManager.GetCurrentMessageIndex();
                       //         Debug.Log(eSDialogueManager.CurrentMessageIndex);
                            }
                        }
                        GUILayout.Space(5.5f);
                        if (GUILayout.Button("Back", GUILayout.Width(65), GUILayout.Height(20)))
                        {
                            if (eSDialogueManager.DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].IsconnectedIn)
                            {
                                bool connected_to_des = eSDialogueManager.DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].connectedtodecision;
                                int connecteindexdout = eSDialogueManager.DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].ConnectedIndexOut;
                                int connectedindexout_des = eSDialogueManager.DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].desparentindex;
                                int temp_index = connected_to_des == false ? connecteindexdout : connectedindexout_des;

                                eSDialogueManager.SetCurrentMessageIndex(temp_index);
                                CurrentMessageIndex = eSDialogueManager.GetCurrentMessageIndex();
                                stmt_posx = eSDialogueManager.DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].stmt_posx;
                                stmt_posy = eSDialogueManager.DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].stmt_posy;

                                enable_scroll = eSDialogueManager.DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].enable_scroll;

                                Spacing = eSDialogueManager.DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].des_spacing;
                            }
                        }
                    }
                   
                    //
                    GUILayout.Space(5.5f);
                    if (GUILayout.Button("Previous Statement", GUILayout.Width(130), GUILayout.Height(20)))
                    {
                        // move to next editor only
                        if (Application.isPlaying == false)
                        {
                            if (eSDialogueManager.DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].Isdecision == false)
                            {
                                if (eSDialogueManager.DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].IsconnectedIn)
                                {
                                    bool connected_to_des = eSDialogueManager.DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].connectedtodecision;
                                    int connecteindexdout = eSDialogueManager.DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].ConnectedIndexOut;
                                    int connectedindexout_des = eSDialogueManager.DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].desparentindex;
                                    int temp_index = connected_to_des == false ? connecteindexdout : connectedindexout_des;

                                    eSDialogueManager.SetCurrentMessageIndex(temp_index);
                                    CurrentMessageIndex =  eSDialogueManager.GetCurrentMessageIndex();
                                   
                                }
                            }
                            else
                            {
                                List<ESDialgoueScriptableObj.StatementLayer.Statements.Decision> decisions = eSDialogueManager.DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].decisions;

                                if (eSDialogueManager.clicked_des_index > 0)
                                {
                                    eSDialogueManager.clicked_des_index--;
                                }
                            }
                            stmt_posx = eSDialogueManager.DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].stmt_posx;
                            stmt_posy = eSDialogueManager.DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].stmt_posy;

                            enable_scroll = eSDialogueManager.DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].enable_scroll;

                            Spacing = eSDialogueManager.DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].des_spacing;
                        }
                        //Debug.Log(eSDialogueManager.move);
                    }
                    //
                   
                    //
                    EditorGUILayout.EndHorizontal();
                    GUILayout.Space(10.5f);
                    GUILayout.Label("Decision Texture", EditorStyles.boldLabel);
                    EditorGUILayout.BeginHorizontal();

                    des_act_texture = EditorGUILayout.ObjectField("Active Texture", eSDialogueManager.des_act_texture, typeof(Texture2D), true) as Texture2D;
                    des_bck_texture = EditorGUILayout.ObjectField("Normal Texture", eSDialogueManager.des_bck_texture, typeof(Texture2D), true) as Texture2D;

                    EditorGUILayout.EndHorizontal();
                    GUILayout.Space(15.5f);
                    GUILayout.Label("Statement Texture", EditorStyles.boldLabel);
                    stmt_bck_texture = EditorGUILayout.ObjectField("Normal Texture", eSDialogueManager.Stmt_bck_texture, typeof(Texture2D), true) as Texture2D;
                    GUILayout.Label("Panel Texture", EditorStyles.boldLabel);
                    panel_texture = EditorGUILayout.ObjectField("Panel Texture", eSDialogueManager.panel_texture, typeof(Texture2D), true) as Texture2D;
                    GUILayout.Label("Font Settings", EditorStyles.boldLabel);
                    fontsize = EditorGUILayout.IntField("FontSize", eSDialogueManager.FontSize);
                    fontcolor = EditorGUILayout.ColorField("FontColor", eSDialogueManager.FontColor);
                    
                    if (triggerType == ESDialogueManager.TriggerType.Event)
                    {
                        GUILayout.Space(10.8f);
                        GUI.color = Color.gray;
                        //
                        
                        if (eSDialogueManager.DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].Isdecision)
                        {
                            if (eSDialogueManager.DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].BySelection == false && eSDialogueManager.DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].closemsgbox == false)
                            {
                                EditorGUILayout.BeginVertical("Box");
                                GUILayout.Label("EDS has notice that this is a decision layer, would you like to trigger events by selection", EditorStyles.miniBoldLabel, GUILayout.Width(650), GUILayout.Height(35));
                                EditorGUILayout.BeginHorizontal();
                                GUILayout.Space(30.5f);
                                if (GUILayout.Button("Yes", GUILayout.Width(105), GUILayout.Height(15)))
                                {
                                    //add
                                    Undo.RegisterCompleteObjectUndo(eSDialogueManager, "EDS's shit talk");
                                    eSDialogueManager.DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].BySelection = true;
                                    eSDialogueManager.IsSelection = true;
                                    eSDialogueManager.DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].closemsgbox = true;
                                }
                                GUILayout.Space(30.5f);
                                if (GUILayout.Button("No", GUILayout.Width(105), GUILayout.Height(15)))
                                {
                                    //add
                                    Undo.RegisterCompleteObjectUndo(eSDialogueManager, "EDS's shit talk");
                                    eSDialogueManager.DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].BySelection = false;
                                    eSDialogueManager.IsSelection = false;
                                    eSDialogueManager.DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].closemsgbox = true;
                                }
                                EditorGUILayout.EndHorizontal();
                                EditorGUILayout.EndVertical();
                            }
                        }
                       
                       
                        //
                        EditorGUILayout.BeginVertical("Box");
                        GUILayout.Label("Event", EditorStyles.boldLabel);

                        //ver1.1
                        triggerEvent = (ESDialogueManager.TriggerEvent)EditorGUILayout.EnumPopup("TriggerEvent", eSDialogueManager.triggerEvent);
                        GUILayout.Space(5.8f);
                        if (triggerEvent == ESDialogueManager.TriggerEvent.OnClick)
                        {
                            TriggerKey = (KeyCode)EditorGUILayout.EnumPopup("TriggerKey", eSDialogueManager.TriggerKey);
                        }
                        GUILayout.Space(10.5f);
                       
                        GUILayout.Space(5.5f);
                        GUILayout.Label("Add Event()", EditorStyles.miniBoldLabel);
                        GUILayout.Space(2.8f);
                         EditorGUILayout.BeginHorizontal("Box");
                        if (GUILayout.Button("+", GUILayout.Width(55), GUILayout.Height(15)))
                        {
                            //add
                            Undo.RegisterCompleteObjectUndo(eSDialogueManager, "+");
                            ESDialogueManager.EventClass eventClass = new ESDialogueManager.EventClass();
                            eventClass.SelectedEventIndex = 3;
                            eSDialogueManager.eventClasses.Add(eventClass);
                        }
                        GUILayout.Space(50.8f);
                        if (GUILayout.Button("-", GUILayout.Width(55), GUILayout.Height(15)))
                        {
                            //remove
                            Undo.RegisterCompleteObjectUndo(eSDialogueManager, "-");
                            if (eSDialogueManager.eventClasses.Count >= 1)
                            {
                                eSDialogueManager.eventClasses.RemoveAt(eSDialogueManager.eventClasses.Count - 1);
                            }
                          
                        }
                        EditorGUILayout.EndHorizontal();
                        GUILayout.Space(7.8f);
                        //
                        Array.Resize(ref eSDialogueManager.s_eventclasses, eSDialogueManager.eventClasses.Count);
                        Array.Resize(ref tagobj, eSDialogueManager.eventClasses.Count);
                       
                        Array.Resize(ref tagname, eSDialogueManager.eventClasses.Count);
                        Array.Resize(ref sceneindex, eSDialogueManager.eventClasses.Count);
                        Array.Resize(ref scenename, eSDialogueManager.eventClasses.Count);
                        Array.Resize(ref cooldowntime, eSDialogueManager.eventClasses.Count);
                        Array.Resize(ref triggerid, eSDialogueManager.eventClasses.Count);
                        Array.Resize(ref loadSceneType, eSDialogueManager.eventClasses.Count);
                        Array.Resize(ref destroythis, eSDialogueManager.eventClasses.Count);
                        Array.Resize(ref LoadType, eSDialogueManager.eventClasses.Count);
                        Array.Resize(ref dialogueManagers, eSDialogueManager.eventClasses.Count);
                        Array.Resize(ref decisionid, eSDialogueManager.eventClasses.Count);
                        Array.Resize(ref repeatamount, eSDialogueManager.eventClasses.Count);
                        Array.Resize(ref repeattime, eSDialogueManager.eventClasses.Count);

                        //
                        //
                        if (eSDialogueManager.eventClasses != null)
                        {
                            if (eSDialogueManager.eventClasses.Count > 0)
                            {
                                for (int i = 0; i < eSDialogueManager.eventClasses.Count; ++i)
                                {
                                    
                                    GUILayout.Space(38.8f);
                                    EditorGUILayout.BeginHorizontal();
                                    
                                    GUI.color = Color.white;
                                    GUILayout.Label("IF TRIGGERED {", EditorStyles.miniBoldLabel);
                                    eSDialogueManager.eventClasses[i].SelectedEventIndex = EditorGUILayout.Popup(eSDialogueManager.eventClasses[i].SelectedEventIndex, eSDialogueManager.eventClasses[i].Events, EditorStyles.miniPullDown);
                                    GUILayout.Space(33.8f);
                                    if (eSDialogueManager.eventClasses[i].lastEventindex != eSDialogueManager.eventClasses[i].SelectedEventIndex)
                                    {
                                        eSDialogueManager.eventClasses[i].SpawnPoints.Clear();
                                        eSDialogueManager.eventClasses[i].myobjects.Clear();
                                        eSDialogueManager.eventClasses[i].active.Clear();
                                       
                                        eSDialogueManager.eventClasses[i].lastEventindex = eSDialogueManager.eventClasses[i].SelectedEventIndex;
                                    }
                                    GUILayout.Label("} THEN", EditorStyles.miniBoldLabel);

                                    EditorGUILayout.EndHorizontal();
                                    GUILayout.Space(13.8f);
                                    //
                                    if (eSDialogueManager.DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].BySelection)
                                    {
                                         decisionid[i] = EditorGUILayout.TextField("SlectedDecisionID", eSDialogueManager.eventClasses[i].SelectedDecisionIdNo);
                                    }
                                    GUILayout.Space(5.8f);
                                    if (eSDialogueManager.eventClasses[i].SelectedEventIndex == 0 || eSDialogueManager.eventClasses[i].SelectedEventIndex == 1 || eSDialogueManager.eventClasses[i].SelectedEventIndex == 2)
                                    {
                                        if (eSDialogueManager.eventClasses[i].SelectedEventIndex == 1)
                                        {
                                        
                                            repeatamount[i] = EditorGUILayout.IntField("RepeatAmount", eSDialogueManager.eventClasses[i].RepeatAmount);
                                            if (repeatamount[i] > 0)
                                            {
                                                repeattime[i] = EditorGUILayout.FloatField("Repaetime", eSDialogueManager.eventClasses[i].RepeatTime);
                                            }
                                        }
                                        //
                                        GUILayout.Space(5.3f);
                                        //
                                        EditorGUILayout.BeginHorizontal("Box");
                                        if (GUILayout.Button("+", GUILayout.Width(30), GUILayout.Height(15)))
                                        {
                                            //add
                                            Undo.RegisterCompleteObjectUndo(eSDialogueManager, "+ regarding eventobj");
                                            GameObject obj = null;
                                            if (eSDialogueManager.eventClasses[i].SelectedEventIndex == 2)
                                            {
                                                bool mybool = new bool();
                                                eSDialogueManager.eventClasses[i].active.Add(mybool);
                                            }
                                            eSDialogueManager.eventClasses[i].myobjects.Add(obj);

                                            //
                                        }
                                        //Array.Resize(ref eSDialogueManager.s_eventclasses[i].eventobj, eSDialogueManager.eventClasses.Count);
                                        //
                                        if (GUILayout.Button("-", GUILayout.Width(30), GUILayout.Height(15)))
                                        {
                                            //remove
                                            Undo.RegisterCompleteObjectUndo(eSDialogueManager, "- regarding eventobj");
                                            if (eSDialogueManager.eventClasses[i].myobjects.Count >= 1)
                                            {
                                                eSDialogueManager.eventClasses[i].myobjects.RemoveAt(eSDialogueManager.eventClasses[i].myobjects.Count - 1);
                                            }
                                            //
                                        }
                                      
                                        Array.Resize(ref eSDialogueManager.s_eventclasses[i].eventobj, eSDialogueManager.eventClasses[i].myobjects.Count);
                                        Array.Resize(ref eSDialogueManager.s_eventclasses[i].active, eSDialogueManager.eventClasses[i].active.Count);

                                        GUILayout.Space(15f);
                                        GUILayout.Label("Add/Remove Gameobject", EditorStyles.miniBoldLabel);
                                      
                                        EditorGUILayout.EndHorizontal();
                                       
                                        //
                                        GUILayout.Space(25f);
                                        //draw event objects 
                                      
                                        for (int j = 0; j < eSDialogueManager.eventClasses[i].myobjects.Count; ++j)
                                        {
                                            if (eSDialogueManager.eventClasses[i].SelectedEventIndex == 2)
                                            {
                                              
                                            }
                                            EditorGUILayout.BeginHorizontal();
                                            GUILayout.Label("Gameobject :", EditorStyles.boldLabel);
                                            GUILayout.Space(10.4f);
                                            eSDialogueManager.s_eventclasses[i].eventobj[j] = (GameObject)EditorGUILayout.ObjectField("", eSDialogueManager.eventClasses[i].myobjects[j], typeof(GameObject), true);
                                            if (eSDialogueManager.eventClasses[i].SelectedEventIndex == 2)
                                            {
                                                GUILayout.Space(60.4f);
                                                eSDialogueManager.s_eventclasses[i].active[j] = EditorGUILayout.Toggle("", eSDialogueManager.eventClasses[i].active[j]);
                                                
                                            }
                                            EditorGUILayout.EndHorizontal();
                                            GUILayout.Space(15.4f);
                                        }
                                        //
                                    }
                                    //
                                    if (eSDialogueManager.eventClasses[i].SelectedEventIndex == 0)
                                    {
                                        destroythis[i] = EditorGUILayout.Toggle("DestroyThis()", eSDialogueManager.eventClasses[i].destroythis);
                                    }
                                    //
                                    if (eSDialogueManager.eventClasses[i].SelectedEventIndex == 1)
                                    {
                                      
                                        GUILayout.Space(15f);
                                        EditorGUILayout.BeginHorizontal("Box");
                                        if (GUILayout.Button("+", GUILayout.Width(30), GUILayout.Height(15)))
                                        {
                                            //add
                                            Undo.RegisterCompleteObjectUndo(eSDialogueManager, "+ regarding spawnpoints");
                                            Transform spawnpoint = null;
                                            eSDialogueManager.eventClasses[i].SpawnPoints.Add(spawnpoint);
                                            
                                            //
                                        }
                                        //
                                        if (GUILayout.Button("-", GUILayout.Width(30), GUILayout.Height(15)))
                                        {
                                            //remove
                                            Undo.RegisterCompleteObjectUndo(eSDialogueManager, "- regarding spawnpoints");
                                            if (eSDialogueManager.eventClasses[i].SpawnPoints.Count >= 1)
                                            {
                                                eSDialogueManager.eventClasses[i].SpawnPoints.RemoveAt(eSDialogueManager.eventClasses[i].SpawnPoints.Count - 1);
                                            }
                                            //
                                        }
                                        GUILayout.Space(15f);
                                        GUILayout.Label("Add/Remove SpawnPoints", EditorStyles.miniBoldLabel);
                                        EditorGUILayout.EndHorizontal();
                                       
                                        Array.Resize(ref eSDialogueManager.s_eventclasses[i].spawnpoints, eSDialogueManager.eventClasses[i].SpawnPoints.Count);
                                       
                                        //
                                        if (eSDialogueManager.eventClasses[i].SpawnPoints.Count > 0)
                                        {
                                            GUILayout.Space(25f);
                                            for (int j = 0; j < eSDialogueManager.eventClasses[i].SpawnPoints.Count; ++j)
                                            {
                                                eSDialogueManager.s_eventclasses[i].spawnpoints[j] = (Transform)EditorGUILayout.ObjectField("SpawnPoints", eSDialogueManager.eventClasses[i].SpawnPoints[j], typeof(Transform), true);
                                            }
                                           
                                        }
                                        
                                    }
                                    //
                                 
                                   //
                                    if (eSDialogueManager.eventClasses[i].SelectedEventIndex == 3 || eSDialogueManager.eventClasses[i].SelectedEventIndex == 4)
                                    {
                                        if (eSDialogueManager.eventClasses[i].SelectedEventIndex == 3)
                                        {
                                            loadSceneType[i] = (ESDialogueManager.EventClass.LoadSceneType)EditorGUILayout.EnumPopup("LoadBY", eSDialogueManager.eventClasses[i].SceneType);
                                            //
                                            LoadType[i] = (ESDialogueManager.EventClass.SceneLoadType)EditorGUILayout.EnumPopup("SceneLoadType", eSDialogueManager.eventClasses[i].LoadType);
                                        }
                                           
                                        GUILayout.Space(2.8f);
                                        if (loadSceneType[i] == ESDialogueManager.EventClass.LoadSceneType.ByIndex)
                                        {
                                            sceneindex[i] = EditorGUILayout.IntField("SceneIndex", eSDialogueManager.eventClasses[i].Sceneindex);
                                        }
                                        else
                                        {
                                            if(eSDialogueManager.eventClasses[i].SelectedEventIndex == 3)
                                            scenename[i] = EditorGUILayout.TextField("SceneName", eSDialogueManager.eventClasses[i].SceneName);
                                        }
                                    }
                                    //
                                    if (eSDialogueManager.eventClasses[i].SelectedEventIndex == 5)
                                    {
                                        GUILayout.Label("Application.Quit()", EditorStyles.miniBoldLabel);
                                    }
                                    //
                                    if (eSDialogueManager.eventClasses[i].SelectedEventIndex == 6)
                                    {
                                        dialogueManagers[i] = (ESDialogueManager)EditorGUILayout.ObjectField("DialogueManager", eSDialogueManager.eventClasses[i].DialogueManager, typeof(ESDialogueManager), true);
                                        triggerid[i] = EditorGUILayout.IntField("LayerID", eSDialogueManager.eventClasses[i].LayerId);
                                        cooldowntime[i] = EditorGUILayout.FloatField("CoolDownTime", eSDialogueManager.eventClasses[i].CoolDownTime);
                                    }
                                    //
                                    if (eSDialogueManager.eventClasses[i].SelectedEventIndex == 7)
                                    {
                                       
                                        tagobj[i] = (GameObject)EditorGUILayout.ObjectField("GameObject", eSDialogueManager.eventClasses[i].tagobject, typeof(GameObject), true);
                                        tagname[i] = EditorGUILayout.TagField("TagList",eSDialogueManager.eventClasses[i].TagName);
                                    }
                                    GUILayout.Space(13.8f);
                                    GUILayout.Label("End", EditorStyles.miniBoldLabel);
                                    if (GUILayout.Button("", GUILayout.Width(Screen.width), GUILayout.Height(5)))
                                    {
                                        //line
                                    }
                                    GUILayout.Space(8.8f);
                                }
                            }
                        }
                       
                        GUILayout.Space(7.8f);
                        EditorGUILayout.EndVertical();
                    }
                    else
                    {
                        GUILayout.Space(7.8f);
                    }
                }

                EditorGUILayout.EndVertical();
                if (GUI.changed)
                {
                    EditorUtility.SetDirty(eSDialogueManager);
                }
            }

        }
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RegisterFullObjectHierarchyUndo(eSDialogueManager, "Changes");
            eSDialogueManager.DialogueGraph = eSDialgoueScriptable;
            if (eSDialogueManager.DialogueGraph != null)
            {
                eSDialogueManager.triggerType = triggerType;
                eSDialogueManager.wrapMode = wrapMode;
                eSDialogueManager.panelAnchor = panelAnchor;
                eSDialogueManager.Visible = visible;
                eSDialogueManager.PanelSize = panelsize;
                eSDialogueManager.BorderSize = bordersize;
                eSDialogueManager.scroll_V_value = scroll_v_val;
                eSDialogueManager.posx = posx;
                eSDialogueManager.posy = posy;
                eSDialogueManager.Center = center;
                eSDialogueManager.spacing = Spacing;
                eSDialogueManager.scroll_H_value = scroll_h_val;
                eSDialogueManager.stmt_posx = stmt_posx;
                eSDialogueManager.Enablr_Scroll = enable_scroll;
                eSDialogueManager.stmt_posy = stmt_posy;
                eSDialogueManager.Stmt_bck_texture = stmt_bck_texture;
                eSDialogueManager.des_act_texture = des_act_texture;
                eSDialogueManager.des_bck_texture = des_bck_texture;
                eSDialogueManager.panel_texture = panel_texture;
                eSDialogueManager.addsize = addsize;
                eSDialogueManager.ObjectTagName = Tagname;
                eSDialogueManager.FontSize = fontsize;
                eSDialogueManager.FontColor = fontcolor;
                eSDialogueManager.FreezeTime = freezetime;
                eSDialogueManager.ScaleWithDisplayWidth = usedisplaywidth;
                eSDialogueManager.Use_3D_Audio = _3d_audio;
                //
                //ver1.1
                eSDialogueManager.triggerEvent = triggerEvent;
                eSDialogueManager.TriggerKey = TriggerKey;

                //
                if(eSDialogueManager.eventClasses != null)
                {

                    if (eSDialogueManager.eventClasses.Count > 0)
                    {
                        for (int i = 0; i < eSDialogueManager.eventClasses.Count; ++i)
                        {
                           
                            
                            eSDialogueManager.eventClasses[i].SceneType = loadSceneType[i];
                            eSDialogueManager.eventClasses[i].SceneName = scenename[i];
                            eSDialogueManager.eventClasses[i].Sceneindex = sceneindex[i];
                            eSDialogueManager.eventClasses[i].CoolDownTime = cooldowntime[i];
                            eSDialogueManager.eventClasses[i].LayerId = triggerid[i];
                            eSDialogueManager.eventClasses[i].tagobject = tagobj[i];
                            eSDialogueManager.eventClasses[i].TagName = tagname[i];
                            eSDialogueManager.eventClasses[i].destroythis = destroythis[i];
                            eSDialogueManager.eventClasses[i].LoadType = LoadType[i];
                            eSDialogueManager.eventClasses[i].DialogueManager = dialogueManagers[i];
                            eSDialogueManager.eventClasses[i].SelectedDecisionIdNo = decisionid[i];
                            eSDialogueManager.eventClasses[i].RepeatAmount = repeatamount[i];
                            eSDialogueManager.eventClasses[i].RepeatTime = repeattime[i];
                            //
                            if (eSDialogueManager.eventClasses[i].myobjects.Count > 0)
                            {
                                for (int j = 0; j < eSDialogueManager.eventClasses[i].myobjects.Count; ++j)
                                {
                                    eSDialogueManager.eventClasses[i].myobjects[j] = eSDialogueManager.s_eventclasses[i].eventobj[j];
                                }
                            }
                            //
                            if (eSDialogueManager.eventClasses[i].active.Count > 0)
                            {
                                for (int j = 0; j < eSDialogueManager.eventClasses[i].active.Count; ++j)
                                {
                                    eSDialogueManager.eventClasses[i].active[j] = eSDialogueManager.s_eventclasses[i].active[j];
                                }
                            }
                            //
                            if (eSDialogueManager.eventClasses[i].SpawnPoints.Count > 0 && eSDialogueManager.s_eventclasses[i].spawnpoints.Length > 0)
                            {
                                for (int j = 0; j < eSDialogueManager.eventClasses[i].SpawnPoints.Count; ++j)
                                {
                                    eSDialogueManager.eventClasses[i].SpawnPoints[j] = eSDialogueManager.s_eventclasses[i].spawnpoints[j];
                                }
                            }
                           
                        }
                    }
                }
              
                //end
            }
        }
    }
}
