// #define LOCAL_TEST

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public static class SaveService
{
#if LOCAL_TEST
    private static readonly ISaveClient Client = new PlayerPrefClient();
#else
    private static readonly ISaveClient Client = new CloudSaveClient();
#endif

    private static string GetSlotName(int slot) => $"slot{slot}";

    public static async Task<CharacterData> GetSlot(int index)
    {
        return await Client.Load<CharacterData>(GetSlotName(index));
    }

    public static async Task<List<CharacterData>> GetSlots()
    {
        var slotData = await Client.Load<CharacterData>(GetSlotName(0), GetSlotName(1), GetSlotName(2));
        return slotData.ToList();
    }

    public static async Task SaveSlotData(CharacterData data)
    {
        await Client.Save(GetSlotName(data.Slot), data);
    }

    public static async Task DeleteSlotData(int slot)
    {
        await Client.Delete(GetSlotName(slot));
    }

    #region Example client usage

    private class ExampleObject
    {
        public string Some;
        public int Stuff;
    }

    public static async void SimpleExample()
    {
        // Save primitive
        await Client.Save("one", "just a string");

        // Load
        var stringData = await Client.Load<string>("one");

        // Save complex
        await Client.Save("one", new ExampleObject { Some = "Example", Stuff = 420 });

        // Load complex
        var objectData = await Client.Load<ExampleObject>("one");

        // Delete
        await Client.Delete("one");

        // Save multiple
        await Client.Save(("one", new ExampleObject { Some = "More", Stuff = 69 }),
            ("two", "string data"),
            ("three", "Another set"));

        // Load multiple. Restricted to same type
        var multipleData = await Client.Load<string>("two", "three");

        // Delete all
        await Client.DeleteAll();
    }

    #endregion
}