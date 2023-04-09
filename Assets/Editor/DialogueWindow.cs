using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Linq;

public class DialogueWindow : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    private DialogueLineData m_activeItem = null;
    private InspectorElement m_detailInspector = null;
    private GroupBox m_listBox = null; //MAIN CONTENT BOX, CONTAINS THE DIALOGUE LINE VARIABLE
    private GroupBox m_detailBox = null; //DETAIL BOX, CONTAINS ALL OF DIALOGUE LINE'S IMPORTANT INFO AND VARIABLES
    private Button m_addButton = null; //ADD DIALOGUE LINE BUTTON 
    private Button m_delButton = null; //DELETE DIALOGUE LINE BUTTON
    private ListView m_listView = null; //LIST VIEW DIALOGUE LINE OBJECTS
    private DialogueDatabase m_currentDatabase = null;

    [MenuItem("Window/My Tools/DialogueWindow")]
    public static void ShowExample()
    {
        DialogueWindow wnd = GetWindow<DialogueWindow>();
        wnd.titleContent = new GUIContent("DialogueWindow");
    }

    private void BindFunc(VisualElement e, int i)
    {
        Label item = e as Label;
        item.text = m_currentDatabase.list[i].name; //SHOWS THE CURRENT DIALOGUE'S LINE DATA IN THE VISUAL ELEMENT
    }

    public void PopulateDialogueList()
    {
        m_listBox.Clear(); //EMPTY'S OUT THE LIST

        if (m_currentDatabase != null)
        {
            //INTERNAL TYPE THAT SETS THE TYPE OF THE FUNCTION, GENERIC <return type: VisualElement>, 
            Func<VisualElement> makeItem = () => new Label(); //CREATES A NEW ANONYMOUS FUNCTION THAT RETURNS A LABEL

            //BINDS THE DIALOGUE LINE DATA TO THE VISUAL TREE
            Action<VisualElement, int> bindItem = BindFunc;

            //CONSTRUCTOR TO DYNAMICALLY CREATE A NEW LIST VIEW
            m_listView = new ListView(m_currentDatabase.list, 
                EditorGUIUtility.singleLineHeight, 
                makeItem, bindItem);

            //ONLY ABLE TO SELECT ONE ITEM AT A TIME
            m_listView.selectionType = SelectionType.Single;

            m_listBox.Add(m_listView);

            //CALLS FUNCTION WHEN 'ONCLICK' SIMILAR TO JAVASCRIPT EVENT LISTENERS
            m_listView.onSelectionChange += DialogueListSelectionChanged;
        }
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;
        
        //OBJECT FIELD THAT WILL CONTAIN THE DIALOGUE DATABASE
        var objectField = new ObjectField(); //INSTANTIATE NEW INSTANCE OF THE CLASS
        objectField.label = "Dialogue Database"; // LABEL IT
        objectField.objectType = typeof(DialogueDatabase); //SPECIFY WHAT TYPE OF DATA THIS OBJECT FIELD WILL ACCEPT

        objectField.RegisterCallback<ChangeEvent<UnityEngine.Object>>(e =>
        {
            if(objectField.value != null)
            {
                m_currentDatabase = objectField.value as DialogueDatabase;
            }
            else
            {
                m_currentDatabase = null;
            }

            PopulateDialogueList();
        });

        root.Add(objectField);

        //LOAD THE PATH
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/DialogueWindow.uxml");

        VisualElement uxmlData = visualTree.Instantiate();
        uxmlData.style.flexGrow = 1.0f;
        root.Add(uxmlData);

        //ADD THESE TO THE UI ROOT
        m_listBox = root.Query<GroupBox>("MainContent").First();
        m_detailBox = root.Query<GroupBox>("DetailBox").First();

        m_addButton = root.Query<Button>("AddButton").First();
        m_addButton.clicked += () => CreateNewDialogueLine(); //CALLS ANONYMOUS FUNCTION 'CreateNewDialogueLine()' WHEN THIS BUTTON IS CLICKED

        m_delButton = root.Query<Button>("DeleteButton").First();
        m_delButton.clicked += () => DeleteSelectedDialogueLine(); 
    }

    private void CreateNewDialogueLine()
    {
        string path = EditorUtility.SaveFilePanelInProject("Create Dialogue File",
            "DefaultDialogueLine", "asset", "Please select a name for the dialogue file.");

        if(path.Length != 0)
        {
            DialogueLineData line = ScriptableObject.CreateInstance<DialogueLineData>();
            AssetDatabase.CreateAsset(line, path);
            AssetDatabase.Refresh();

            m_currentDatabase.list.Add(line);

            EditorUtility.SetDirty(m_currentDatabase); //TELLS THE EDITOR THAT THE FILE THAT IS ON MEMORY IS DIFFERENT THAN ON THE DISK
            //MEANS THAT THE FILE IS UNSAVED

            //REFRESHES THE CONTENT IN THE LIST
            PopulateDialogueList();

            //SELECT THE LAST ITEM IN THE LIST
            m_listView.SetSelection(m_currentDatabase.list.Count - 1);
        }
    }
    private void DeleteSelectedDialogueLine()
    {
        if(m_activeItem!= null)
        {
            m_currentDatabase.list.Remove(m_activeItem);

            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(m_activeItem)); //FUNCTION THAT DELETES THE FILE AT THE ASSET PATH
            EditorUtility.SetDirty(m_currentDatabase);

            m_activeItem = null;

            //REPOPULATE THE LIST WITH THE CURRENT DIALOGUES
            PopulateDialogueList();

            //SETS THE SELECTED ITEM TO THE FIRST DIALOGUE LINE
            if(m_currentDatabase.list.Count > 0)
                m_listView.SetSelection(0);
        }
    }

    private void DialogueListSelectionChanged(IEnumerable<object> selectedItems)
    {
        foreach(DialogueLineData line in selectedItems)
        {
            m_activeItem = line;
        }

        if(m_detailInspector == null)
        {
            m_detailInspector = new InspectorElement(); // CREATES NEW INSTANCE OF THE CLASS IF ITS NULL
            m_detailInspector.style.flexGrow = 1.0f; //ALLOWS INSPECTOR TO GROW 
            m_detailBox.Add(m_detailInspector); //ADDS TO THE LIST BOX
        }

        if (selectedItems.Count() > 0)
            m_detailInspector.Bind(new SerializedObject(m_activeItem));
    }
}