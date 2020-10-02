import { ErrorHandler, Injectable, Injector, NgZone } from '@angular/core';

import { ErrorLogService } from './../../services/error-log.service';
import { Router } from '@angular/router';

@Injectable()
export class GlobalErrorHandler extends ErrorHandler {
    /**
     * 
     * @param errorLogService is the service responsible to handle the app errors
     * @param zone is needed for running tasks inside the angular main thread (like router.navigate)
     * @param injector to avoid a loop in dependency injection
     */
    constructor(private errorLogService: ErrorLogService, private zone: NgZone, private injector: Injector) {
        super();
    }
    handleError(error) {
        this.errorLogService.logError(error, this.injector.get(Router), this.zone);
    }
}

