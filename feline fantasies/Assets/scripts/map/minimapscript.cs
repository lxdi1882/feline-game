using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MiniMap : MonoBehaviour
{
    public Transform player; // Assign the player
    public RectTransform miniMapRect; // The UI panel for the mini-map
    public float mapScale = 5f; // Scale factor for world-to-mini-map conversion

    // NPC Icons
    [System.Serializable]
    public class NPCType
    {
        public string typeName; // Name of NPC type (e.g., "QuestGiver")
        public GameObject iconPrefab;
    }
    public List<NPCType> npcTypes; // List of different NPC types

    // Item Icons
    [System.Serializable]
    public class ItemType
    {
        public string typeName; // Name of item type (e.g., "HealthPotion")
        public GameObject iconPrefab;
    }
    public List<ItemType> itemTypes; // List of different Item types

    // Lists for different types of objectives
    public List<Transform> npcs;
    public List<string> npcTypesAssigned; // Matches each NPC with a type
    public List<Transform> items;
    public List<string> itemTypesAssigned; // Matches each Item with a type
    public List<Transform> enemies;
    public GameObject iconPrefab_Enemy; // Single enemy icon

    private Dictionary<Transform, RectTransform> icons = new Dictionary<Transform, RectTransform>();
    public RectTransform playerIcon; // Player icon in mini-map

    void Start()
    {
        // Create icons for NPCs
        CreateNPCIcons(npcs, npcTypesAssigned);

        // Create icons for Items
        CreateItemIcons(items, itemTypesAssigned);

        // Create icons for Enemies
        CreateGenericIcons(enemies, iconPrefab_Enemy);
    }

    void Update()
    {
        // Keep the player icon centered
        playerIcon.anchoredPosition = Vector2.zero;

        // Update all objective icons
        List<Transform> toRemove = new List<Transform>();

        foreach (var entry in icons)
        {
            Transform worldTarget = entry.Key;
            RectTransform iconTransform = entry.Value;

            if (worldTarget != null)
            {
                // Check if the object is disabled
                if (!worldTarget.gameObject.activeInHierarchy)
                {
                    // Mark for removal if object is disabled
                    toRemove.Add(worldTarget);
                }
                else
                {
                    // Update icon position if object is active
                    Vector2 relativePos = (worldTarget.position - player.position) * mapScale;
                    iconTransform.anchoredPosition = relativePos;
                }
            }
            else
            {
                // If the object is destroyed, mark it for removal
                toRemove.Add(worldTarget);
            }
        }

        // Remove icons for destroyed or disabled objects
        foreach (Transform obj in toRemove)
        {
            if (icons.ContainsKey(obj))
            {
                Destroy(icons[obj].gameObject);  // Remove the icon from the mini-map
                icons.Remove(obj);  // Remove the object from the dictionary
            }
        }
    }


    void CreateNPCIcons(List<Transform> objects, List<string> typeNames)
    {
        for (int i = 0; i < objects.Count; i++)
        {
            Transform obj = objects[i];
            string typeName = typeNames[i];

            GameObject selectedIconPrefab = null;

            foreach (var type in npcTypes)
            {
                if (type.typeName == typeName)
                {
                    selectedIconPrefab = type.iconPrefab;
                    break;
                }
            }

            if (selectedIconPrefab != null)
            {
                GameObject newIcon = Instantiate(selectedIconPrefab, miniMapRect);
                icons[obj] = newIcon.GetComponent<RectTransform>();
            }
        }
    }

    void CreateItemIcons(List<Transform> objects, List<string> typeNames)
    {
        for (int i = 0; i < objects.Count; i++)
        {
            Transform obj = objects[i];
            string typeName = typeNames[i];

            GameObject selectedIconPrefab = null;

            foreach (var type in itemTypes)
            {
                if (type.typeName == typeName)
                {
                    selectedIconPrefab = type.iconPrefab;
                    break;
                }
            }

            if (selectedIconPrefab != null)
            {
                GameObject newIcon = Instantiate(selectedIconPrefab, miniMapRect);
                icons[obj] = newIcon.GetComponent<RectTransform>();
            }
        }
    }

    void CreateGenericIcons(List<Transform> objects, GameObject iconPrefab)
    {
        foreach (Transform obj in objects)
        {
            GameObject newIcon = Instantiate(iconPrefab, miniMapRect);
            icons[obj] = newIcon.GetComponent<RectTransform>();
        }
    }
}
