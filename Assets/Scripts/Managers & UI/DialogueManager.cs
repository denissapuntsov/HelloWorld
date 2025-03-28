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
    [SerializeField] AudioSource globalAudioSource;

    string activeLine;
    MenuManager menuManager;

    bool dialogueIsPlaying, skipDialogueWarningShown;

    [SerializeField] GameObject skipDialogueWarningUI;

    private void Start()
    {
        Clear();
        menuManager = FindAnyObjectByType<MenuManager>();
    }

    private void Update()
    {
        // on left click, skip to next line of dialogue, unless it's the last line
        if (Input.GetMouseButtonDown(0) && dialogueIsPlaying && !menuManager.isPaused && !FindAnyObjectByType<Telephone>().firstCallEnded)
        {
            if (!skipDialogueWarningShown)
            {
                skipDialogueWarningShown = true;
                skipDialogueWarningUI.SetActive(true);
                return;
            }

            dialogueIsPlaying = false;
            FindAnyObjectByType<Telephone>().firstCallPlaying = false;
            skipDialogueWarningUI.SetActive(false);
            globalAudioSource.Stop();
            globalAudioSource.clip = null;
            StopAllCoroutines();
            Clear();
            FindAnyObjectByType<Telephone>().SkipFirstCall();
        }
    }

    public void PlayBlock(string blockId)
    {
        dialogueIsPlaying = true;
        Clear();
        List<BlockOfLines> activeBlocks = new List<BlockOfLines>();
        activeBlocks.Add(GetBlockWithId(blockId));
        StartCoroutine(TypeBlocks(activeBlocks));
    }
    public void PlayBlocks(string[] blockIdArray)
    {
        dialogueIsPlaying = true;
        Clear();
        List<BlockOfLines> activeBlocks = new List<BlockOfLines>();
        foreach (string blockId in blockIdArray)
        {
            activeBlocks.Add(GetBlockWithId(blockId));
        }
        StartCoroutine(TypeBlocks(activeBlocks));
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

    

    IEnumerator TypeBlocks(List<BlockOfLines> blockList)
    {
        foreach (BlockOfLines block in blockList)
        {
            globalAudioSource.clip = block.clip;
            globalAudioSource.Play();
            foreach (Line line in block.lines)
            {
                caption.text = line.text;
                yield return new WaitForSeconds(line.timeToDisappear);
            }
            Clear();
        }
        dialogueIsPlaying = false;
    }

    public BlockOfLines GetBlockWithId(string blockId)
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
