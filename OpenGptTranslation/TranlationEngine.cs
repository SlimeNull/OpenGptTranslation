using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenAI;
using OpenAI.Models;

namespace OpenGptTranslation
{
    public class TranslationEngine
    {
        public TranslationEngine(OpenAIClient client)
        {
            Client = client;
        }

        public OpenAIClient Client { get; }

        public Dictionary<string, string> Nouns { get; } =
            new Dictionary<string, string>();

        public async Task<string[]> GetNounsAsync(string text)
        {
            string prompt =
                Prompts.GetNouns(text);

            var result =
                await Client.CompletionsEndpoint.CreateCompletionAsync(prompt, temperature: 0, topP: 0, model: Model.Davinci, maxTokens: 1000);

            var choice =
                result.Completions.FirstOrDefault();

            if (choice == null)
                return Array.Empty<string>();

            return choice.Text
                .Trim()
                .Split(',')
                .Select(noun => noun.Trim())
                .ToArray();
        }

        public async Task<Dictionary<string, string>> GetNounsTranslationAsync(string text, string language)
        {
            string prompt =
                Prompts.GetNounsTranslations(language, text);

            var result =
                await Client.CompletionsEndpoint.CreateCompletionAsync(prompt, temperature: 0, topP: 0, model: Model.Davinci, maxTokens: 1000);

            var choice =
                result.Completions.FirstOrDefault();

            if (choice == null)
                return new Dictionary<string, string>();

            return choice.Text
                .Split("\n")
                .Select(line => line.Trim())
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Where(line => line.IndexOf(':') >= 0)
                .ToDictionary(line => line.Substring(0, line.IndexOf(':')).Trim(), line => line.Substring(line.IndexOf(':') + 1).Trim());
        }

        public async Task<string?> TranslateAsync(string language, string text, Dictionary<string, string> nouns)
        {
            string prompt =
                Prompts.GetTranslation(language, text, nouns);

            var result =
                await Client.CompletionsEndpoint.CreateCompletionAsync(prompt, temperature: 0, topP: 0, model: Model.Davinci, maxTokens: 1000);

            var choice =
                result.Completions.FirstOrDefault();

            if (choice == null)
                return null;

            return choice.Text.Trim();
        }

        public async Task StreamTranslateAsync(string language, string text, Dictionary<string, string> nouns, Action<string> resultHandler)
        {
            string prompt =
                Prompts.GetTranslation(language, text, nouns);

            StringBuilder sb = new StringBuilder();

            await Client.CompletionsEndpoint.StreamCompletionAsync(response =>
            {
                string? content = response.Completions.FirstOrDefault()?.Text;
                if (!string.IsNullOrEmpty(content))
                {
                    sb.Append(content);

                    while (sb.Length > 0 && char.IsWhiteSpace(sb[0]))
                        sb.Remove(0, 1);

                    resultHandler.Invoke(sb.ToString());
                }
            }, prompt, temperature: 0, topP: 0, model: Model.Davinci, maxTokens: 1000);
        }

        public async Task<string> NextAsync(string language, string text)
        {
            var newNouns =
                    await GetNounsTranslationAsync("Simplified Chinese", text);

            foreach (var newNoun in newNouns)
            {
                if (!Nouns.ContainsKey(newNoun.Key))
                    Nouns[newNoun.Key] = newNoun.Value;
            }

            Dictionary<string, string> nounsForTranslation =
                new Dictionary<string, string>();

            foreach (var noun in newNouns)
                nounsForTranslation.Add(noun.Key, Nouns[noun.Key]);

            string? translation =
                await TranslateAsync(language, text, nounsForTranslation);

            if (translation == null)
                return text;

            return translation;
        }

        public async Task StreamNext(string language, string text, Action<string> resultHandler)
        {
            var newNouns =
                    await GetNounsTranslationAsync("Simplified Chinese", text);

            foreach (var newNoun in newNouns)
            {
                if (!Nouns.ContainsKey(newNoun.Key))
                    Nouns[newNoun.Key] = newNoun.Value;
            }

            Dictionary<string, string> nounsForTranslation =
                new Dictionary<string, string>();

            foreach (var noun in newNouns)
                nounsForTranslation.Add(noun.Key, Nouns[noun.Key]);

            await StreamTranslateAsync(language, text, nounsForTranslation, resultHandler);
        }
    }
}
