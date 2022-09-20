using Newtonsoft.Json;

namespace Scuti.GraphQL{
    public class GQLQuery {
        public string query;

        public GQLQuery(string query = null){
            this.query = query == null ? "" : query;
        }

        public static GQLQuery FromString(string query){
            return new GQLQuery(query);
        }

        public static GQLQuery FromObject(object obj){
            return new GQLQuery(JsonConvert.SerializeObject(obj));
        }
    }
}
