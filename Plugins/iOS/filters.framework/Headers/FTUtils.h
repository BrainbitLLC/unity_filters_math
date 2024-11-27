//
//  NTUtils.h
//  neurosdk
//
//  Created by Aseatari on 06.09.2022.
//

#ifndef NTUtils_h
#define NTUtils_h

extern "C" {
    #include "f_types.h"
}

@interface FTUtils : NSObject

+ (void) raise_exception_if: (TOpStatus) opStatus;

@end

#endif /* NTUtils_h */
