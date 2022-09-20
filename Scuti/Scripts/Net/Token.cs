using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Scuti.GraphQL
{
    /// <summary>
    /// A JToken wrapper that represents leaf of branch in the GQL response JSON
    /// </summary>
    public class GQLToken
    {
        readonly JToken JToken;

        public GQLToken(JToken token)
        {
            JToken = token;
        }

        public GQLToken this[string key]
        {
            get
            {
                // If there is no '.', this is not a heirarchical retrieval
                if (!key.Contains('.'))
                {
                    // Check if InnerToken is an array
                    // if so, cast to int and return using that as the index
                    if (JToken is JArray)
                    {
                        int numeric = int.Parse(key);
                        return new GQLToken(JToken[numeric]);
                    }
                    else
                        return new GQLToken(JToken[key]);
                }

                // If there are '.' in the key, this is heirarchical
                var splits = key.Split('.');
                // Split and start from the first split
                JToken token = JToken[splits[0]];
                for (int i = 1; i < splits.Length; i++)
                {
                    // If token is an array, cast the split into int and use as index
                    if (token is JArray)
                    {
                        int numeric = int.Parse(splits[i]);
                        token = token[numeric];
                    }
                    else
                        token = token[splits[i]];
                }

                return new GQLToken(token);
            }
        }

        public GQLToken this[int i]
        {
            get { return new GQLToken(JToken[i]); }
        }

        public GQLToken Get(string key)
        {
            return this[key];
        }

        public bool Contains(string key)
        {
            return JToken.Contains(key);
        }

        public T Get<T>(string key)
        {
            return Get(key).Cast<T>();
        }

        public bool IsArray
        {
            get { return (JToken is JArray); }
        }

        public T Cast<T>()
        {
            return JToken.ToObject<T>();
        }

        public override string ToString()
        {
            return JToken.ToString();
        }

        public T Deserialize<T>()
        {
            return JsonConvert.DeserializeObject<T>(ToString());
        }
    }
}