using SphrLibrary.Entities.OpenmHealth;
using SphrLibrary.Enums;
using System.Runtime.Serialization;

namespace SphrLibrary.Entities.SPHR
{
    /// <summary>
    /// SPHRプロファイル情報を表します。
    /// </summary>
    [DataContract()]
    [Serializable()]
    public class SphrProfile
    {
        #region "Public Property"

        /// <summary>
        /// SPHR インデックス情報を取得または設定します。
        /// </summary>
        [DataMember()]
        public IndexPhr? IndexPhr { get; set; } = null;

        /// <summary>
        /// 歩数データのディクショナリを取得または設定します。
        /// </summary>
        [DataMember()]
        public Dictionary<ModalityTypeEnum, PhysicalActivity>? PhysicalActivities { get; set; } = null;

        /// <summary>
        /// 血圧データのディクショナリを取得または設定します。
        /// </summary>
        [DataMember()]
        public Dictionary<ModalityTypeEnum, BloodPressure>? BloodPressures { get; set; } = null;

        /// <summary>
        /// 体重データのディクショナリを取得または設定します。
        /// </summary>
        [DataMember()]
        public Dictionary<ModalityTypeEnum, BodyWeight>? BodyWeights { get; set; } = null;

        /// <summary>
        /// 体温データのディクショナリを取得または設定します。
        /// </summary>
        [DataMember()]
        public Dictionary<ModalityTypeEnum, BodyTemperature>? BodyTemperatures { get; set; } = null;

        /// <summary>
        /// 酸素飽和度(SpO2)データのディクショナリを取得または設定します。
        /// </summary>
        [DataMember()]
        public Dictionary<ModalityTypeEnum, OxygenSaturation>? OxygenSaturations { get; set; } = null;

        #endregion

        #region "Constructor"

        /// <summary>
        /// <see cref="SphrProfile"/>クラスの新しいインスタンスを初期化します。
        /// </summary>
        public SphrProfile() { }

        #endregion

        #region "Public Method"

        /// <summary>
        /// <see cref="SphrProfile"/>クラスを初期化します。
        /// </summary>
        /// <param name="index">SPHRインデックスクラス。</param>
        public void Initialize(IndexPhr index)
        {
            this.IndexPhr = index;
            this.PhysicalActivities = new Dictionary<ModalityTypeEnum, PhysicalActivity>();
            this.BloodPressures = new Dictionary<ModalityTypeEnum, BloodPressure>();
            this.BodyWeights = new Dictionary<ModalityTypeEnum, BodyWeight>();
            this.BodyTemperatures = new Dictionary<ModalityTypeEnum, BodyTemperature>();
            this.OxygenSaturations = new Dictionary<ModalityTypeEnum, OxygenSaturation>();
        }

        #endregion
    }
}
