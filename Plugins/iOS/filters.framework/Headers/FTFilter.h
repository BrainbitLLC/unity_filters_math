#ifndef FTFILTER_H
#define FTFILTER_H

#include "Foundation/Foundation.h"
#include "filters.h"
#include "FTFilterParam.h"

@interface FTFilter : NSObject

//@property (nonatomic) TFilter* _Nonnull filter_ptr;

-(id _Nonnull ) initWithParam:(FTFilterParam*_Nonnull) param;
-(id _Nonnull ) initWithName:(char*_Nonnull) rowParams;
-(void) reset;
-(void) clearParams;
-(void) setParams:(char*_Nonnull) params;
-(double) filter:(double) number;
-(NSArray<NSNumber*>*_Nonnull) filterArray:(NSArray<NSNumber*>*_Nonnull) array;
-(TFilterID) getID;
//-(void) delete_TFilter(TFilter*);

@end

#endif
