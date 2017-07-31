using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonToCsv
{
    public class JsonToCsvConverter
    {
        private readonly List<String> _keys = new List<String>();

        public String Convert(String jsonStr)
        {
            var obj = JObject.Parse(jsonStr);
            return ToFlat(obj, "root");

        }
        private String ToFlat(JObject obj, String parent)
        {
            String result = null;

            foreach (var item in obj)
            {
                if (item.Value.Type == JTokenType.Object)
                {
                    var child = (JObject)item.Value;
                    var tmp = ToFlat(child, item.Key);
                    result += tmp;
                }
                else if (item.Value.Type == JTokenType.Array)
                {
                    var jarray = (JArray)item.Value;
                    if (jarray.Count == 0 && !_keys.Contains(item.Key))
                    {
                        result += $"'{item.Key}':{new JArray()},";
                        _keys.Add(item.Key);
                    }
                    else
                    {
                        foreach (var jitem in jarray)
                        {
                            if (jitem.HasValues)
                            {
                                var jchild = (JObject)jitem;
                                String tmp = ToFlat(jchild, item.Key);
                                result += tmp;
                            }
                            else if (!_keys.Contains(item.Key))
                            {
                                result += String.Format("'{0}':{1},", item.Key, new JArray() { jitem });
                                _keys.Add(item.Key);
                            }
                        }
                    }
                }
                else
                {
                    var value = item.Value ?? " ";

                    if (String.IsNullOrEmpty(parent) && !_keys.Contains(item.Key))
                    {
                        result += String.Format("'{0}':\"{1}\",", item.Key, value);
                        _keys.Add(item.Key);
                    }
                    else if (!_keys.Contains(parent + "_" + item.Key))
                    {
                        result += String.Format("'{0}_{1}':'{2}',", parent, item.Key, value);
                        _keys.Add(parent + "_" + item.Key);
                    }
                }
            }
            return result;
        }
    }
}