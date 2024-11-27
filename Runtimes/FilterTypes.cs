using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Neurotech.Filters
{
	public enum IIRFilterType : byte
	{
		FtHP,
		FtLP,
		FtBandStop,
		FtBandPass,
		FtNone
	};

	[StructLayout(LayoutKind.Sequential)]
	public struct IIRFilterParam
    {
		[MarshalAs(UnmanagedType.U1)]
		public IIRFilterType type;
		public int samplingFreq;
		public double cutoffFreq;
	}

    [StructLayout(LayoutKind.Sequential)]
    public struct FIRFilterParam
    {
        public int coefsNumb;
        public int samplingRate;
        public int networkFreq;
        public double refNoiseAmpl;
    }
}
