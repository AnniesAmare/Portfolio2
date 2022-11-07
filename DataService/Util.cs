﻿using System.Runtime.CompilerServices;
using System.Text.Json;

namespace DataLayer;

public static class Util
{
    public static string RemoveSpaces(this string data)
    {
        return string.Concat(data.Where(c => !char.IsWhiteSpace(c)));
    }
}