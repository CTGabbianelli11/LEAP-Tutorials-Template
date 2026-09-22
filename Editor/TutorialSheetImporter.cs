#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace LEAPGroup.Tutorials
{
    public static class TutorialSheetImporter
    {
        static string fullCSV;
        static List<SheetRow> sheetRows = new List<SheetRow>();

        static readonly Regex SpreadsheetIdPattern = new Regex(@"/spreadsheets/d/([a-zA-Z0-9_-]+)");
        static readonly Regex GidPattern = new Regex(@"[#&?]gid=([0-9]+)");

        // Splits a pasted Google Sheets tab address into its two halves. Both are required:
        // a bare gid or an id with no tab is a wiring error the developer should see rather
        // than a request that 404s or silently pulls the wrong tab.
        public static bool TryParseSheetUrl(string url, out string spreadsheetId, out string gid)
        {
            spreadsheetId = null;
            gid = null;

            if (string.IsNullOrWhiteSpace(url))
            {
                return false;
            }

            Match idMatch = SpreadsheetIdPattern.Match(url);
            Match gidMatch = GidPattern.Match(url);

            if (!idMatch.Success || !gidMatch.Success)
            {
                return false;
            }

            spreadsheetId = idMatch.Groups[1].Value;
            gid = gidMatch.Groups[1].Value;
            return true;
        }

        public async static void CreateTutorials(string spreadsheetId, string gid, string assetPath)
        {
            FindFolder(assetPath);

            string fullUrl = $"https://docs.google.com/spreadsheets/d/{spreadsheetId}/export?format=csv&gid={gid}";

            using var www = UnityWebRequest.Get(fullUrl);

            var operation = www.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Tutorial sheet import failed ({www.result}): {www.error}\n{fullUrl}");
                return;
            }

            fullCSV = www.downloadHandler.text;

            BuildSheetRows(fullCSV);
            CheckForTutorialObject(assetPath + "/Tutorials");
        }

        public static void FindFolder(string path)
        {
            if (!AssetDatabase.IsValidFolder(path + "/Tutorials"))
                AssetDatabase.CreateFolder(path, "Tutorials");
        }

        public static void BuildSheetRows(string csv)
        {
            Debug.Log(csv);

            sheetRows.Clear();
            string[] rows = csv.Split('\n');

            int index = 0;
            foreach (string row in rows)
            {
                List<string> cells = ParseCSVLine(row);

                if (cells.Count >= 3)
                {
                    cells[2] = cells[2].Replace(';', ',');

                    SheetRow temp = new SheetRow(index, cells[0], cells[1], cells[2]);
                    sheetRows.Add(temp);
                    index++;
                }
                else
                {
                    Debug.LogWarning($"Row doesn't have enough cells: {row}");
                }
            }
        }

        private static List<string> ParseCSVLine(string line)
        {
            List<string> result = new List<string>();
            bool inQuotes = false;
            string currentCell = "";

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (c == '"')
                {
                    if (i + 1 < line.Length && line[i + 1] == '"')
                    {
                        currentCell += '"';
                        i++;
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (c == ',' && !inQuotes)
                {
                    result.Add(currentCell);
                    currentCell = "";
                }
                else
                {
                    currentCell += c;
                }
            }

            result.Add(currentCell);
            return result;
        }

        public static void CheckForTutorialObject(string path)
        {
            List<TutorialObject> list = new List<TutorialObject>();
            string[] files = Directory.GetFiles(path, "*.asset", SearchOption.TopDirectoryOnly);

            foreach (string file in files)
            {
                list.Add(AssetDatabase.LoadAssetAtPath(file, typeof(TutorialObject)) as TutorialObject);
            }

            list = list.OrderBy(x => x.name).ToList();

            TutorialObject tutorialObject;

            foreach (SheetRow row in sheetRows)
            {
                row.title = ProcessSpecialCharacters(row.title);
                row.description = ProcessSpecialCharacters(row.description);

                if (list.Contains(list.Find(x=>x.name == row.key)))
                {
                    tutorialObject = list.Find(x => x.name == row.key);
                    string assetPath = AssetDatabase.GetAssetPath(tutorialObject);
                    AssetDatabase.RenameAsset(assetPath, row.key);
                }
                else
                {
                    tutorialObject = ScriptableObject.CreateInstance<TutorialObject>();
                    AssetDatabase.CreateAsset(tutorialObject, path + "/" + row.key+".asset");
                }

                tutorialObject.title = row.title;
                tutorialObject.description = row.description;

                EditorUtility.SetDirty(tutorialObject);
            }

            AssetDatabase.SaveAssets();
        }

        private static string ProcessSpecialCharacters(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            text = text.Replace("[xb]", "X̄");
            text = text.Replace("[xdb]", "X̿");
            text = text.Replace("[sigma]", "σ");
            text = text.Replace("[Sigma]", "Σ");
            text = text.Replace("[alpha]", "α");
            text = text.Replace("[beta]", "β");
            text = text.Replace("[mu]", "μ");
            text = text.Replace("[delta]", "δ");

            return text;
        }
    }

    public class SheetRow
    {
        public int index;
        public string key;
        public string title;
        public string description;

        public SheetRow(int index, string key, string title, string description)
        {
            this.index = index;
            this.key = key;
            this.title = title;
            this.description = description;
        }
    }
}
#endif