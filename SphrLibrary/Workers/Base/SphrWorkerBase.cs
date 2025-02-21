using SphrLibrary.Entities.SPHR;
using SphrLibrary.Enums;
using SphrLibrary.Helpers;

namespace SphrLibrary.Workers.Base
{
    /// <summary>
    /// 汎用モジュールが使用する各機能の引数の基本クラスを表します。
    /// </summary>
    /// <typeparam name="TArgs">引数クラス。</typeparam>
    /// <typeparam name="TResults">戻り値クラス。</typeparam>
    internal abstract class SphrWorkerBase<TArgs, TResults>
        where TArgs : SphrWorkerArgsBase
        where TResults : SphrWorkerResultsBase, new()
    {
        #region "Variable"

        /// <summary>
        /// 実行種別を保持します。
        /// </summary>
        private SphrOperationTypeEnum _operationType = SphrOperationTypeEnum.None;

        #endregion

        #region "Public Property"

        /// <summary>
        /// 実行種別を取得します。
        /// </summary>
        public SphrOperationTypeEnum OperationType { get { return this._operationType; } }

        #endregion

        #region "Constructor"

        /// <summary>
        /// デフォルトコンストラクタ は使用できません。
        /// </summary>
        private SphrWorkerBase() { }

        /// <summary>
        /// 実行種別を指定して、<see cref="SphrWorkerBase"/>クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="operationType">実行種別。</param>
        protected SphrWorkerBase(SphrOperationTypeEnum operationType)
        {
            this._operationType = operationType;
        }

        #endregion

        #region "MustOverride Method"

        /// <summary>
        /// 同期処理を実行します。
        /// </summary>
        /// <param name="args">引数クラス。</param>
        /// <returns>戻り値クラス。</returns>
        public abstract TResults Execute(TArgs args);

        #endregion

        #region "Public Method"

        /// <summary>
        /// 同期処理を実行します。
        /// </summary>
        /// <param name="args">引数クラス。</param>
        /// <returns>戻り値クラス。</returns>
        public TResults TryExecute(TArgs args)
        {
            TResults results = new TResults() { IsSuccess = false, Results = new List<SphrResult>() };
            SphrResult? result = null;

            // TODO BuildErrorResult -> BuildResult
            string workerName = typeof(TArgs).Name.ToLower().Replace("sphr", "").Replace("workerargs", "");

            // http://nonsoft.la.coocan.jp/SoftSample/CS.NET/SampleException.html
            try {
                results = this.Execute(args);

                // 例外が発生しない時に処理する正常時のコード
                if (results.IsSuccess) {
                    // 正常コードはここでだけ格納。正常でも警告等は入れられる
                    if (results.Results == null) results.Results = new List<SphrResult>();
                    results.Results.Add(SphrHelper.BuildResult(SphrResultCodeTypeEnum.Information, this.OperationType, 200, "正常に完了しました。"));
                } else {
                    // 警告、エラー(throwしないパターン)はExecute内で設定すること
                    // Nullの場合は汎用エラーコードを設定
                    if (results.Results == null) results.Results = new List<SphrResult>() { 
                        SphrHelper.BuildResult(SphrResultCodeTypeEnum.Error, this.OperationType, 991, "処理に失敗しました。") };
                }
            } catch (System.AccessViolationException ex) {
                // 保護されたメモリの読み取りまたは書き込みが試行されたときにスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 1);
            } catch (System.AppDomainUnloadedException ex) {
                // アンロードされたアプリケーション ドメインにアクセスしようとするとスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 2);
            } catch (System.ArgumentNullException ex) {
                // null 参照 (Visual Basic では Nothing) を有効な引数として受け付けないメソッドに 
                // null 参照を渡した場合にスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 3);
            } catch (System.ArgumentOutOfRangeException ex) {
                // 呼び出されたメソッドで定義されている許容範囲外の値が引数として渡された場合にスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 4);
            } catch (System.ArrayTypeMismatchException ex) {
                // 間違った型の要素を配列に格納しようとするとスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 5);
            } catch (System.BadImageFormatException ex) {
                // DLL または実行可能プログラムのファイル イメージが無効である場合にスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 6);
            } catch (System.CannotUnloadAppDomainException ex) {
                // アプリケーション ドメインをアンロードしようとして失敗した場合にスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 7);
            } catch (System.ContextMarshalException ex) {
                // コンテキストの境界を越えてオブジェクトをマーシャリングしようとして失敗した場合にスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 8);
            } catch (System.DataMisalignedException ex) {
                // データの単位が、データ サイズの倍数ではないアドレスから読み込まれたり、
                // アドレスに書き込まれたりしたときにスローされる例外。このクラスは継承できません。
                result = SphrHelper.BuildErrorResult(ex, workerName, 9);
            } catch (System.DivideByZeroException ex) {
                // 整数値または小数値を 0 で除算しようとするとスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 10);
            } catch (System.DllNotFoundException ex) {
                // DLL インポートで指定した DLL が見つからない場合にスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 11);
            } catch (System.DuplicateWaitObjectException ex) {
                // 同期オブジェクトの配列に 1 つのオブジェクトが 2 回以上現れた場合にスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 12);
            } catch (System.EntryPointNotFoundException ex) {
                // 開始メソッドが指定されていないことが原因でクラスの読み込みに失敗した場合にスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 13);
            } catch (System.ExecutionEngineException ex) {
                // 共通言語ランタイムの実行エンジンに内部エラーが発生した場合にスローされる例外。
                // このクラスは継承できません。
                result = SphrHelper.BuildErrorResult(ex, workerName, 14);
            } catch (System.FieldAccessException ex) {
                // クラス内部のプライベート フィールドまたはプロテクト フィールドに対して無効なアクセスが試行された場合にスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 15);
            } catch (System.IndexOutOfRangeException ex) {
                // 配列の境界外のインデックスを使用して配列の要素にアクセスしようとした場合にスローされる例外。このクラスは継承できません。
                result = SphrHelper.BuildErrorResult(ex, workerName, 16);
            } catch (System.InsufficientMemoryException ex) {
                // この例外は、使用可能なメモリが十分に残っているかどうかのチェックで、
                // 要件が満たされなかった場合にスローされます。このクラスは継承できません。
                result = SphrHelper.BuildErrorResult(ex, workerName, 17);
            } catch (System.InvalidCastException ex) {
                // 無効なキャストまたは明示的な型変換に対してスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 18);
            } catch (System.InvalidOperationException ex) {
                // オブジェクトの現在の状態に対して無効なメソッド呼び出しが行われた場合にスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 19);
            } catch (System.InvalidProgramException ex) {
                // プログラムに無効な MSIL (Microsoft intermediate language) またはメタデータが
                // 含まれている場合にスローされる例外。通常、これはプログラムを生成した
                // コンパイラのバグを示します。
                result = SphrHelper.BuildErrorResult(ex, workerName, 20);
            } catch (System.MethodAccessException ex) {
                // クラス内部のプライベート メソッドまたはプロテクト メソッドに対して無効なアクセスが
                // 試行された場合にスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 21);
            } catch (System.MissingFieldException ex) {
                // 存在しないフィールドに動的にアクセスしようとした場合にスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 22);
            } catch (System.MissingMethodException ex) {
                // 存在しないメソッドに動的にアクセスしようとした場合にスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 23);
            } catch (System.MissingMemberException ex) {
                // 存在しないクラス メンバに動的にアクセスしようとした場合にスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 24);
            } catch (System.MulticastNotSupportedException ex) {
                // いずれか一方のオペランドが null 参照 (Visual Basic の場合は Nothing) でない場合は
                // 組み合わせることができない 2 つのデリゲート型のインスタンスを組み合わせようとした
                // 場合にスローされる例外。このクラスは継承できません。
                result = SphrHelper.BuildErrorResult(ex, workerName, 25);
            } catch (System.NotFiniteNumberException ex) {
                // 浮動小数点値が正の無限大、負の無限大、または非数 (NaN) の場合にスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 26);
            } catch (System.NotImplementedException ex) {
                // 要求されたメソッドまたは操作が実装されない場合にスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 27);
            } catch (System.NullReferenceException ex) {
                // null オブジェクト参照を逆参照しようとした場合にスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 28);
            } catch (System.OperationCanceledException ex) {
                // この例外は、スレッドによって実行されている操作がキャンセルされたときに、
                // そのスレッドでスローされます。
                result = SphrHelper.BuildErrorResult(ex, workerName, 29);
            } catch (System.OutOfMemoryException ex) {
                // プログラムの実行を継続するためのメモリが不足している場合にスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 30);
            } catch (System.OverflowException ex) {
                // checked コンテキストで、算術演算、キャスト演算、
                // または変換演算の結果オーバーフローが発生した場合にスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 31);
            } catch (System.PlatformNotSupportedException ex) {
                // 特定のプラットフォームで機能が実行されない場合にスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 32);
            } catch (System.RankException ex) {
                // 間違った次元数の配列がメソッドに渡された際にスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 33);
            } catch (System.StackOverflowException ex) {
                // 保留状態のメソッド呼び出しが多くなりすぎ、
                // 実行スタックがオーバーフローした場合にスローされる例外。このクラスは継承できません。
                result = SphrHelper.BuildErrorResult(ex, workerName, 34);
            } catch (System.TimeoutException ex) {
                // プロセスまたは操作用に割り当てられた時間が経過したときにスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 35);
            } catch (System.TypeLoadException ex) {
                // 型の読み取りエラーが発生したときにスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 36);
            } catch (System.TypeUnloadedException ex) {
                // アンロードされたクラスにアクセスしようとした場合にスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 37);
            } catch (System.Collections.Generic.KeyNotFoundException ex) {
                // コレクション内の要素にアクセスするために指定されたキーが、
                // コレクションのいずれのキーとも一致しない場合にスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 38);
            } catch (System.IO.DirectoryNotFoundException ex) {
                // ファイルまたはディレクトリの一部が見つからない場合にスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 39);
            } catch (System.IO.DriveNotFoundException ex) {
                // 使用できないドライブまたは共有にアクセスしようとするとスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 40);
            } catch (System.IO.EndOfStreamException ex) {
                // ストリームの末尾を越えて読み込もうとしたときにスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 41);
            } catch (System.IO.FileLoadException ex) {
                // マネージ アセンブリが見つかったが、読み込むことができない場合にスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 42);
            } catch (System.IO.FileNotFoundException ex) {
                // ディスク上に存在しないファイルにアクセスしようとして失敗したときにスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 43);
            } catch (System.IO.PathTooLongException ex) {
                // パス名またはファイル名がシステム定義の最大長よりも長いときにスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 44);
            } catch (System.IO.IsolatedStorage.IsolatedStorageException ex) {
                // 分離ストレージの操作で障害が発生するとスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 45);
            } catch (System.Reflection.AmbiguousMatchException ex) {
                // メンバへのバインド時に、バインディング基準に一致するメンバが複数ある場合に
                // スローされる例外。このクラスは継承できません。
                result = SphrHelper.BuildErrorResult(ex, workerName, 46);
            } catch (System.Reflection.CustomAttributeFormatException ex) {
                // カスタム属性のバイナリ形式が無効な場合にスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 47);
            } catch (System.Reflection.InvalidFilterCriteriaException ex) {
                // 使用するフィルタの種類に対してフィルタ基準が無効な場合に
                // System.Type.FindMembers(System.Reflection.MemberTypes,System.Reflection.BindingFlags,
                // System.Reflection.MemberFilter,System.Object)でスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 48);
            } catch (System.Reflection.TargetException ex) {
                // 無効なターゲットを呼び出そうとするとスローされる例外を表します。
                result = SphrHelper.BuildErrorResult(ex, workerName, 49);
            } catch (System.Reflection.TargetParameterCountException ex) {
                // 呼び出し時に指定されたパラメータの数が、必要なパラメータ数と異なる場合に
                // スローされる例外。このクラスは継承できません。
                result = SphrHelper.BuildErrorResult(ex, workerName, 50);
            } catch (System.Resources.MissingManifestResourceException ex) {
                // 適切なサテライト アセンブリがないために、ニュートラル カルチャ リソースが
                // 必要な場合に、メイン アセンブリにニュートラル カルチャ リソースが含まれていないと
                // スローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 51);
            } catch (System.Resources.MissingSatelliteAssemblyException ex) {
                // ニュートラル カルチャのリソースのサテライト アセンブリが見つからない場合にスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 52);
            } catch (System.Runtime.InteropServices.COMException ex) {
                // COM メソッドの呼び出しによって、認識されない HRESULT が返された場合にスローされる例外です。
                result = SphrHelper.BuildErrorResult(ex, workerName, 53);
            } catch (System.Runtime.InteropServices.InvalidComObjectException ex) {
                // 無効な COM オブジェクトを使用したときにスローされる例外です。
                result = SphrHelper.BuildErrorResult(ex, workerName, 54);
            } catch (System.Runtime.InteropServices.InvalidOleVariantTypeException ex) {
                // マネージ コードにマーシャリングできないバリアント型の引数が見つかった場合に、マーシャラによってスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 55);
            } catch (System.Runtime.InteropServices.MarshalDirectiveException ex) {
                // マーシャラが、サポートしていないSystem.Runtime.InteropServices.MarshalAsAttributeを検出した場合にスローする例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 56);
            } catch (System.Runtime.InteropServices.SafeArrayRankMismatchException ex) {
                // 着信 SAFEARRAY のランクが、マネージ シグネチャで指定したランクと一致しない場合にスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 57);
            } catch (System.Runtime.InteropServices.SafeArrayTypeMismatchException ex) {
                // 着信 SAFEARRAY の型が、マネージ シグネチャで指定した型と一致しない場合にスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 58);
            } catch (System.Runtime.InteropServices.SEHException ex) {
                // 構造化例外ハンドラ (SEH: Structured Exception Handler) エラーを表します。
                result = SphrHelper.BuildErrorResult(ex, workerName, 59);
                //} catch (System.Runtime.Remoting.RemotingException ex) {
                //    // リモート処理中に何かが失敗すると、スローされる例外。
                //} catch (System.Runtime.Remoting.RemotingTimeoutException ex) {
                //    // 以前に指定した期間内にサーバーまたはクライアントに到達できないと、
                //    // スローされる例外。
                //} catch (System.Runtime.Remoting.ServerException ex) {
                //    // クライアントが、例外をスローできない非 .NET Framework アプリケーションに
                //    // 接続する場合に、クライアントにエラーを通知するためにスローされる例外。
            } catch (System.Runtime.Serialization.SerializationException ex) {
                // シリアル化中または逆シリアル化中にエラーが発生するとスローされる例外。
                //} catch (System.Security.HostProtectionException ex) {
                //    // ホスト リソースの拒否が検出されたときにスローされる例外です。
                result = SphrHelper.BuildErrorResult(ex, workerName, 60);
            } catch (System.Security.SecurityException ex) {
                // セキュリティ エラーが検出されたときにスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 61);
            } catch (System.Security.VerificationException ex) {
                // セキュリティ ポリシーでコードをタイプ セーフにする必要があり、
                // 検証プロセスでコードがタイプ セーフかどうかを検証できないときにスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 62);
                //} catch (System.Security.XmlSyntaxException ex) {
                //    // XML 解析で構文エラーが存在する場合にスローされる例外。このクラスは継承できません。
            } catch (System.Security.AccessControl.PrivilegeNotHeldException ex) {
                // System.Security.AccessControl名前空間内のメソッドが、
                // そのメソッドに設定されていない特権を有効にしようとするとスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 63);
            } catch (System.Security.Cryptography.CryptographicUnexpectedOperationException ex) {
                // 暗号操作中に予期しない操作が発生するとスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 64);
            } catch (System.Security.Cryptography.CryptographicException ex) {
                // 暗号操作中にエラーが発生すると、スローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 65);
                //} catch (System.Security.Policy.PolicyException ex) {
                //    // ポリシーでコードの実行を禁止するとスローされる例外。
            } catch (System.Security.Principal.IdentityNotMappedException ex) {
                // ID を既知の ID に割り当てることができないプリンシパルの例外を表します。
                result = SphrHelper.BuildErrorResult(ex, workerName, 66);
            } catch (System.Text.DecoderFallbackException ex) {
                // デコーダ フォールバック操作が失敗したときにスローされる例外。
                // このクラスは継承できません。
                result = SphrHelper.BuildErrorResult(ex, workerName, 67);
            } catch (System.Text.EncoderFallbackException ex) {
                // エンコーダ フォールバック操作が失敗したときにスローされる例外。
                // このクラスは継承できません。
                result = SphrHelper.BuildErrorResult(ex, workerName, 68);
            } catch (System.Threading.AbandonedMutexException ex) {
                // スレッドが、別のスレッドが解放せずに終了することによって放棄した
                // System.Threading.Mutexオブジェクトを取得したときにスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 69);
            } catch (System.Threading.SynchronizationLockException ex) {
                // 指定した Monitor でロックを所有していることが呼び出し元の条件となるメソッドを、
                // そのロックを所有していない呼び出し元が呼び出した場合にスローされる例外です。
                result = SphrHelper.BuildErrorResult(ex, workerName, 70);
            } catch (System.Threading.ThreadInterruptedException ex) {
                // System.Threading.Threadが待機状態のときに中断されるとスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 71);
            } catch (System.Threading.ThreadStateException ex) {
                // メソッドの呼び出しでSystem.Threading.Threadが
                // 無効なSystem.Threading.Thread.ThreadStateである場合は、例外がスローされます。
                result = SphrHelper.BuildErrorResult(ex, workerName, 72);
            } catch (System.Threading.WaitHandleCannotBeOpenedException ex) {
                // 存在しないシステム ミューテックスまたはシステム セマフォを開こうとしたときにスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 73);
            } catch (System.UnauthorizedAccessException ex) {
                // オペレーティング システムが I/O エラーまたは特定の種類のセキュリティ エラーのために
                // アクセスを拒否する場合、スローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 74);
            } catch (System.ApplicationException ex) {
                // 致命的ではないアプリケーション エラーが発生した場合にスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 75);
            } catch (System.FormatException ex) {
                // 引数の書式が、呼び出されたメソッドのパラメータの仕様に一致していない場合にスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 76);
            } catch (System.NotSupportedException ex) {
                // 呼び出されたメソッドがサポートされていない場合、または呼び出された機能を備えていない
                // ストリームに対して読み取り、シーク、書き込みが試行された場合にスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 77);
            } catch (System.ArithmeticException ex) {
                // 算術演算、キャスト演算、または変換演算におけるエラーが原因でスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 78);
            } catch (System.IO.IOException ex) {
                // I/O エラーが発生したときにスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 79);
            } catch (System.MemberAccessException ex) {
                // クラス メンバにアクセスしようとして失敗した場合にスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 80);
            } catch (System.ArgumentException ex) {
                // メソッドに渡された引数のいずれかが無効な場合にスローされる例外。
                result = SphrHelper.BuildErrorResult(ex, workerName, 81);
            } catch (System.SystemException ex) {
                // 名前空間内の定義済み例外の基本クラスを定義します。
                result = SphrHelper.BuildErrorResult(ex, workerName, 901);
            } catch (Exception ex) {
                // 
                result = SphrHelper.BuildErrorResult(ex, workerName, 998);
            } finally {
                // 例外が発生しても発生しなくても、最後に処理されるコード
                if (result != null) {
                    if (results.Results == null || !results.Results.Any()) results.Results = new List<SphrResult> { result };
                    LogHelper.Write(string.Format("{0}:{1}/{2}", this.OperationType.ToString(), result.code, result.detail));
                } else if (results != null && results.Results != null && results.Results.Any()) {
                    results.Results.ForEach(x => LogHelper.Write(string.Format("{0}:{1}/{2}", this.OperationType.ToString(), x.code, x.detail)));
                }
            }

            return results;
        }

        /// <summary>
        /// 非同期処理を実行します。
        /// </summary>
        /// <param name="args">引数クラス。</param>
        /// <returns>戻り値クラス。</returns>
        public async Task<TResults> ExecuteAsync(TArgs args)
        {
            List<string> errors = new List<string>();
            return await Task.Run(() => this.TryExecute(args));
        }

        /// <summary>
        /// コールバックメソッドを指定して、非同期処理を実行します。
        /// </summary>
        /// <param name="args">引数クラス。</param>
        /// <param name="callback">戻り値クラスを返却するコールバックメソッド。</param>
        public async void ExecuteAsync(TArgs args, Action<Task<TResults>> callback)
        {
            await Task.Run(() => this.TryExecute(args)).ContinueWith((t) => callback).ConfigureAwait(false);
        }

        #endregion
    }
}
