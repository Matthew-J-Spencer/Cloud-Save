using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


public class PlayerPrefClient : ISaveClient
{
    public Task Delete(string key)
    {
        PlayerPrefs.DeleteKey(key);
        return Task.CompletedTask;
    }

    public Task DeleteAll()
    {
        PlayerPrefs.DeleteAll();
        return Task.CompletedTask;
    }

    public Task<T> Load<T>(string key)
    {
        var data = PlayerPrefs.GetString(key);
        if (!string.IsNullOrEmpty(data)) return Task.FromResult(JsonConvert.DeserializeObject<T>(data));
        return Task.FromResult<T>(default);
    }

    public async Task<IEnumerable<T>> Load<T>(params string[] keys)
    {
        return await Task.WhenAll(keys.Select(Load<T>));
    }

    public Task Save(string key, object value)
    {
        if (value is string s)
        {
            PlayerPrefs.SetString(key, s);
        }
        else
        {
            var data = JsonConvert.SerializeObject(value);
            PlayerPrefs.SetString(key, data);
        }

        return Task.CompletedTask;
    }

    public async Task Save(params (string key, object value)[] values)
    {
        foreach (var (key, value) in values)
        {
            await Save(key, value);
        }
    }
}