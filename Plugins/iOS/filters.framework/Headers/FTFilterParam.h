#ifndef FTFILTER_PARAM_H
#define FTFILTER_PARAM_H

#include "Foundation/Foundation.h"


typedef NS_ENUM (UInt8, FTFilterType) {
    FTHP = 0,
    FTLP = 1,
    FTBandStop = 2,
    FTBandPass = 3,
    FTNone = 4
};

@interface FTFilterParam : NSObject
@property (nonatomic) enum FTFilterType type;
@property (nonatomic) int samplingFreq;
@property (nonatomic) double cutoffFreq;

-(id) initWithType:(FTFilterType)type andSamplingFreq :(int)freq andCutoffFreq:(double)cutoffFreq;
 
@end


#endif
