using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Scuti.GraphQL {
	public class GQLResponse {
		public string Raw { get; private set; }
		readonly JObject JObject;

		public GQLResponse(string raw) {
			if (string.IsNullOrEmpty(raw))
				throw new Exception("Cannot create GraphQLResponse with null or empty raw string");

			Raw = raw;
			try {
				JObject = JObject.Parse(raw);
				if (Root == null)
					throw new Exception("Could not construct GQLResponse as the root node is not named \"data\"");
			}
			catch (Exception e) {
				throw new Exception("Could not construct GQLResponse --> ", e);
			}
		}

		GQLToken m_Root;
		public GQLToken Root {
			get {
				if(m_Root == null)
					m_Root = new GQLToken(JObject["data"]);
				return m_Root;
			}
		}

		public GQLToken this[string key] {
			get {
				if (!key.Contains('.')) {
					if (key.All(char.IsDigit)) {
						int numeric = int.Parse(key);
						return Root[numeric];
					}
					else
						return Root[key];
				}

				var splits = key.Split('.');
				var token = Root[splits[0]];
				for (int i = 1; i < splits.Length; i++) {
					if(token.IsArray) {
						int numeric = int.Parse(splits[i]);
						token = token[numeric];
					}
					else
						token = token[splits[i]];
				}

				return token;
			}
		}

        public bool TryGetValue(string key,  out JToken value)
        {
            return JObject.TryGetValue(key, out value);
        }

		public GQLToken this[int i] {
			get { return this[i.ToString()]; }
		}

        public T Cast<T>(){
            return Root.Cast<T>();
        }
                
		public override string ToString() {
			return Root.ToString();
		}
	}
}