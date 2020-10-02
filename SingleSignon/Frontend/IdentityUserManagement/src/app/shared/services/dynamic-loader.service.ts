import { ComponentFactoryResolver, ComponentRef, EmbeddedViewRef, Injectable } from '@angular/core';

import { BehaviorSubject } from 'rxjs';
import { LoaderComponent } from '../loader/loader.component';
import { Subject } from 'rxjs';

/**
 * Service to create and get the main loader component for the global loader animation of the app
 * 
 * @export
 * @class DynamicLoaderService
 */
@Injectable()
export class DynamicLoaderService {

    private mainViewContainer;
    public mainInsertedComponent: ComponentRef<LoaderComponent>;

    constructor(private factoryResolver: ComponentFactoryResolver) { }

    /**
     * Sets the app component view container where the main loader will be included
     * 
     * @param {any} viewContainerRef 
     * @memberof DynamicLoaderService
     */
    public setMainViewContainerRef(viewContainerRef) {
        this.mainViewContainer = viewContainerRef;
    }

    /**
     * Get the app component view container where the main loader will be included
     * 
     * @memberof DynamicLoaderService
     */
    public getMainViewContainerRef() {
        return this.mainViewContainer;
    }

    /**
     * Get the main loader component created
     * 
     * @memberof DynamicLoaderService
     */
    public getMainLoaderComponent() {
        return this.mainInsertedComponent;
    }

    /**
     * Adds the main loader to app main component
     * 
     * @memberof DynamicLoaderService
     */
    public addMainLoaderComponent() {
        const factory = this.factoryResolver.resolveComponentFactory(LoaderComponent);

        this.mainInsertedComponent = factory.create(this.mainViewContainer.parentInjector);

        this.mainViewContainer.insert(this.mainInsertedComponent.hostView);

        this.mainInsertedComponent.instance.setAsMainLoader(true);
    }
}
