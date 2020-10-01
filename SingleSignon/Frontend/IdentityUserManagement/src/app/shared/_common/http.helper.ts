import { Headers } from '@angular/http';
import { Injectable } from '@angular/core';
import { SessionHelper } from './session.helper';

@Injectable()
export class HttpHelper {

    constructor(private sessionHelper: SessionHelper) { }

    encodeQueryData(data) {
        const ret = ['?'];

        for (const d in data) {
            if (d !== null && d !== undefined) {
                ret.push(encodeURIComponent(d) + '=' + encodeURIComponent(data[d]) + '&');
            }
        }

        const returnString = ret.join('');
        return returnString.substring(0, returnString.length - 1);
    }

    // Returns the assembled header { Content-Type, Accept, Authorization  }
    getHttpHeaders() {

        const jwtToken = localStorage.getItem('authToken');

        const headers = new Headers({
            'Content-Type': 'application/json',
            'Accept': 'application/json',
            'Authorization': 'Bearer ' + jwtToken
        });

        return headers;
    }

}
