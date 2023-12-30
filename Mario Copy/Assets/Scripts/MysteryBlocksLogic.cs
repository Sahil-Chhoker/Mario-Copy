using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class MysteryBlocksLogic : MonoBehaviour
{
    public GameObject[] popoutItems;
    public float offset;
    private GameObject currentGameObject;

    bool canSpawnObject;
    
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite changedBlock;

    void Update()
    {
        if(spriteRenderer.sprite == changedBlock)
        {
            canSpawnObject = false;
        }
        else
        {
            canSpawnObject = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("MysteryBlockBreaker"))
        {
            FindObjectOfType<AudioManager>().Play("Block Hit");
            spriteRenderer.sprite = changedBlock;
            if(canSpawnObject)
            {
                PickRandomObject();
            }
        }
    }

    void PickRandomObject()
    {
        int rand = Random.Range(0, popoutItems.Length);
        currentGameObject = popoutItems[rand];

        Vector2 pos = new Vector2(transform.position.x, transform.position.y + offset);
        FindObjectOfType<AudioManager>().Play("Item Appear");
        Instantiate(currentGameObject, pos, Quaternion.identity);
    }
}
