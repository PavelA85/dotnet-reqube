﻿using System;
using System.IO;
using System.Text;

namespace ReQube.Utils
{
    public static class FileUtils
    {
        // See https://stackoverflow.com/questions/1546419/convert-file-path-to-a-file-uri 
        // for details on why a custom function is needed
        public static string FilePathToFileUrl(string filePath)
        {
            var uri = new StringBuilder();
            var finalFilePath = Path.IsPathRooted(filePath) ? filePath : Path.GetFullPath(filePath);

            foreach (char ch in finalFilePath)
            {
                if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z') || (ch >= '0' && ch <= '9') ||
                  ch == '+' || ch == '/' || ch == ':' || ch == '.' || ch == '-' || ch == '_' || ch == '~' ||
                  ch > '\xFF')
                {
                    uri.Append(ch);
                }
                else if (ch == Path.DirectorySeparatorChar || ch == Path.AltDirectorySeparatorChar)
                {
                    uri.Append('/');
                }
                else
                {
                    uri.Append(String.Format($"%{(int) ch:X2}"));
                }
            }

            if (uri.Length >= 2 && uri[0] == '/' && uri[1] == '/')
            {
                // UNC path
                uri.Insert(0, "file:");
            }
            else
            {
                uri.Insert(0, "file:///");
            }

            return uri.ToString();
        }

        public static (int StartColumn, int EndColumn) FindLineOffset(
            string content, int globalStart, int globalEnd)
        {
            var endOfLineIndex = content.LastIndexOf('\n', globalStart);

            // Some R# issues like statement termination starts at the end of the line, SQ throws an error in this case
            // So we should point to the last char of the line
            if (endOfLineIndex == globalStart)
            {
                return (globalStart - 1, globalStart - 1);
            }

            int lineStart = endOfLineIndex + 1;
            int startColumn = globalStart - lineStart;
            int endColumn = globalEnd - lineStart;

            return (startColumn, endColumn);
        }
    }
}