using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization.Json;
using System.Text;

namespace SphrLibrary.Helpers
{
    /// <summary>
    /// 汎用モジュールのJsonシリアル化に関する機能を提供します。
    /// このクラスは継承できません。
    /// </summary>
    public sealed class SphrJsonSerializer
    {
        #region "Constant"

        /// <summary>
        /// オブジェクト が シリアル 化できないことを表す メッセージ です。
        /// </summary>
        private const string CANNOT_SERIALIZE = "型 パラメーター で指定された オブジェクト は シリアル 化できません。";

        #endregion

        #region "Constructor"

        /// <summary>
        /// <see cref="SphrJsonSerializer" /> クラス の新しい インスタンス を初期化します。
        /// </summary>
        public SphrJsonSerializer() { }

        #endregion

        #region "Private Method"

        /// <summary>
        /// 型 パラメーター で指定された オブジェクト が シリアル 化可能か判定します。
        /// </summary>
        /// <typeparam name="T">判定する オブジェクト の型。</typeparam>
        /// <returns>
        /// シリアル 化可能なら True、
        /// シリアル 化不可能なら False。
        /// </returns>
        private static bool IsSerializable<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] T>() where T : class
        {
            // TODO IEnumerable<T>を許容する
            bool result = false;
            if (typeof(T).GetConstructor(Type.EmptyTypes) != null && typeof(T).IsDefined(typeof(SerializableAttribute), false)) { 
                    result = true;
            } else {
                Type? type = typeof(T).GetElementType();
                if (type != null && type.IsDefined(typeof(SerializableAttribute), false)) {
                    result = true;
                }
            }
            return result;

            // return typeof(T).GetConstructor(Type.EmptyTypes) != null && typeof(T).IsDefined(typeof(SerializableAttribute), false);
        }

        /// <summary>
        /// シリアル化 可能 オブジェクト を バイト 配列から読み込みます。
        /// </summary>
        /// <typeparam name="T">デシリアライズ される オブジェクト の型。</typeparam>
        /// <param name="data">シリアライズ された オブジェクト を格納した バイト 配列。</param>
        /// <returns>
        /// 成功なら デシリアライズ された オブジェクト の インスタンス、
        /// 失敗なら Nothing。
        /// </returns>
        private T DeserializeFromBytes<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] T>(byte[] data) where T : class
        {
            if (!IsSerializable<T>()) throw new TypeAccessException(SphrJsonSerializer.CANNOT_SERIALIZE);

            T? result = null;

            if (data != null && data.Any()) {
                using (MemoryStream stream = new MemoryStream(data)) {
                    try {
                        result = (T?)new DataContractJsonSerializer(typeof(T)).ReadObject(stream);
                    } catch (Exception ex) {
                        LogHelper.Write(ex.Message);
                    }
                }
            }

            return result!;
        }

        /// <summary>
        /// シリアル 化可能 オブジェクト を バイト 配列へ書き込みます。
        /// </summary>
        /// <typeparam name="T">シリアライズ する オブジェクト の型。</typeparam>
        /// <param name="data">オブジェクト の インスタンス。</param>
        /// <returns>
        /// 成功なら シリアライズ された オブジェクト を格納した バイト 配列、
        /// 失敗なら Nothing。
        /// </returns>
        private byte[] SerializeToBytes<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] T>(T? data) where T : class
        {
            if (!IsSerializable<T>()) throw new TypeAccessException(SphrJsonSerializer.CANNOT_SERIALIZE);

            byte[]? result = null;

            if (data != null) {
                using (MemoryStream stream = new MemoryStream()) {
                    try {
                        new DataContractJsonSerializer(typeof(T)).WriteObject(stream, data);
                        result = stream.ToArray();
                    } catch (Exception ex) {
                        LogHelper.Write(ex.Message);
                    }
                }
            }

            return result!;
        }

        /// <summary>
        /// シリアル 化可能 オブジェクト を文字列から読み込みます。
        /// </summary>
        /// <typeparam name="T">デシリアライズ される オブジェクト の型。</typeparam>
        /// <param name="data">シリアライズ された オブジェクト を格納した文字列。</param>
        /// <returns>
        /// 成功なら デシリアライズ された オブジェクト の インスタンス、
        /// 失敗なら Nothing。
        /// </returns>
        private T DeserializeFromString<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] T>(string data) where T : class
        {
            T? result = null;

            try {
                result = this.DeserializeFromBytes<T>(Encoding.UTF8.GetBytes(data));
            } catch (Exception ex) { 
                LogHelper.Write(ex.Message);
            }

            return result!;
        }

        /// <summary>
        /// シリアル 化可能 オブジェクト を文字列へ書き込みます。
        /// </summary>
        /// <typeparam name="T">シリアライズ する オブジェクト の型。</typeparam>
        /// <param name="data">オブジェクト の インスタンス。</param>
        /// <returns>
        /// 成功なら シリアライズ された オブジェクト を格納した文字列、
        /// 失敗なら String.Empty。
        /// </returns>
        private string SerializeToString<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] T>(T data) where T : class
        {
            string result = string.Empty;

            try {
                byte[] bytes = this.SerializeToBytes(data)!;
                if (bytes != null) {
                    result = SphrConst.ENCODING.GetString(bytes);
                }
            } catch (Exception ex) { 
                LogHelper.Write(ex.Message);
            }

            return result;
        }

        #endregion

        #region "Public Method"

        /// <summary>
        /// シリアル 化可能 オブジェクト を文字列から読み込みます。
        /// </summary>
        /// <typeparam name="T">デシリアライズ される オブジェクト の型。</typeparam>
        /// <param name="data">シリアライズ された オブジェクト を格納した文字列。</param>
        /// <returns>
        /// 成功なら デシリアライズ された オブジェクト の インスタンス、
        /// 失敗なら Nothing。
        /// </returns>
        public T Deserialize<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] T>(string data) where T : class
        {
            return this.DeserializeFromString<T>(data)!;
        }

        /// <summary>
        /// シリアル 化可能 オブジェクト を文字列へ書き込みます。
        /// </summary>
        /// <typeparam name="T">シリアライズ する オブジェクト の型。</typeparam>
        /// <param name="data">オブジェクト の インスタンス。</param>
        /// <returns>
        /// 成功なら シリアライズ された オブジェクト を格納した文字列、
        /// 失敗なら String.Empty。
        /// </returns>
        public string Serialize<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] T>(T data) where T : class
        {
            return this.SerializeToString(data);
        }

        #endregion
    }
}
