import { Injectable, NgZone } from '@angular/core';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { HttpErrorResponse } from '@angular/common/http';
import { HttpStatus } from '../_common/_enums/http.status';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { SessionHelper } from '../_common/session.helper';

@Injectable()
export class ErrorLogService {
    private name: String = 'ErrorLogService';

    private updateErrorSubject: BehaviorSubject<string> = new BehaviorSubject<string>('');

    errorUpdate: Observable<string> = this.updateErrorSubject.asObservable();

    constructor(private sessionHelper: SessionHelper) {}

    updateErrorMessage(message: string) {
        this.updateErrorSubject.next(message);
    }

   logError(error: any, router: Router, zone: NgZone) {
        if (error instanceof HttpErrorResponse) {
            // if ((<HttpErrorResponse>error).status === HttpStatus.FORBIDDEN) {
            //     zone.run(() => router.navigate(['/nopermissions']));
            // } else if ((<HttpErrorResponse>error).status === HttpStatus.NOT_FOUND) {
            //     zone.run(() => router.navigate(['/notfound']));
            // } else if ((<HttpErrorResponse>error).status === HttpStatus.INTERNAL_SERVER_ERROR ||
            //     (<HttpErrorResponse>error).status === HttpStatus.ERR_EMPTY_RESPONSE) {
            //     this.updateErrorMessage(error.message);
            //     zone.run(() => router.navigate(['/internalerror']));
            // }
        } else if (error instanceof TypeError) {
            console.error('There was a Type error.', error.stack);
        } else if (error instanceof Error) {
            console.error('There was a general error.', error.stack);
        } else {
            console.error('Nobody threw an error but something happened!', error);
        }
    }
}
