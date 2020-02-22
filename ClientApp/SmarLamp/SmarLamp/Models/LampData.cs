using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace SmarLamp.Models
{
    public class LampData
    {
        public bool UsePattern { get; set; }
        public Color Color { get; set; } = new Color();
        public int Gamma { get; set; }
        public List<Pixel> Pattern { get; set; } = new List<Pixel>();
        public bool On { get; set; }

        public byte[] ToByteArray()
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this));
        }

    }
}
