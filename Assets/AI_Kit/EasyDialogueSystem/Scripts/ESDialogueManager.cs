using System;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

[ExecuteAlways]
public  class ESDialogueManager : MonoBehaviour
{
    //verson 1.1
    //events
    [Serializable]
    public class shadoweventclass
    {
        public Transform[] spawnpoints;
        public  GameObject[] eventobj;
        public  bool[] active;
    }
    public shadoweventclass[] s_eventclasses;
    [System.Serializable]
    public class EventClass
    {
        public readonly string[] Events = { "Destroy Object", "Instantiate Object", "SetActive", "LoadScene", "UnLoadScene", "Quit","TriggerOtherLayer","ChangeGameobjectTag"};
        public GameObject tagobject;
        public string SceneName;
        public int Sceneindex;
        public int LayerId;
        public float RepeatTime = 0.0f;
        public int RepeatAmount = 4;
        public bool HasPlayed = false; 
        public ESDialogueManager DialogueManager;
        public List<Transform> SpawnPoints = new List<Transform>(1);
        public List<GameObject> myobjects = new List<GameObject>(1);
        public string SelectedDecisionIdNo;
        public enum SceneLoadType
        {
            Addictive,
            Single
        }
        public SceneLoadType LoadType = SceneLoadType.Single;
        public float CoolDownTime = 0.3f;
        public string TagName = "Player";
        public bool destroythis;
        public int SelectedDecisionIndex;
        public int lastEventindex;
        public List<bool> active = new List<bool>(1);
        //
        public enum LoadSceneType
        {
            ByIndex,
            ByName
        }
        public LoadSceneType SceneType = LoadSceneType.ByIndex;
        //
        public int SelectedEventIndex = 0;
    }
    public List<EventClass> eventClasses = new List<EventClass>(1);
    
    public enum TriggerEvent
    {
        OnAwake,
        OnClick,
        Manual
    }
    //
    public TriggerEvent triggerEvent = TriggerEvent.OnAwake;
    public KeyCode TriggerKey = KeyCode.Escape;
    
    //end version 1.1
    public ESDialgoueScriptableObj DialogueGraph;
    public enum TriggerType
    {
        Collider,
        Event
    }
    public TriggerType triggerType = TriggerType.Collider;
    public string ObjectTagName = "UnTagged";
    public string[] listme;
    
    
    // display panel settings
    public enum PanelAnchor
    {
        Top,
        Center,
        Down
    }
    //
    public enum WrapMode
    {
        Loop,
        Once
    }
    //
    public PanelAnchor panelAnchor = PanelAnchor.Down;
    public WrapMode wrapMode = WrapMode.Once;
    public bool Visible = true;
    //public Texture2D PanelTexture;
    public Vector2 Temp_PanelSize = new Vector2(256,256);
    public Vector2 PanelSize;
    
    public float BorderSize = 50f;
    public float Center = 0f;
    public float scroll_V_value;
    public float scroll_H_value;
    public Rect scrollrect;
    public float T_spacing = 50.0f;
    public float spacing;
    //end panel settings
    //desicion box
    public float posx;
    public float posy = 50.0f;
    public float stmt_posx = 0.0f;
    public float stmt_posy = 0.0f;
    public Vector2 des_position = Vector2.zero;
    public bool move;
    public bool T_Enablr_Scroll = true;
    public bool Enablr_Scroll;
    public Texture2D Stmt_bck_texture;
    public Texture2D des_bck_texture;
    public Texture2D des_act_texture;
    public Texture2D panel_texture;
    public float maxwidth;
    public Vector2 addsize;
    public bool IsPlaying;
    public bool FreezeTime;
    public int FontSize = 17;
    public Color FontColor = new Vector4(0,0,0,255);
    public bool Use_3D_Audio = true;
    public bool HasPlayed = false;
    public bool ScaleWithDisplayWidth = false;
    public bool IsSelection;
    //end
    public int clicked_des_index;
  
    //
    [SerializeField] int CurrentMessageIndex = 0;
    [SerializeField] string DecisionID;
    [SerializeField] int selectedindex = 0;
    [SerializeField] int StmtLayerId;
    //
    Vector2 Button_scrollPosition = Vector2.zero;
    Rect boxrect;
    Vector2 texturesize = Vector2.zero;
    float overall_height;
    float total_width;
    float overall_width;
    int img_index;
    bool next = false;
    float totalheight = 0f;
    float maxheight = 0f;
    string val;
    Rect des_rect;
    int selected_Height_i;
    int selected_Width_i;
    GameObject activeobj;
    bool triggered = false;
    bool playonce = false;
    float mytime;
    float movetonext;
    bool stoproutine;
    float stoptime;
    bool isdecision;
    bool justplayed;
    bool reset;
    bool someplaying,some_not_playing;
    float timecounter;
    int repeatcounter;
    float timeval;
    [SerializeField]AudioSource source;
    [SerializeField]GameObject audio_engine;
    [SerializeField]int pre_stmt_index;
    float CoolDownTime;
    float temp_scale;
    public ESDialogueManager[] othermangers;
    [SerializeField] private ESManagerCtrl gameobj; 
  
    // Start is called before the first frame update
    private void Awake()
    {
       
        pre_stmt_index = 1;
        gameobj = GameObject.FindObjectOfType<ESManagerCtrl>();
        if (Application.isPlaying)
        {
            if (gameobj == null)
            {
                GameObject go = new GameObject("DialogueManagerController");
                go.AddComponent<ESManagerCtrl>();
            }
           
        }
       
        temp_scale = Time.timeScale;
        source = null;
        CurrentMessageIndex = 0;
       // selectedindex = 0;
       // pre_stmt_index = 0;
        audio_engine = GameObject.Find("EasyDialogueAudioEngine");
        if (Use_3D_Audio == false)
        {
            if (audio_engine == null)
            {
                Debug.Log("Please do not rename <EasyDialogueAudioEngine> gameobject");
                GameObject go = new GameObject("EasyDialogueAudioEngine");
                go.AddComponent<AudioSource>();

                audio_engine = go;
                source = audio_engine.GetComponent<AudioSource>();
            }
            else
            {
                source = audio_engine.GetComponent<AudioSource>();
            }
        }
        if (DialogueGraph != null)
        {
            StmtLayerId = DialogueGraph.statementLayers[selectedindex].ID;
          
            if (DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].audio == null)
            {
                if (CurrentMessageIndex == 0 && pre_stmt_index == 1)
                {
                    pre_stmt_index = 0;
                }
            }
        }

        //ver1.1
        if (triggerEvent == TriggerEvent.OnAwake && triggerType == TriggerType.Event)
        {
            CallEvents(GetStatementLayerID(),true);
        }
        //end
    }
    private void OnGUI()
    {
#if UNITY_EDITOR
        activeobj =  Selection.activeGameObject;
#endif
        
        if (DialogueGraph != null)
        {
            if (!Application.isPlaying)
            {
                if (this.gameObject == activeobj)
                {
                    if (othermangers.Length > 0)
                    {
                       
                        for (int i = 0; i < othermangers.Length; ++i)
                        {
                          
                            if (othermangers[i].DialogueGraph != null)
                            {

                                if (othermangers[i].gameObject != this.gameObject)
                                { 
                                    othermangers[i].CurrentMessageIndex = 0;
                                    if (othermangers[i].selectedindex == activeobj.GetComponent<ESDialogueManager>().selectedindex)
                                    {
                                       
                                       if (othermangers[i].DialogueGraph == this.DialogueGraph)
                                       {
                                           Debug.Log("WARNING : Duplicate Not allowed dialogue layer has already been used");
                                           activeobj.GetComponent<ESDialogueManager>().DialogueGraph.statementLayers[activeobj.GetComponent<ESDialogueManager>().selectedindex].panelsize = othermangers[i].Temp_PanelSize;
                                       }
                                       
                                    }
                                }
                            }
                        }
                    }
                    //
                    //CurrentMessageIndex = 0;
                    if(DialogueGraph.statementLayers.Count > 0)
                    {
                        if (DialogueGraph.statementLayers[selectedindex].Active)
                            DrawGUI();
                    }
                   
                }
            }
            if (Application.isPlaying)
            {
                if (DialogueGraph.statementLayers.Count > 0)
                {
                    if (DialogueGraph.statementLayers[selectedindex].isplaying == true)
                    {
                        if (DialogueGraph.statementLayers[selectedindex].Active)
                            DrawGUI();
                    }
                }
            }
        }
    }
    // Update is called once per frame
    private void Update()
    {
        //
        
        othermangers = GameObject.FindObjectsOfType<ESDialogueManager>();
         //
        //ver1.1
        if (triggerEvent == TriggerEvent.OnClick && triggerType == TriggerType.Event)
        {
            if (Input.GetKeyDown(TriggerKey))
            {
                CallEvents(GetStatementLayerID(), false);
            }

        }
        //end
        // //
        if (IsSelection == false)
        {
            
            if (justplayed == false && IsPlaying)
            {
                justplayed = true;

            }
            //
            if (justplayed == true && !IsPlaying)
            {
                PerformEvents();
            }
        }
        if (Application.isPlaying)
        {
            PlayDialogue();
        }
        if (IsSelection)
        {

            for (int i = 0; i < eventClasses.Count; ++i)
            {

                if (DecisionID == eventClasses[i].SelectedDecisionIdNo)
                {
                    print("dfffd");
                    //based on selection only;
                    PerformEvents();
                }
            }

        }
       
    }
    //
    private void OnDisable()
    {
        if (DialogueGraph != null)
        {
           if (DialogueGraph.statementLayers[selectedindex].Play == true)
           {
                DialogueGraph.statementLayers[selectedindex].Play = false;
           }
            DialogueGraph.statementLayers[selectedindex].isplaying = false;
            triggered = false;
            CurrentMessageIndex = 0;
        }
    }
    //
    private void PlayDialogue()
    {
        if (playonce == true) return;
        if (DialogueGraph != null)
        {
         
            if (DialogueGraph.statementLayers[selectedindex].Play == true)
            {
                DialogueGraph.statementLayers[selectedindex].isplaying = true;
                ResetEvents();

                DialogueGraph.statementLayers[selectedindex].Play = false;
               
            }
            if (DialogueGraph.statementLayers[selectedindex].reset)
            {
                CurrentMessageIndex = 0;
                clicked_des_index = 0;
                DialogueGraph.statementLayers[selectedindex].reset = false;
            }
            if (DialogueGraph.statementLayers[selectedindex].isplaying)
            {
                if (FreezeTime)
                {
                    Time.timeScale = 0;
                }
              
                //
                if (next)
                {
                    //code here
                    if (isdecision)
                    {
                        List<ESDialgoueScriptableObj.StatementLayer.Statements> statements = DialogueGraph.statementLayers[selectedindex].statements;

                       
                        if (statements[CurrentMessageIndex].decisions.Count > 0)
                        {
                            if (statements[CurrentMessageIndex].decisions[clicked_des_index].IsconnectedOut == true)
                            {
                                int temp_index = statements[CurrentMessageIndex].decisions[clicked_des_index].ConnectedIndexIn;
                                CurrentMessageIndex = temp_index;
                            }
                            DecisionID = statements[CurrentMessageIndex].decisions[clicked_des_index].ID;
                        }
                        //
                        if (statements[CurrentMessageIndex].decisions[clicked_des_index].IsconnectedOut == false)
                        {
                            Stop(0.0f);
                        }
                        isdecision = false;
                    }
                    if (DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].Click)
                    {
                        if (DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].IsconnectedOut == false)
                        {
                            //stop
                            Stop(0.0f);
                        }
                        if (DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].IsconnectedOut)
                        {
                            int temp_index = DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].ConnectedIndexIn;
                            CurrentMessageIndex = temp_index;
                        }
                    }
                  

                    next = false;
                }
                if (!DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].Click)
                {
                    if (DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].Isdecision == false)
                    {
                        //here
                        if (DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].IsconnectedOut)
                        {
                            movetonext = FreezeTime ? movetonext = DeltaTime(movetonext) : movetonext += Time.deltaTime;
                            if (movetonext >= DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].Timer)
                            {
                                int temp_index = DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].ConnectedIndexIn;
                                CurrentMessageIndex = temp_index;
                              
                                movetonext = 0;
                            }

                        }

                        //print(movetonext);
                        if (DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].IsconnectedOut == false)
                        {
                            movetonext = FreezeTime ? movetonext = DeltaTime(movetonext) : movetonext += Time.deltaTime;
                            if (movetonext >= DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].Timer)
                            {
                                //stop
                                Stop(0.0f);
                                movetonext = 0;
                            }
                        }
                    }
                }
                
            }
            if (IsPlaying)
            {
                if (DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].audio != null)
                {
                    if (IsPlaying)
                    {
                        if (Use_3D_Audio)
                        {
                            if (pre_stmt_index != CurrentMessageIndex)
                            {
                                if (source != null)
                                {
                                    if (source.isPlaying == true)
                                    {
                                        source.Stop();
                                        source.loop = false;
                                    }
                                    source.spatialBlend = 1;
                                }

                            }

                            audio_engine = GameObject.FindGameObjectWithTag(DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].CharacterTagName);
                            if (audio_engine != null)
                            {
                                //print(audio_engine.name);
                                if (audio_engine.GetComponent<AudioSource>() == null)
                                {
                                    // print(go.name);
                                    audio_engine.AddComponent<AudioSource>();
                                }
                                source = audio_engine.GetComponent<AudioSource>();
                                if (source.isPlaying == false)
                                {
                                    source.spatialBlend = 1;
                                }
                            }
                        }
                        source.clip = DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].audio;
                        if (pre_stmt_index != CurrentMessageIndex)
                        {
                            if (source != null)
                            {
                                if (source.isPlaying == false)
                                {
                                    source.Play();
                                    source.loop = false;
                                }
                            }
                            if (CurrentMessageIndex != 0)
                                pre_stmt_index = CurrentMessageIndex;
                            if (CurrentMessageIndex == 0 && pre_stmt_index == 1)
                                pre_stmt_index = 0;
                        }

                    }
                    else
                    {
                        if (source != null)
                        {
                            if (source.isPlaying == true)
                            {
                                print(CurrentMessageIndex);
                                source.Stop();
                            }
                        }
                    }
                }
                else
                {
                    if (source != null)
                    {
                        if (source.isPlaying == true)
                        {
                            print(CurrentMessageIndex);
                            source.Stop();
                        }
                    }
                }
            }
            //print(triggered);
            ResetTriggeredLayer();
            IsPlaying = DialogueGraph.statementLayers[selectedindex].isplaying;
        }
    }
    //
    public void Stop(float delaytime)
    {
        stoproutine = true;
       
        stoptime = !FreezeTime ? stoptime += Time.deltaTime : stoptime = DeltaTime(stoptime);
        if (stoptime > delaytime)
        {
            if (DialogueGraph != null)
            {
                DialogueGraph.statementLayers[selectedindex].isplaying = false;
                IsPlaying = false;
                if (source != null)
                {
                    if (source.isPlaying)
                    {
                        source.Stop();
                    }
                }
             
            }
            if (stoproutine)
            {
                CurrentMessageIndex = 0;
                if (FreezeTime)
                {
                    Time.timeScale = temp_scale;
                   // print(temp_scale);
                }
                //
                stoptime = 0.0f;
                stoproutine = false;
                if (wrapMode == WrapMode.Once)
                {
                    playonce = true;
                }
                else
                {
                    playonce = false;
                }
                HasPlayed = true;
                return;
            }
        }
        print(stoptime);
    }
    //
    private void DrawGUI()
    {
        //do reset
        //oh please dont try to understand this method if cant , else if you can go ahead ;)
        if (DialogueGraph.statementLayers[selectedindex].reset)
        {
            CurrentMessageIndex = 0;
            clicked_des_index = 0;
            DialogueGraph.statementLayers[selectedindex].reset = false;
            //Debug.Log(DialogueGraph.statementLayers[selectedindex].reset);
        }
        //draw panel
        GUIStyle guimessageboxstyle = new GUIStyle();
        guimessageboxstyle.normal.background = Stmt_bck_texture;
        guimessageboxstyle.border = new RectOffset(12, 12, 12, 12);
        //guimessageboxstyle.normal.textColor = FontColor;
        guimessageboxstyle.fontSize = FontSize;
        guimessageboxstyle.normal.textColor = FontColor;
        guimessageboxstyle.alignment = TextAnchor.MiddleCenter;
        guimessageboxstyle.clipping = GUI.skin.textField.clipping;
        guimessageboxstyle.overflow = GUI.skin.textArea.overflow;
        guimessageboxstyle.wordWrap = true;
        //
        Rect panel_rect = new Rect();
        Rect statement_rect = DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].Statement_rect;
        float button_space = 0f;
        float img_space = 0f;
        string s = DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].StatementContent;
        GUIContent gUIContent = new GUIContent(s);
        float height = guimessageboxstyle.CalcHeight(gUIContent, 500f);
        Vector2 size = guimessageboxstyle.CalcSize(gUIContent);
        //
        if (DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].Click)
        {
            Vector2 but_size = DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].Size;
            if (panelAnchor == PanelAnchor.Down || panelAnchor == PanelAnchor.Top)
            {
               button_space = but_size.y;        
            }

            if (DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].connetedtoimg)
            {
                img_space = button_space >= 0.1f ? texturesize.y - button_space : texturesize.y;
            }
        }
        //
        Temp_PanelSize = ScaleWithDisplayWidth == false ?
           DialogueGraph.statementLayers[selectedindex].panelsize : new Vector2(Display.main.renderingWidth,
           DialogueGraph.statementLayers[selectedindex].panelsize.y);
        //
        if (ScaleWithDisplayWidth == false)
        {
            float dist_panel_screen = Display.main.renderingWidth - Temp_PanelSize.x;
            Temp_PanelSize.x = dist_panel_screen < 0 ?
                Temp_PanelSize.x - Mathf.Abs(dist_panel_screen) : Temp_PanelSize.x;
        }
     
        //
        PanelSize.x = ScaleWithDisplayWidth ? Display.main.renderingWidth:PanelSize.x;
        //
        DialogueGraph.statementLayers[selectedindex].panelsize = PanelSize;
        
       
        switch (panelAnchor)
        {
            case PanelAnchor.Center:
                panel_rect = new Rect(Screen.width / 2 - Temp_PanelSize.x / 2, Screen.height / 2 - Temp_PanelSize.y/ 2, Temp_PanelSize.x, Temp_PanelSize.y);
               
                break;
            case PanelAnchor.Down:
               
                panel_rect = new Rect(Screen.width / 2 - Temp_PanelSize.x / 2, (Screen.height - Temp_PanelSize.y) - button_space, Temp_PanelSize.x, Temp_PanelSize.y);
                break;
            case PanelAnchor.Top:
                panel_rect = new Rect(Screen.width / 2 - Temp_PanelSize.x / 2, (0.0f) + button_space + Mathf.Abs(img_space), Temp_PanelSize.x, Temp_PanelSize.y);
                break;
        }
        //
        float tempx = DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].stmt_posx;
        float tempy = DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].stmt_posy;

        statement_rect = new Rect(boxrect.x + tempx, boxrect.y + tempy, size.x, size.y);

        DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].stmt_posx = stmt_posx;
        DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].stmt_posy = stmt_posy;

        if (DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].connetedtoimg)
        {
            Rect imgrect = new Rect();
            int currentimgindex = DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].ConnectedImgIndex;
            GUIStyle imgstyle = new GUIStyle();
            imgstyle.normal.background = DialogueGraph.statementLayers[selectedindex].characterPictures[currentimgindex].texture;
            Vector2 imgsize = DialogueGraph.statementLayers[selectedindex].characterPictures
                [DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].ConnectedImgIndex].Size;
            //
            texturesize = imgsize;
            img_index = currentimgindex;
            //
            if (DialogueGraph.statementLayers[selectedindex].characterPictures[currentimgindex].center)
            {
                //print((panel_rect.y - imgsize.y));
                float actualheight = Mathf.Abs((panel_rect.height * 0.5f) - imgsize.y);
                
                float checkheight = (panel_rect.height / 2) >= imgsize.y ? panel_rect.height - actualheight : panel_rect.height + actualheight; 
                imgrect = new Rect(panel_rect.center - new Vector2(imgsize.x/2, checkheight) , imgsize);
            }
            else if (DialogueGraph.statementLayers[selectedindex].characterPictures[currentimgindex].right)
            {
                 imgrect = new Rect(panel_rect.x + panel_rect.width - imgsize.x, panel_rect.y - imgsize.y, imgsize.x, imgsize.y);
            }
            else if (DialogueGraph.statementLayers[selectedindex].characterPictures[currentimgindex].left)
            {
              imgrect = new Rect(panel_rect.x, panel_rect.y - imgsize.y, imgsize.x, imgsize.y);
            }
             
            if (DialogueGraph.statementLayers[selectedindex].characterPictures[currentimgindex].texture != null)
            {
                GUI.Box(imgrect, "", imgstyle);
            }
            else
            {
                GUI.Box(imgrect, "");
            }
          
        }
        //
        if (DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].Click)
        {
            Rect but_rect = new Rect();
          
            Vector2 but_size = DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].Size;
            string but_text = DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].ButtonText;
            //***;
            GUIStyle but_style = new GUIStyle();
            but_style.border = GUI.skin.button.border;
            but_style.normal.background = DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].buttontexture;
            but_style.active.background = GUI.skin.button.active.background;
            
            if (DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].TopLeft)
            {
                but_rect = new Rect(panel_rect.x, panel_rect.y - but_size.y, but_size.x, but_size.y);
            }
            else if (DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].TopRight)
            {
                but_rect = new Rect(panel_rect.x + panel_rect.width - but_size.x, panel_rect.y - but_size.y, but_size.x, but_size.y);
            }
            else if (DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].DownLeft)
            {
                but_rect = new Rect(panel_rect.x, panel_rect.y + panel_rect.height, but_size.x, but_size.y);
            }
            else if (DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].DownRight)
            {
                but_rect = new Rect(panel_rect.x + panel_rect.width - but_size.x, panel_rect.height + panel_rect.y, but_size.x, but_size.y);
            }
            //
            if (DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].buttontexture == null)
            {
                if (GUI.Button(but_rect, but_text))
                {
                    //eat me ;}
                    next = true;
                  
                }
            }
            else
            {
                if (GUI.Button(but_rect, but_text,but_style))
                {
                    //eat me ;}
                    next = true;
               
                }
            }
            //
        }
        //
        GUIStyle panelstyle = new GUIStyle();
        panelstyle.border = GUI.skin.box.border;
        panelstyle.normal.background = panel_texture != null ? panel_texture : GUI.skin.box.normal.background;
        //eat me :"""()
        if (Visible)
        {
            GUI.Box(panel_rect, "", panelstyle);
        }
       
        boxrect = new Rect(panel_rect.x + BorderSize + Center, panel_rect.y + BorderSize, panel_rect.width - BorderSize * 2 - Center, panel_rect.height - BorderSize * 2);
        Rect veiwrect = new Rect(boxrect.x, boxrect.y, overall_width, overall_height);
        GUIStyle teststyle = new GUIStyle(); 
        teststyle.normal.background = Resources.Load("Textures/Box") as Texture2D;
        
        //
        //GUI.Box(veiwrect, "",teststyle);
        //
        T_spacing = DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].des_spacing;
        DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].des_spacing = spacing;
        T_Enablr_Scroll = DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].enable_scroll;
        DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].enable_scroll = Enablr_Scroll;
        if (T_Enablr_Scroll)
        {
            Button_scrollPosition = GUI.BeginScrollView(boxrect, Button_scrollPosition,veiwrect);
            if (Application.isPlaying == false)
            {
                Button_scrollPosition = new Vector2(scroll_H_value, scroll_V_value);
                GUI.changed = true;
            }
            //print(veiwrect.width);
        }
        else
        {
            GUI.changed = true;
        }
        //
        GUI.Box(statement_rect, s, guimessageboxstyle);
        //
        if (DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].Isdecision)
        {

            ESDialgoueScriptableObj.StatementLayer.Statements statements = DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex];
            //Rect des_rect = new Rect();
            GUIStyle guidesboxstyle = new GUIStyle();
            guidesboxstyle.border = GUI.skin.button.border;
            guidesboxstyle.normal.textColor = Color.black;
            guidesboxstyle.alignment = TextAnchor.MiddleCenter;
            guidesboxstyle.fontSize = FontSize;
            guidesboxstyle.normal.textColor = FontColor;
            guidesboxstyle.clipping = GUI.skin.textField.clipping;
            guidesboxstyle.overflow = GUI.skin.textArea.overflow;
            guidesboxstyle.wordWrap = true;
            guidesboxstyle.normal.background = des_bck_texture;
            guidesboxstyle.active.background = des_act_texture;
            Vector2 des_size = new Vector2();
            if (statements.decisions.Count > 0)
            {
                for (int i = 0; i < statements.decisions.Count; ++i)
                {
                    string _content = "";
                    GUIContent des_gUIContent = new GUIContent();
                    
                    //List<ESDialgoueScriptableObj.StatementLayer.Statements.Decision> decision = DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].decisions;
                    //
                    if (statements.vertical)
                    {
                        if (i == 0)
                        {
                            _content = statements.decisions[i].content;
                            des_gUIContent = new GUIContent(_content);

                            des_size = guimessageboxstyle.CalcSize(des_gUIContent) + addsize;
                            statements.decisions[i].display_rect = new Rect(statement_rect.x + posx, statement_rect.y + posy, des_size.x, des_size.y);
                            //GUI.Box(statements.decisions[i].display_rect, _content, guidesboxstyle);
                            //
                        }
                        else
                        {
                            _content = statements.decisions[i].content;
                            des_gUIContent = new GUIContent(_content);

                            des_size = guimessageboxstyle.CalcSize(des_gUIContent) + addsize;

                            statements.decisions[i].display_rect = new Rect(statement_rect.x + posx, statements.decisions[i - 1].display_rect.y + statements.decisions[i - 1].display_rect.height + T_spacing, des_size.x, des_size.y);
                            //GUI.Box(statements.decisions[i].display_rect, _content, guidesboxstyle);

                        }
                    }
                    //
                    if (statements.horizontal)
                    {
                        if (i == 0)
                        {
                            _content = statements.decisions[i].content;
                            des_gUIContent = new GUIContent(_content);

                            des_size = guimessageboxstyle.CalcSize(des_gUIContent) + addsize;
                            statements.decisions[i].display_rect = new Rect(statement_rect.x + posx, statement_rect.y + statement_rect.height + posy, des_size.x, des_size.y);
                            //GUI.Box(statements.decisions[i].display_rect, _content, guidesboxstyle);
                            //
                        }
                        else
                        {
                            _content = statements.decisions[i].content;
                            des_gUIContent = new GUIContent(_content);

                            des_size = guimessageboxstyle.CalcSize(des_gUIContent)+ addsize;
                            statements.decisions[i].display_rect = new Rect(statements.decisions[i - 1].display_rect.x + statements.decisions[i - 1].display_rect.width + T_spacing, statement_rect.y + statement_rect.height + posy, des_size.x, des_size.y);

                            //GUI.Box(statements.decisions[i].display_rect, _content, guidesboxstyle);

                        }
                    }
                    //
                    //print(clicked_des_index);
                    //GUI.Box(statements.decisions[i].display_rect, _content, guidesboxstyle);
                    if (Application.isPlaying == false)
                    {
                        if (clicked_des_index != i)
                        {
                            if (GUI.Button(statements.decisions[i].display_rect, _content, guidesboxstyle))
                            {
                                next = true;
                                isdecision = true;
                                //print(i);
                                clicked_des_index = i;
                            }
                        }

                        if (clicked_des_index == i)
                        {

                            GUIStyle selected_desboxstyle = new GUIStyle();
                            selected_desboxstyle.border = GUI.skin.button.border;
                            selected_desboxstyle.normal.textColor = FontColor;
                            selected_desboxstyle.fontSize = FontSize;
                            selected_desboxstyle.alignment = TextAnchor.MiddleCenter;
                            selected_desboxstyle.clipping = GUI.skin.textField.clipping;
                            selected_desboxstyle.overflow = GUI.skin.textArea.overflow;
                            selected_desboxstyle.wordWrap = true;
                            selected_desboxstyle.normal.background = Resources.Load("Textures/selected") as Texture2D;
                            //
                            if (GUI.Button(statements.decisions[i].display_rect, _content, selected_desboxstyle))
                            {
                                next = true;
                                isdecision = true;
                                //print(i);
                                clicked_des_index = i;
                            }
                        }

                    }

                    if (Application.isPlaying)
                    {
                        if (GUI.Button(statements.decisions[i].display_rect, _content, guidesboxstyle))
                        {
                            next = true;
                            isdecision = true;
                            //print(i);
                            clicked_des_index = i;
                        }
                    }

                }
                //

                if (statements.vertical)
                {
                    totalheight = (statements.decisions[statements.decisions.Count - 1].display_rect.y - statement_rect.y) + statements.decisions[statements.decisions.Count - 1].display_rect.height + statement_rect.height + stmt_posy;
                    scrollrect.height = veiwrect.width > boxrect.width ? Mathf.Abs((totalheight) - boxrect.height) + 16.0f : Mathf.Abs((totalheight) - boxrect.height);
                    for (int i = 0; i < statements.decisions.Count; i++)
                    {
                        //float temp_totalheight = statements.decisions[i].display_rect.height;
                        float temp_totalwidth = statements.decisions[i].display_rect.width;

                        //maxheight = 0f;

                        if (statements.decisions[i].display_rect.width > maxwidth)
                        {
                            maxwidth = statements.decisions[i].display_rect.width;
                            selected_Width_i = i;
                           // print(i);
                        }
                    }
                    //

                    if (statements.decisions[selected_Width_i].display_rect.width != maxwidth)
                    {
                        //reset that mofuka
                        maxwidth = 0.0f;
                    }
                    //
                    des_rect = statements.decisions[selected_Width_i].display_rect;
                    Rect anchor = new Rect(statement_rect.x + statement_rect.width, statement_rect.y, 5f, 5f);
                    if ((des_rect.x + des_rect.width) < anchor.x)
                    {
                        // fuuuuuuuuck ya bitch gave me though time :{}
                        overall_width = (statement_rect.width + stmt_posx);
                        scrollrect.width = Mathf.Abs((statement_rect.x + statement_rect.width) - (boxrect.x + boxrect.width)) + 16.0f;
                    }
                    else
                    {
                        float addedwidth = (des_rect.x + des_rect.width) - (statement_rect.x + statement_rect.width);
                        overall_width = (statement_rect.width + stmt_posx) + addedwidth;
                        float temp_scrollwidth = Mathf.Abs((des_rect.x + des_rect.width) - (boxrect.x + boxrect.width)) + 16.0f;
                        scrollrect.width = temp_scrollwidth;
                    }
                }

                if (statements.horizontal)
                {
                    for (int i = 0; i < statements.decisions.Count; i++)
                    {
                        //float temp_totalheight = statements.decisions[i].display_rect.height;
                        float temp_totalwidth = statements.decisions[i].display_rect.width;

                        //maxheight = 0f;

                        if (statements.decisions[i].display_rect.height > maxheight)
                        {
                            maxheight = statements.decisions[i].display_rect.height;
                            selected_Height_i = i;

                            //print(i);
                        }
                    }
                    //
                    if (statements.decisions[selected_Height_i].display_rect.height != maxheight)
                    {
                        //reset that mofuka

                        maxheight = 0.0f;
                    }
                    //
                    des_rect = statements.decisions[statements.decisions.Count - 1].display_rect;
                    float desspace = Mathf.Abs((statement_rect.x) - (des_rect.x));
                    Rect anchor = new Rect(statement_rect.x + statement_rect.width, statement_rect.y, 5f, 5f);
                    if ((des_rect.x + des_rect.width) < anchor.x)
                    {
                        // fuuuuuuuuck ya bitch gave me though time :{}
                        overall_width = (statement_rect.width + stmt_posx);
                        scrollrect.width = Mathf.Abs((statement_rect.x + statement_rect.width) - (boxrect.x + boxrect.width)) + 16.0f;
                    }
                    else
                    {
                        float addedwidth = (des_rect.x + des_rect.width) - (statement_rect.x + statement_rect.width);
                        overall_width = (statement_rect.width + stmt_posx) + addedwidth;
                        float temp_scrollwidth = Mathf.Abs((des_rect.x + des_rect.width) - (boxrect.x + boxrect.width)) + 16.0f;
                        scrollrect.width = temp_scrollwidth;
                    }

                    totalheight = (statements.decisions[selected_Height_i].display_rect.height + posy) + statement_rect.height + stmt_posy;
                    scrollrect.height = veiwrect.width > boxrect.width ? Mathf.Abs((totalheight) - boxrect.height) + 16.0f : Mathf.Abs((totalheight) - boxrect.height);

                }
                //


                overall_height = totalheight;
            }
                //print(Button_scrollPosition.y);
            }
            else
            {
                totalheight = statement_rect.height + stmt_posy;
                scrollrect.height = veiwrect.width > boxrect.width ? Mathf.Abs((totalheight) - boxrect.height) + 16.0f : Mathf.Abs((totalheight) - boxrect.height);
                //
                total_width = statement_rect.width + stmt_posx;
                scrollrect.width = veiwrect.height > boxrect.height ? Mathf.Abs((total_width) - boxrect.width) + 16.0f : Mathf.Abs((total_width) - boxrect.width);
                //
                overall_width = total_width;
                overall_height = totalheight;
            }
            if (T_Enablr_Scroll)
            {
                GUI.EndScrollView();
            }
            //

      
      //  print(Button_scrollPosition.x);
        DialogueGraph.statementLayers[selectedindex].statements[CurrentMessageIndex].Statement_rect = statement_rect;
        //:{}
        //
    }
    //
    public void TriggerLayer(int layer_ID,float cooldowntime)
    {
        if (triggerEvent != TriggerEvent.Manual) return;
        if (triggerType == TriggerType.Event)
        {
            if (DialogueGraph != null)
            {
                if (layer_ID == DialogueGraph.statementLayers[selectedindex].ID && !triggered)
                {
                    DialogueGraph.statementLayers[selectedindex].Play = true;
                    triggered = true;
                    CoolDownTime = cooldowntime;
                }
               
            }
        }
    }
    //
    private void ResetTriggeredLayer()
    {
        if (triggered && DialogueGraph.statementLayers[selectedindex].isplaying == false)
        {
            if (mytime < CoolDownTime)
            {
                mytime = FreezeTime ? mytime = DeltaTime(mytime) : mytime += Time.deltaTime;
            }
            if (mytime >= CoolDownTime)
            {
                triggered = false;
                mytime = 0;
            }
        }
    }
    //
    public int GetCurrentMessageIndex()
    {
        return CurrentMessageIndex;
    }
    //
    public int GetCurrentLayerIndex()
    {
        return selectedindex;
    }
    //
    public int GetCurrentDecisionIndex()
    {
        return clicked_des_index;
    }
    public string GetSelectedDecisionID()
    {
        return DecisionID;
    }
    //
    public int GetStatementLayerID()
    {
        return StmtLayerId;
    }
    //
    public void SetCurrentMessageIndex(int value)
    {
        CurrentMessageIndex = value;
    }
    //
    public void SetCurrentLayerIndex(int value)
    {
        selectedindex = value;
    }
    //
    /*
     * dont unlock this for your good ;{}
    public void SetCurrentDecision(int value)
    {
        clicked_des_index = value;
    }
    */
    //
    //
    private float DeltaTime(float value)
    {
        //we are men of letters #legacy :))
        value += 0.019f;
        return value;
    }
    //

    private void OnTriggerEnter(Collider collider)
    {
        if (triggerType == TriggerType.Collider)
        {
            if (DialogueGraph != null)
            {
                if (collider.tag == ObjectTagName)
                    DialogueGraph.statementLayers[selectedindex].Play = true;
            }
        }
    }

    //
    private void CallEvents(int layer_ID,bool IsAwake)
    {
        if (triggerEvent == TriggerEvent.Manual) return;
        if (Application.isPlaying)
        {
            if (IsPlaying == false)
            {
                if (DialogueGraph != null)
                {
                    if (layer_ID == DialogueGraph.statementLayers[selectedindex].ID && !triggered)
                    {
                        DialogueGraph.statementLayers[selectedindex].Play = true;
                        triggered = true;
                        if (eventClasses.Count > 0)
                        {
                            for (int i = 0; i < eventClasses.Count; ++i)
                            {
                                if (eventClasses[i].SelectedEventIndex == 0)
                                {
                                    CoolDownTime = eventClasses[i].CoolDownTime;
                                }
                            }
                        }
                    }

                }

            }
            //end
        }
    }
    //
    private void ResetEvents()
    {
        for (int i = 0; i < eventClasses.Count; ++i)
        {
            eventClasses[i].HasPlayed = false;
        }
        justplayed = false;
        DecisionID = "";
    }
    //
    private void PerformEvents()
    {
        //ver1.1
        if (eventClasses.Count > 0)
        {
            for (int i = 0; i < eventClasses.Count; ++i)
            {
                if (eventClasses[i].SelectedEventIndex == 0)
                {
                    if (eventClasses[i].HasPlayed == false)
                    {
#if UNITY_EDITOR
                        for (int j = 0; j < eventClasses[i].myobjects.Count; j++)
                        {
                            DestroyImmediate(eventClasses[i].myobjects[j]);
                        }
#else
                    for (int j = 0; j < eventClasses[i].myobjects.Count; j++)
                    {
                        Destroy(eventClasses[i].myobjects[j]);
                    }
#endif
                        //
                        if (eventClasses[i].destroythis == true)
                        {
#if UNITY_EDITOR
                            DestroyImmediate(this.gameObject);
#else
                        Destroy(this.gameObject);
#endif
                        }
                        //
                        eventClasses[i].HasPlayed = false;
                    }
                }
                else if (eventClasses[i].SelectedEventIndex == 1)
                {
                    if (!eventClasses[i].HasPlayed)
                    {
                        timeval += DeltaTime(timecounter);
                        if (timeval >= eventClasses[i].RepeatTime)
                        {
                            int selected_spawnpoint_index = UnityEngine.Random.Range(0, eventClasses[i].SpawnPoints.Count);
                            int selected_spawn_obj_index = UnityEngine.Random.Range(0, eventClasses[i].myobjects.Count);

                            if (eventClasses[i].myobjects[selected_spawn_obj_index] != null)
                            {
                                Instantiate(eventClasses[i].myobjects[selected_spawn_obj_index], eventClasses[i].SpawnPoints[selected_spawnpoint_index].position, Quaternion.identity);
                            }
                            timeval = 0.0f;
                            timecounter = timeval;
                            repeatcounter++;
                        }
                    }
                    
                    if (repeatcounter >= eventClasses[i].RepeatAmount)
                    {
                        eventClasses[i].HasPlayed = true;

                        repeatcounter = 0;
                    }
                }
                else if (eventClasses[i].SelectedEventIndex == 2)
                {
                    if (!eventClasses[i].HasPlayed)
                    {
                        for (int j = 0; j < eventClasses[i].myobjects.Count; j++)
                        {
                            if (eventClasses[i].myobjects[j] != null)
                                eventClasses[i].myobjects[j].SetActive(eventClasses[i].active[j]);
                        }

                        eventClasses[i].HasPlayed = true;
                    }
                  
                  
                 
                }
                else if (eventClasses[i].SelectedEventIndex == 3)
                {
                    //
                 
                    if (!eventClasses[i].HasPlayed)
                    {
                        if (eventClasses[i].LoadType == EventClass.SceneLoadType.Single)
                        {
                            if (eventClasses[i].SceneType == EventClass.LoadSceneType.ByName)
                                SceneManager.LoadScene(eventClasses[i].SceneName, LoadSceneMode.Single);
                            else
                                SceneManager.LoadScene(eventClasses[i].Sceneindex, LoadSceneMode.Single);
                        }
                        else if (eventClasses[i].LoadType == EventClass.SceneLoadType.Addictive)
                        {
                            if (eventClasses[i].SceneType == EventClass.LoadSceneType.ByName)
                                SceneManager.LoadScene(eventClasses[i].SceneName, LoadSceneMode.Additive);
                            else
                                SceneManager.LoadScene(eventClasses[i].Sceneindex, LoadSceneMode.Additive);
                        }
                        //
                        eventClasses[i].HasPlayed = true;
                    }

                 
                }
                else if (eventClasses[i].SelectedEventIndex == 4)
                {
                  
                    if (!eventClasses[i].HasPlayed)
                    {
                        SceneManager.UnloadSceneAsync(eventClasses[i].Sceneindex);
                        eventClasses[i].HasPlayed = true;
                    }
               
                }
                else if (eventClasses[i].SelectedEventIndex == 5)
                {
#if UNITY_EDITOR
                   
                  
                    EditorApplication.isPlaying = false;

#else
                   Application.Quit();             
#endif
                    
                }
                else if (eventClasses[i].SelectedEventIndex == 6)
                {
                  
                    if (!eventClasses[i].HasPlayed)
                    {
                        if (DialogueGraph != null)
                        {
                            if (IsSelection)
                            {
                                if (Application.isPlaying)
                                {
                                    if (eventClasses[i].DialogueManager.IsPlaying == false)
                                    {
                                        if (eventClasses[i].DialogueManager != null)
                                        {
                                            int myindex = eventClasses[i].DialogueManager.selectedindex;
                                            if (!eventClasses[i].DialogueManager.triggered)
                                            {
                                                eventClasses[i].DialogueManager.DialogueGraph.statementLayers[myindex].Play = true;
                                                eventClasses[i].DialogueManager.triggered = true;
                                                if (eventClasses.Count > 0)
                                                {
                                                    if (eventClasses[i].SelectedEventIndex == 6)
                                                    {
                                                        eventClasses[i].DialogueManager.CoolDownTime = eventClasses[i].CoolDownTime;
                                                    }
                                                }
                                            }

                                        }

                                    }

                                }
                                eventClasses[i].HasPlayed = true;

                            }
                           
                        }
                 
                    }
                    
                }
                else if (eventClasses[i].SelectedEventIndex == 7)
                {
                    
                    if (!eventClasses[i].HasPlayed)
                    {
                        if (eventClasses[i].tagobject != null)
                        {
                            eventClasses[i].tagobject.tag = eventClasses[i].TagName;
                        }
                        eventClasses[i].HasPlayed = true;
                    }
                  
                }

            }
            //
            //
        }
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                