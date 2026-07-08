using System.Linq;
using Newtonsoft.Json.Linq;

/// <summary>
/// Convierte entre objetos C# planos y el formato de "fields" tipado que usa
/// la API REST de Firestore (https://firestore.googleapis.com/v1/.../documents).
/// Firestore no envía JSON plano: cada valor va envuelto según su tipo, p.ej.
/// {"stringValue": "hola"} o {"mapValue": {"fields": {...}}} para objetos anidados.
/// </summary>
public static class FirestoreValue
{
    /// <summary>Serializa un objeto C# (incluyendo objetos anidados) al formato "fields" de un documento de Firestore.</summary>
    public static JObject ToDocumentFields<T>(T obj)
    {
        JObject plain = JObject.FromObject(obj);
        return ToFields(plain);
    }

    /// <summary>Deserializa el documento devuelto por Firestore (con su envoltorio "fields") de vuelta a T.</summary>
    public static T FromDocument<T>(JObject firestoreDocument)
    {
        var fieldsToken = firestoreDocument["fields"] as JObject ?? new JObject();
        JObject plain = FromFields(fieldsToken);
        return plain.ToObject<T>();
    }

    public static JObject ToFields(JObject plain)
    {
        var fields = new JObject();
        foreach (JProperty prop in plain.Properties())
        {
            fields[prop.Name] = Encode(prop.Value);
        }
        return fields;
    }

    public static JObject Encode(JToken token)
    {
        switch (token.Type)
        {
            case JTokenType.String:
                return new JObject { ["stringValue"] = token.Value<string>() };
            case JTokenType.Boolean:
                return new JObject { ["booleanValue"] = token.Value<bool>() };
            case JTokenType.Integer:
                return new JObject { ["integerValue"] = token.Value<long>().ToString() };
            case JTokenType.Float:
                return new JObject { ["doubleValue"] = token.Value<double>() };
            case JTokenType.Null:
                return new JObject { ["nullValue"] = null };
            case JTokenType.Object:
                return new JObject { ["mapValue"] = new JObject { ["fields"] = ToFields((JObject)token) } };
            case JTokenType.Array:
                var values = new JArray();
                foreach (JToken item in (JArray)token)
                {
                    values.Add(Encode(item));
                }
                return new JObject { ["arrayValue"] = new JObject { ["values"] = values } };
            default:
                return new JObject { ["stringValue"] = token.ToString() };
        }
    }

    public static JObject FromFields(JObject fields)
    {
        var plain = new JObject();
        foreach (JProperty prop in fields.Properties())
        {
            plain[prop.Name] = Decode((JObject)prop.Value);
        }
        return plain;
    }

    public static JToken Decode(JObject valueWrapper)
    {
        JProperty prop = valueWrapper.Properties().FirstOrDefault();
        if (prop == null) return JValue.CreateNull();

        switch (prop.Name)
        {
            case "stringValue":
                return prop.Value.Value<string>();
            case "booleanValue":
                return prop.Value.Value<bool>();
            case "integerValue":
                return long.Parse(prop.Value.Value<string>());
            case "doubleValue":
                return prop.Value.Value<double>();
            case "nullValue":
                return JValue.CreateNull();
            case "mapValue":
                var mapFields = prop.Value["fields"] as JObject ?? new JObject();
                return FromFields(mapFields);
            case "arrayValue":
                var result = new JArray();
                if (prop.Value["values"] is JArray rawValues)
                {
                    foreach (JToken v in rawValues)
                    {
                        result.Add(Decode((JObject)v));
                    }
                }
                return result;
            default:
                return JValue.CreateNull();
        }
    }
}
