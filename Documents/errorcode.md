

## エラーコード一覧

コード|説明|.NET 対応 Exception
 ------------- | ------------- | ------------- 
i.sphr.[Method].200|正常終了|-
e.sphr.[Method].1|保護されたメモリの読み取りまたは書き込みが試行されたときにスローされる例外。|System.AccessViolationException
e.sphr.[Method].2|アンロードされたアプリケーション ドメインにアクセスしようとするとスローされる例外。|System.AppDomainUnloadedException
e.sphr.[Method].3|null 参照 (Visual Basic では Nothing) を有効な引数として受け付けないメソッドに null 参照を渡した場合にスローされる例外。|System.ArgumentNullException
e.sphr.[Method].4|呼び出されたメソッドで定義されている許容範囲外の値が引数として渡された場合にスローされる例外。|System.ArgumentOutOfRangeException
e.sphr.[Method].5|間違った型の要素を配列に格納しようとするとスローされる例外。|System.ArrayTypeMismatchException
e.sphr.[Method].6|DLL または実行可能プログラムのファイル イメージが無効である場合にスローされる例外。|System.BadImageFormatException
e.sphr.[Method].7|アプリケーション ドメインをアンロードしようとして失敗した場合にスローされる例外。|System.CannotUnloadAppDomainException
e.sphr.[Method].8|コンテキスト境界を越えてオブジェクトをマーシャリングする試みが失敗したときにスローされる例外。|System.ContextMarshalException
e.sphr.[Method].9|データの単位が、データ サイズの倍数ではないアドレスから読み込まれたり、アドレスに書き込まれたりしたときにスローされる例外。|System.DataMisalignedException
e.sphr.[Method].10|整数値または小数値を 0 で除算しようとするとスローされる例外。|System.DivideByZeroException
e.sphr.[Method].11|DLL インポートで指定した DLL が見つからない場合にスローされる例外。|System.DllNotFoundException
e.sphr.[Method].12|同期オブジェクトの配列に 1 つのオブジェクトが 2 回以上現れた場合にスローされる例外。|System.DuplicateWaitObjectException
e.sphr.[Method].13|開始メソッドが指定されていないことが原因でクラスの読み込みに失敗した場合にスローされる例外。|System.EntryPointNotFoundException
e.sphr.[Method].14|共通言語ランタイムの実行エンジンに内部エラーが発生した場合にスローされる例外。|System.ExecutionEngineException
e.sphr.[Method].15|クラス内部のプライベート フィールドまたはプロテクト フィールドに対して無効なアクセスが試行された場合にスローされる例外。|System.FieldAccessException
e.sphr.[Method].16|配列の境界外のインデックスを使用して配列の要素にアクセスしようとした場合にスローされる例外。|System.IndexOutOfRangeException
e.sphr.[Method].17|この例外は、使用可能なメモリが十分に残っているかどうかのチェックで、要件が満たされなかった場合にスローされます。|System.InsufficientMemoryException
e.sphr.[Method].18|無効なキャストまたは明示的な型変換に対してスローされる例外。|System.InvalidCastException
e.sphr.[Method].19|オブジェクトの現在の状態に対して無効なメソッド呼び出しが行われた場合にスローされる例外。|System.InvalidOperationException
e.sphr.[Method].20|プログラムに無効な MSIL (Microsoft intermediate language) またはメタデータが含まれている場合にスローされる例外。通常、これはプログラムを生成したコンパイラのバグを示します。|System.InvalidProgramException
e.sphr.[Method].21|クラス内部のプライベート メソッドまたはプロテクト メソッドに対して無効なアクセスが試行された場合にスローされる例外。|System.MethodAccessException
e.sphr.[Method].22|存在しないフィールドに動的にアクセスしようとした場合にスローされる例外。|System.MissingFieldException
e.sphr.[Method].23|存在しないメソッドに動的にアクセスしようとした場合にスローされる例外。|System.MissingMethodException
e.sphr.[Method].24|存在しないクラス メンバに動的にアクセスしようとした場合にスローされる例外。|System.MissingMemberException
e.sphr.[Method].25|いずれか一方のオペランドが null 参照 (Visual Basic の場合は Nothing) でない場合は組み合わせることができない 2 つのデリゲート型のインスタンスを組み合わせようとした場合にスローされる例外。|System.MulticastNotSupportedException
e.sphr.[Method].26|浮動小数点値が正の無限大、負の無限大、または非数 (NaN) の場合にスローされる例外。|System.NotFiniteNumberException
e.sphr.[Method].27|要求されたメソッドまたは操作が実装されない場合にスローされる例外。|System.NotImplementedException
e.sphr.[Method].28|null オブジェクト参照を逆参照しようとした場合にスローされる例外。|System.NullReferenceException
e.sphr.[Method].29|この例外は、スレッドによって実行されている操作がキャンセルされたときに、そのスレッドでスローされます。|System.OperationCanceledException
e.sphr.[Method].30|プログラムの実行を継続するためのメモリが不足している場合にスローされる例外。|System.OutOfMemoryException
e.sphr.[Method].31|checked コンテキストで、算術演算、キャスト演算、または変換演算の結果オーバーフローが発生した場合にスローされる例外。|System.OverflowException
e.sphr.[Method].32|特定のプラットフォームで機能が実行されない場合にスローされる例外。|System.PlatformNotSupportedException
e.sphr.[Method].33|間違った次元数の配列がメソッドに渡された際にスローされる例外。|System.RankException
e.sphr.[Method].34|保留状態のメソッド呼び出しが多くなりすぎ、実行スタックがオーバーフローした場合にスローされる例外。|System.StackOverflowException
e.sphr.[Method].35|プロセスまたは操作用に割り当てられた時間が経過したときにスローされる例外。|System.TimeoutException
e.sphr.[Method].36|型の読み取りエラーが発生したときにスローされる例外。|System.TypeLoadException
e.sphr.[Method].37|アンロードされたクラスにアクセスしようとした場合にスローされる例外。|System.TypeUnloadedException
e.sphr.[Method].38|コレクション内の要素にアクセスするために指定されたキーが、コレクションのいずれのキーとも一致しない場合にスローされる例外。|System.Collections.Generic.KeyNotFoundException
e.sphr.[Method].39|ファイルまたはディレクトリの一部が見つからない場合にスローされる例外。|System.IO.DirectoryNotFoundException
e.sphr.[Method].40|使用できないドライブまたは共有にアクセスしようとするとスローされる例外。|System.IO.DriveNotFoundException
e.sphr.[Method].41|ストリームの末尾を越えて読み込もうとしたときにスローされる例外。|System.IO.EndOfStreamException
e.sphr.[Method].42|マネージ アセンブリが見つかったが、読み込むことができない場合にスローされる例外。|System.IO.FileLoadException
e.sphr.[Method].43|ディスク上に存在しないファイルにアクセスしようとして失敗したときにスローされる例外。|System.IO.FileNotFoundException
e.sphr.[Method].44|パス名またはファイル名がシステム定義の最大長よりも長いときにスローされる例外。|System.IO.PathTooLongException
e.sphr.[Method].45|分離ストレージの操作で障害が発生するとスローされる例外。|System.IO.IsolatedStorage.IsolatedStorageException
e.sphr.[Method].46|メンバへのバインド時に、バインディング基準に一致するメンバが複数ある場合にスローされる例外。|System.Reflection.AmbiguousMatchException
e.sphr.[Method].47|カスタム属性のバイナリ形式が無効な場合にスローされる例外。|System.Reflection.CustomAttributeFormatException
e.sphr.[Method].48|使用するフィルタの種類に対してフィルタ基準が無効な場合にSystem.Type.FindMembers(System.Reflection.MemberTypes, System.Reflection.BindingFlags, System.Reflection.MemberFilter, System.Object)でスローされる例外。|System.Reflection.InvalidFilterCriteriaException
e.sphr.[Method].49|無効なターゲットを呼び出そうとするとスローされる例外を表します。|System.Reflection.TargetException
e.sphr.[Method].50|呼び出し時に指定されたパラメータの数が、必要なパラメータ数と異なる場合にスローされる例外。|System.Reflection.TargetParameterCountException
e.sphr.[Method].51|適切なサテライト アセンブリがないために、ニュートラル カルチャ リソースが必要な場合に、メイン アセンブリにニュートラル カルチャ リソースが含まれていないとスローされる例外。|System.Resources.MissingManifestResourceException
e.sphr.[Method].52|ニュートラル カルチャのリソースのサテライト アセンブリが見つからない場合にスローされる例外。|System.Resources.MissingSatelliteAssemblyException
e.sphr.[Method].53|COM メソッドの呼び出しによって、認識されない HRESULT が返された場合にスローされる例外です。|System.Runtime.InteropServices.COMException
e.sphr.[Method].54|無効な COM オブジェクトを使用したときにスローされる例外です。|System.Runtime.InteropServices.InvalidComObjectException
e.sphr.[Method].55|マネージ コードにマーシャリングできないバリアント型の引数が見つかった場合に、マーシャラによってスローされる例外。|System.Runtime.InteropServices.InvalidOleVariantTypeException
e.sphr.[Method].56|マーシャラが、サポートしていないSystem.Runtime.InteropServices.MarshalAsAttributeを検出した場合にスローする例外。|System.Runtime.InteropServices.MarshalDirectiveException
e.sphr.[Method].57|着信 SAFEARRAY のランクが、マネージ シグネチャで指定したランクと一致しない場合にスローされる例外。|System.Runtime.InteropServices.SafeArrayRankMismatchException
e.sphr.[Method].58|着信 SAFEARRAY の型が、マネージ シグネチャで指定した型と一致しない場合にスローされる例外。|System.Runtime.InteropServices.SafeArrayTypeMismatchException
e.sphr.[Method].59|構造化例外ハンドラ (SEH: Structured Exception Handler) エラーを表します。|System.Runtime.InteropServices.SEHException
e.sphr.[Method].60|シリアル化中または逆シリアル化中にエラーが発生するとスローされる例外。|System.Runtime.Serialization.SerializationException
e.sphr.[Method].61|セキュリティ エラーが検出されたときにスローされる例外。|System.Security.SecurityException
e.sphr.[Method].62|セキュリティ ポリシーでコードをタイプ セーフにする必要があり、検証プロセスでコードがタイプ セーフかどうかを検証できないときにスローされる例外。|System.Security.VerificationException
e.sphr.[Method].63|System.Security.AccessControl名前空間内のメソッドが、そのメソッドに設定されていない特権を有効にしようとするとスローされる例外。|System.Security.AccessControl.PrivilegeNotHeldException
e.sphr.[Method].64|暗号操作中に予期しない操作が発生するとスローされる例外。|System.Security.Cryptography.CryptographicUnexpectedOperationException
e.sphr.[Method].65|暗号操作中にエラーが発生すると、スローされる例外。|System.Security.Cryptography.CryptographicException
e.sphr.[Method].66|ID を既知の ID に割り当てることができないプリンシパルの例外を表します。|System.Security.Principal.IdentityNotMappedException
e.sphr.[Method].67|デコーダ フォールバック操作が失敗したときにスローされる例外。|System.Text.DecoderFallbackException
e.sphr.[Method].68|エンコーダ フォールバック操作が失敗したときにスローされる例外。|System.Text.EncoderFallbackException
e.sphr.[Method].69|スレッドが、別のスレッドが解放せずに終了することによって放棄したSystem.Threading.Mutexオブジェクトを取得したときにスローされる例外。|System.Threading.AbandonedMutexException
e.sphr.[Method].70|指定した Monitor でロックを所有していることが呼び出し元の条件となるメソッドを、そのロックを所有していない呼び出し元が呼び出した場合にスローされる例外です。|System.Threading.SynchronizationLockException
e.sphr.[Method].71|System.Threading.Threadが待機状態のときに中断されるとスローされる例外。|System.Threading.ThreadInterruptedException
e.sphr.[Method].72|メソッドの呼び出しでSystem.Threading.Threadが無効なSystem.Threading.Thread.ThreadStateである場合は、例外がスローされます。|System.Threading.ThreadStateException
e.sphr.[Method].73|存在しないシステム ミューテックスまたはシステム セマフォを開こうとしたときにスローされる例外。|System.Threading.WaitHandleCannotBeOpenedException
e.sphr.[Method].74|オペレーティング システムが I/O エラーまたは特定の種類のセキュリティ エラーのためにアクセスを拒否する場合、スローされる例外。|System.UnauthorizedAccessException
e.sphr.[Method].75|致命的ではないアプリケーション エラーが発生した場合にスローされる例外。|System.ApplicationException
e.sphr.[Method].76|引数の書式が、呼び出されたメソッドのパラメータの仕様に一致していない場合にスローされる例外。|System.FormatException
e.sphr.[Method].77|呼び出されたメソッドがサポートされていない場合、または呼び出された機能を備えていないストリームに対して読み取り、シーク、書き込みが試行された場合にスローされる例外。|System.NotSupportedException
e.sphr.[Method].78|算術演算、キャスト演算、または変換演算におけるエラーが原因でスローされる例外。|System.ArithmeticException
e.sphr.[Method].79|I/O エラーが発生したときにスローされる例外。|System.IO.IOException
e.sphr.[Method].80|クラス メンバにアクセスしようとして失敗した場合にスローされる例外。|System.MemberAccessException
e.sphr.[Method].81|メソッドに渡された引数のいずれかが無効な場合にスローされる例外。|System.ArgumentException
e.sphr.[Method].901|その他のシステム例外。|System.SystemException
e.sphr.[Method].991|その他のアプリケーション例外。|-
e.sphr.[Method].998|不明な例外。|System.Exception
