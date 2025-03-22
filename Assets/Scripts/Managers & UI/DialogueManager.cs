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
        List<BlockOfLines> blocksToType = new List<BlockOfLines>();
        blocksToType.Add(GetBlockWithId(blockId));
        StartCoroutine(TypeBlocks(blocksToType));
    }

    public float GetBlocksTotalLength(string[] blockIdArray)
    {
        List<BlockOfLines> blocksToCheck = new List<BlockOfLines>();
        foreach (string blockId in blockIdArray)
        {
            blocksToCheck.Add(GetBlockWithId(blockId));
        }

        float totalLength = 0;

        foreach (BlockOfLines block in blocksToCheck)
        {
            foreach (Line line in block.lines)
            {
                totalLength += line.timeToDisappear;
            }
        }

        return totalLength;
    }

    public void PlayBlocks(string[] blockIdArray)
    {
        Clear();
        List<BlockOfLines> blocksToType = new List<BlockOfLines>();
        foreach (string blockId in blockIdArray)
        {
            blocksToType.Add(GetBlockWithId(blockId));
        }
        StartCoroutine(TypeBlocks(blocksToType));
    }

    IEnumerator TypeBlocks(List<BlockOfLines> blockList)
    {
        foreach (BlockOfLines block in blockList)
        {
            foreach (Line line in block.lines)
            {
                caption.text = line.text;
                yield return new WaitForSeconds(line.timeToDisappear);
            }
            Clear();
        }
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
