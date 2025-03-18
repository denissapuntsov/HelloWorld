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

    public void PlayBlock(VoiceTriggerData voiceTriggerData)
    {
        Clear();
        BlockOfLines blockToType = GetBlockWithId(voiceTriggerData.blockId);
        voiceTriggerData.source.clip = blockToType.clip;
        voiceTriggerData.source.Play();
        StartCoroutine(TypeBlock(blockToType));
    }

    IEnumerator TypeBlock(BlockOfLines block)
    {
        foreach (Line line in block.lines)
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
