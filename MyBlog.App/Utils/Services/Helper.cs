﻿namespace MyBlog.App.Utils.Services
{
    public class Helper
    {
        public static int GetIntValue(string str)
        {
            int.TryParse(str, out var value);
            return value;
        }
    }
}