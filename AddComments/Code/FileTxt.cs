using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddComments.Code
{
    /// <summary>
    /// Работа с txt файлом
    /// </summary>
    class FileTxt
    {
        /// <summary>
        /// Сформировать строки sql файла из txt
        /// </summary>
        /// <param name="file">Список строк из файла txt</param>
        /// <returns>Список строк файла sql</returns>
        public static List<string> ConvertTxtToSql(List<string> file)
        {
            List<string> newText = new List<string>();      //новый текст
            List<string> tableList = new List<string>();    //список таблиц
            tableList.Add("--Список таблиц:");

            bool tableSectionEnd = false;

            FileLine fileLine = new FileLine();
            foreach (string line in file)
            {
                if (line.Contains("Добавить комментарии"))
                {
                    string newLine = "-- " + line.Replace("Добавить комментарии", "Добавление комментариев");
                    newText.Add(newLine);
                    continue;
                }
                fileLine.CurrLine = line;
                switch (line)
                {
                    case "Таблицы:":
                        string newLine = "-- " + line;
                        newText.Add(newLine);
                        break;
                    case "Колонки:":
                        tableSectionEnd = true;
                        newLine = "-- " + line;
                        newText.Add(newLine);
                        break;
                    case "":
                        newText.Add(line);
                        break;
                    default:
                        if (tableSectionEnd)
                        {
                            string tableName;
                            if (fileLine.IsNew(out tableName))
                            {

                                tableList.Add("-- " + tableName);
                                newText.Add("-- Таблица " + tableName);
                            }
                            newLine = String.Format("comment on column " + line + "\nis '{0}';", FileLine.DefaultCommentString);
                            newText.Add(newLine);
                        }
                        else
                        {
                            newLine = "comment on table " + line + "\nis 'TEXT';";
                            newText.Add(newLine);
                        }
                        break;

                }
            }

            tableList.Add("");
            tableList.AddRange(newText);

            //string writePath = readPath.Replace(".txt", ".sql");
            /*
            using (StreamWriter sw = new StreamWriter(writePath, false, Encoding.Default))
            {
                foreach (string line in tableList)
                    sw.WriteLine(line);
                sw.WriteLine("");
                foreach (string line in newText)
                    sw.WriteLine(line);
            }
            return "Готово";
            */
            return tableList;
        }
        /// <summary>
        /// Сформировать строки sql файла из txt с комментариями
        /// </summary>
        /// <param name="file">Список строк из файла txt с комментариями</param>
        /// <returns>Список строк файла sql</returns>
        public static List<string> ConvertTxtToSql(List<SplitingLine> file)
        {
            List<string> newText = new List<string>();      //новый текст
            List<string> tableList = new List<string>();    //список таблиц
            tableList.Add("--Список таблиц:");

            bool tableSectionEnd = false;

            FileLine fileLine = new FileLine();
            foreach (var splitLine in file)
            {
                
                List<SplitingLine.CommentFrom> comments;
                tableSectionEnd = splitLine.Column != null;
                string line = SplitingLine.JoinLine(splitLine, out comments);
                fileLine.CurrLine = line;
                switch (line)
                {
                    case "Таблицы:":
                        string newLine = "-- " + line;
                        newText.Add(newLine);
                        break;
                    case "Колонки:":
                        tableSectionEnd = true;
                        newLine = "-- " + line;
                        newText.Add(newLine);
                        break;
                    case "":
                        newText.Add(line);
                        break;
                    default:
                        if (tableSectionEnd)
                        {
                            string tableName;
                            if (fileLine.IsNew(out tableName))
                            {

                                tableList.Add("-- " + tableName);
                                newText.Add("-- Таблица " + tableName);
                            }
                            if (splitLine.Comments.Count == 0)
                            {
                                newLine = String.Format("comment on column " + line + "\nis '{0}';", splitLine.Comment);
                            }
                            else
                            {
                                string commentList = "";
                                if (splitLine.Comments.Count > 1)
                                    commentList += "\n-- Несколько вариантов:";
                                foreach (var comment in splitLine.Comments)
                                {
                                    commentList += String.Format("\n-- Из таблицы {1} \nis '{0}';", comment.Value, comment.Table);
                                }

                                newLine = "comment on column " + line + commentList;
                            }
                            newText.Add(newLine);
                        }
                        else
                        {
                            newLine = "comment on table " + line + "\nis 'TEXT';";
                            newText.Add(newLine);
                        }
                        break;

                }
            }

            tableList.Add("");
            tableList.AddRange(newText);

            //string writePath = readPath.Replace(".txt", ".sql");
            /*
            using (StreamWriter sw = new StreamWriter(writePath, false, Encoding.Default))
            {
                foreach (string line in tableList)
                    sw.WriteLine(line);
                sw.WriteLine("");
                foreach (string line in newText)
                    sw.WriteLine(line);
            }
            return "Готово";
            */
            return tableList;
        }
    }
}
