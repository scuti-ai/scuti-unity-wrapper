using System;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;

public static class CSharpExtensions {
	public static string ToJson(this object obj) {
		return JsonConvert.SerializeObject(obj, Formatting.Indented);
	}

    public static bool IsNullOrEmpty(this string str){
        return string.IsNullOrEmpty(str);
    }

    public static T FromLast<T>(this List<T> list, int index){
        if (index >= list.Count)
            throw new IndexOutOfRangeException(index + " is out of range");
        return list[list.Count - 1 - index];
    }

    public static T Last<T>(this List<T> list){
        return list.FromLast(0);
    }

    public static void RemoveLast<T>(this List<T> list){
        var last = list.Last();
        list.Remove(last);
    }
    
    public static byte[] ToUTF8Bytes(this string str) {
        return Encoding.UTF8.GetBytes(str);
    }

    public static string ToUTF8String(this byte[] bytes) {
        return Encoding.UTF8.GetString(bytes);
    }

    public static T DeserializeJSON<T>(this string jsonString) {
        return JsonConvert.DeserializeObject<T>(jsonString);
    }

    public static string SerializeJSON(this object obj) {
        return JsonConvert.SerializeObject(obj);
    }

    public static T GetAttributeOfType<T>(this Enum enumVal) where T : System.Attribute
    {
        var type = enumVal.GetType();
        var memInfo = type.GetMember(enumVal.ToString());
        var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
        return (attributes.Length > 0) ? (T)attributes[0] : null;
    }
}
