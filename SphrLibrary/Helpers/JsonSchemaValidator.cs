//using Newtonsoft.Json.Schema;
using System.Text.Json.Nodes;
using Json.Schema;
using SphrLibrary.Enums;

namespace SphrLibrary.Helpers
{
    /// <summary>
    /// Jsonスキーマ バリデーションに関する機能を提供します。
    /// </summary>
    [Obsolete("実証範囲では未実装。技術調査、単体テストのみ。")]
    internal class JsonSchemaValidator
    {
        /// <summary>
        /// Jsonスキーマを用いて、Json文字列を検証します。
        /// </summary>
        /// <param name="type"></param>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static bool Validate(DocumentReferenceTypeEnum type, string jsonString)
        {
            bool result = false;

            string schema = string.Empty;
            switch (type) {
                case DocumentReferenceTypeEnum.PhysicalActivity:
                    schema = SphrConst.PHYSICAL_ACTIVITY_JSON_SCHEMA;
                    break;
                case DocumentReferenceTypeEnum.BloodPressure:
                    schema = SphrConst.BLOOD_PRESSURE_JSON_SCHEMA;
                    break;
                default:
                    break;
            }

            var jsonSchema = JsonSchema.FromText(schema);
            var evalResults = jsonSchema.Evaluate(JsonNode.Parse(jsonString));
            if (!evalResults.IsValid) {
                LogHelper.Write("Invalid document");
                if (evalResults.Errors != null && evalResults.HasErrors) {
                    foreach (var error in evalResults.Errors) {
                        LogHelper.Write(error.Key + ": " + error.Value);
                    }
                }
            } else {
                result = true;
            }

            return result;
        }
    }
}
