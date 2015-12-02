using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class CustomDropdown : MonoBehaviour {
    //public int ItemCount;
    public float ItemHeight;
    public float Width;

    //public List<CustomDropdownItem> ItemList;
    public GameObject ItemListObject;
    public RectTransform ItemListPanel;
    public RectTransform TemplateItemPanel;

    public GameObject ItemObjectTemplate;
    //public CustomDropdownItem ItemClassTemplate;

    public int SelectedItem;
    public ClickClass clickClass;
    public MenuScript menuScript;

    public bool HideListOnClick;

    //public GameObject CustomDropdownManager;
    
    //public bool isOpened;


    // Use this for initialization
    void Start()
    {
        //clickClass = this.GetComponent<ClickClass>() as ClickClass;
        //menuScript = this.GetComponent<MenuScript>() as MenuScript; 
        //HideListObject();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void CalcHeight()
    {
        float NewHeight = 0.0f;//ItemListPanel.sizeDelta.y;
        NewHeight = ItemHeight * ItemListObject.transform.childCount;
        Debug.Log("New height = " + NewHeight);
        ItemListPanel.sizeDelta = new Vector2(Width, NewHeight);
        /*Left*/
        //rectTransform.offsetMin.x;
        /*Right*/
        //rectTransform.offsetMax.x;
        /*Top*/
        //rectTransform.offsetMax.y;
        /*Bottom*/
        //rectTransform.offsetMin.y;
    }

    private void AddItem(int i, string itemName, bool enabled)
    {
        GameObject NewItemObject = Instantiate(ItemObjectTemplate) as GameObject;
        
        //NewItemObject.name = "Item" + ItemList.Count;
        //Debug.Log("Width = " + NewItemObject.GetComponent<RectTransform>().sizeDelta.x);
        //Debug.Log("Height = " + NewItemObject.GetComponent<RectTransform>().sizeDelta.y);

        //NewItemObject.GetComponent<CustomDropdownItem>().title.text = itemName;
        NewItemObject.GetComponentInChildren<Text>().text = itemName;
        //NewItemObject.GetComponent<CustomDropdownItem>().Number = ItemList.Count;

        //NewItemObject.GetComponent<CustomDropdownItem>().Enable(enabled);

        NewItemObject.GetComponent<Button>().enabled = enabled;
        NewItemObject.GetComponent<Button>().onClick.AddListener(() => { clickClass.DoClickAction(i); }); 

        //ItemList.Add(NewItemObject.GetComponent<CustomDropdownItem>());
        NewItemObject.transform.parent = ItemListObject.gameObject.transform;
        NewItemObject.gameObject.SetActive(true);
    }

    public void RemoveItems()
    {
        Debug.Log("RemoveItems");
        //ItemList.Clear();
        foreach (Transform child in ItemListObject.transform)
        {
            Destroy(child.gameObject);
        }

        //CalcHeight();
        //HideListObject();
    }

    public void SetItemList(List<int> ItemKeyList, List<string> ItemNameList, List<bool> EnableList)
    {
        Debug.Log("SetItemList");
        RemoveItems();
 
        if (ItemNameList.Count > 0 && ItemNameList.Count == EnableList.Count)
        {
            for (int i = 0; i < ItemNameList.Count; i++)
            {
                Debug.Log("Item was added");
                //Add enabled or disabled action to dropdown list
                AddItem(i, ItemNameList[i], EnableList[i]);
            }

            CalcHeight();
        }
        else
        {
            Debug.Log("Problem with Item's list");
        }
    }

    /*public void OnClick(int i)
    {
        //If menu is visible
        if (ItemListObject.gameObject.activeSelf)
        {
            clickClass.DoClickAction(i);
        }
    }*/

    public void HideAndClearListObject()
    {
        if (HideListOnClick)
        {
            Debug.Log("HideListObject");
            //DestroyListObject();
            //
            RemoveItems();
            this.gameObject.SetActive(false);
            //menuScript.HideMenu();// = false;
            
            //isOpened = false;
        }
    }
}
