import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { CompanyTypeEnum } from './_enums/company.enum';
import { HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ProfileCodesString } from './_enums/profile.enum';
import { Headers } from '@angular/http';
@Injectable()
export class SessionHelper {

    private loggedIn = this.getApiToken() != null
        ? new BehaviorSubject<boolean>(true)
        : new BehaviorSubject<boolean>(false);


    public jwtRefreshed = new BehaviorSubject<boolean>(false);

    private isTrialCompany = new BehaviorSubject<boolean>(false);

    constructor() { }

    parseJwt(token) {
        if (token) {
            const base64Url = token.split('.')[1];
            const base64 = base64Url.replace('-', '+').replace('_', '/');
            return JSON.parse(window.atob(base64));
        } else {
            return null;
        }
    }

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

    getRequestHeader(): Headers {

        const header = new Headers({
            'Cache-Control': 'no-cache',
            'Pragma': 'no-cache',
            'Expires': 'Sat, 01 Jan 2000 00:00:00 GMT',
            'Content-Type': 'application/json',
            'Accept': 'application/json',
            'Authorization': 'Bearer ' + this.getApiToken()
        });

        return header;
    }

    getRequestHttpClientOptions(): {} {
        return {
            headers: this.getRequestHttpClientHeader(),
            reportProgress: true,
            observe: 'response'
        };
    }

    getRequestHttpClientHeader(): HttpHeaders {

        const header = new HttpHeaders({
            'Cache-Control': 'no-cache',
            'Pragma': 'no-cache',
            'Expires': 'Sat, 01 Jan 2000 00:00:00 GMT',
            'Content-Type': 'application/json',
            'Accept': 'application/json',
            'Authorization': 'Bearer ' + this.getApiToken()
        });

        return header;
    }

    getApiToken() {
        return localStorage.getItem('authToken');
    }

    getCurrentUserId() {
        return localStorage.getItem('currentUser');
    }

    getCurrentUser() {
        return JSON.parse(localStorage.getItem('user'));
    }

    getCurrentUserImage() {
        if (localStorage.getItem('UserImage') !== null) {
            return localStorage.getItem('UserImage');
        } else {
            return '../../assets/images/default-profile-pic.png';
        }
    }

    getLanguage() {
        return localStorage.getItem('lang');
    }

    setLanguage(lang: string) {
        localStorage.setItem('lang', lang);
    }

    setSessionUserImage(imageId, userId) {
        // localStorage.setItem('UserImage', fileReader);
        const userData = this.getCurrentUser();
        if (userData && userData.id === userId) {
            userData.profileImage = imageId;
        }
    }


    renewSessionToken(token) {
        localStorage.removeItem('authToken');
        localStorage.setItem('authToken', token);
        this.jwtRefreshed.next(true);
    }

    setSession(currentUser: string, token: string): void {
        // TODO(zok): Make sure the localstorage is domain safe
        localStorage.setItem('currentUser', currentUser);
        localStorage.setItem('authToken', token);
        this.loggedIn.next(true);
        if (!this.introIsCompleted()) {
            localStorage.setItem('doLogin', 'true');
        }
    }

    cleanSession(): void {
        localStorage.removeItem('UserImage');
        localStorage.removeItem('currentUser');
        localStorage.removeItem('authToken');
        localStorage.removeItem('user');
    }

    doLogout(callback) {

        this.cleanSession();

        this.loggedIn.next(false);

        this.isTrialCompany.next(false);

        // callback();
    }

    isAuthenticatd() {
        return this.loggedIn.asObservable();
    }

    getIsTrialCompany() {
        return this.isTrialCompany.asObservable();
    }


    getUserProfiles() {

        const jwt = this.parseJwt(this.getApiToken());

        return jwt ? jwt.roles.replace(/[\[\]"]/g, '').split(',') : null;
    }
    getUserCompanyId() {

        const jwt = this.parseJwt(this.getApiToken());

        return jwt ? jwt.companyId : null;
    }
    getUserCompanyType() {

        const jwt = this.parseJwt(this.getApiToken());

        return jwt ? jwt.companyType : null;
    }

    checkUserHasPermission(permission) {
        let authorized = false;
        if (this.isAuthenticatd()) {
            const userRoles = this.getUserProfiles();

            if (userRoles && userRoles.filter(role => role === permission).length > 0) {
                authorized = true;
            }
        }
        return authorized;
    }

    checkUserHasAnyPermission(permissions: string[]) {
        let authorized = false;
        if (this.isAuthenticatd()) {
            const userRoles = this.getUserProfiles();

            if (userRoles) {
                let i = 0;
                while (i < permissions.length && !authorized) {
                    if (userRoles.includes(permissions[i])) {
                        authorized = true;
                    }

                    i++;
                }
            }
        }
        return authorized;
    }

    checkUserCompanyHasAccess(companyTypes: number[]) {
        let authorized = false;
        if (this.isAuthenticatd()) {
            const userCompanyType = this.getUserCompanyType();
            const isAdmin = this.checkUserHasPermission(ProfileCodesString.system_management);

            if (isAdmin || userCompanyType && companyTypes.includes(userCompanyType)) {
                authorized = true;
            }
        }
        return authorized;
    }

    introIsCompleted() {
        return localStorage.getItem('introductionSkipped') !== null && localStorage.getItem('introductionSkipped') === 'true';
    }

    setIntroductionCompleted() {
        localStorage.setItem('introductionSkipped', 'true');
    }


}

