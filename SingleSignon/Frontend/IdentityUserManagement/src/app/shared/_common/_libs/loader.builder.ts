import { ComponentFactoryResolver, ComponentRef, EmbeddedViewRef, Injectable } from "@angular/core";

import { DynamicLoaderService } from "../../services/dynamic-loader.service";
import { LoaderComponent } from "../../loader/loader.component";

export class LoaderBuilder {
    public factoryResolver: ComponentFactoryResolver;
    public loaderService: DynamicLoaderService;
    private rootViewContainer;
    private mainViewContainer;
    private insertedComponent: ComponentRef<LoaderComponent>;
    public mainInsertedComponent: ComponentRef<LoaderComponent>;

    constructor() { }

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
     * Adds the main loader to app main component
     * 
     * @memberof DynamicLoaderService
     */
    public addMainLoaderComponent() {
        const factory = this.factoryResolver.resolveComponentFactory(LoaderComponent);

        if (!this.mainViewContainer) {
            this.mainViewContainer = this.loaderService.getMainViewContainerRef();
        }

        this.mainInsertedComponent = factory.create(this.mainViewContainer.parentInjector);

        this.mainViewContainer.insert(this.mainInsertedComponent.hostView);

        this.mainInsertedComponent.instance.setAsMainLoader(true);
    }

    /**
     * Sets the parent component view container where the loader will be included
     * 
     * @param {any} viewContainerRef 
     * @memberof DynamicLoaderService
     */
    public setRootViewContainerRef(viewContainerRef) {
        this.rootViewContainer = viewContainerRef;
    }

    /**
     * Adds the loader to the root of the defined rootViewContainerRef
     * 
     * @memberof DynamicLoaderService
     */
    public addDynamicComponent() {
        const factory = this.factoryResolver.resolveComponentFactory(LoaderComponent);

        if (!this.rootViewContainer) {
            this.rootViewContainer = this.loaderService.getMainViewContainerRef();
        }

        this.insertedComponent = factory.create(this.rootViewContainer.parentInjector);

        this.rootViewContainer.insert(this.insertedComponent.hostView);

        this.insertedComponent.instance.setAsMainLoader(false);
    }

    /**
     * Adds the loader to a specific HTML element related to the defined rootViewContainerRef
     * 
     * @param {any} targetElement - the HTML element
     * @memberof DynamicLoaderService
     */
    public addEmbeddedDynamicComponent(targetElement: HTMLElement) {
        if (!this.rootViewContainer) {
            this.rootViewContainer = this.loaderService.getMainViewContainerRef();
        }
        // 1. Create a component reference from the component 
        this.insertedComponent = this.factoryResolver.resolveComponentFactory(LoaderComponent).create(this.rootViewContainer.parentInjector);

        // 2. Attach component to the appRef so that it's inside the ng component tree
        this.rootViewContainer.insert(this.insertedComponent.hostView);

        // 3. Get DOM element from component
        const domElem = (this.insertedComponent.hostView as EmbeddedViewRef<any>).rootNodes[0] as HTMLElement;

        domElem.style.width = targetElement.offsetWidth + 'px';
        domElem.style.margin = targetElement.style.margin;
        domElem.style.padding = targetElement.style.padding;
    
        // 4. Append DOM element to the target element
        targetElement.appendChild(domElem);

        this.insertedComponent.instance.setAsMainLoader(false);
        this.insertedComponent.instance.insideElement(true);
    }

    /**
     * Displays the loader animation
     * 
     * @memberof DynamicLoaderService
     */
    public display(showPercent: boolean = true) {
        if (this.insertedComponent) {
            const comp = this.insertedComponent.instance;
            if (comp) {
                comp.displayLoader(showPercent);
            }
        } else {
            const comp = this.mainInsertedComponent.instance;
            if (comp) {
                comp.displayLoader();
            }
        }
    }

    /**
     * Hides the loader animation
     * 
     * @memberof DynamicLoaderService
     */
    public hide() {
        if (this.insertedComponent) {
            const comp = this.insertedComponent.instance;
            if (comp) {
                comp.hideLoader();
            }
        } else {
            const comp = this.mainInsertedComponent.instance;
            if (comp) {
                comp.hideLoader();
            }
        }
    }

    /**
     * Sets the percentage to show on loader animation
     * 
     * @param {number} percent 
     * @memberof DynamicLoaderService
     */
    public setPercentage(percent: number) {
        if (this.insertedComponent) {
            const comp = this.insertedComponent.instance;
            if (comp) {
                comp.setPercentage(percent);
            }
        }
    }

    /**
     * Destroys the inserted component, so the main component should be used instead
     * 
     * @memberof DynamicLoaderService
     */
    public destroyInserted() {
        this.insertedComponent = null;
    }
}

@Injectable()
export class LoaderFactory {
    constructor(private component: ComponentFactoryResolver, 
        private loaderService: DynamicLoaderService) {}

    public createInstance() {
      const loader = new LoaderBuilder();
  
      loader.factoryResolver = this.component;
      loader.loaderService = this.loaderService;
      loader.mainInsertedComponent = this.loaderService.getMainLoaderComponent();
  
      return loader;
    }
}