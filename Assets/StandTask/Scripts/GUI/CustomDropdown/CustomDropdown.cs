using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class CustomDropdown : MonoBehaviour
{
    //public int ItemCount;
    public float ItemHeight;
    public float Width;
    public float ScrolbarSize;

    //public List<CustomDropdownItem> ItemList;
    public GameObject ItemListObject;
    public RectTransform ItemListPanel;
    public GameObject TemplateItemObject;
    public RectTransform TemplateItemPanel;


    //public CustomDropdownItem ItemClassTemplate;

    public int SelectedItem;
    public ClickClass clickClass;
    public MenuScript menuScript;

    public bool HideListOnClick;

    //public GameObject CustomDropdownManager;

    //public bool isOpened;


    // Use this for initialization
    void Awake()
    {
        ItemListPanel = ItemListObject.GetComponent<RectTransform>();
        TemplateItemPanel = TemplateItemObject.GetComponent<RectTransform>();
        //clickClass = this.GetComponent<ClickClass>() as ClickClass;
        //menuScript = this.GetComponent<MenuScript>() as MenuScript; 
        //HideListObject();
        //ScrolbarSize = 20.0f;
        Width = this.GetComponent<RectTransform>().rect.width;
        ItemHeight = TemplateItemPanel.rect.height;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetClickClass(ClickClass newClickClass)
    {
        clickClass = newClickClass;
    }

    private void CalcHeight()
    {
        float NewHeight = 0.0f;//ItemListPanel.sizeDelta.y;

        Debug.Log("List size = " + ItemListObject.transform.childCount);

        NewHeight = ItemHeight * ItemListObject.transform.childCount;
        Debug.Log("New height = " + NewHeight);
        //ItemListPanel.sizeDelta = new Vector2(Width - ScrolbarSize, NewHeight);
        ItemListPanel.rect.Set(0, 0, Width - ScrolbarSize, NewHeight);
        //ItemListPanel.localPosition = new Vector3(0, 0, 0);
    }

    private void CalcHeight(int itemCount)
    {
        float NewHeight = 0.0f;//ItemListPanel.sizeDelta.y;

        Debug.Log("List size = " + itemCount);

        NewHeight = ItemHeight * itemCount;
        Debug.Log("New height = " + NewHeight);

        if (NewHeight > 0)
        {
            ItemListPanel.sizeDelta = new Vector2(Width - ScrolbarSize, NewHeight);
            ItemListPanel.rect.Set(0, 0, Width - ScrolbarSize, NewHeight);
            ItemListObject.GetComponent<ScrollRect>().verticalNormalizedPosition = 0.0f;
        }

        //ItemListPanel.rect.Set(0,0, Width - ScrolbarSize, NewHeight)localPosition = new Vector3(50, 0, 0);
    }

    public void AddItem(int i, string itemName, bool enabled)
    {
        GameObject NewItemObject = Instantiate(TemplateItemObject) as GameObject;
        NewItemObject.GetComponent<Button>().gameObject.SetActive(true); //Unhide template
        NewItemObject.GetComponent<Button>().gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(20.0f, 0.0f);
        //NewItemObject.name = "Item" + ItemList.Count;
        //Debug.Log("Width = " + NewItemObject.GetComponent<RectTransform>().sizeDelta.x);
        //Debug.Log("Height = " + NewItemObject.GetComponent<RectTransform>().sizeDelta.y);

        //NewItemObject.GetComponent<CustomDropdownItem>().title.text = itemName;
        NewItemObject.GetComponentInChildren<Text>().text = itemName;
        //NewItemObject.GetComponent<CustomDropdownItem>().Number = ItemList.Count;

        //NewItemObject.GetComponent<CustomDropdownItem>().Enable(enabled);

        NewItemObject.GetComponent<Button>().enabled = enabled; //Enable or disable click on button
        NewItemObject.GetComponent<Button>().onClick.AddListener(() => { clickClass.DoClickAction(i); }); //Add ClickClass to button with int i paramter 

        //ItemList.Add(NewItemObject.GetComponent<CustomDropdownItem>());
        NewItemObject.transform.SetParent(ItemListObject.gameObject.transform, false);
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

    //SetItemList with Enable/Disable elements
    public void SetItemList(List<int> ItemKeyList, List<string> ItemNameList, List<bool> EnableList)
    {
        //Debug.Log("SetItemList");
        RemoveItems();

        if (ItemNameList.Count > 0 && ItemNameList.Count == EnableList.Count && ItemNameList.Count == ItemKeyList.Count)
        {
            for (int i = 0; i < ItemNameList.Count; i++)
            {
                //Debug.Log("Item was added");
                //Add enabled or disabled action to dropdown list
                AddItem(ItemKeyList[i], ItemNameList[i], EnableList[i]);
            }

            CalcHeight(ItemNameList.Count);
        }
        else
        {
            //Debug.Log("Problem with Item's count in list");
        }
    }

    public void SetItemList(List<string> ItemNameList, List<bool> EnableList)
    {
        //Debug.Log("SetItemList");
        RemoveItems();

        if (ItemNameList.Count > 0 && ItemNameList.Count == EnableList.Count)
        {
            for (int i = 0; i < ItemNameList.Count; i++)
            {
                //Debug.Log("Item was added");
                //Add enabled or disabled action to dropdown list
                AddItem(i, ItemNameList[i], EnableList[i]);
            }

            CalcHeight(ItemNameList.Count);
        }
        else
        {
            //Debug.Log("Problem with Item's count in list");
        }
    }

    public void SetItemList(List<string> ItemNameList)
    {
        //Debug.Log("SetItemList");
        RemoveItems();


        for (int i = 0; i < ItemNameList.Count; i++)
        {
            //Debug.Log("Item was added");
            //Add enabled or disabled action to dropdown list
            AddItem(i, ItemNameList[i], true);
        }

        CalcHeight(ItemNameList.Count);

    }

    //SetItemList without Enable/Disable elements (all is enabled)
    public void SetItemList(List<int> ItemKeyList, List<string> ItemNameList)
    {
        //Debug.Log("SetItemList");
        RemoveItems();

        if (ItemNameList.Count > 0 && ItemNameList.Count == ItemKeyList.Count)
        {
            for (int i = 0; i < ItemNameList.Count; i++)
            {
                //Debug.Log("Item was added");
                //Add enabled or disabled action to dropdown list
                AddItem(ItemKeyList[i], ItemNameList[i], true);
            }

            CalcHeight(ItemNameList.Count);
        }
        else
        {
            //Debug.Log("Problem with Item's count in list");
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
            //Debug.Log("HideListObject");
            //DestroyListObject();
            //
            RemoveItems();
            this.gameObject.SetActive(false);
            //menuScript.HideMenu();// = false;

            //isOpened = false;
        }
    }
}
