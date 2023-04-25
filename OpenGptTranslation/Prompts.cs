using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGptTranslation
{
    public class Prompts
    {
        public static string GetNouns(string text)
        {
            return
                $"""
                #Requirements:
                将以下内容中的名词例如人名,地名,或者其他名称解析出来.每一个结果使用','隔开

                #Text:
                {text}

                #Result:
                """;
        }
            

        public static string GetNounsTranslations(string language, string text)
        {
            return 
                $"""
                #Requirements:
                将以下内容中的名词例如人名,地名,或者其他名称解析出来. 并翻译成{language}, 每一行输出一个结果,每个结果的原词与翻译结果使用':'隔开

                #Text:
                {text}

                #Result:
                """;
        }

        public static string GetTranslation(string language, string text, IEnumerable<KeyValuePair<string, string>> nouns)
        {
            return 
                $"""
                #Requirements:
                将指定文本中翻译成{language}, 并在翻译中使用指定的名词映射.

                #Text:
                {text}

                #NounsMapping:
                {string.Join('\n', nouns.Select(noun => $"{noun.Key}: {noun.Value}"))}

                #Result:
                """;
        }
    }
}
