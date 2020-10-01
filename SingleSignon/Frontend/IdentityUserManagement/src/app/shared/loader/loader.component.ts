import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';

import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-loader',
  templateUrl: './loader.component.html',
  styleUrls: ['./loader.component.css']
})
export class LoaderComponent implements OnInit, OnDestroy {

  show$: Observable<boolean>;
  showMain$: Observable<boolean>;

  showPercentage$: Observable<boolean>;
  loadPercentage$: Observable<number>;
  
  private legacyLoaderSubs: Subscription;

  private loadPercentage = new BehaviorSubject<number>(0);

  private toShow = new BehaviorSubject<boolean>(false);

  private showMainLoader = new BehaviorSubject<boolean>(false);

  private displayPercentage = new BehaviorSubject<boolean>(false);

  private isInsideElement = false;

  private isMainLoader = false;

  // Get the needed dependencies
  constructor(private cdRef : ChangeDetectorRef) { }

  ngOnInit() {

    this.showMain$ = this.showMainLoader.asObservable();
    this.show$ = this.toShow.asObservable();
    this.showPercentage$ = this.displayPercentage.asObservable();
    this.loadPercentage$ = this.loadPercentage.asObservable();

    // this.legacyLoaderSubs = this.loader.toShow().subscribe(data => {
    //   this.showMainLoader.next(data);
    // });
  }

  insideElement(isRelative: boolean) {
    this.isInsideElement = isRelative;
  }

  setAsMainLoader(isMain: boolean) {
    this.isMainLoader = isMain;
  }

  displayLoader(showPercentage: boolean = true) {
    if (this.isMainLoader) {
      this.showMainLoader.next(true);
    } else {
      this.displayPercentage.next(showPercentage);
      this.toShow.next(true);
    }
    this.cdRef.detectChanges();
  }

  hideLoader() {
    if (this.isMainLoader) {
      this.showMainLoader.next(false);
    } else {
      this.toShow.next(false);
    }
  }

  setPercentage(percent: number) {
    if (!this.isMainLoader) {
      this.loadPercentage.next(percent);
    }
  }

  ngOnDestroy() {
    if (this.legacyLoaderSubs && !this.legacyLoaderSubs.closed) {
      this.legacyLoaderSubs.unsubscribe();
    }
  }
}


