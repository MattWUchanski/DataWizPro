using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

public static class DataTransformer
{
    public static T ConvertToClass<T>(DataRow row) where T : new()
    {
        T obj = new T();

        foreach (DataColumn column in row.Table.Columns)
        {
            PropertyInfo prop = typeof(T).GetProperty(column.ColumnName);
            if (prop != null && row[column] != DBNull.Value)
            {
                object value = row[column];
                if (prop.PropertyType.IsEnum)
                {
                    // If the property is an enum, convert the value to an enum
                    value = Enum.ToObject(prop.PropertyType, value);
                }
                else
                {
                    // Otherwise, use standard conversion
                    value = Convert.ChangeType(value, prop.PropertyType);
                }
                prop.SetValue(obj, value, null);
            }
        }

        return obj;
    }

    //public static List<T> ConvertToList<T>(DataTable dataTable) where T : new()
    //{
    //    List<T> list = new List<T>();

    //    foreach (DataRow row in dataTable.Rows)
    //    {
    //        T obj = new T();
    //        foreach (DataColumn column in dataTable.Columns)
    //        {
    //            PropertyInfo prop = typeof(T).GetProperty(column.ColumnName);
    //            if (prop != null && row[column] != DBNull.Value)
    //            {
    //                object value = row[column];
    //                if (prop.PropertyType.IsEnum)
    //                {
    //                    // If the property is an enum, convert the value to an enum
    //                    value = Enum.ToObject(prop.PropertyType, value);
    //                }
    //                else
    //                {
    //                    // Otherwise, use standard conversion
    //                    value = Convert.ChangeType(value, prop.PropertyType);
    //                }
    //                prop.SetValue(obj, value, null);
    //            }
    //        }
    //        list.Add(obj);
    //    }

    //    return list;
    //}


    public static List<T> ConvertToList<T>(DataTable dataTable) where T : new()
    {
        List<T> list = new List<T>();

        foreach (DataRow row in dataTable.Rows)
        {
            T obj = new T();
            foreach (DataColumn column in dataTable.Columns)
            {
                PropertyInfo prop = typeof(T).GetProperty(column.ColumnName);
                if (prop != null && row[column] != DBNull.Value)
                {
                    prop.SetValue(obj, Convert.ChangeType(row[column], prop.PropertyType), null);
                }
            }
            list.Add(obj);
        }

        return list;
    }

    public static Dictionary<TKey, Dictionary<string, object>> ConvertToDictionary<TKey>(DataTable dataTable, string keyColumnName)
    {
        var dictionary = new Dictionary<TKey, Dictionary<string, object>>();

        foreach (DataRow row in dataTable.Rows)
        {
            var key = (TKey)Convert.ChangeType(row[keyColumnName], typeof(TKey));
            var subDictionary = new Dictionary<string, object>();

            foreach (DataColumn column in dataTable.Columns)
            {
                if (column.ColumnName != keyColumnName)
                {
                    subDictionary[column.ColumnName] = row[column] != DBNull.Value ? Convert.ChangeType(row[column], column.DataType) : null;
                }
            }

            dictionary[key] = subDictionary;
        }

        return dictionary;
    }

    public static List<Dictionary<string, object>> ConvertToDictionaryList(DataTable dataTable)
    {
        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

        foreach (DataRow row in dataTable.Rows)
        {
            var rowDictionary = new Dictionary<string, object>();
            foreach (DataColumn column in dataTable.Columns)
            {
                rowDictionary[column.ColumnName] = row[column] != DBNull.Value ? row[column] : null;
            }
            list.Add(rowDictionary);
        }

        return list;
    }

    public static Dictionary<string, object> ConvertSingleRowToDictionary(DataTable dataTable)
    {
        if (dataTable == null || dataTable.Rows.Count == 0)
        {
            throw new InvalidOperationException("DataTable is empty or null.");
        }

        var dictionary = new Dictionary<string, object>();
        DataRow row = dataTable.Rows[0];

        foreach (DataColumn column in dataTable.Columns)
        {
            dictionary[column.ColumnName] = row[column] != DBNull.Value ? row[column] : null;
        }

        return dictionary;
    }

    public static int ToInt(object value, int defaultValue = 0)
    {
        if (value == null || value is DBNull)
            return defaultValue;

        int result;
        return int.TryParse(value.ToString(), out result) ? result : defaultValue;
    }

    public static string ToString(object value)
    {
        return value == null ? string.Empty : value.ToString();
    }

    public static DateTime ToDateTime(object value, DateTime defaultValue = default(DateTime))
    {
        if (value == null || value is DBNull)
            return defaultValue;

        DateTime result;
        return DateTime.TryParse(value.ToString(), out result) ? result : defaultValue;
    }

    public static double ToDouble(object value, double defaultValue = 0.0)
    {
        if (value == null || value is DBNull)
            return defaultValue;

        double result;
        return double.TryParse(value.ToString(), out result) ? result : defaultValue;
    }

    public static bool ToBool(object value, bool defaultValue = false)
    {
        if (value == null || value is DBNull)
            return defaultValue;

        bool result;
        return bool.TryParse(value.ToString(), out result) ? result : defaultValue;
    }
}

