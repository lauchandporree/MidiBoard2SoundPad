using System.Collections.Generic;
using Newtonsoft.Json;

namespace MidiBoard2SoundPad
{
    public class Configuration
    {
        [JsonProperty(propertyName:"keypad_name")]
        public string KeypadName;

        public Dictionary<int, int> Mapping = new();
    }
}