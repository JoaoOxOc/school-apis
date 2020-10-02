import 'rxjs/add/operator/map';

import { ComponentFactoryResolver, Injectable, OnInit, ViewContainerRef } from '@angular/core';

import { HttpClient, HttpEventType, HttpHeaders, HttpRequest } from '@angular/common/http';
import { HttpProgressEvent, HttpResponse } from '@angular/common/http';
import { LoaderBuilder, LoaderFactory } from './../_common/_libs/loader.builder';

import {Observable} from 'rxjs';
import { Router } from '@angular/router';
import { SessionHelper } from '../_common/session.helper';
import { Subscriber } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable()
export abstract class StorageService {

    private userToken: string;
    private requestHeaders: HttpHeaders;
    private dynamicLoader: LoaderBuilder;
    private headerJson = {
        'Cache-Control': 'no-cache',
        'Pragma': 'no-cache',
        'Expires': 'Sat, 01 Jan 2000 00:00:00 GMT',
        'Accept':  'application/json'
    };

    constructor(protected httpClient: HttpClient,
        protected router: Router,
        protected sessionHelper: SessionHelper,
        protected factory: LoaderFactory) {

        this.dynamicLoader = factory.createInstance();
    }

    /**
     * Abstract method for dealing with the API data response
     * Should be implement on the class that extends this one
     */
    protected abstract receiveStorageServiceData(data: any, dataType: string);

    /**
     * Abstract method for dealing with the API error response
     * Should be implement on the class that extends this one
     */
    protected abstract receiveStorageServiceError(error: any);

    /**
     * Defines the element where the loading animation should be displayed
     * 
     * If is main loader, no element should be defined
     * 
     * @protected
     * @param {ViewContainerRef} [parentRef=null] 
     * @param {boolean} [isMainLoader=false] 
     * @memberof StorageService
     */
    protected defineParentElementOfLoader(parentRef: ViewContainerRef = null, isMainLoader = false) {
        if (parentRef) {
            this.dynamicLoader.setRootViewContainerRef(parentRef);
            this.dynamicLoader.addDynamicComponent();
        }
    }

    /**
     * Defines the html element of a parent container view component where the loading animation should be displayed
     * 
     * @protected
     * @param {ViewContainerRef} parentRef 
     * @param {any} targetElement 
     * @memberof StorageService
     */
    protected defineParentHtmlElementOfLoader(parentRef: ViewContainerRef, targetElement) {
        this.dynamicLoader.setRootViewContainerRef(parentRef);
        this.dynamicLoader.addEmbeddedDynamicComponent(targetElement);
    }

    /**
     * Necessary headers for the http request
     * @param authRequired defines if the bearer token should be included
     * @param isFile defines the content type
     */
    private loadRequestData(authRequired: boolean, isFile: boolean = false) {
        if (isFile) {
            // headerJson['Content-Type'] = 'multipart/form-data';
        } else {
            this.headerJson['Content-Type'] = 'application/json';
        }
        if (authRequired) {
            this.userToken = this.sessionHelper.getApiToken();
            this.headerJson['Authorization'] = 'Bearer ' +  this.userToken;
        }
        this.requestHeaders = new HttpHeaders(this.headerJson);
    }

    /**
     * Method to start the loading animation
     */
    private showLoading(show: boolean) {
        if (show) {
            this.dynamicLoader.display();
        }
    }

    /**
     * Method to stop the loading animation
     */
    private hideLoading(show: boolean) {
        if (show) {
            setTimeout(() => {
                this.dynamicLoader.hide();
                this.dynamicLoader.setPercentage(0);
            }, 100);
        }
    }

    /**
     * Method to deal with http request download progress
     */
    private processRequestDownloadProgress(event: HttpProgressEvent, showLoader: boolean) {
        // This is an upload progress event. Compute and show the % done:
        const percentDone = Math.round(100 * event.loaded / event.total);
        if (showLoader) {
            this.dynamicLoader.setPercentage(percentDone);
        }
    }

    /**
     * Method to deal with http request download data
     */
    private processRequestDownloadData(data, showLoader: boolean, dataType: string) {
        if (showLoader) {
            setTimeout(() => {
                this.dynamicLoader.hide();
                this.dynamicLoader.setPercentage(0);
            }, 100);
        }
        this.receiveStorageServiceData(data, dataType);
    }

    /**
     * Method to deal with http request upload progress
     */
    private processRequestUploadProgress(event: HttpProgressEvent, showLoader: boolean) {
        // This is an upload progress event. Compute and show the % done:
        const percentDone = Math.round(100 * event.loaded / event.total);
        if (showLoader) {
            this.dynamicLoader.setPercentage(percentDone);
        }
    }

    /**
     * Method to deal with http request upload response
     */
    private processRequestUploadData(data, showLoader: boolean, dataType: string) {
        if (showLoader) {
            setTimeout(() => {
                this.dynamicLoader.hide();
                this.dynamicLoader.setPercentage(0);
            }, 100);
    }
        this.receiveStorageServiceData(data, dataType);
    }

    /**
     * Method to deal with http request error
     */
    private processRequestError(error, showLoader: boolean) {
        if (showLoader) {
            setTimeout(() => {
                this.dynamicLoader.hide();
                this.dynamicLoader.setPercentage(0);
            }, 100);
        }
        this.receiveStorageServiceError(error);
    }

    protected addToHeader(key: string, value: string) {
        this.headerJson[key] = value;
        //this.requestHeaders.append(key, value);
    }

    /**
     * Generic method for a GET Http request with JSON response
     * @param apiUrl to invoke
     */
    protected getData<T>(apiUrl: string, dataType: string, showLoader: boolean, authRequired = true) {
        this.showLoading(showLoader);

        this.loadRequestData(authRequired);

        const req = new HttpRequest<T>('GET', apiUrl, {
            headers: this.requestHeaders,
            reportProgress: true
        });

        this.httpClient.request(req).subscribe(event => {
            // Via this API, you get access to the raw event stream.
            if (event.type === HttpEventType.DownloadProgress) {
                this.processRequestDownloadProgress(event, showLoader);
            } else if (event instanceof HttpResponse) {
                this.processRequestDownloadData(event, showLoader, dataType);
            }
        },
        error => {
            this.processRequestError(error, showLoader);
        });
    }

    /**
     * Generic method for a GET Http request with TEXT response
     * @param apiUrl to invoke
     */
    protected getTextData<text>(apiUrl: string, showLoader: boolean, authRequired = true) {
        this.showLoading(showLoader);

        this.loadRequestData(authRequired);

        const req = new HttpRequest<text>('GET', apiUrl, {
            headers: this.requestHeaders,
            responseType: 'text',
            reportProgress: true
        });

        this.httpClient.request(req).subscribe(event => {
            if (event.type === HttpEventType.DownloadProgress) {
                this.processRequestDownloadProgress(event, showLoader);
            } else if (event instanceof HttpResponse) {
                this.processRequestDownloadData(event, showLoader, 'text');
            }
        },
        error => {
            this.processRequestError(error, showLoader);
        });
    }

    /**
     * Generic method for a GET Http request with BLOB (file) response
     * @param apiUrl to invoke
     */
    protected getFileData<blob>(apiUrl: string, showLoader: boolean, authRequired = true) {
        this.showLoading(showLoader);

        this.loadRequestData(authRequired);

        const req = new HttpRequest<blob>('GET', apiUrl, {
            headers: this.requestHeaders,
            responseType: 'blob',
            reportProgress: true
        });

        this.httpClient.request(req).subscribe(event => {
            if (event.type === HttpEventType.DownloadProgress) {
                this.processRequestDownloadProgress(event, showLoader);
            } else if (event instanceof HttpResponse) {
                this.processRequestDownloadData(event, showLoader, 'blob');
            }
        },
        error => {
            this.processRequestError(error, showLoader);
        });
    }

    /**
     * Generic method for a PUT Http request
     * @param data for upload
     * @param apiUrl to invoke
     */
    protected putData<T>(data: T, apiUrl: string, dataType: string, showLoader: boolean, authRequired = true) {
        this.showLoading(showLoader);

        this.loadRequestData(authRequired);

        const req = new HttpRequest<T>('PUT', apiUrl, data, {
            headers: this.requestHeaders,
            reportProgress: true
        });

        this.httpClient.request(req).subscribe(event => {
            if (event.type === HttpEventType.UploadProgress) {
              this.processRequestUploadProgress(event, showLoader);
            } else if (event instanceof HttpResponse) {
                this.processRequestUploadData(event, showLoader, dataType);
            }
        },
        error => {
            this.processRequestError(error, showLoader);
        });
    }

    /**
     * Generic method to http PUT of a TEXT in an API
     * @param apiUrl to invoke
     */
    protected putTextData<text>(file: text, apiUrl: string, showLoader: boolean, authRequired = true) {
        this.showLoading(showLoader);

        this.loadRequestData(authRequired, true);

        const req = new HttpRequest<text>('PUT', apiUrl, file, {
            headers: this.requestHeaders,
            reportProgress: true
        });

        this.httpClient.request(req).subscribe(event => {
            if (event.type === HttpEventType.UploadProgress) {
                this.processRequestUploadProgress(event, showLoader);
              } else if (event instanceof HttpResponse) {
                  this.processRequestUploadData(event, showLoader, 'text');
              }
          },
          error => {
              this.processRequestError(error, showLoader);
          });
    }


    /**
     * Generic method to http PUT of a BLOB (file) in an API
     * @param apiUrl to invoke
     */
    protected putFileData<blob>(file: blob, dataType: string, apiUrl: string, showLoader: boolean, authRequired = true) {
        this.showLoading(showLoader);

        this.loadRequestData(authRequired, true);

        const req = new HttpRequest<blob>('PUT', apiUrl, file, {
            headers: this.requestHeaders,
            reportProgress: true
        });

        this.httpClient.request(req).subscribe(event => {
            if (event.type === HttpEventType.UploadProgress) {
                this.processRequestUploadProgress(event, showLoader);
              } else if (event instanceof HttpResponse) {
                  this.processRequestUploadData(event, showLoader, dataType);
              }
          },
          error => {
              this.processRequestError(error, showLoader);
          });
    }

    /**
     * Generic method for a POST Http request
     * @param data for upload
     * @param apiUrl to invoke
     */
    protected postData<T>(data: T, apiUrl: string, dataType: string, showLoader: boolean, authRequired = true) {
        this.showLoading(showLoader);

        this.loadRequestData(authRequired);

        const req = new HttpRequest<T>('POST', apiUrl, data, {
            headers: this.requestHeaders,
            reportProgress: true
        });

        this.httpClient.request(req).subscribe(event => {
            if (event.type === HttpEventType.UploadProgress) {
                this.processRequestUploadProgress(event, showLoader);
              } else if (event instanceof HttpResponse) {
                  this.processRequestUploadData(event, showLoader, dataType);
              }
          },
          error => {
              this.processRequestError(error, showLoader);
          });
    }

    /**
     * Generic method to http PUT of a TEXT in an API
     * @param apiUrl to invoke
     */
    protected postTextData<text>(file: text, apiUrl: string, showLoader: boolean, authRequired = true) {
        this.showLoading(showLoader);

        this.loadRequestData(authRequired, true);

        const req = new HttpRequest<text>('POST', apiUrl, file, {
            headers: this.requestHeaders,
            reportProgress: true
        });

        this.httpClient.request(req).subscribe(event => {
            if (event.type === HttpEventType.UploadProgress) {
                this.processRequestUploadProgress(event, showLoader);
              } else if (event instanceof HttpResponse) {
                  this.processRequestUploadData(event, showLoader, 'text');
              }
          },
          error => {
              this.processRequestError(error, showLoader);
          });
    }

    /**
     * Generic method to http PUT of a BLOB (file) in an API
     * @param apiUrl to invoke
     */
    protected postFileData<blob>(file: blob, dataType: string, apiUrl: string, showLoader: boolean, authRequired = true) {
        this.showLoading(showLoader);

        this.loadRequestData(authRequired, true);

        const req = new HttpRequest<blob>('POST', apiUrl, file, {
            headers: this.requestHeaders,
            reportProgress: true
        });

        this.httpClient.request(req).subscribe(event => {
            if (event.type === HttpEventType.UploadProgress) {
                this.processRequestUploadProgress(event, showLoader);
              } else if (event instanceof HttpResponse) {
                  this.processRequestUploadData(event, showLoader, dataType);
              }
          },
          error => {
              this.processRequestError(error, showLoader);
          });
    }


    /**
     * Generic method to http DELETE request
     * @param apiUrl to invoke
     */
    protected deleteData<T>(apiUrl: string, dataType: string, showLoader: boolean, authRequired = true) {
        this.showLoading(showLoader);

        this.loadRequestData(authRequired);

        const req = new HttpRequest<T>('DELETE', apiUrl, {
            headers: this.requestHeaders,
            reportProgress: true
        });

        this.httpClient.request(req).subscribe(event => {
            if (event.type === HttpEventType.DownloadProgress) {
                this.processRequestDownloadProgress(event, showLoader);
            } else if (event instanceof HttpResponse) {
                this.processRequestDownloadData(event, showLoader, dataType);
            }
        },
        error => {
            this.processRequestError(error, showLoader);
        });
    }


    /**
     * Generic method for a GET Http request with JSON response return from promise
     * @param apiUrl
     * @param showLoader
     * @param authRequired
     */
    protected getDataPromise<T>(apiUrl: string, dataType: string, showLoader: boolean, authRequired = true) {
        this.showLoading(showLoader);

        this.loadRequestData(authRequired);

        const req = new HttpRequest<T>('GET', apiUrl, {
            headers: this.requestHeaders,
            reportProgress: true
        });

        return this.httpClient.request(req).pipe(map(event => {
            if (event.type === HttpEventType.DownloadProgress) {
                this.processRequestDownloadProgress(event, showLoader);
            } else if (event instanceof HttpResponse) {
                this.hideLoading(showLoader);
                return <HttpResponse<T>>event;
            }
        },
        error => {
            this.processRequestError(error, showLoader);
        })).toPromise();
    }


    /**
     * Generic method for a GET Http request with text (file) response return from promise
     * @param apiUrl
     * @param showLoader
     * @param authRequired
     */
    protected getTextDataPromise<text>(apiUrl: string, showLoader: boolean, authRequired = true) {
        this.showLoading(showLoader);

        this.loadRequestData(authRequired);

        const req = new HttpRequest<text>('GET', apiUrl, {
            headers: this.requestHeaders,
            responseType: 'text',
            reportProgress: true
        });

        return this.httpClient.request(req).pipe(map(event => {
            if (event.type === HttpEventType.DownloadProgress) {
                this.processRequestDownloadProgress(event, showLoader);
            } else if (event instanceof HttpResponse) {
                this.hideLoading(showLoader);
                return event;
            }
        },
        error => {
            this.processRequestError(error, showLoader);
        })).toPromise();
    }

    /**
     * Generic method for a GET Http request with BLOB (file) response return from promise
     * @param apiUrl
     * @param showLoader
     * @param authRequired
     */
    protected getFileDataPromise<Blob>(apiUrl: string, showLoader: boolean, authRequired = true) {
        this.showLoading(showLoader);

        this.loadRequestData(authRequired);

        const req = new HttpRequest<Blob>('GET', apiUrl, {
            headers: this.requestHeaders,
            responseType: 'blob',
            reportProgress: true
        });

        return this.httpClient.request(req).pipe(map(event => {
            if (event.type === HttpEventType.DownloadProgress) {
                this.processRequestDownloadProgress(event, showLoader);
            } else if (event instanceof HttpResponse) {
                this.hideLoading(showLoader);
                return event;
            }
        },
        error => {
            this.processRequestError(error, showLoader);
        })).toPromise();
    }

    /**
     * Generic method for a PUT Http request with JSON response return from promise
     * @param apiUrl
     * @param showLoader
     * @param authRequired
     */
    protected putDataPromise<T>(data: T, apiUrl: string, dataType: string, showLoader: boolean, authRequired = true) {
        this.showLoading(showLoader);

        this.loadRequestData(authRequired);

        const req = new HttpRequest<T>('PUT', apiUrl, data, {
            headers: this.requestHeaders,
            reportProgress: true
        });

        return this.httpClient.request(req).pipe(map(event => {
            if (event.type === HttpEventType.UploadProgress) {
              this.processRequestUploadProgress(event, showLoader);
            } else if (event instanceof HttpResponse) {
                this.hideLoading(showLoader);
                return <HttpResponse<T>>event;
            }
        },
        error => {
            this.processRequestError(error, showLoader);
        })).toPromise();
    }

    /**
     * Generic method for a POST Http request with JSON response return from promise
     * @param apiUrl
     * @param showLoader
     * @param authRequired
     */
    protected postDataPromise<T>(data: T, apiUrl: string, dataType: string, showLoader: boolean, authRequired = true) {
        this.showLoading(showLoader);

        this.loadRequestData(authRequired);

        const req = new HttpRequest<T>('POST', apiUrl, data, {
            headers: this.requestHeaders,
            reportProgress: true
        });

        return this.httpClient.request(req).pipe(map(event => {
            if (event.type === HttpEventType.UploadProgress) {
                this.processRequestUploadProgress(event, showLoader);
              } else if (event instanceof HttpResponse) {
                this.hideLoading(showLoader);
                return <HttpResponse<T>>event;
              }
          },
          error => {
              this.processRequestError(error, showLoader);
          })).toPromise();
    }

    /**
     * Method to download file that returns an observable so the download can be async
     * @param apiUrl
     * @param showLoader
     * @param authRequired
     */
    downloadFile<Blob>(apiUrl: string, authRequired = true): Observable<any> {
        return new Observable((observer: Subscriber<any>) => {

            let objectUrl: string = null;

            this.loadRequestData(authRequired);

            const req = new HttpRequest<Blob>('GET', apiUrl, {
                headers: this.requestHeaders,
                responseType: 'blob',
                reportProgress: true
            });

            this.httpClient.request(req).subscribe(event => {
                if (event.type === HttpEventType.DownloadProgress) {
                    const percentDone = Math.round(100 * event.loaded / event.total);
                    observer.next(percentDone);
                } else if (event instanceof HttpResponse) {
                    objectUrl = URL.createObjectURL(event.body);
                    observer.next(objectUrl);
                }
            },
            error => {
                observer.next(error);
            });

            return () => {
                if (objectUrl) {
                    URL.revokeObjectURL(objectUrl);
                    objectUrl = null;
                }
            };
        });
    }

    protected postExportExcel<blob>(apiUrl: string, data, showLoader: boolean, authRequired = true) {
        this.showLoading(showLoader);

        this.loadRequestData(authRequired);

        const req = new HttpRequest('POST', apiUrl, data, {
            headers: this.requestHeaders,
            responseType: 'blob',
            reportProgress: true
        });

        this.httpClient.request(req).subscribe(event => {
            if (event.type === HttpEventType.DownloadProgress) {
                this.processRequestDownloadProgress(event, showLoader);
            } else if (event instanceof HttpResponse) {
                this.processRequestDownloadData(event, showLoader, 'blob');
            }
        },
        error => {
  
            this.processRequestError(error, showLoader);
        });
    }
}
