using NewMaxApi.Enums;
using NewMaxApi.Entities.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NewMaxApi.Entities
{
    [JsonConverter(typeof(MarkupElementConverter))]
    public abstract class MarkupElement
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("from")]
        public int From { get; set; }

        [JsonPropertyName("length")]
        public int Length { get; set; }
    }


    public class MarkupElementLink : MarkupElement
    {
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;
    }

    public class MarkupElementUserMention : MarkupElement
    {
        [JsonPropertyName("user_link")]
        public string? UserLink { get; set; }

        [JsonPropertyName("user_id")]
        public long? UserId { get; set; }
    }

    public class MarkupElementUnderline : MarkupElement { }
    public class MarkupElementStrong : MarkupElement { }
    public class MarkupElementEmphasized : MarkupElement { }
    public class MarkupElementStrikethrough : MarkupElement { }
    public class MarkupElementHeading : MarkupElement { }
    public class MarkupElementHighlighted : MarkupElement { }
    public class MarkupElementMonospaced : MarkupElement { }


}
