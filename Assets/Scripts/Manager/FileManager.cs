using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class FileManager : BitGameManager<FileManager>
{
    private readonly Queue<TextReadQueueItem> _textReadQueue = new Queue<TextReadQueueItem>();

    private bool _isReading;

    public void ReadText(string path, Action<string> callback)
    {
        _textReadQueue.Enqueue(new TextReadQueueItem {Path = path, Callback = callback});
        if (!_isReading)
        {
            StartCoroutine(ReadText());
        }
    }

    private IEnumerator ReadText()
    {
        _isReading = true;
        while (_textReadQueue.Count > 0)
        {
            var item = _textReadQueue.Dequeue();
            if (item.Path.Contains("://"))
            {
                var www = UnityWebRequest.Get(item.Path);
                yield return www.SendWebRequest();
                item.Callback(www.downloadHandler.text);
            }
            else
            {
                // Debug
                yield return new WaitForSeconds(.1f);
                item.Callback(File.ReadAllText(item.Path));
            }
        }
        _isReading = false;
    }
}

internal class TextReadQueueItem
{
    public string Path;
    public Action<string> Callback;
}