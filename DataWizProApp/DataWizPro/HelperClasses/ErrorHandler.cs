using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ErrorHandler
{
    // Generic method for handling and logging exceptions for methods that return a value
    public static T ExecuteWithHandling<T>(Func<T> action, T defaultValue = default)
    {
        try
        {
            return action();
        }
        catch (Exception ex)
        {
            LogError(ex);
            return defaultValue; // or rethrow the exception
        }
    }

    // Overload for void-returning methods
    public static void ExecuteWithHandling(Action action)
    {
        try
        {
            action();
        }
        catch (Exception ex)
        {
            LogError(ex);
            // Rethrow, handle differently, or do nothing
        }
    }

    // Implement the logging logic
    private static void LogError(Exception ex)
    {
        // Log the exception here
        // This could include writing to a log file, sending an email, etc.
    }
}

