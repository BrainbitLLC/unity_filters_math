#ifndef FTPREINSTALLED_FILTERS_H
#define FTPREINSTALLED_FILTERS_H

#include "Foundation/Foundation.h"
#include "FTFilterParam.h"

@interface FTPreinstalledFilters : NSObject
@property (class,nonatomic, readonly) int preinstalledFilterCount;
+(NSArray<FTFilterParam*>*)getList;
@end


#endif
