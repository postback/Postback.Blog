using System;

public static class TypeExtensions
{
    public static bool CanBeCastTo<T>(this Type type)
    {
        if (type == null) return false;

        if (type == typeof(T)) return true;

        return typeof(T).IsAssignableFrom(type);
    }
}