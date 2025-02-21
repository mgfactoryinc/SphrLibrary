namespace SphrLibrary.Extensions
{
    /// <summary>
    /// 汎用モジュールで使用する型を、
    /// 異なる型へ変換するための拡張機能を提供します。
    /// </summary>
    public static class ObjectTypeConverterExtension
    {
        #region "Public Method"

        #region "Date"

        /// <summary>
        /// API 用日時文字列へ変換します。
        /// </summary>
        /// <param name="target">変換元日時。</param>
        /// <returns>API 用日時文字列（yyyy-MM-ddTHH:mm:ss.fffffffzzz）。</returns>
        public static string ToDateString(this DateTime target)
        {
            // TODO 日本時間のタイムゾーンに固定する必要あり？
            if (target != DateTime.MinValue) {
                return new DateTimeOffset(target, TimeZoneInfo.Local.GetUtcOffset(target)).ToString("O");
            } else {
                return DateTime.MinValue.ToString("O");
            }
        }

        /// <summary>
        /// API 用日時文字列へ変換します。
        /// </summary>
        /// <param name="target">変換元文字列。</param>
        /// <param name="format">変換元文字列の書式。</param>
        /// <returns>API 用日時文字列（yyyy-MM-ddTHH:mm:ss.fffffffzzz）。</returns>
        public static string ToDateString(this string target, string format)
        {
            string result = string.Empty;
            DateTime dateValue = DateTime.MinValue;

            if (!string.IsNullOrWhiteSpace(target) &&
                !string.IsNullOrWhiteSpace(format) &&
                DateTime.TryParseExact(target.Trim(), format, null, System.Globalization.DateTimeStyles.None, out dateValue) &&
                dateValue != DateTime.MinValue) {
                result = new DateTimeOffset(dateValue, TimeZoneInfo.Local.GetUtcOffset(dateValue)).ToString("O");
            } else {
                result = DateTime.MinValue.ToString("O");
            }

            return result;
        }

        #endregion

        #region "Guid"

        /// <summary>
        /// API 用 GUID 文字列へ変換します。
        /// </summary>
        /// <param name="target">変換元 GUID。</param>
        /// <returns>32 桁 の API 用 GUID 文字列（00000000000000000000000000000000）。</returns>
        public static string ToGuidString(this Guid target, bool hyphen = true)
        {
            return hyphen ? target.ToString("D") : target.ToString("N");
            //return target.ToString("N");
        }

        #endregion

        #region "String"

        /// <summary>
        /// 値の文字列形式を、
        /// 基本 データ 型、列挙体、GUID へ変換します。
        /// </summary>
        /// <typeparam name="T">変換先の型。</typeparam>
        /// <param name="value">変換元の値の文字列形式。</param>
        /// <returns>
        /// 成功なら指定した型へ変換された値、
        /// 失敗なら指定した型の初期値。
        /// </returns>
        public static T ToValueType<T>(this string value) where T : struct
        {
            T result = default;

            try {
                if (typeof(T).IsEnum) {
                    // 列挙体
                    if (System.Attribute.IsDefined(result.GetType(), typeof(FlagsAttribute), false)) {
                        // Flags 属性有り
                        result = (T)System.Enum.Parse(typeof(T), value);

                        result.ToString()?.Split(","[0]).ToList().ForEach((i) => {
                            if (!System.Enum.IsDefined(typeof(T), (T)System.Enum.Parse(typeof(T), i)))
                            {
                                throw new InvalidCastException(string.Format("値\"{0}\"は{1}のメンバではありません。", result, typeof(T).Name));
                            }
                        });
                    } else {
                        // Falgs 属性無し
                        result = (T)System.Enum.Parse(typeof(T), value);

                        if (!System.Enum.IsDefined(typeof(T), result))
                        {
                            throw new InvalidCastException(string.Format("値\"{0}\"は{1}のメンバではありません。", result, typeof(T).Name));
                        }
                    }
                } else if (typeof(T) == typeof(Guid)) {
                    // GUID
                    result = (T)(object)Guid.Parse(value);
                } else {
                    // 基本 データ 型
                    result = (T)Convert.ChangeType(value, typeof(T));
                }
            } catch {
                throw;
            }

            return result;
        }

        /// <summary>
        /// 初期値を指定して、値の文字列形式を、
        /// 基本 データ 型、列挙体、GUID へ変換します。
        /// </summary>
        /// <typeparam name="T">変換先の型。</typeparam>
        /// <param name="value">変換元の値の文字列形式。</param>
        /// <param name="initialValue">変換に失敗した場合に返却される初期値を指定。</param>
        /// <returns>
        /// 成功なら指定した型へ変換された値、
        /// 失敗なら指定した初期値。
        /// </returns>
        public static T TryToValueType<T>(this string value, T initialValue) where T : struct
        {
            T result = initialValue;

            if (typeof(T).IsEnum) {
                // 列挙体
                if (System.Attribute.IsDefined(result.GetType(), typeof(FlagsAttribute), false)) {
                    // Flags 属性有り
                    result = (T)System.Enum.Parse(typeof(T), value);

                    result.ToString()?.Split(","[0]).ToList().ForEach((i) => {
                        if (!System.Enum.IsDefined(typeof(T), (T)System.Enum.Parse(typeof(T), i))) { throw new Exception(); }
                    });
                } else {
                    // Falgs 属性無し
                    result = (T)System.Enum.Parse(typeof(T), value);

                    if (!System.Enum.IsDefined(typeof(T), result)) { throw new Exception(); }
                }
            } else if (typeof(T) == typeof(Guid)) {
                // GUID
                try {
                    result = (T)(object)Guid.Parse(value);
                } catch { }
            } else {
                // 基本 データ 型
                try {
                    result = (T)Convert.ChangeType(value, typeof(T));
                } catch { }
            }

            return result;
        }

        #endregion

        #endregion
    }
}
