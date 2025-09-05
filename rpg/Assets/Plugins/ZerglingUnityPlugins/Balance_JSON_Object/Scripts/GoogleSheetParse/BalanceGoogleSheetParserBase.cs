using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Defective.JSON;
using Plugins.ZerglingUnityPlugins.Balance_Total_JSON.Scripts.Configs;
using UnityEngine;
using ZerglingUnityPlugins.Tools.Scripts.Log;

namespace Plugins.ZerglingUnityPlugins.Balance_Total_JSON.Scripts.GoogleSheetParse
{
    public interface IBalanceGoogleSheetParserBase
    {
        void Setup(IBalanceConfig config);
        void ParseBalance();
    }
    
    public abstract class BalanceGoogleSheetParserBase : IBalanceGoogleSheetParserBase
    {
        protected IBalanceConfig _config;

        protected Dictionary<string, Func<IEnumerator>> _sheets;

        protected string _currentSheetName;
        protected JSONObject _json;

        protected List<string> _lines;
        private string _pageText;

        public BalanceGoogleSheetParserBase()
        {
            _sheets = new Dictionary<string, Func<IEnumerator>>();
            _json = new JSONObject();
            
            _pageText = string.Empty;
            _lines = new List<string>();
        }

        public void Setup(IBalanceConfig config)
        {
            _config = config;
        }

        public void ParseBalance()
        {
            FillSheetsDictionary();

            var coroutine = ParsingProcess();
            while (coroutine.MoveNext()) ;
        }

        protected abstract void FillSheetsDictionary();

        private IEnumerator ParsingProcess()
        {
            foreach (var sheet in _sheets)
            {
                var sheetName = sheet.Key;
                var download = DownloadProcess(sheetName);
                while (download.MoveNext())
                    yield return null;

                var fixFirstRows = FixFirstRowsProcess();
                while (fixFirstRows.MoveNext())
                    yield return null;

                var parseFunc = sheet.Value;
                var parse = ParseProcess(parseFunc);
                while (parse.MoveNext()) 
                    yield return null;
            }

            WriteJsonToFile();
        }
        
        private IEnumerator DownloadProcess(string sheetName)
        {
            LogUtils.Info(this, $"{sheetName} : downloading...");

            var googleSheetUrl = $"https://docs.google.com/spreadsheets/d/{_config.GoogleSheetId}/gviz/tq?tqx=out:csv&sheet=";

            _currentSheetName = sheetName;
            var url = $"{googleSheetUrl}{_currentSheetName}";
            var www = new WWW(url);

            while (www.MoveNext() && !www.isDone)
                yield return null;

            _pageText = www.text;
        }

        private IEnumerator FixFirstRowsProcess()
        {
            LogUtils.Info(this, $"{_currentSheetName} : fixing first rows...");

            var pageSplit = _pageText.Split('\n');

            var lines = new List<string>(pageSplit);

            var firstRow = lines[0];
            var firstRowSplit = SplitCSVLine(firstRow);

            var firstItem = firstRowSplit[0];

            if (!firstItem.Contains(" "))
            {
                LogUtils.Error(this, $"{_currentSheetName} : dont need fixing...");
                _lines.AddRange(lines);
                _pageText = string.Empty;
                yield break;
            }

            var firstRowBuilder = new StringBuilder();
            var secondRowBuilder = new StringBuilder();

            var lastIndex = firstRowSplit.Length - 1;
            
            for (int i = 0; i < firstRowSplit.Length; i++)
            {
                var item = firstRowSplit[i];

                if (string.IsNullOrEmpty(item))
                {
                    firstRowBuilder.Append("\"\"");
                    secondRowBuilder.Append("\"\"");

                    if (i != lastIndex)
                    {
                        firstRowBuilder.Append(",");
                        secondRowBuilder.Append(",");
                    }

                    continue;
                }

                var split = item.Split(' ');
                firstRowBuilder.Append($"\"{split[0]}\",");
                secondRowBuilder.Append($"\"{split[1]}\",");
            }

            lines[0] = firstRowBuilder.ToString();
            lines.Insert(1, secondRowBuilder.ToString());

            _lines.AddRange(lines);
            _pageText = string.Empty;
            yield return null;
        }

        private IEnumerator ParseProcess(Func<IEnumerator> parseFunc)
        {
            LogUtils.Info(this, $"{_currentSheetName} : {parseFunc.Method.Name} parsing...");

            var parseCoroutine = parseFunc();

            while (parseCoroutine.MoveNext())
                yield return null;
        }

        private void WriteJsonToFile()
        {
            LogUtils.Info(this, $"Writing JSON to file...");

            var projectPath = $"{Application.dataPath}/_Project";
            if (!Directory.Exists(projectPath))
                Directory.CreateDirectory(projectPath);

            var resourcesPath = $"{projectPath}/Resources";
            if (!Directory.Exists(resourcesPath))
                Directory.CreateDirectory(resourcesPath);

            var path = $"{resourcesPath}/Balance.json";
            File.WriteAllText(path, _json.ToString(true));

            LogUtils.Error(this, $"Balance parsing SUCCESS!");

            Debug.LogError(_json.ToString(true));
        }
        
        protected string[] SplitCSVLine(string line)
        {
            var substring = line.Substring(1, line.Length - 2);
            var split = substring.Split(new[] { "\",\"" }, StringSplitOptions.None);
            return split;
        }
        
        protected IEnumerator ParseAsIsDictionary()
        {
            var json = new JSONObject();

            var headers = SplitCSVLine(_lines[0]);
            var types = SplitCSVLine(_lines[1]);

            for (int i = 2; i < _lines.Count; i++)
            {
                var line = _lines[i];
                
                Debug.Log(line);
                
                var lineData = SplitCSVLine(_lines[i]);
                var isNullOrEmpty = string.IsNullOrEmpty(lineData[0]);
                if (isNullOrEmpty)
                    continue;

                var id = lineData[0];

                var jObject = new JSONObject();
                for (int j = 0; j < lineData.Length; j++)
                {
                    var header = headers[j];
                    if (string.IsNullOrEmpty(header))
                        continue;

                    jObject[header] = ParseLineData(lineData[j], types[j]);
                }

                json[id] = jObject;

                yield return null;
            }

            _json[_currentSheetName] = json;
            _lines.Clear();
            yield return null;
        }
        
        protected IEnumerator ParseAsIsConfig()
        {
            var json = new JSONObject();

            var lineData = SplitCSVLine(_lines[0]);
            var isNullOrEmpty = string.IsNullOrEmpty(lineData[0]);

            if (isNullOrEmpty)
            {
                _json[_currentSheetName] = json;
                yield break;
            }

            for (int i = 1; i < _lines.Count; i++)
            { 
                var line = _lines[i];
                
                Debug.Log(line);
                
                lineData = SplitCSVLine(line);

                isNullOrEmpty = string.IsNullOrEmpty(lineData[0]);

                if (isNullOrEmpty)
                    continue;

                var header = lineData[0];
                var type = lineData[1];
                var value = lineData[2];

                json[header] = ParseLineData(value, type);
            }

            _json[_currentSheetName] = json;
            _lines.Clear();
            yield return null;
        }
        
        // ===========================================================================================================
        // ===========================================================================================================
        // ===========================================================================================================
        // ===========================================================================================================
        // ===========================================================================================================

        #region PrimitivesParsingFunctions
        
        private JSONObject ParseLineData(string lineDataString, string dataType)
        {
            JSONObject result = null;

            switch (dataType)
            {
                case BalanceParseKeys.INT:
                    var intValue = ParseInt(lineDataString);
                    result =  JSONObject.Create(intValue);
                    break;

                case BalanceParseKeys.FLOAT:
                    var floatValue = ParseFloat(lineDataString);
                    result = JSONObject.Create(floatValue);
                    break;

                case BalanceParseKeys.BOOL:
                    var boolValue = ParseBool(lineDataString);
                    result = JSONObject.Create(boolValue);
                    break;

                case BalanceParseKeys.VECTOR_2:
                    result = ParseVector2(lineDataString);
                    break;

                case BalanceParseKeys.VECTOR_2_INT:
                    result = ParseVector2Int(lineDataString);
                    break;

                case BalanceParseKeys.VECTOR_3:
                    result = ParseVector3(lineDataString);
                    break;

                case BalanceParseKeys.VECTOR_3_INT:
                    result = ParseVector3Int(lineDataString);
                    break;

                default:
                    if (dataType.Contains(BalanceParseKeys.ARRAY))
                    {
                        result = ParseArray(lineDataString, dataType);
                        break;
                    }

                    result = JSONObject.CreateStringObject(lineDataString);
                    break;
            }

            return result;
        }

        private int ParseInt(string s)
        {
            if (string.IsNullOrEmpty(s))
                return 0;

            var result = int.Parse(s);
            return result;
        }

        private float ParseFloat(string s)
        {
            if (string.IsNullOrEmpty(s))
                return 0.0f;
            
            float result = 0f;

#if UNITY_EDITOR_WIN
            // винда не дружит с точками
            try
            {
                result = float.Parse(s);
            }
            catch (Exception e)
            {
                s = s.Replace(".", ",");
                result = float.Parse(s);
            }
#else
            // мак ось не дружит с запятыми
            if (s.Contains(","))
                s = s.Replace(",", ".");
            
            result = float.Parse(s);
#endif

            // Билл и Стив - два придурка :)

            return result;
        }

        private bool ParseBool(string s)
        {
            if (string.IsNullOrEmpty(s))
                return false;

            var result = bool.Parse(s);
            return result;
        }

        private JSONObject ParseVector2(string s)
        {
            var split = s.Split(';');
            var result = new JSONObject();
            result["x"] = JSONObject.Create(ParseFloat(split[0]));
            result["y"] = JSONObject.Create(ParseFloat(split[1]));
            return result;
        }

        private JSONObject ParseVector2Int(string s)
        {
            var split = s.Split(';');
            var result = new JSONObject();
            result["x"] = JSONObject.Create(ParseInt(split[0]));
            result["y"] = JSONObject.Create(ParseInt(split[1]));
            return result;
        }

        private JSONObject ParseVector3(string s)
        {
            var split = s.Split(';');
            var result = new JSONObject();
            result["x"] = JSONObject.Create(ParseFloat(split[0]));
            result["y"] = JSONObject.Create(ParseFloat(split[1]));
            result["z"] = JSONObject.Create(ParseFloat(split[2]));
            return result;
        }

        private JSONObject ParseVector3Int(string s)
        {
            var split = s.Split(';');
            var result = new JSONObject();
            result["x"] = JSONObject.Create(ParseInt(split[0]));
            result["y"] = JSONObject.Create(ParseInt(split[1]));
            result["z"] = JSONObject.Create(ParseInt(split[2]));
            return result;
        }

        private JSONObject ParseArray(string s, string dataType)
        {
            dataType = dataType.Substring(0, dataType.Length - 2);
            var split = s.Split(',');

            var result = new JSONObject();

            for (int i = 0; i < split.Length; i++)
            {
                var splitData = split[i];
                if (splitData[0] == ' ')
                    splitData = splitData.Substring(1);

                var arrayObject = ParseLineData(splitData, dataType);
                result.Add(arrayObject);
            }

            return result;
        }
        
        #endregion
    }
}
