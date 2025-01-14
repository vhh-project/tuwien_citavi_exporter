﻿using System;
using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Shell;

public class Groups
{
	public static void Export(String path)
	{
		Project activeProject = Program.ActiveProjectShell.Project;
		string FOLDERPATH = path+ "\\groups.json";
		try
		{
		
			System.IO.File.WriteAllBytes(FOLDERPATH, new byte[0]);
			char[] charsToTrim = { ' ', '“', '”', };
			int count = 0;

			using (System.IO.StreamWriter file = new System.IO.StreamWriter(FOLDERPATH, true))
			{
				file.WriteLine("{\n\"keywords\":[");


				foreach (var key in activeProject.Groups)
				{
					string temp = "";
					if (count > 0)
					{
						temp = ",\n" +
						"{\n" +
						"\"Keyword\":\"" + JavaScriptStringEncode(key.FullName.Trim(charsToTrim),false) + "\"\n" +
                        ", \"Notes\":\"" + JavaScriptStringEncode(key.Notes.Trim(charsToTrim), false) + "\"\n" +
						"}";
					}
					else
					{
						temp = "{\n" +
						"\"Keyword\":\"" + JavaScriptStringEncode(key.FullName.Trim(charsToTrim), false) + "\"\n" +
						", \"Notes\":\"" + JavaScriptStringEncode(key.Notes.Trim(charsToTrim), false) + "\"\n" +
						"}";
                    }




					file.WriteLine(temp);


					count++;

				}


				file.WriteLine("]\n}");

			}
			DebugMacro.WriteLine("Success");
		}
		catch (Exception e)
		{
			DebugMacro.WriteLine("Exception:" + e.Message);
			DebugMacro.WriteLine(e.StackTrace);
		}
	}

    public static string JavaScriptStringEncode(string value, bool addDoubleQuotes)
    {
        if (string.IsNullOrEmpty(value))
            return addDoubleQuotes ? "\"\"" : string.Empty;

        int len = value.Length;
        bool needEncode = false;
        char c;
        for (int i = 0; i < len; i++)
        {
            c = value[i];

            if (c >= 0 && c <= 31 || c == 34 || c == 39 || c == 60 || c == 62 || c == 92)
            {
                needEncode = true;
                break;
            }
        }

        if (!needEncode)
            return addDoubleQuotes ? "\"" + value + "\"" : value;

        var sb = new System.Text.StringBuilder();
        if (addDoubleQuotes)
            sb.Append('"');

        for (int i = 0; i < len; i++)
        {
            c = value[i];
            if (c >= 0 && c <= 7 || c == 11 || c >= 14 && c <= 31 || c == 39 || c == 60 || c == 62)
                sb.AppendFormat("\\u{0:x4}", (int)c);
            else switch ((int)c)
                {
                    case 8:
                        sb.Append("\\b");
                        break;

                    case 9:
                        sb.Append("\\t");
                        break;

                    case 10:
                        sb.Append("\\n");
                        break;

                    case 12:
                        sb.Append("\\f");
                        break;

                    case 13:
                        sb.Append("\\r");
                        break;

                    case 34:
                        sb.Append("\\\"");
                        break;

                    case 92:
                        sb.Append("\\\\");
                        break;

                    default:
                        sb.Append(c);
                        break;
                }
        }

        if (addDoubleQuotes)
            sb.Append('"');

        return sb.ToString();
    }

}
