using System.Text.Json.Serialization;

namespace NikuAPI.Entities
{
    public class User
    {
        public string FullName { get; set; }

        [JsonIgnore]
        public byte[] PassWord { get; set; }
    }
}
