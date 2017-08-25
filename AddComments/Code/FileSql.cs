using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AddComments.Code;

namespace AddComments.Code
{
    /// <summary>
    /// Работа с sql файлом
    /// </summary>
    class FileSql
    {
        public static List<SplitingLine> ConvertSqlToTxt(List<string> file)
        {
            List<string> newText = new List<string>();
            List<SplitingLine> newSplitingText = new List<SplitingLine>();
            file.RemoveAll(x => Regex.IsMatch(x, "^--") || x == "");
            int fileRange = file.Count;
            for (int i = 0; i < fileRange; i += 2)
            {
                string line = file[i].Replace("comment on column ", "").Replace("comment on table ", "");
                string comment = file[i + 1].Replace("is '", "").Replace("';", "");
                newSplitingText.Add(SplitingLine.SplitLine(line, comment));
            }

            List<SplitingLine> newSplitingTextWithComments = new List<SplitingLine>();
            foreach (var line in newSplitingText)
            {
                newSplitingTextWithComments.Add(CommentSearch.Find(newSplitingText, line));
            }

            return newSplitingTextWithComments;
        }
    }
}
