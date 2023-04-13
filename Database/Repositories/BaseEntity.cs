using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace OnlinePreferance2_api.Database.Repositories
{
    public class BaseEntity
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
    }
}
