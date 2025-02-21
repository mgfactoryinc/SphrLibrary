using System.Runtime.Serialization;

namespace SphrLibrary.Entities.OpenmHealth
{
    /// <summary>
    /// Open mHealth リソースの基本クラスを表します。
    /// </summary>
    [DataContract()]
    [Serializable()]
    public abstract class OmhEntityBase<TBody> where TBody : OmhBodyEntityBase
    {
        /// <summary>
        /// ヘッダ部を取得または設定します。
        /// </summary>
        [DataMember()]
        public OmhHeader header { get; set; } = new OmhHeader();

        /// <summary>
        /// ボディ部の配列を取得または設定します。
        /// </summary>
        [DataMember()]
        public TBody[] body { get; set; } = [];

        /// <summary>
        /// <see cref="OmhEntityBase"/>クラスの新しいインスタンスを初期化します。
        /// </summary>
        public OmhEntityBase() { }

        /// <summary>
        /// 各プロパティの有効性を検証します。
        /// </summary>
        /// <param name="isCheckAny">ボディ部に値が含まれるかをチェックするかどうか。</param>
        /// <returns>全て有効ならtrue、1つでも無効ならfalse。</returns>
        public bool IsValid(bool isCheckAny = true)
        {
            return this.header != null && this.body != null && (!isCheckAny || this.body.Any());
        }

        /// <summary>
        /// ボディ部にデータを追加します。
        /// </summary>
        /// <param name="bodies"></param>
        public void AddBody(TBody[] bodies)
        {
            if (this.body == null) this.body = [];

            if (bodies != null) {
                // TODO ここにも重複チェックが必要かも（自サービスのデータなので重複しないでほしいけど）
                this.body = this.body.Union(bodies).ToArray();
            }
        }
    }
}
