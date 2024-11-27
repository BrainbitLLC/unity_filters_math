#ifndef FTFILTER_LIST_H
#define FTFILTER_LIST_H

#include <Foundation/Foundation.h>
#include "filters.h"
#include "FTFilter.h"

@interface FTFilterList : NSObject

-(void) addFilter:(FTFilter*_Nonnull) filter;
-(double) filter:(double) number;
-(NSArray<NSNumber*>*_Nonnull) filterArray:(NSArray<NSNumber*>*_Nonnull) array;
-(void) clearFilters;
-(void) reset;
-(void) deleteFilter:(FTFilter*) filterId;

@end
#endif
