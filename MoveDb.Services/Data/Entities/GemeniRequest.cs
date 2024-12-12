using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MoveDb.Services.Data.Entities {

    public class GemeniRequest {
        [JsonPropertyName("contents")]
        public List<Content> Content { get; set; } // Corresponds to "contents"

        [JsonPropertyName("generationConfig")]
        public GenerationConfig GenerationConfig { get; set; } // Corresponds to "generationConfig"

        [JsonPropertyName("safetySettings")]
        public List<object> SafetySettings { get; set; } // Corresponds to "safetySettings"
    }
    public class Part {

        [JsonPropertyName("text")]
        public string Text { get; set; }
    }

    public class Content {

        [JsonPropertyName("role")]
        public string Role { get; set; } // Corresponds to "role"

        [JsonPropertyName("parts")]
        public List<Part> Parts { get; set; } // Corresponds to "parts"
    }

    public class GenerationConfig {

        [JsonPropertyName("temperature")]
        public double Temperature { get; set; } // Corresponds to "temperature"


        [JsonPropertyName("topK")]
        public int TopK { get; set; } // Corresponds to "topK"


        [JsonPropertyName("topP")]
        public double TopP { get; set; } // Corresponds to "topP"

        [JsonPropertyName("maxOutputTokens")]
        public int MaxOutputTokens { get; set; } // Corresponds to "maxOutputTokens"

        [JsonPropertyName("stopSequences")]
        public List<string> StopSequences { get; set; } // Corresponds to "stopSequences"
    }
    // Gemini Response
    public class GeminiResponse
    {

        [JsonPropertyName("candidates")]
        public List<Candidate> Candidates { get; set; }

        [JsonPropertyName("usageMetadata")]
        public UsageMetadata UsageMetadata { get; set; }

        [JsonPropertyName("modelVersion")]
        public string ModelVersion { get; set; }
    }
    public class Candidate {

        [JsonPropertyName("content")]
        public Content Content { get; set; }

        [JsonPropertyName("finishReason")]
        public string FinishReason { get; set; }

        [JsonPropertyName("index")]
        public int Index { get; set; }

        [JsonPropertyName("safetyRatings")]
        public List<SafetyRating> SafetyRatings { get; set; }
    }
    public class SafetyRating {

        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("probability")]
        public string Probability { get; set; }
    }

    public class UsageMetadata {

        [JsonPropertyName("promptTokenCount")]
        public int PromptTokenCount { get; set; }

        [JsonPropertyName("candidatesTokenCount")]
        public int CandidatesTokenCount { get; set; }

        [JsonPropertyName("totalTokenCount")]
        public int TotalTokenCount { get; set; }
    }
}
