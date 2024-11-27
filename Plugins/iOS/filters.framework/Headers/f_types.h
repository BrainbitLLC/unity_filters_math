#ifndef F_TYPES_H
#define F_TYPES_H

#include "lib_export.h"
#include <stdint.h>
typedef int TFilterID;
typedef uint8_t TOpStatus; // 0 - no error, 1 - has error

enum IIRFilterType : uint8_t
{
	FtHP = 0,
	FtLP = 1,
	FtBandStop = 2,
	FtBandPass = 3,
	FtNone = 4
};

typedef struct _IIRFilterParam
{
	enum IIRFilterType type;
	int samplingFreq;
	double cutoffFreq;
}IIRFilterParam;

typedef struct _FIRFilterParam
{
	int coefsNumb;
	int samplingRate;
	int networkFreq;
	double refNoiseAmpl;
	double mu;
	int downsample_scale;
}FIRFilterParam;

#endif