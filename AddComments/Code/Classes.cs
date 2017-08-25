using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddComments.Code
{
    class Classes
    {
    }

    public class CommentSearch
    {
        public static SplitingLine Find(List<SplitingLine> inputText, SplitingLine inputLine)
        {
            SplitingLine outputLine = inputLine;
            outputLine.Comments = new List<SplitingLine.CommentFrom>();
            if (inputLine.Comment == FileLine.DefaultCommentString)
            {
                foreach (SplitingLine line in inputText)
                {
                    if (line.Column == inputLine.Column && line.Table != inputLine.Table && line.Comment != FileLine.DefaultCommentString)
                    {
                        if (!inputLine.Comments.Exists(x => x.Value == line.Comment))
                        {
                            SplitingLine.CommentFrom comment = new SplitingLine.CommentFrom()
                            {
                                Value = line.Comment,
                                Table = line.Table
                            };
                            outputLine.Comments.Add(comment);
                        }
                    }
                }
            }
            return outputLine;
        }
    }

    /// <summary>
    /// строка разделенная на БД, Таблицу, Столбец, Комментарий
    /// </summary>
    public class SplitingLine
    {
        public string Base { get; set; }
        public string Table { get; set; }
        public string Column { get; set; }
        public string Comment { get; set; }
        public List<CommentFrom> Comments { get; set; }

        public class CommentFrom
        {
            public string Value { get; set; }
            public string Table { get; set; }
        }

        public static SplitingLine SplitLine(string inputLine, string comment = "")
        {
            string[] spliting = inputLine.Split('.');
            if (spliting.Count() == 2)
                return new SplitingLine()
                {
                    Base = spliting[0],
                    Table = spliting[1],
                };
            if (spliting.Count() == 3 && (comment == "" /*|| comment == FileLine.DefaultCommentString*/))
                return new SplitingLine()
                {
                    Base = spliting[0],
                    Table = spliting[1],
                    Column = spliting[2],
                };
            if (spliting.Count() == 3 && (comment != "" /*|| comment != FileLine.DefaultCommentString*/))
                return new SplitingLine()
                {
                    Base = spliting[0],
                    Table = spliting[1],
                    Column = spliting[2],
                    Comment = comment,
                };
            return new SplitingLine();
        }
        public static string JoinLine(SplitingLine inputLine, out List<CommentFrom> comments)
        {
            comments = new List<CommentFrom>();
            if (inputLine.Comments != null)
                comments = inputLine.Comments;

            string result;
            if (inputLine.Column == null)
                result = inputLine.Base + '.' + inputLine.Table;
            else
                result = inputLine.Base + '.' + inputLine.Table + '.' + inputLine.Column;

            return result;
        }
    }

    /// <summary>
    /// Строка текстового файла
    /// </summary>
    public class FileLine
    {
        public static string DefaultCommentString = "TEXT";

        private string _currLine;
        private string _prevLine;

        public string CurrLine
        {
            get { return _currLine; }
            set
            {
                _prevLine = _currLine;
                _currLine = value;
            }
        }
        public string PrevLine
        {
            get { return _prevLine; }
            //set { }
        }
        public FileLine()
        {
            _currLine = "";
            _prevLine = "";
        }

        //check if table name on next line is new
        public bool IsNew(out string tableName)
        {
            tableName = "";
            if (GetTableName(_prevLine) == GetTableName(_currLine))
                return false;
            tableName = GetTableName(_currLine);
            return true;
        }
        //get name of table
        public string GetTableName(string line)
        {
            if (line == "")
                return "";
            string[] split = line.Split('.');
            string tableName = split.Count() == 1 ? "" : split[1];
            return tableName;
        }
    }
}
