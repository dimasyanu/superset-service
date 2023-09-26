using IDX_DPS.Utility.Attributes;
using Microsoft.EntityFrameworkCore;
using SupersetService.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace SupersetService
{
    public class ImportRepository
    {
        private readonly Serilog.ILogger _logger;
        private readonly AppDbContext _dbContext;
        private readonly IEnumerable<string> _excludedProps;

        public ImportRepository(Serilog.ILogger logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
            _excludedProps = new[] {
                "createddate",
                "updateddate",
            };
        }

        public async Task BatchImport<T>(IEnumerable<T> items)
        {
            items = items.Take(100);
            if (!items.Any()) return;

            var modelType = typeof(T);
            var props = modelType.GetProperties()
                .Where(x => !Attribute.IsDefined(x, typeof(KeyAttribute))); // Exclude id fields

            var transactKeys = props.Where(x => Attribute.IsDefined(x, typeof(TransactionKeyAttribute))).Select(x => x.Name);
            var updateFields = new List<string>();
            var insertFields = new List<string>();

            var propsStr = string.Join(", ", props.Select(x => $"[{x.Name}]"));
            var keyConditions = "AND " + string.Join(" AND ", transactKeys.Select(x => $"x.[{x}] = y.[{x}]"));

            // Properties
            foreach (var prop in props) {
                insertFields.Add(_excludedProps.Contains(prop.Name.ToLower()) 
                    ? "GETDATE()"
                    : $"y.[{prop.Name}]"
                );

                if (transactKeys.Contains(prop.Name) || prop.Name.ToLower() == "createddate") continue;
                if (prop.Name.ToLower() == "updateddate") {
                    updateFields.Add($"x.[{prop.Name}] = GETDATE()");
                    continue;
                }
                updateFields.Add($"x.[{prop.Name}] = y.[{prop.Name}]");
            }

            // Items value mapping
            var limit = 2000;
            var totalCount = items.Count();
            var startIndex = 0;

            do {
                var values = new List<string>();
                var chunk = items.Skip(startIndex).Take(limit);
                var counter = 0;
                var valueProps = props.Where(x => !_excludedProps.Contains(x.Name.ToLower()));
                var valuePropsStr = string.Join(", ", valueProps.Select(x => $"[{x.Name}]"));
                foreach (var item in chunk) {
                    var itemValues = new List<string>();
                    foreach (var prop in valueProps) {
                        var val = prop.GetValue(item);

                        if (val == null) {
                            itemValues.Add("NULL");
                            continue;
                        }

                        var strVal = val.ToString();
                        if (strVal == null) {
                            itemValues.Add("NULL");
                            continue;
                        }

                        var propTypeName = prop.PropertyType.ToString().ToLower();
                        if (propTypeName.Contains("bool")) {
                            itemValues.Add(strVal.ToLower() == "true" ? "1" : "0");
                            continue;
                        }

                        if (prop.Name.ToLower().Contains("date")) {
                            if (string.IsNullOrEmpty(val.ToString())) {
                                itemValues.Add("NULL");
                                continue;
                            }
                            var dt = (DateTime)val;
                            itemValues.Add($"'{dt:yyyy-MM-dd}'");
                            continue;
                        }

                        if (prop.PropertyType == typeof(string)) {
                            strVal = strVal.Replace("\n", " ")
                                .Replace("'", "''");
                            itemValues.Add($"'{strVal}'");
                            continue;
                        }

                        itemValues.Add($"{strVal.Replace(",", ".")}");
                    }
                    values.Add("(" + string.Join(", ", itemValues) + ")");
                    counter++;
                }

                var updateFieldsStr = string.Join(",\n", updateFields);
                var insertFieldsStr = string.Join(",\n", insertFields);
                var valuesStr = string.Join(", \n", values.OrderBy(x => x));

                var tableName = modelType.GetCustomAttribute<TableAttribute>()?.Name ?? modelType.Name;
                var sql = $"DBCC CHECKIDENT ('{tableName}', RESEED);\nMERGE [{tableName}] WITH (SERIALIZABLE) AS x USING(VALUES\n";
                sql += valuesStr;
                sql += $"\n) AS y ({valuePropsStr}) ON 1=1 {keyConditions}";
                sql += $"\nWHEN MATCHED THEN UPDATE SET {updateFieldsStr}";
                sql += $"\nWHEN NOT MATCHED THEN INSERT ({propsStr}) VALUES ({insertFieldsStr});";
                try {
                    await _dbContext.Database.ExecuteSqlRawAsync(sql);
                    var lastRow = startIndex + limit + 1;
                    if (lastRow > totalCount) lastRow = totalCount;

                    _dbContext.SaveChanges();
                }
                catch (Exception e) {
                    _logger.Error(e.Message);
                    throw;
                }
                startIndex += limit;
            } while (startIndex < totalCount);
        }
    }
}
