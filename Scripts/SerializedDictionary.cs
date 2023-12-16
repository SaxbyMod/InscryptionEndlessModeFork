using DiskCardGame;

namespace EndlessMode
{
    [Serializable]
    internal class SerializedDictionary
    {
        public int[] keys;
        public int[] values;

        public SerializedDictionary(IDictionary<Opponent.Type, int> dictionary)
        {
            keys = new int[dictionary.Count];
            values = new int[dictionary.Count];

            int i = 0;
            foreach (KeyValuePair<Opponent.Type, int> kvp in dictionary)
            {
                keys[i] = (int)kvp.Key;
                values[i] = kvp.Value;
                i++;
            }
        }

        public void LoadIntoDictionary(Dictionary<Opponent.Type, int> dictionary)
        {
            for (int i = 0; i < keys.Length; i++)
            {
                dictionary.Add((Opponent.Type)keys[i], values[i]);
            }
        }
    }
}