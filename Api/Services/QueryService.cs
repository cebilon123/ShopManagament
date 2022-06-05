using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Api.Models;
using CsvHelper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Api.Services
{
    public class QueryService
    {
        private readonly IConfiguration _configuration;

        public QueryService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public MemoryStream ExecuteQuery(string sql)
        {
            var connectionString = _configuration["connection"];
            using var stream = new MemoryStream();
            using var textWriter = new StreamWriter(stream);
            using var conn = new SqlConnection(connectionString);
            using var cmd = conn.CreateCommand();
            conn.Open();
            cmd.CommandText = sql;
            using var reader = cmd.ExecuteReader();
            using var csvWriter = new CsvWriter(textWriter, CultureInfo.InvariantCulture);
            var records = new List<dynamic>();
            while (reader.Read())
            {
                var fieldsAndValues = Enumerable.Range(0, reader.FieldCount)
                    .Select(idx => (reader.GetName(idx), reader.GetValue(idx)))
                    .ToArray();

                var @object = new System.Dynamic.ExpandoObject();
                foreach (var field in fieldsAndValues)
                {
                    ((IDictionary<String, Object>)@object).Add(field.Item1, field.Item2);
                }
                records.Add(@object);
            }
            
            csvWriter.WriteRecords(records);

            conn.Close();
            return stream;
        }
    }
}