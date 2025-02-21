using SphrLibrary.Entities.OpenmHealth;
using SphrLibrary.Enums;
using SphrLibrary.Extensions;

namespace SphrLibrary.Helpers
{
    /// <summary>
    /// アンマネージドからマネージド制御するためのラッパー機能を提供します。
    /// </summary>
    public static class SphrWrapper
    {
        #region "Public Method"

        /// <summary>
        /// 値を指定して、Open mHealth ヘッダ情報を生成します。
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="creationDate"></param>
        /// <param name="schema"></param>
        /// <param name="modality"></param>
        /// <returns>ヘッダクラス。</returns>
        public static OmhHeader CreateHeader(Guid uuid, DateTime creationDate, DocumentReferenceTypeEnum schema, ModalityTypeEnum modality)
        {
            ValueUnit? rate = null;
            SchemaId schemaId = new SchemaId() {
                @namespace = SphrConst.SCHEMA[schema].schema.@namespace,
                name = SphrConst.SCHEMA[schema].schema.name,
                version = SphrConst.SCHEMA[schema].schema.version,
            };

            switch (schema) {
                case DocumentReferenceTypeEnum.BloodPressure:
                    rate = new ValueUnit() { value = 1, unit = "mmHg" };
                    break;
                case DocumentReferenceTypeEnum.PhysicalActivity:
                    rate = new ValueUnit() { value = 1, unit = "steps" };
                    break;
                default:
                    break;
            }

            return new OmhHeader() {
                acquisition_rate = rate,
                //external_data_sheets = new List<ExternalDataSheet>() { 
                //    new ExternalDataSheet() {
                //        datasheet_reference = "",
                //        datasheet_type = "",
                //    }
                //},
                modality = modality.ToString(),
                schema_id = schemaId,
                source_creation_date_time = creationDate.ToDateString(),
                uuid = uuid.ToGuidString(),
            };
        }

        /// <summary>
        /// 値を指定して、Open mHealth 歩数データを生成します。
        /// </summary>
        /// <param name="uuid">UUID。</param>
        /// <param name="recordDate">測定日時。</param>
        /// <param name="modality">モダリティ（測定機器等）。</param>
        /// <param name="activityName">アクティビティ名。</param>
        /// <param name="steps">歩数(1日分丸め)。</param>
        /// <param name="distanceKm">移動距離(単位km)。</param>
        /// <param name="creationDate">作成日時（最終更新日時）。</param>
        /// <returns>歩数データクラス。</returns>
        public static PhysicalActivity CreatePhysicalActivity(Guid uuid, DateTime recordDate, ModalityTypeEnum modality, string activityName, double steps, double distanceKm, DateTime creationDate)
        {
            return new PhysicalActivity() {
                header = CreateHeader(uuid, creationDate, DocumentReferenceTypeEnum.PhysicalActivity, modality),
                body = [
                    new PhysicalActivityBody() {
                        activity_name = activityName,
                        base_movement_quantity = new ValueUnit() { value = steps, unit = "steps" },
                        distance = new ValueUnit() { value = distanceKm, unit = "km" },
                        effective_time_frame = new TimeFrame() {
                            time_interval = new TimeInterval() {
                                start_date_time = new DateTime(new DateOnly(recordDate.Year, recordDate.Month,recordDate.Day), new TimeOnly(0,0,0,0)).ToDateString(),
                                end_date_time = new DateTime(new DateOnly(recordDate.Year, recordDate.Month,recordDate.Day), new TimeOnly(23,59,59,999)).ToDateString(),
                            }
                        }
                    }
                ]
            };
        }

        /// <summary>
        /// 値を指定して、Open mHealth 歩数データを生成します。
        /// </summary>
        /// <param name="recordDate">測定日時。</param>
        /// <param name="modality">モダリティ（測定機器等）。</param>
        /// <param name="activityName">アクティビティ名。</param>
        /// <param name="steps">歩数(1日分丸め)。</param>
        /// <param name="distanceKm">移動距離(単位km)。</param>
        /// <param name="creationDate">作成日時（最終更新日時）。</param>
        /// <returns>歩数データクラス。</returns>
        public static PhysicalActivity CreatePhysicalActivity(DateTime recordDate, ModalityTypeEnum modality, string activityName, double steps, double distanceKm, DateTime creationDate)
        {
            return CreatePhysicalActivity(Guid.NewGuid(), recordDate, modality, activityName, steps, distanceKm, creationDate);
        }

        /// <summary>
        /// 値を指定して、Open mHealth 血圧データを生成します。
        /// </summary>
        /// <param name="uuid">UUID。</param>
        /// <param name="recordDate">測定日時。</param>
        /// <param name="modality">モダリティ（測定機器等）。</param>
        /// <param name="systolic">収縮時血圧（上）。</param>
        /// <param name="diastolic">拡張時血圧（下）。</param>
        /// <param name="bodyPosture">測定時の姿勢。</param>
        /// <param name="measurementLocation">測定位置。</param>
        /// <param name="temporalRelationship">睡眠との時間的関係。</param>
        /// <param name="creationDate">作成日時（最終更新日時）。</param>
        /// <returns>血圧データクラス。</returns>
        public static BloodPressure CreateBloodPressure(
            Guid uuid, DateTime recordDate, ModalityTypeEnum modality, int systolic, int diastolic, 
            BodyPostureTypeEnum? bodyPosture, BloodPressureMeasurementLocationTypeEnum? measurementLocation,
            TemporalRelationshipToPhysicalActivityTypeEnum? temporalRelationship, DateTime creationDate)
        {
            return new BloodPressure() {
                header = CreateHeader(uuid, creationDate, DocumentReferenceTypeEnum.BloodPressure, modality),
                body = [
                    new BloodPressureBody() {
                        systolic_blood_pressure = new ValueUnit() { value = systolic, unit = "mmHg" },
                        diastolic_blood_pressure = new ValueUnit() { value= diastolic, unit = "mmHg" },
                        body_posture = bodyPosture.HasValue ? bodyPosture.Value.ToString() : null,
                        measurement_location = measurementLocation.HasValue ? measurementLocation.Value.ToString() : null,
                        temporal_relationship_to_physical_activity = temporalRelationship.HasValue ? temporalRelationship.Value.ToString() : null,
                        effective_time_frame = new TimeFrame() {
                            date_time = recordDate.ToDateString()
                        }
                    }
                ]
            };
        }

        /// <summary>
        /// 値を指定して、Open mHealth 血圧データを生成します。
        /// </summary>
        /// <param name="recordDate">測定日時。</param>
        /// <param name="modality">モダリティ（測定機器等）。</param>
        /// <param name="systolic">収縮時血圧（上）。</param>
        /// <param name="diastolic">拡張時血圧（下）。</param>
        /// <param name="bodyPosture">測定時の姿勢。</param>
        /// <param name="measurementLocation">測定位置。</param>
        /// <param name="temporalRelationship">睡眠との時間的関係。</param>
        /// <param name="creationDate">作成日時（最終更新日時）。</param>
        /// <returns>血圧データクラス。</returns>
        public static BloodPressure CreateBloodPressure(
            DateTime recordDate, ModalityTypeEnum modality, int systolic, int diastolic, 
            BodyPostureTypeEnum? bodyPosture, BloodPressureMeasurementLocationTypeEnum? measurementLocation,
            TemporalRelationshipToPhysicalActivityTypeEnum? temporalRelationship, DateTime creationDate)
        {
            return CreateBloodPressure(Guid.NewGuid(), recordDate, modality, systolic, diastolic, bodyPosture, measurementLocation, temporalRelationship, creationDate);
        }

        // public static BloodPressure CreateBloodPressureSample()
        // {
        //     // sample data 
        //     return CreateBloodPressure(Guid.NewGuid(), new DateTime(2024, 12, 1), ModalityTypeEnum.self_reported, 144, 82, BodyPostureTypeEnum.sitting, BloodPressureMeasurementLocationTypeEnum.rightupperarm, TemporalRelationshipToPhysicalActivityTypeEnum.at_rest, DateTime.Now);
        // }

        // public static PhysicalActivity CreatePhysicalActivitySample()
        // {
        //     return CreatePhysicalActivity(Guid.NewGuid(), new DateTime(2024, 12, 1), ModalityTypeEnum.self_reported, "Walking", 5000, 1.8, DateTime.Now);
        // }

        #endregion
    }
}
