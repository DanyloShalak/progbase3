using System;
using System.Collections.Generic;
using CsvHelper;
using System.IO;
using System.Globalization;
using System.Linq;
using CsvHelper.Configuration.Attributes;




namespace ConsoleProj
{
    public class PostData
    {
        public string title;
        public string category;
        public string post_id;
    }
    public static class CSVParser
    {
        static public List<PostData> SplitCsvLine(string filePath)
        {
            StreamReader reader = new StreamReader(filePath);
            CsvReader csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csvReader.GetRecords<PostData>().ToList();
            return records;
        }
    }
}