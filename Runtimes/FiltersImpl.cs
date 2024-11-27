using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace Neurotech.Filters
{
    internal class SdkLib
    {
        public const string LibNameDefault = "filters";
#if __IOS__ || UNITY_IOS
        public const string LibName = "__Internal";
#else
        public const string LibName = LibNameDefault;
#endif
    }

    public class FilterList : IDisposable
    {
        public bool IsDisposed { get; private set; }
        private IntPtr _flPtr;

        public FilterList()
        {
            _flPtr = SDKApiFactory.Inst.CreateFilterList();
        }

        public void AddFilter(IFilter filter)
        {
            SDKApiFactory.Inst.AddFilter(_flPtr, (filter as Filter).FPtr);
        }

        public void FreezeWeights(bool freeze)
        {
            SDKApiFactory.Inst.FilterListFreezeWeights(_flPtr, freeze);
        }

        public void ClearFilters()
        {
            SDKApiFactory.Inst.ClearFilters(_flPtr);
        }

        public double Filter(double value)
        {
            return SDKApiFactory.Inst.Filter(_flPtr, value);
        }

        public void FilterArray(double[] values)
        {
            SDKApiFactory.Inst.FilterArray(_flPtr, values);
        }

        public void Reset()
        {
            SDKApiFactory.Inst.ResetFilterList(_flPtr);
        }

        public void DeleteFilter(IFilter filter)
        {
            SDKApiFactory.Inst.DeleteFilter(_flPtr, filter as Filter);
        }

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing)
        {
            if (IsDisposed)
                return;
            IsDisposed = true;
            try
            {
                if (_flPtr != IntPtr.Zero)
                {
                    SDKApiFactory.Inst.DeleteList(_flPtr);
                }
                _flPtr = IntPtr.Zero;
            }
            catch (Exception)
            {
            }
        }
        #endregion
    }

    public interface IFilter
    {
        void Reset();
        void ClearParams();
        double FilterValue(double value);
        void FilterArray(double[] values);

    }

    public class Filter : IDisposable, IFilter
    {
        protected Filter() { }

        private bool _disposed = false;
        public IntPtr FPtr { get; protected set; }

        public int GUID => SDKApiFactory.Inst.getID(FPtr);

        public void Reset()
        {
            SDKApiFactory.Inst.ResetFilter(FPtr);
        }
        public void ClearParams()
        {
            SDKApiFactory.Inst.ClearFilterParams(FPtr);
        }
        public double FilterValue(double value)
        {
            return SDKApiFactory.Inst.FilterF(FPtr, value);
        }
        public void FilterArray(double[] values)
        {
            SDKApiFactory.Inst.FilterFArray(FPtr, values);
        }

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            _disposed = true;
            try
            {
                if (FPtr != IntPtr.Zero)
                {
                    SDKApiFactory.Inst.DeleteFilter(FPtr);
                }

                FPtr = IntPtr.Zero;
            }
            catch (Exception)
            {
            }
        }

        #endregion
    }

    public class IIRFilterBuilder
    {
        public static IIRFilter CreateWithCustomParams(string filter)
        {
            return new IIRFilter(filter);
        }

        public static IIRFilter CreateWithParams(IIRFilterParam fParams)
        {
            return new IIRFilter(fParams);
        }
        public static IIRFilter CreateWithPath(string fPath)
        {
            return new IIRFilter(fPath, true);
        }

    }

    public class IIRFilter : Filter
    {
        public IIRFilterParam FilterParam { get; private set; }
        internal IIRFilter(string filter)
        {
            FPtr = SDKApiFactory.Inst.CreateCustomFilter(filter);
        }

        internal IIRFilter(string fPath, bool isFilePath)
        {
            FPtr = SDKApiFactory.Inst.CreateCustomFilterFromPath(fPath);
        }

        internal IIRFilter(IIRFilterParam fParams)
        {
            FilterParam = fParams;
            FPtr = SDKApiFactory.Inst.CreateFilterByParams(fParams);
        }

        public void SetParams(string fParams)
        {
            SDKApiFactory.Inst.SetFilterParams(FPtr, fParams);
        }
    }

    public class FIRFilter : Filter
    {
        public FIRFilter(string fileName)
        {
            FPtr = SDKApiFactory.Inst.CreateCustomFIRFilter(fileName);
        }
    }

    public class AdaptiveFIRFilter : Filter
    {
        public AdaptiveFIRFilter(FIRFilterParam fParams)
        {
            FPtr = SDKApiFactory.Inst.CreateAdaptiveFIRFilter(fParams);
        }

        public void FreezeWeights(bool freeze)
        {
            SDKApiFactory.Inst.AdaptiveFilterFreezeWeights(FPtr, freeze);
        }
    }

    public class PreinstalledFilters{

        public static IIRFilterParam[] List()
        {
            SDKApi inst = SDKApiFactory.Inst;
            int countPreinstaled = inst.GetPreinstalledFilterCount();
            IIRFilterParam[] filters = new IIRFilterParam[countPreinstaled];
            SDKApiFactory.Inst.GetPreinstalledFilterList(filters);
            return filters;
        }

    }
    

    internal sealed class SDKApiFactory
    {
        private static readonly Lazy<SDKApi> _api = new Lazy<SDKApi>(() =>
        {
            return new SDKApi();
        });
        private SDKApiFactory() { }
        public static SDKApi Inst
        {
            get => _api.Value;
        }
    }

    #region InternalWrapAPI
    internal class SDKApi
    {
        #region Native import
        [DllImport(SdkLib.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr create_TFilterList(out byte opSt);
        [DllImport(SdkLib.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void TFilterList_AddFilter(IntPtr flist, IntPtr f, out byte opSt);
        [DllImport(SdkLib.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void TFilter_List_ClearFilters(IntPtr flist, out byte opSt);
        [DllImport(SdkLib.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void TFilterList_Delete_TFilter(IntPtr flist, IntPtr f, out byte opSt);
        [DllImport(SdkLib.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void TFilterList_FreezeWeights(IntPtr flist, byte freeze, out byte opSt);
        [DllImport(SdkLib.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern double TFilterList_Filter(IntPtr flist, double value, out byte opSt);
        [DllImport(SdkLib.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void TFilterList_Filter_array(IntPtr flist, double[] values, int size, out byte opSt);
        [DllImport(SdkLib.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void TFilterList_ResetFilters(IntPtr flist, out byte opSt);
        [DllImport(SdkLib.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void delete_TFilterList(IntPtr flist, out byte opSt);

        [DllImport(SdkLib.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr create_TFilter_by_param(IIRFilterParam filterParam, out byte opSt);
        [DllImport(SdkLib.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr create_custom_FIR_Filter(string fName, out byte opSt);
        [DllImport(SdkLib.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr create_adaptive_FIR_Filter(FIRFilterParam param, out byte opSt);
        [DllImport(SdkLib.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr create_custom_TFilter(string filter, out byte opSt);
        [DllImport(SdkLib.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr create_custom_TFilter_FromFile(string filter, out byte opSt);
        [DllImport(SdkLib.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void adaptive_filter_freeze_weights(IntPtr f, byte freeze, out byte opSt);
        [DllImport(SdkLib.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void TFilter_Reset(IntPtr f, out byte opSt);
        [DllImport(SdkLib.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void TFilter_ClearParams(IntPtr f, out byte opSt);
        [DllImport(SdkLib.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void TFilter_SetParams(IntPtr f, string fparams, out byte opSt);
        [DllImport(SdkLib.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern double TFilter_Filter(IntPtr f, double value, out byte opSt);
        [DllImport(SdkLib.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void TFilter_Filter_array(IntPtr f, double[] values, int size, out byte opSt);
        [DllImport(SdkLib.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int getID_TFilter(IntPtr f, out byte opSt);
        [DllImport(SdkLib.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void delete_TFilter(IntPtr f, out byte opSt);

        [DllImport(SdkLib.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void get_preinstalled_iir_filter_count(ref int count, out byte opSt);
        [DllImport(SdkLib.LibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void get_preinstalled_iir_filter_list([In,Out] IIRFilterParam[] fParams, out byte opSt);
        #endregion

        #region Impl
        public IntPtr CreateFilterList()
        {
            IntPtr res = create_TFilterList(out byte hasError);
            if (hasError == 1) throw new Exception("Someting went wrong!");
            return res;
        }

        public void AddFilter(IntPtr flist, IntPtr f)
        {
            TFilterList_AddFilter(flist, f, out byte hasError);
            if (hasError == 1) throw new Exception("Someting went wrong!");
        }

        public void ClearFilters(IntPtr flist)
        {
            TFilter_List_ClearFilters(flist, out byte hasError);
            if (hasError == 1) throw new Exception("Someting went wrong!");
        }

        public double Filter(IntPtr flist, double value)
        {
            double res = TFilterList_Filter(flist, value, out byte hasError);
            if (hasError == 1) throw new Exception("Someting went wrong!");
            return res;
        }

        public void FilterListFreezeWeights(IntPtr fl, bool freeze)
        {
            TFilterList_FreezeWeights(fl, freeze ? (byte)1 : (byte)0, out byte hasError);
            if (hasError == 1) throw new Exception("Cannot freeze weight");
        }

        public void FilterArray(IntPtr flist, double[] values)
        {
            TFilterList_Filter_array(flist, values, values.Length, out byte hasError);
            if (hasError == 1) throw new Exception("Someting went wrong!");
        }

        public void ResetFilterList(IntPtr flist)
        {
            TFilterList_ResetFilters(flist, out byte hasError);
            if (hasError == 1) throw new Exception("Someting went wrong!");
        }

        public void DeleteFilter(IntPtr flist, Filter filter) 
        {
            TFilterList_Delete_TFilter(flist, filter.FPtr, out byte hasError);
            if (hasError == 1) throw new Exception("Someting went wrong!");
        }


        public void DeleteList(IntPtr flist)
        {
            delete_TFilterList(flist, out byte hasError);
            if (hasError == 1) throw new Exception("Someting went wrong!");
        }

        public IntPtr CreateCustomFilter(string filter)
        {
            IntPtr res = create_custom_TFilter(filter, out byte hasError);
            if (hasError == 1) throw new Exception("Someting went wrong!");
            return res;
        }

        public IntPtr CreateCustomFilterFromPath(string path)
        {
            IntPtr res = create_custom_TFilter_FromFile(path, out byte hasError);
            if (hasError == 1) throw new Exception("Someting went wrong!");
            return res;
        }

        public IntPtr CreateFilterByParams(IIRFilterParam fParams)
        {
            IntPtr res = create_TFilter_by_param(fParams, out byte hasError);
            if (hasError == 1) throw new Exception("Someting went wrong!");
            return res;
        }

        public IntPtr CreateCustomFIRFilter(string fileName)
        {
            IntPtr res = create_custom_FIR_Filter(fileName, out byte hasError);
            if (hasError == 1) throw new Exception("Cannot create FIR filter");
            return res;
        }

        public IntPtr CreateAdaptiveFIRFilter(FIRFilterParam fParams)
        {
            IntPtr res = create_adaptive_FIR_Filter(fParams, out byte hasError);
            if (hasError == 1) throw new Exception("Cannot create adaptive FIR filter");
            return res;
        }

        public void AdaptiveFilterFreezeWeights(IntPtr f, bool freeze)
        {
            adaptive_filter_freeze_weights(f, freeze ? (byte)1 : (byte)0, out byte hasError);
            if (hasError == 1) throw new Exception("Cannot freeze adaptive FIR filter");
        }

        public void ResetFilter(IntPtr f)
        {
            TFilter_Reset(f, out byte hasError);
            if (hasError == 1) throw new Exception("Someting went wrong!");
        }

        public void ClearFilterParams(IntPtr f)
        {
            TFilter_ClearParams(f, out byte hasError);
            if (hasError == 1) throw new Exception("Someting went wrong!");
        }

        public void SetFilterParams(IntPtr f, string fParams)
        {
            TFilter_SetParams(f, fParams, out byte hasError);
            if (hasError == 1) throw new Exception("Someting went wrong!");
        }

        public double FilterF(IntPtr f, double value)
        {
            double res = TFilter_Filter(f, value, out byte hasError);
            if (hasError == 1) throw new Exception("Someting went wrong!");
            return res;
        }

        public void FilterFArray(IntPtr f, double[] values)
        {
            TFilter_Filter_array(f, values, values.Length, out byte hasError);
            if (hasError == 1) throw new Exception("Someting went wrong!");
        }

        public int getID(IntPtr f)
        {
            int id = getID_TFilter(f, out byte hasError);
            if (hasError == 1) throw new Exception("Someting went wrong!");
            return id;
        }

        public void DeleteFilter(IntPtr f)
        {
            delete_TFilter(f, out byte hasError);
            if (hasError == 1) throw new Exception("Someting went wrong!");
        }

        public int GetPreinstalledFilterCount()
        {
            int count = 0;
            get_preinstalled_iir_filter_count(ref count, out byte hasError);
            if (hasError == 1) throw new Exception("Someting went wrong!");
            return count;
        }

        public void GetPreinstalledFilterList(IIRFilterParam[] fParams)
        {
            get_preinstalled_iir_filter_list(fParams, out byte hasError);
            if (hasError == 1) throw new Exception("Someting went wrong!");
        }


        #endregion
    }
    #endregion
}
