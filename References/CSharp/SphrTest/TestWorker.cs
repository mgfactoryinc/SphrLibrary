using SphrLibrary.Entities.OpenmHealth;
using SphrLibrary.Entities.SPHR;
using SphrLibrary.Enums;
using SphrLibrary.Helpers;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

namespace SphrTest
{
    internal static class TestWorker
    {
        public static string GetVersion()
        {
            string name = Assembly.GetExecutingAssembly().GetName().Name!;
            Version ver = Assembly.GetExecutingAssembly().GetName().Version!;

            return string.Format("{0}<{1}.{2}.{3}>", name, ver.Major, ver.Minor, ver.Build);

        }

        [RequiresDynamicCode("Calls System.Runtime.InteropServices.Marshal.SizeOf(Type)")]
        internal static IntPtr ToPtr<T>(T obj) where T : class
        {
            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(T)));
            Marshal.StructureToPtr(obj, ptr, false);
            return ptr;
        }

        public static PhysicalActivity CreatePhysicalActivitySample()
        {
            return SphrWrapper.CreatePhysicalActivity(
                Guid.NewGuid(),
                new DateTime(2024, 11, 11),
                SphrLibrary.Enums.ModalityTypeEnum.self_reported,
                "Walking",
                8000,
                2.5,
                DateTime.Now
            );
        }

        public static BloodPressure CreateBloodPressureSample()
        {
            return SphrWrapper.CreateBloodPressure(
                Guid.NewGuid(),
                new DateTime(2024, 12, 1),
                ModalityTypeEnum.self_reported,
                126,
                82,
                BodyPostureTypeEnum.sitting,
                BloodPressureMeasurementLocationTypeEnum.leftupperarm,
                TemporalRelationshipToPhysicalActivityTypeEnum.at_rest,
                DateTime.Now
            );
        }

        public static bool Export(ref string exportPath) {
            bool result = false;
            const int COUNT = 10;

            //// 歩数データ
            //PhysicalActivity physicalActivity = TestWorker.CreatePhysicalActivitySample(); // サンプル -> 実データに置き換え

            //// 歩数データをセットする
            //SphrLibrary.SphrLibrary.Set(physicalActivity);

            //// 血圧データ
            //BloodPressure bloodPressure = TestWorker.CreateBloodPressureSample(); // サンプル -> 実データに置き換え

            //// 血圧データをセットする
            //SphrLibrary.SphrLibrary.Set(bloodPressure);

            
            for (int i = 0; i < COUNT; i++) {
                PhysicalActivity physicalActivity = TestWorker.CreatePhysicalActivitySample();
                SphrLibrary.SphrLibrary.Set(physicalActivity);
                BloodPressure bloodPressure = TestWorker.CreateBloodPressureSample();
                SphrLibrary.SphrLibrary.Set(bloodPressure);
            }

            // エクスポート
            exportPath = SphrLibrary.SphrLibrary.Export(DateTime.Now);
            result = !string.IsNullOrWhiteSpace(exportPath);

            return result;
        }

        public static bool Import(string importPath) {
            bool result = false;

            if (File.Exists(importPath)) {
                // インポート実行（抽出項目をビットフラグで指定）
                bool isSuccess = SphrLibrary.SphrLibrary.Import(importPath);

                if (SphrLibrary.SphrLibrary.Errors != null && SphrLibrary.SphrLibrary.Errors.Any()) { 
                    // TODO インポートでエラーになるケースを洗い出し
                    // 一部失敗の場合、成功した項目は処理を継続していいのかは、汎用モジュール、サービサー双方で検討
                } else { 
                    result = true;
                }
            }

            return result;
        }

        public static SphrProfile? Extract()
        {
            DocumentReferenceTypeEnum type = DocumentReferenceTypeEnum.BloodPressure | DocumentReferenceTypeEnum.PhysicalActivity;
            return SphrLibrary.SphrLibrary.Extract(type);

            //SphrProfile? result = null;
            //IntPtr ptr = TestWorker.ExtractToJson((ulong)type);
            ////IntPtr ptr = Marshal.GetFunctionPointerForDelegate(ExtractToJson);
            ////nint ptr = SphrUnmanagedLibrary.ExtractToJson((ulong)type);
            //string? json = Marshal.PtrToStringAnsi(ptr);
            //if (!string.IsNullOrWhiteSpace(json)) { 
            //    result = new SphrJsonSerializer().Deserialize<SphrProfile>(json);
            //}
            //return result;
            ////return Marshal.PtrToStructure<SphrProfile>(result);

        }

        // OLD アンマネージドのテスト(途中)
        //[DllImport("SphrLibrary.dll")]
        //public static extern IntPtr ExtractToJson(ulong extractType);

        //public delegate IntPtr ExtractToJsonDelegate();

        //static IntPtr GetFuncPtr(MethodInfo methodInfo)
        //{
        //    DynamicMethod dm = new DynamicMethod("", MethodAttributes.Public | MethodAttributes.Static, CallingConventions.Standard, typeof(IntPtr), new Type[] { }, typeof(Delegate), true);
        //    ILGenerator ilgen = dm.GetILGenerator();
        //    ilgen.Emit(OpCodes.Ldftn, methodInfo);
        //    ilgen.Emit(OpCodes.Ret);

        //    return ((Func<IntPtr>)dm.CreateDelegate(typeof(Func<IntPtr>)))();
        //}

        //static IntPtr GetFuncPtr(Func<IntPtr> func)
        //{
        //    return GetFuncPtr(func.GetMethodInfo());
        //}
    }
}
