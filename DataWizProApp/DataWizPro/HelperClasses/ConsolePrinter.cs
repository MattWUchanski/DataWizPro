using System;
using System.Reflection;

public static class ObjectPropertyPrinter
{
    public static void PrintProperties(object obj)
    {
        if (obj == null)
        {
            Console.WriteLine("Object is null.");
            return;
        }

        PropertyInfo[] properties = obj.GetType().GetProperties();
        foreach (PropertyInfo propertyInfo in properties)
        {
            string propertyName = propertyInfo.Name;
            object propertyValue = propertyInfo.GetValue(obj, null);

            Console.WriteLine("{0}: {1}", propertyName, propertyValue);
        }
        Console.WriteLine(); // Adds an empty line for readability
    }
}

