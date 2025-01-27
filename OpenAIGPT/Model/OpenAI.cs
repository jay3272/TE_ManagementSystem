﻿using System.Diagnostics;
using System.Text.Json.Serialization;

namespace OpenAIGPT.Model
{
    public class OpenAI
    {
        public class CompletionRequest
        {
            [JsonPropertyName("model")]
            public string? Model
            {
                get;
                set;
            }
            [JsonPropertyName("prompt")]
            public string? Prompt
            {
                get;
                set;
            }
            [JsonPropertyName("max_tokens")]
            public int? MaxTokens
            {
                get;
                set;
            }
        }

        public class CompletionResponse
        {
            [JsonPropertyName("choices")]
            public List<ChatGPTChoice>? Choices
            {
                get;
                set;
            }
            [JsonPropertyName("usage")]
            public ChatGPTUsage? Usage
            {
                get;
                set;
            }
        }

        public class ChatGPTUsage
        {
            [JsonPropertyName("prompt_tokens")]
            public int PromptTokens
            {
                get;
                set;
            }
            [JsonPropertyName("completion_token")]
            public int CompletionTokens
            {
                get;
                set;
            }
            [JsonPropertyName("total_tokens")]
            public int TotalTokens
            {
                get;
                set;
            }
        }
        [DebuggerDisplay("Text = {Text}")]

        public class ChatGPTChoice
        {
            [JsonPropertyName("text")]
            public string? Text
            {
                get;
                set;
            }
        }

    }
}
