using SphrLibrary.Entities.FHIR;
using SphrLibrary.Entities.OpenmHealth;
using SphrLibrary.Enums;
using System.Text;

namespace SphrLibrary.Helpers
{
    /// <summary>
    /// 汎用モジュールが使用する定数クラスを表します。
    /// </summary>
    internal static class SphrConst
    {
        /// <summary>
        /// 入出力ファイルのエンコードを表します。
        /// </summary>
        internal static Encoding ENCODING = new System.Text.UTF8Encoding(false); // UTF-8 BOMなし

        // ルート以下に作成するフォルダ
        internal static string SPHRDB_FOLDER = "sphrdb";
        internal static string LOG_FOLDER = "log";
        internal static string TEMP_FOLDER = "temp";

        internal static int USER_ID_STORAGE_LAYERS = 2; // 利用者IDフォルダ階層分割数
        internal static int USER_ID_STORAGE_LAYER_LENGTH = 4; // 階層ごとのフォルダのID桁数
        internal static int USER_ID_DIGITS_MIN = 8; // 利用者IDの最小(0詰め)桁数

        // zip関連
        internal static string SPHR_FILE_EXTENSION = ".sphr"; //".zip"; // 運用は.sphr（検証中は.zipにすると拡張子変えて中身見る手間が減る）
        internal static string SPHR_FILE_CONTENT_TYPE = "application/zip"; // .sphrでもzip扱い
        //internal static Regex REGEX_ZIP_FILE_NAME = new Regex(@"^sphr_[\w]+_[0-9]{17}$");
        internal static string INDEX_PHR = "index.phr";

        /// <summary>
        /// 汎用モジュールの設定クラスを保持するJsonファイル名を表します。
        /// </summary>
        internal static string SETTINGS_FILE_NAME = "SphrLibrarySettings.json";

        /// <summary>
        /// サポートしているPHRデータのコードシステムを表します。
        /// </summary>
        internal static Dictionary<DocumentReferenceTypeEnum, Coding> CODE_SYSTEM = new Dictionary<DocumentReferenceTypeEnum, Coding>() {
            {DocumentReferenceTypeEnum.BodyWeight, new Coding("omh-body-weight-2.0", "OMH体重記録", "http://phr.or.jp/fhir/doctype") },
            {DocumentReferenceTypeEnum.BloodPressure, new Coding("omh-blood-pressure-4.0", "OMH血圧記録", "http://phr.or.jp/fhir/doctype") },
            {DocumentReferenceTypeEnum.PhysicalActivity, new Coding("ieee-physical-activity-1.0", "IEEE歩数記録", "http://phr.or.jp/fhir/doctype") },
            {DocumentReferenceTypeEnum.BodyTemperature, new Coding("omh-body-temperature-3.0", "OMH体温記録", "http://phr.or.jp/fhir/doctype") },
            {DocumentReferenceTypeEnum.OxygenSaturation, new Coding("omh-oxygen-saturation-2.0", "OMH酸素飽和度記録", "http://phr.or.jp/fhir/doctype") },
            {DocumentReferenceTypeEnum.SpecificHealthCheckupHL7CDA, new Coding("10", "特定健診情報(HL7 CDA)", "1.2.392.200119.6.1001") },
            {DocumentReferenceTypeEnum.SpecificHealthCheckupFHIR, new Coding("10", "特定健診情報(FHIR)", "urn:oid:1.2.392.200119.6.1001") },
            {DocumentReferenceTypeEnum.MynaPrescription, new Coding("myna-prescription-202407", "調剤歴(マイナポータル)", "http://phr.or.jp/fhir/doctype") },
            {DocumentReferenceTypeEnum.JahisPrescription, new Coding("JAHISTC-08", "調剤歴(お薬手帳QRコード)", "http://phr.or.jp/fhir/doctype") },
            {DocumentReferenceTypeEnum.MynaVaccination, new Coding("myna-vaccination-202407", "予防接種歴", "http://phr.or.jp/fhir/doctype") },
        };

        /// <summary>
        /// サポートしているPHRデータのスキーマ（規格、ファイル名フォーマット、MIMEタイプ）を表します。
        /// </summary>
        internal static Dictionary<DocumentReferenceTypeEnum, (SchemaId schema, string format, string contentType)> SCHEMA = new Dictionary<DocumentReferenceTypeEnum, (SchemaId, string, string)>() {
            {DocumentReferenceTypeEnum.BodyWeight, (new SchemaId("omh", "body-weight", "2.0"), "body-weight_{0}.json", "application/json") },
		    {DocumentReferenceTypeEnum.BloodPressure, (new SchemaId("omh", "blood-pressure", "4.0"), "blood-pressure_{0}.json", "application/json") },
            {DocumentReferenceTypeEnum.PhysicalActivity, (new SchemaId("ieee", "physical-activity", "1.0"), "physical-activity_{0}.json", "application/json") },
		    {DocumentReferenceTypeEnum.BodyTemperature, (new SchemaId("omh", "body-temperature", "3.0"), "body-temperature_{0}.json", "application/json") },
		    {DocumentReferenceTypeEnum.OxygenSaturation, (new SchemaId("omh", "oxygen-saturation", "2.0"), "oxygen-saturation_{0}.json", "application/json") },
        };

        /// <summary>
        /// 歩数データのJsonスキーマ（ieee-physical-activity-1.0）を表します。
        /// </summary>
        internal static string PHYSICAL_ACTIVITY_JSON_SCHEMA = 
        @"{
            ""$schema"": ""http://json-schema.org/draft-04/schema#"",
            ""deprecation"": {
                ""reason"": ""This schema is now deprecated, in favor of the IEEE 1752.1 schema with the same name."",
                ""supersededBy"": ""https://w3id.org/ieee/ieee-1752-schema/physical-activity.json"",
                ""date"": ""2022-12-01""
            },
            ""description"": ""This schema represents a single episode of physical activity."",
            ""type"": ""object"",
            ""references"": [
                {
                    ""description"": ""The SNOMED code represents Physical activity (observable entity)"",
                    ""url"": ""http://purl.bioontology.org/ontology/SNOMEDCT/68130003""
                }
            ],
            ""definitions"": {
                ""activity_name"": {
                    ""$ref"": ""activity-name-1.x.json""
                },
                ""length_unit_value"": {
                    ""$ref"": ""length-unit-value-1.x.json""
                },
                ""kcal_unit_value"": {
                    ""$ref"": ""kcal-unit-value-1.x.json""
                },
                ""time_frame"": {
                    ""$ref"": ""time-frame-1.x.json""
                }
            },

            ""properties"": {
                ""activity_name"": {
                    ""$ref"": ""#/definitions/activity_name""
                },
                ""effective_time_frame"": {
                    ""$ref"": ""#/definitions/time_frame""
                },
                ""distance"": {
                    ""description"": ""The distance covered, if applicable."",
                    ""$ref"": ""#/definitions/length_unit_value""
                },
                ""kcal_burned"": {
                    ""description"": ""The calories burned during the activity."",
                    ""$ref"": ""#/definitions/kcal_unit_value""
                },
                ""reported_activity_intensity"": {
                    ""description"": ""Self-reported intensity of the activity performed."",
                    ""type"": ""string"",
                    ""enum"": [""light"", ""moderate"", ""vigorous""]
                },
                ""met_value"": {
                    ""description"": ""Metabolic Equivalent of Task value for the activity"",
                    ""type"": ""number""
                }
            },

            ""required"": [""activity_name""]
        }";

        /// <summary>
        /// 血圧データのJsonスキーマ（omh-blood-pressure-4.0）を表します。
        /// </summary>
        internal static string BLOOD_PRESSURE_JSON_SCHEMA =
        @"{
            ""$schema"": ""http://json-schema.org/draft-07/schema#"",
            ""$id"": ""https://w3id.org/openmhealth/schemas/omh/blood-pressure-4.0.json"",
            ""description"": ""This schema represents a person's blood pressure as a combination of systolic and diastolic blood pressure."",
            ""type"": ""object"",
            ""references"": [
                {
                    ""description"": ""The SNOMED codes represents Blood pressure (observable entity)"",
                    ""url"": ""http://purl.bioontology.org/ontology/SNOMEDCT/75367002""
                }
            ],
            ""definitions"": {
                ""systolic_blood_pressure"": {
                    ""$ref"": ""systolic-blood-pressure-1.x.json""
                },
                ""diastolic_blood_pressure"": {
                    ""$ref"": ""diastolic-blood-pressure-1.x.json""
                },
                ""time_frame"": {
                    ""$ref"": ""https://w3id.org/ieee/ieee-1752-schema/time-frame-1.0.json""
                },
                ""body_posture"": {
                    ""$ref"": ""https://w3id.org/ieee/ieee-1752-schema/body-posture-1.0.json""
                },
                ""body_location"": {
                    ""$ref"": ""body-location-1.x.json""
                },
                ""temporal_relationship_to_physical_activity"": {
                    ""$ref"": ""temporal-relationship-to-physical-activity-1.x.json""
                },
                ""descriptive_statistic"": {
                    ""$ref"": ""https://w3id.org/ieee/ieee-1752-schema/descriptive-statistic-1.0.json""
                }
            },

            ""properties"": {
                ""systolic_blood_pressure"": {
                    ""$ref"": ""#/definitions/systolic_blood_pressure""
                },
                ""diastolic_blood_pressure"": {
                    ""$ref"": ""#/definitions/diastolic_blood_pressure""
                },
                ""effective_time_frame"": {
                    ""$ref"": ""#/definitions/time_frame""
                },
                ""body_posture"": {
                    ""$ref"": ""#/definitions/body_posture""
                },
                ""descriptive_statistic"": {
                    ""$ref"": ""#/definitions/descriptive_statistic""
                },
                ""measurement_location"": {
                    ""description"": ""The location on the body where the blood pressure measuring device is placed for measurement."",
                    ""$ref"": ""#/definitions/body_location""
                },
                ""temporal_relationship_to_physical_activity"": {
                    ""$ref"": ""#/definitions/temporal_relationship_to_physical_activity""
                }
            },

            ""required"": [
                ""systolic_blood_pressure"",
                ""diastolic_blood_pressure"",
                ""effective_time_frame""
            ]
        }";
        
        // マイナは同じデータでもXMLとJSONどちらも出力できる要件がある？
        //internal static Dictionary<DocumentReferenceTypeEnum, string> CONTENT_TYPE = new Dictionary<DocumentReferenceTypeEnum, string>() {
        //    {DocumentReferenceTypeEnum.BodyWeight, "application/json" },
        //    {DocumentReferenceTypeEnum.BloodPressure, "application/json" },
        //    {DocumentReferenceTypeEnum.PhysicalActivity, "application/json" },
        //    {DocumentReferenceTypeEnum.BodyTemperature, "application/json" },
        //    {DocumentReferenceTypeEnum.OxygenSaturation, "application/json" },
        //    {DocumentReferenceTypeEnum.SpecificHealthCheckupHL7CDA, "application/xml" },
        //    {DocumentReferenceTypeEnum.SpecificHealthCheckupFHIR, "application/json" },
        //    {DocumentReferenceTypeEnum.MynaPrescription, "application/xml" },
        //    {DocumentReferenceTypeEnum.JahisPrescription, "text/csv" },
        //    {DocumentReferenceTypeEnum.MynaVaccination, "application/xml" },
        //};
    }
}
