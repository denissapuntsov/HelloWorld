using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI caption;
    [SerializeField] BlockOfLines[] blocks;

    string activeLine;

    private void Start()
    {
        Clear();
    }

    public void PlayBlock(string blockId)
    {
        Clear();
        StartCoroutine(TypeBlock(blockId));
    }
    IEnumerator TypeBlock(string blockId)
    {
        BlockOfLines blockToType = GetBlockWithId(blockId);

        foreach (Line line in blockToType.lines)
        {
            caption.text = line.text;
            yield return new WaitForSeconds(line.timeToDisappear);
        }
        Clear();
    }

    private BlockOfLines GetBlockWithId(string blockId)
    {
        foreach (BlockOfLines block in blocks)
        {
            if (block.id == blockId) { return block; }
        }

        Debug.LogError($"{nameof(DialogueManager)}.{nameof(GetBlockWithId)} No block with {blockId} found.");
        return null;
    }

    public void Clear()
    {
        caption.text = "";
    }
}
